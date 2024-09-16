FROM firely/server
CMD ["/bin/sh"]

ENV APP_UID=1650
ENV APP_USER='1010800000'
ENV APP_GROUP='1010800000'

WORKDIR /app
USER root

RUN addgroup  --gid=$APP_UID $APP_GROUP && adduser  --uid=$APP_UID  --ingroup=$APP_GROUP  --disabled-password $APP_USER


RUN  chown -R $APP_USER:$APP_GROUP /app
COPY --chown=$APP_USER:$APP_GROUP . .

#RUN chmod 777 /app/data
USER $APP_USER:$APP_GROUP

ENTRYPOINT ["dotnet", "Firely.Server.dll"]

