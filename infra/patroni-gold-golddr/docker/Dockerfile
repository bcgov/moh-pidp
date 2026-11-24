ARG POSTGRES_VERSION=16
FROM postgres:${POSTGRES_VERSION}-bookworm

ARG POSTGRES_VERSION=16
ARG PATRONI_VERSION=4.1.5

LABEL maintainer="BC Gov MoH PIDP"
LABEL description="Patroni HA PostgreSQL ${POSTGRES_VERSION} container for OpenShift"

# Install Python 3, pip, Patroni with Kubernetes DCS support, and utilities needed by OpenShift probes
# Optional corporate proxy CA can be passed at build time without baking it into the image:
#   docker build --secret id=ca_cert,src=../moh-pidp_local_overlay/company.crt ...
RUN --mount=type=secret,id=ca_cert \
    apt-get update && apt-get install -y --no-install-recommends \
    ca-certificates \
    curl \
    jq \
    procps \
    python3 \
    python3-pip \
    python3-psycopg2 \
    && rm -rf /var/lib/apt/lists/* \
    && if [ -f /run/secrets/ca_cert ]; then \
         cat /etc/ssl/certs/ca-certificates.crt > /tmp/pip-ca.crt && \
         printf '\n' >> /tmp/pip-ca.crt && \
         tr -d '\r' < /run/secrets/ca_cert >> /tmp/pip-ca.crt && \
         PIP_CERT=/tmp/pip-ca.crt pip3 install --no-cache-dir --break-system-packages --only-binary :all: --no-binary ydiff "patroni[kubernetes]==${PATRONI_VERSION}" && \
         rm -f /tmp/pip-ca.crt; \
       else \
         pip3 install --no-cache-dir --break-system-packages --only-binary :all: --no-binary ydiff "patroni[kubernetes]==${PATRONI_VERSION}"; \
       fi

# Set environment variables for Postgres and Patroni
ENV HOME=/home/postgres \
    PGDATA=/home/postgres/pgdata/pgroot/data \
    PATH="/usr/lib/postgresql/${POSTGRES_VERSION}/bin:${PATH}"

# Copy bootstrap init script
COPY post_init.sh /usr/share/scripts/patroni/post_init.sh

# OpenShift arbitrary user (UID >= 10000) & group 0 support
RUN mkdir -p /home/postgres/pgdata /home/postgres/scripts /usr/share/scripts/patroni && \
    sed -i 's/\r$//' /usr/share/scripts/patroni/post_init.sh && \
    chmod 755 /usr/share/scripts/patroni/post_init.sh && \
    chown -R 1001:0 /home/postgres && \
    chmod -R 775 /home/postgres && \
    chmod 664 /etc/passwd

USER 1001
WORKDIR /home/postgres
EXPOSE 5432 8008

ENTRYPOINT []
CMD ["/usr/bin/entrypoint.sh"]
