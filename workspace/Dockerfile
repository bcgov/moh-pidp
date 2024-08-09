FROM public.ecr.aws/bitnami/nginx:1.21

WORKDIR /app

COPY apps/pidp /app/.
COPY fe-nginx-sb.conf /opt/bitnami/nginx/conf/server_blocks/

EXPOSE 8080
