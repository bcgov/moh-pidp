ARG NGINX_VERSION=stable
FROM docker.io/nginx/nginx-unprivileged:${NGINX_VERSION}

WORKDIR /app

COPY dist/apps/pidp /app/.
COPY fe-nginx-sb.conf /etc/nginx/conf.d/default.conf

EXPOSE 8080
