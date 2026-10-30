FROM postgres:16-bookworm

LABEL maintainer="BC Gov MoH PIDP"
LABEL description="Patroni HA PostgreSQL 16 container for OpenShift"

# Install Python 3, pip, Patroni with Kubernetes DCS support (v3.3+), and utilities needed by OpenShift probes
RUN apt-get update && apt-get install -y --no-install-recommends \
    curl \
    jq \
    procps \
    python3 \
    python3-pip \
    python3-psycopg2 \
    python3-ydiff \
    && rm -rf /var/lib/apt/lists/* \
    && pip3 install --no-cache-dir --break-system-packages --only-binary :all: "patroni[kubernetes]==4.1.5"

# Set environment variables for Postgres and Patroni
ENV HOME=/home/postgres \
    PGDATA=/home/postgres/pgdata/pgroot/data \
    PATH="/usr/lib/postgresql/16/bin:${PATH}"

# Copy bootstrap init script
COPY post_init.sh /usr/share/scripts/patroni/post_init.sh

# OpenShift arbitrary user (UID >= 10000) & group 0 support
RUN mkdir -p /home/postgres/pgdata /home/postgres/scripts /usr/share/scripts/patroni && \
    chmod 755 /usr/share/scripts/patroni/post_init.sh && \
    chown -R 1001:0 /home/postgres && \
    chmod -R 775 /home/postgres && \
    chmod 664 /etc/passwd

USER 1001
WORKDIR /home/postgres
EXPOSE 5432 8008

CMD ["patroni"]
