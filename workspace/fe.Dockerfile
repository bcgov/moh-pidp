FROM public.ecr.aws/bitnami/nginx:1.21

WORKDIR /app

COPY dist/apps/pidp /app/.

EXPOSE 8080