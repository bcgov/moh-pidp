# Infrastructure applications

Infrastructure applications are deployed either as helm chart or openshift template on the OpenShift platform. Infrastructure applications include backup-container, fluentbit, mailhog, metabase,  midas-probe, patroni, rabitmq are designed for one-time setp and deployment and require updates only once a year.

## Backup Container

[BCGov Backup Container](https://developer.gov.bc.ca/docs/default/component/platform-developer-docs/docs/database-and-api-management/database-backup-best-practices/) is a trusted backup utility tool, designed by BCGOV DevOps team build for developers working on the BCGov's OpenShift platform. It takes a full and complete backup of data into a single database dump file using pg_dump command every hour for the pidp_* databases. The backup files are stored on an nfs-file-backup PVC (persistent volume claim) on OpenShift platform. You can modify the backup cycle and schedule in the [backup-container-conf](infra\backup-container\chart\templates\configmap.yaml)

## Mailhog

MailHog is an email testing tool for developers:

- Configure your application to use MailHog for SMTP delivery
- View messages in the web UI, or retrieve them with the JSON API
- Optionally release messages to real SMTP servers for delivery

## Metabase

Metabase is an open-source business intelligence (BI) platform designed to make data exploration and visualization simple for everyone, from non-technical users to experienced data analysts. It provides tools to query, visualize, and share insights without requiring deep technical expertise, while still offering advanced capabilities for SQL users.

It supports interactive dashboards, embeddable charts, and customizable reports, enabling teams to monitor KPIs, track trends, and make data-driven decisions efficiently. Users can connect Metabase to various databases and data warehouses, then explore data through a GUI query builder or by writing custom SQL queries. Key Capabilities:

* Data Visualization: Build charts, tables, and maps directly from your data.
* Interactive Dashboards: Combine multiple visualizations with filters for dynamic exploration.
* SQL & GUI Querying: Use a visual query builder or write raw SQL for complex analysis.
* Alerts & Notifications: Set up automated alerts when data meets certain conditions.
* Data Modeling: Define metrics, annotate charts, and create reusable query templates.
* Embedding & Sharing: Publish dashboards internally or embed them into applications.

## Midas-probe

It checks if there's a failover incident from GOLD to GOLDDR. You need create a new slack channel to receive the notifications from midas and update the value of slack_webhook variable in midas-probe configmap if you
Firstly and most importantly, you need to check the current IP address of OneHealthID application. If it returns 142.34.64.4, it means there's a failover and a manual failback from GOLD to GOLDDR is required.

# Patroni

Please read [BCGOV database and API management](https://developer.gov.bc.ca/docs/default/component/platform-developer-docs/docs/database-and-api-management/postgres-how-to) documentation to become familiar with patroni tool. Patroni manages its high availability features by using a combination of OpenShift's built-in functionality and the Patroni management software built into the image used to run each database pod. Patroni is managed primarily using the [patronictl](https://patroni.readthedocs.io/en/latest/README.html) command line interface. You can use the terminal on the OpenShift web console or you can use oc ssh to access the command line of any pod in your Patroni cluster to run patronictl.

# RabbitMQ

[RabbitMQ](https://www.rabbitmq.com/) has been added to the webapi application as the Message Broker to efficiently process message queues, quarante message delivery, ensuring seamless communication between microservices, distributed applications, and large-scale systems. It is based on the AMQP (Advanced Message Queuing Protocol) and is known for its reliability, flexibility, and ease of configuration. As an example, whenevr a party email gets updated, we persist the changes in a message queue using party-email-updated-bc-provider-queue.

[MassTransit](https://masstransit.io/documentation/configuration/transports/rabbitmq) is an open-source distributed application framework for .NET that provides a consistent abstraction on top of the supported message transports.
