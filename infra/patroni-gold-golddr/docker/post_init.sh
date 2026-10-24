#!/bin/bash
set -e

echo "Running post_init.sh..."

if [ -n "$APP_USER" ] && [ -n "$APP_PASSWORD" ]; then
    DB="${APP_DATABASE:-$APP_USER}"
    echo "Creating application user '$APP_USER' and database '$DB'..."
    psql -v ON_ERROR_STOP=1 --username "postgres" <<-EOSQL
        DO \$\$
        BEGIN
            IF NOT EXISTS (SELECT FROM pg_catalog.pg_roles WHERE rolname = '${APP_USER}') THEN
                CREATE ROLE "${APP_USER}" WITH LOGIN ENCRYPTED PASSWORD '${APP_PASSWORD}';
            ELSE
                ALTER ROLE "${APP_USER}" WITH ENCRYPTED PASSWORD '${APP_PASSWORD}';
            END IF;
        END
        \$\$;

        SELECT 'CREATE DATABASE "${DB}" OWNER "${APP_USER}"'
        WHERE NOT EXISTS (SELECT FROM pg_database WHERE datname = '${DB}')\gexec

        GRANT ALL PRIVILEGES ON DATABASE "${DB}" TO "${APP_USER}";
EOSQL
    echo "Application user and database configured successfully."
fi
