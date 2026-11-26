#!/usr/bin/env bash
# Automated failback from GOLDDR to GOLD for Patroni 4.x / PostgreSQL 16.
# Orchestrates both OpenShift clusters using separate KUBECONFIG files so no
# cross-cluster ServiceAccount tokens need to live inside OpenShift.
#
# Required environment variables:
#   ENV          : target environment ("test" or "prod")
#   KUBE_GOLD    : path to kubeconfig authenticated to GOLD
#   KUBE_GOLDDR  : path to kubeconfig authenticated to GOLDDR
#
# Optional environment variables:
#   LICENSE_PLATE       : default "f088b1"
#   DRY_RUN             : "true" (default) or "false"
#   TARGET_REPLICAS     : "auto" (default, inherits GoldDR replicas or 3) or integer
#   ALLOW_NO_PVC_BACKUP : "false" (default) or "true"
#   GSLB_GOLD_IP        : default "142.34.229.4"
#   GSLB_GOLDDR_IP      : default "142.34.64.4"
#   SYNC_TIMEOUT_SEC    : default "1800" (30 minutes)
#   GSLB_TIMEOUT_SEC    : default "600"  (10 minutes)

set -Eeuo pipefail
IFS=$'\n\t'

# ---------------------------------------------------------------------------
# Configuration & helpers
# ---------------------------------------------------------------------------
ENV="${ENV:?ENV must be set (test or prod)}"
KUBE_GOLD="${KUBE_GOLD:?KUBE_GOLD must be set}"
KUBE_GOLDDR="${KUBE_GOLDDR:?KUBE_GOLDDR must be set}"

LICENSE_PLATE="${LICENSE_PLATE:-f088b1}"
NS="${LICENSE_PLATE}-${ENV}"
DB_NAME="pidp_${ENV}"
DRY_RUN="${DRY_RUN:-true}"
TARGET_REPLICAS="${TARGET_REPLICAS:-auto}"
ALLOW_NO_PVC_BACKUP="${ALLOW_NO_PVC_BACKUP:-false}"
GSLB_GOLD_IP="${GSLB_GOLD_IP:-142.34.229.4}"
GSLB_GOLDDR_IP="${GSLB_GOLDDR_IP:-142.34.64.4}"
SYNC_TIMEOUT_SEC="${SYNC_TIMEOUT_SEC:-1800}"
GSLB_TIMEOUT_SEC="${GSLB_TIMEOUT_SEC:-600}"
TS="$(date -u +%Y%m%d_%H%M%S)"

DEFAULT_PG_CONFIG='{"postgresql":{"use_pg_rewind":true,"parameters":{"max_connections":100,"max_prepared_transactions":0,"max_locks_per_transaction":64}}}'

log()  { printf '[%s] [INFO]  %s\n' "$(date -u +%H:%M:%S)" "$*"; }
warn() { printf '[%s] [WARN]  %s\n' "$(date -u +%H:%M:%S)" "$*" >&2; }
err()  { printf '[%s] [ERROR] %s\n' "$(date -u +%H:%M:%S)" "$*" >&2; }

oc_gold()   { KUBECONFIG="$KUBE_GOLD"   oc -n "$NS" "$@"; }
oc_golddr() { KUBECONFIG="$KUBE_GOLDDR" oc -n "$NS" "$@"; }

if [[ "$ENV" != "test" && "$ENV" != "prod" ]]; then
  err "Unsupported ENV='$ENV'. Only 'test' and 'prod' have GoldDR clusters."
  exit 1
fi

for bin in oc jq; do
  if ! command -v "$bin" >/dev/null 2>&1; then
    err "Required tool '$bin' not found in PATH."
    exit 1
  fi
done

resolve_host_ip() {
  local host="$1"
  if command -v dig >/dev/null 2>&1; then
    dig +short A "$host" | grep -E '^[0-9]+\.[0-9]+\.[0-9]+\.[0-9]+$' | head -n 1 || true
  elif command -v getent >/dev/null 2>&1; then
    getent ahostsv4 "$host" | awk '{print $1; exit}' || true
  else
    python3 -c 'import socket,sys
try:
    print(socket.gethostbyname(sys.argv[1]))
except Exception:
    pass' "$host" || true
  fi
}

get_patroni_list_json() {
  local cluster_fn="$1"
  local pod
  pod="$("$cluster_fn" get pods -l app.kubernetes.io/name=patroni -o jsonpath='{.items[?(@.status.phase=="Running")].metadata.name}' | awk '{print $1}')"
  if [[ -z "$pod" ]]; then
    echo "[]"
    return 0
  fi
  "$cluster_fn" exec "$pod" -- patronictl -c /home/postgres/patroni.yml list --format=json 2>/dev/null || echo "[]"
}

print_patroni_table() {
  local label="$1"
  local cluster_fn="$2"
  local pod
  pod="$("$cluster_fn" get pods -l app.kubernetes.io/name=patroni -o jsonpath='{.items[?(@.status.phase=="Running")].metadata.name}' | awk '{print $1}')"
  log "Patroni cluster status on ${label}:"
  if [[ -z "$pod" ]]; then
    log "  (no running patroni pods)"
    return 0
  fi
  "$cluster_fn" exec "$pod" -- patronictl -c /home/postgres/patroni.yml list || true
}

check_rbac() {
  local cluster_name="$1"
  local cluster_fn="$2"
  shift 2
  local missing=0
  local item verb resource
  for item in "$@"; do
    verb="${item%%:*}"
    resource="${item#*:}"
    if "$cluster_fn" auth can-i "$verb" "$resource" >/dev/null 2>&1; then
      printf '  [OK]   %s: %s %s\n' "$cluster_name" "$verb" "$resource"
    else
      printf '  [FAIL] %s: %s %s\n' "$cluster_name" "$verb" "$resource" >&2
      missing=1
    fi
  done
  return "$missing"
}

find_backup_pod() {
  local cluster_fn="$1"
  "$cluster_fn" get pods -o json \
    | jq -r '.items[]
        | select(.status.phase == "Running")
        | select((.metadata.name | startswith("backup-container")) or (.metadata.labels["app.kubernetes.io/name"] == "backup-container"))
        | select((.status.containerStatuses // []) | all(.ready == true))
        | .metadata.name' \
    | head -n 1
}

patch_patroni_config_standby() {
  local cluster_fn="$1"
  local remote_host="$2"
  local remote_port="$3"

  local current_cfg new_cfg patch_json
  current_cfg="$("$cluster_fn" get cm patroni-config -o jsonpath='{.metadata.annotations.config}' 2>/dev/null || true)"
  if [[ -z "$current_cfg" ]] || ! jq -e . >/dev/null 2>&1 <<<"$current_cfg"; then
    current_cfg="$DEFAULT_PG_CONFIG"
  fi

  new_cfg="$(jq -c --arg host "$remote_host" --argjson port "$remote_port" \
    '.standby_cluster = {host: $host, port: $port}' <<<"$current_cfg")"
  patch_json="$(jq -nc --arg cfg "$new_cfg" '{metadata:{annotations:{config:$cfg}}}')"

  "$cluster_fn" patch cm patroni-config --type merge -p "$patch_json"
}

patch_patroni_config_promote() {
  local cluster_fn="$1"

  local current_cfg new_cfg patch_json
  current_cfg="$("$cluster_fn" get cm patroni-config -o jsonpath='{.metadata.annotations.config}' 2>/dev/null || true)"
  if [[ -z "$current_cfg" ]] || ! jq -e . >/dev/null 2>&1 <<<"$current_cfg"; then
    current_cfg="$DEFAULT_PG_CONFIG"
  fi

  new_cfg="$(jq -c 'del(.standby_cluster)' <<<"$current_cfg")"
  patch_json="$(jq -nc --arg cfg "$new_cfg" '{metadata:{annotations:{config:$cfg}}}')"

  "$cluster_fn" patch cm patroni-config --type merge -p "$patch_json"
}

wait_for_standby_sync() {
  local cluster_label="$1"
  local cluster_fn="$2"
  local expected_replicas="$3"
  local deadline=$(( $(date +%s) + SYNC_TIMEOUT_SEC ))

  log "Waiting up to ${SYNC_TIMEOUT_SEC}s for ${cluster_label} to become Standby Leader (${expected_replicas} replica(s), 0 MB lag)..."
  while true; do
    local now
    now="$(date +%s)"
    if (( now > deadline )); then
      err "Timed out waiting for ${cluster_label} Standby Leader sync."
      print_patroni_table "$cluster_label" "$cluster_fn"
      return 1
    fi

    local list_json
    list_json="$(get_patroni_list_json "$cluster_fn")"
    if jq -e --argjson n "$expected_replicas" '
      type == "array"
      and (length == $n)
      and ([.[] | select(.Role == "Standby Leader" and (.State == "streaming" or .State == "running"))] | length == 1)
      and ([.[] | select(.Role == "Replica" and (.State == "streaming" or .State == "running"))] | length == ($n - 1))
      and ([.[] | ((.["Lag in MB"] // 0) | tonumber) == 0] | all)
    ' >/dev/null 2>&1 <<<"$list_json"; then
      log "${cluster_label} is fully synced as Standby Leader with 0 MB lag."
      print_patroni_table "$cluster_label" "$cluster_fn"
      return 0
    fi

    local summary
    summary="$(jq -r 'if (type=="array" and length>0) then (.[] | "\(.Member):\(.Role):\(.State):lag=\(.["Lag in MB"] // 0)MB") else "waiting for patroni-0..." end' <<<"$list_json" 2>/dev/null | paste -sd ', ' - || echo "waiting...")"
    log "  ${cluster_label} sync status: ${summary}"
    sleep 15
  done
}

wait_for_primary() {
  local cluster_label="$1"
  local cluster_fn="$2"
  local deadline=$(( $(date +%s) + 300 ))

  log "Waiting for ${cluster_label} to promote to Primary and populate patroni-master endpoint..."
  while true; do
    local now
    now="$(date +%s)"
    if (( now > deadline )); then
      err "Timed out waiting for ${cluster_label} Primary promotion."
      print_patroni_table "$cluster_label" "$cluster_fn"
      return 1
    fi

    local list_json ep_ip
    list_json="$(get_patroni_list_json "$cluster_fn")"
    ep_ip="$("$cluster_fn" get endpoints patroni-master -o jsonpath='{.subsets[0].addresses[0].ip}' 2>/dev/null || true)"

    if [[ -n "$ep_ip" ]] && jq -e '
      type == "array"
      and ([.[] | select((.Role == "Primary" or .Role == "Leader") and .State == "running")] | length == 1)
    ' >/dev/null 2>&1 <<<"$list_json"; then
      log "${cluster_label} promoted to Primary (patroni-master endpoint: ${ep_ip})."
      print_patroni_table "$cluster_label" "$cluster_fn"
      return 0
    fi

    sleep 10
  done
}

# ---------------------------------------------------------------------------
# Phase 0: Pre-flight validation (runs in both DRY_RUN and live modes)
# ---------------------------------------------------------------------------
log "=== Phase 0: Pre-flight checks (env=${ENV}, namespace=${NS}, dry_run=${DRY_RUN}) ==="

log "Checking OpenShift API connectivity..."
GOLD_WHOAMI="$(KUBECONFIG="$KUBE_GOLD" oc whoami)"
GOLDDR_WHOAMI="$(KUBECONFIG="$KUBE_GOLDDR" oc whoami)"
log "  GOLD   authenticated as: ${GOLD_WHOAMI}"
log "  GOLDDR authenticated as: ${GOLDDR_WHOAMI}"

log "Checking required RBAC permissions..."
RBAC_OK=0
check_rbac "GOLD" oc_gold \
  "get:statefulsets" "patch:statefulsets" "patch:statefulsets/scale" \
  "get:persistentvolumeclaims" "list:persistentvolumeclaims" "delete:persistentvolumeclaims" \
  "get:configmaps" "patch:configmaps" \
  "get:services" "get:endpoints" \
  "get:pods" "list:pods" "create:pods/exec" \
  "get:deployments" "patch:deployments" || RBAC_OK=1

check_rbac "GOLDDR" oc_golddr \
  "get:statefulsets" "patch:statefulsets" "patch:statefulsets/scale" \
  "get:persistentvolumeclaims" "list:persistentvolumeclaims" "delete:persistentvolumeclaims" \
  "get:configmaps" "patch:configmaps" \
  "get:services" "get:endpoints" \
  "get:pods" "list:pods" "create:pods/exec" \
  "get:cronjobs" "patch:cronjobs" || RBAC_OK=1

if [[ "$RBAC_OK" -ne 0 ]]; then
  err "Missing required OpenShift RBAC permissions (see [FAIL] lines above). Aborting."
  exit 1
fi

GOLD_TSC_PORT="$(oc_gold get svc patroni-master-golddr -o jsonpath='{.spec.ports[0].port}')"
DR_TSC_PORT="$(oc_golddr get svc patroni-master-gold -o jsonpath='{.spec.ports[0].port}')"
if [[ -z "$GOLD_TSC_PORT" || -z "$DR_TSC_PORT" ]]; then
  err "Failed to resolve TSC ports (GOLD->DR='${GOLD_TSC_PORT}', DR->GOLD='${DR_TSC_PORT}')."
  exit 1
fi
log "Resolved TSC ports: GOLD->GOLDDR=${GOLD_TSC_PORT}, GOLDDR->GOLD=${DR_TSC_PORT}"

# Determine target replica count
CURRENT_DR_REPLICAS="$(oc_golddr get sts patroni -o jsonpath='{.spec.replicas}')"
CURRENT_GOLD_REPLICAS="$(oc_gold get sts patroni -o jsonpath='{.spec.replicas}')"
if [[ "$TARGET_REPLICAS" == "auto" ]]; then
  if [[ -n "$CURRENT_DR_REPLICAS" && "$CURRENT_DR_REPLICAS" -gt 0 ]]; then
    DESIRED_REPLICAS="$CURRENT_DR_REPLICAS"
  else
    DESIRED_REPLICAS=3
  fi
else
  DESIRED_REPLICAS="$TARGET_REPLICAS"
fi
log "Patroni StatefulSet replicas: GOLD=${CURRENT_GOLD_REPLICAS}, GOLDDR=${CURRENT_DR_REPLICAS}, target=${DESIRED_REPLICAS}"

# Check live Patroni roles on both clusters
GOLD_LIST_JSON="$(get_patroni_list_json oc_gold)"
DR_LIST_JSON="$(get_patroni_list_json oc_golddr)"
print_patroni_table "GOLD" oc_gold
print_patroni_table "GOLDDR" oc_golddr

DR_PRIMARY="$(jq -r '.[] | select((.Role == "Primary" or .Role == "Leader") and .State == "running") | .Member' <<<"$DR_LIST_JSON" | head -n 1)"
GOLD_PRIMARY="$(jq -r '.[] | select((.Role == "Primary" or .Role == "Leader") and .State == "running") | .Member' <<<"$GOLD_LIST_JSON" | head -n 1)"

if [[ -z "$DR_PRIMARY" ]]; then
  if [[ "$DRY_RUN" == "true" ]]; then
    warn "GOLDDR does not currently have a running Primary pod (normal while GOLD is active; a live run would SAFETY ABORT here)."
  else
    err "SAFETY ABORT: GOLDDR does not currently have a running Primary pod. Failback from GOLDDR to GOLD cannot proceed."
    exit 1
  fi
else
  log "Confirmed GOLDDR live Primary pod: ${DR_PRIMARY}"
fi

if [[ -n "$GOLD_PRIMARY" ]]; then
  warn "GOLD also reports a Primary pod (${GOLD_PRIMARY}) — split-brain or stale primary detected. Proceeding will wipe GOLD PVCs and re-sync from GOLDDR Primary (${DR_PRIMARY})."
fi

# Check midas-probe and GSLB
HAS_MIDAS="false"
GSLB_HOST=""
if oc_gold get deploy midas-probe >/dev/null 2>&1; then
  HAS_MIDAS="true"
  GSLB_HOST="$(oc_gold get deploy midas-probe -o jsonpath='{.spec.template.spec.containers[?(@.name=="midas-probe")].env[?(@.name=="gslb_host")].value}')"
  if [[ -z "$GSLB_HOST" ]]; then
    GSLB_HOST="$(oc_gold get deploy midas-probe -o jsonpath='{.spec.template.spec.containers[0].env[?(@.name=="gslb_host")].value}')"
  fi
  CURRENT_GSLB_IP=""
  if [[ -n "$GSLB_HOST" ]]; then
    CURRENT_GSLB_IP="$(resolve_host_ip "$GSLB_HOST")"
  fi
  log "Detected midas-probe on GOLD (gslb_host='${GSLB_HOST:-<unset>}', current DNS IP='${CURRENT_GSLB_IP:-<unresolved>}')"
else
  log "No midas-probe deployment found in GOLD/${NS} (GSLB toggle steps will be skipped)."
fi

# Check backup-container availability for in-cluster pg_dump storage (no PHI on GHA runner)
BACKUP_CLUSTER=""
BACKUP_POD=""
DR_BACKUP_POD="$(find_backup_pod oc_golddr || true)"
GOLD_BACKUP_POD="$(find_backup_pod oc_gold || true)"
if [[ -n "$DR_BACKUP_POD" ]]; then
  BACKUP_CLUSTER="golddr"
  BACKUP_POD="$DR_BACKUP_POD"
  log "Safety pg_dump target: GOLDDR pod '${BACKUP_POD}' (/backups/failback/)"
elif [[ -n "$GOLD_BACKUP_POD" ]]; then
  BACKUP_CLUSTER="gold"
  BACKUP_POD="$GOLD_BACKUP_POD"
  log "Safety pg_dump target: GOLD pod '${BACKUP_POD}' (/backups/failback/)"
else
  warn "No running backup-container pod found in GOLDDR or GOLD (${NS})."
  if [[ "$ALLOW_NO_PVC_BACKUP" != "true" ]]; then
    err "SAFETY ABORT: Refusing to run without an in-cluster backup-container PVC target (database dumps are never uploaded to GitHub Actions). Set allow_no_pvc_backup=true to stream-verify pg_dump without persistent storage."
    exit 1
  fi
  warn "allow_no_pvc_backup=true: will stream-verify pg_dump header/size to /dev/null (no persistent dump stored)."
fi

if [[ "$DRY_RUN" == "true" ]]; then
  log "=== DRY RUN COMPLETE ==="
  log "All pre-flight checks passed. Re-run with dry_run=false and confirmation='failback-${ENV}' to execute live failback."
  exit 0
fi

# ---------------------------------------------------------------------------
# Phase 1: Prepare & Safety Backup on GOLDDR
# ---------------------------------------------------------------------------
log "=== Phase 1: Suspend patroni-probe & take safety pg_dump on GOLDDR ==="

if oc_golddr get cronjob patroni-probe >/dev/null 2>&1; then
  oc_golddr patch cronjob patroni-probe -p '{"spec":{"suspend":true}}'
  log "Suspended cronjob/patroni-probe on GOLDDR."
else
  warn "cronjob/patroni-probe not found on GOLDDR/${NS}; continuing."
fi

DUMP_FILE="${DB_NAME}_failback_${TS}.dump"
DUMP_REMOTE_PATH="/backups/failback/${DUMP_FILE}"

if [[ -n "$BACKUP_POD" ]]; then
  log "Streaming pg_dump of '${DB_NAME}' from GOLDDR/${DR_PRIMARY} -> ${BACKUP_CLUSTER^^}/${BACKUP_POD}:${DUMP_REMOTE_PATH}..."
  if [[ "$BACKUP_CLUSTER" == "golddr" ]]; then
    oc_golddr exec "$DR_PRIMARY" -- sh -c 'pg_dump -U "${PATRONI_SUPERUSER_USERNAME:-postgres}" -Fc --no-owner --no-privileges "$1"' _ "$DB_NAME" \
      | oc_golddr exec -i "$BACKUP_POD" -- sh -c 'set -e; mkdir -p /backups/failback; cat > "$1"' _ "$DUMP_REMOTE_PATH"
    DUMP_INFO="$(oc_golddr exec "$BACKUP_POD" -- sh -c 'set -e; test -s "$1"; head -c 5 "$1" | grep -q "^PGDMP$"; ls -lh "$1"' _ "$DUMP_REMOTE_PATH")"
  else
    oc_golddr exec "$DR_PRIMARY" -- sh -c 'pg_dump -U "${PATRONI_SUPERUSER_USERNAME:-postgres}" -Fc --no-owner --no-privileges "$1"' _ "$DB_NAME" \
      | oc_gold exec -i "$BACKUP_POD" -- sh -c 'set -e; mkdir -p /backups/failback; cat > "$1"' _ "$DUMP_REMOTE_PATH"
    DUMP_INFO="$(oc_gold exec "$BACKUP_POD" -- sh -c 'set -e; test -s "$1"; head -c 5 "$1" | grep -q "^PGDMP$"; ls -lh "$1"' _ "$DUMP_REMOTE_PATH")"
  fi
  log "Verified in-cluster backup (PGDMP header OK): ${DUMP_INFO}"
else
  log "Stream-verifying pg_dump of '${DB_NAME}' from GOLDDR/${DR_PRIMARY} (allow_no_pvc_backup=true)..."
  DUMP_BYTES="$(oc_golddr exec "$DR_PRIMARY" -- sh -c 'pg_dump -U "${PATRONI_SUPERUSER_USERNAME:-postgres}" -Fc --no-owner --no-privileges "$1"' _ "$DB_NAME" | wc -c)"
  if [[ -z "$DUMP_BYTES" || "$DUMP_BYTES" -lt 64 ]]; then
    err "Safety pg_dump verification failed (size=${DUMP_BYTES:-0} bytes)."
    exit 1
  fi
  log "Verified pg_dump stream readability (${DUMP_BYTES} bytes; not persisted to runner)."
fi

# ---------------------------------------------------------------------------
# Phase 2: Re-initialize GOLD as Standby & Sync from GOLDDR
# ---------------------------------------------------------------------------
log "=== Phase 2: Re-initialize GOLD Patroni as Standby Leader & sync from GOLDDR ==="

log "Scaling GOLD statefulset/patroni to 0..."
oc_gold scale statefulset patroni --replicas=0
oc_gold wait --for=delete pod -l app.kubernetes.io/name=patroni --timeout=180s || true

log "Deleting stale GOLD Patroni PVCs..."
oc_gold delete pvc -l app.kubernetes.io/name=patroni --wait=true

log "Clearing GOLD patroni-leader & patroni-config initialize annotations and configuring standby_cluster -> patroni-master-golddr:${GOLD_TSC_PORT}..."
oc_gold annotate cm patroni-leader initialize- leader- optime- --overwrite 2>/dev/null || true
oc_gold annotate cm patroni-config initialize- --overwrite 2>/dev/null || true
patch_patroni_config_standby oc_gold "patroni-master-golddr" "$GOLD_TSC_PORT"

log "Scaling GOLD statefulset/patroni to ${DESIRED_REPLICAS}..."
oc_gold scale statefulset patroni --replicas="$DESIRED_REPLICAS"

wait_for_standby_sync "GOLD" oc_gold "$DESIRED_REPLICAS"

# ---------------------------------------------------------------------------
# Phase 3: Promote GOLD to Primary & Switch GSLB Traffic
# ---------------------------------------------------------------------------
log "=== Phase 3: Promote GOLD to Primary & switch traffic ==="

if [[ "$HAS_MIDAS" == "true" ]]; then
  log "Setting notouch=200 on GOLD deployment/midas-probe..."
  oc_gold set env deploy/midas-probe -c midas-probe notouch=200
  oc_gold rollout status deploy/midas-probe --timeout=180s
fi

log "Removing standby_cluster from GOLD patroni-config to promote GOLD to Primary..."
patch_patroni_config_promote oc_gold

wait_for_primary "GOLD" oc_gold

if [[ "$HAS_MIDAS" == "true" ]]; then
  if [[ -n "$GSLB_HOST" ]]; then
    log "Waiting up to ${GSLB_TIMEOUT_SEC}s for GSLB DNS (${GSLB_HOST}) to resolve to GOLD (${GSLB_GOLD_IP})..."
    gslb_deadline=$(( $(date +%s) + GSLB_TIMEOUT_SEC ))
    while true; do
      cur_ip="$(resolve_host_ip "$GSLB_HOST")"
      if [[ "$cur_ip" == "$GSLB_GOLD_IP" ]]; then
        log "GSLB DNS switched to GOLD (${cur_ip})."
        break
      fi
      if (( $(date +%s) > gslb_deadline )); then
        err "Timed out waiting for GSLB DNS (${GSLB_HOST}) to switch to ${GSLB_GOLD_IP} (last seen: '${cur_ip:-unresolved}')."
        err "GOLD is Primary and midas-probe still has notouch=200. GOLDDR has NOT been wiped yet — investigate GSLB before proceeding."
        exit 1
      fi
      log "  Current GSLB IP for ${GSLB_HOST}: '${cur_ip:-unresolved}' (waiting for ${GSLB_GOLD_IP})..."
      sleep 15
    done
  else
    warn "gslb_host env var not found on midas-probe; waiting 60s before removing notouch=200."
    sleep 60
  fi

  log "Removing notouch override from GOLD deployment/midas-probe..."
  oc_gold set env deploy/midas-probe -c midas-probe notouch-
  oc_gold rollout status deploy/midas-probe --timeout=180s
fi

# ---------------------------------------------------------------------------
# Phase 4: Re-sync GOLDDR as Standby Leader & Unsuspend patroni-probe
# ---------------------------------------------------------------------------
log "=== Phase 4: Re-sync GOLDDR as Standby Leader & unsuspend patroni-probe ==="

# Hard safety gate: verify GOLD is still Primary before wiping GOLDDR
wait_for_primary "GOLD" oc_gold

log "Scaling GOLDDR statefulset/patroni to 0..."
oc_golddr scale statefulset patroni --replicas=0
oc_golddr wait --for=delete pod -l app.kubernetes.io/name=patroni --timeout=180s || true

log "Deleting old GOLDDR Patroni PVCs..."
oc_golddr delete pvc -l app.kubernetes.io/name=patroni --wait=true

log "Clearing GOLDDR patroni-leader & patroni-config initialize annotations and configuring standby_cluster -> patroni-master-gold:${DR_TSC_PORT}..."
oc_golddr annotate cm patroni-leader initialize- leader- optime- --overwrite 2>/dev/null || true
oc_golddr annotate cm patroni-config initialize- --overwrite 2>/dev/null || true
patch_patroni_config_standby oc_golddr "patroni-master-gold" "$DR_TSC_PORT"

log "Scaling GOLDDR statefulset/patroni to ${DESIRED_REPLICAS}..."
oc_golddr scale statefulset patroni --replicas="$DESIRED_REPLICAS"

wait_for_standby_sync "GOLDDR" oc_golddr "$DESIRED_REPLICAS"

if oc_golddr get cronjob patroni-probe >/dev/null 2>&1; then
  oc_golddr patch cronjob patroni-probe -p '{"spec":{"suspend":false}}'
  log "Unsuspended cronjob/patroni-probe on GOLDDR."
fi

log "=== FAILBACK COMPLETE (${NS}) ==="
print_patroni_table "GOLD (Final)" oc_gold
print_patroni_table "GOLDDR (Final)" oc_golddr

if [[ -n "${GITHUB_STEP_SUMMARY:-}" ]]; then
  if [[ -n "$BACKUP_POD" ]]; then
    BACKUP_SUMMARY="\`${BACKUP_CLUSTER^^}/${BACKUP_POD}:${DUMP_REMOTE_PATH}\`"
  else
    BACKUP_SUMMARY="stream-verified (no PVC)"
  fi
  {
    echo "## Patroni Failback Complete (\`${NS}\`)"
    echo "- **Mode:** Live failback (\`GOLDDR -> GOLD\`)"
    echo "- **Safety Backup:** ${BACKUP_SUMMARY}"
    echo "- **Target Replicas:** \`${DESIRED_REPLICAS}\`"
    echo "- **GOLD -> GOLDDR TSC Port:** \`${GOLD_TSC_PORT}\` | **GOLDDR -> GOLD TSC Port:** \`${DR_TSC_PORT}\`"
    echo "- **Midas/GSLB Cutover:** \`${HAS_MIDAS}\`${GSLB_HOST:+ (\`${GSLB_HOST}\` -> \`${GSLB_GOLD_IP}\`)}"
  } >> "$GITHUB_STEP_SUMMARY"
fi
