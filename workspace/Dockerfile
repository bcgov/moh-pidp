FROM docker.io/nginxinc/nginx-unprivileged:stable

WORKDIR /app

COPY dist/apps/pidp /app/.
COPY fe-nginx-sb.conf /etc/nginx/conf.d/

EXPOSE 8080
