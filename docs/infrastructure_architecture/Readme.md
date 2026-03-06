# Infrastructure applications

Infrastructure applications are deployed either as helm chart or openshift template on the OpenShift platform. Infrastructure applications include backup-container, fluentbit, mailhog, metabase,  midas-probe, patroni, rabitmq are designed for one-time setp and deployment and require updates only once a year.

## Backup Container

[BCGov Backup Container](https://developer.gov.bc.ca/docs/default/component/platform-developer-docs/docs/database-and-api-management/database-backup-best-practices/) is a trusted backup utility tool, designed by BCGOV DevOps team build for developers working on the BCGov's OpenShift platform. It connects to your database to perform a full and complete backup of data  and/or to perform a test recovery of the most recent backup. For the backup process, it uses pg_dump command every single hour and dumps the pidp_* database into a single backup file. The backup files are stored on an nfs-file-backup PVC (persistent volume claim) on OpenShift platform. You can modify the backup cycle and schedule in the [backup-container-conf](infra\backup-container\chart\templates\configmap.yaml)

## Fluentbit

[Fluent-bit](https://docs.fluentbit.io/manual/about/what-is-fluent-bit) is basically a `log forwarder` tool that can be run as a sidecar container (a docker image) in each pod containing our apps, although it can also be deployed as a stand-alone service (servicing multiple apps). Fluent-bit can forward logs to lots of different outputs for example, HTTP, Opensearch, Slack, AWS Lamda and a lot more (see: https://docs.fluentbit.io/manual/pipeline/outputs). 

We added a Fluent-bit container in webapi, nginx and midas deplyments/applications to collect/process/monitor Logs inside the apps and alert the PIDP team in a slack channel if certain keywords, regular expressions, etc. are matched in the log stream.

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

It checks if there's a failover incident from GOLD to GOLDDR. This service is used to monitor the readiness and health status of specified services on Openshift such as patroni. It has two primary purposes:
1.	It will display the results of the services (if its running or not) based on the availability of pods for that service. In essence it depends on the OCP deployment readiness probes.
2.	It acts as a keepalive for the Server Load Balancer (GSLB). It runs on the Gold cluster and if any of the services monitored by Midas does not have available pods, Midas will return a 500 code to the load balancer and load balancer switch the DNS from pointing to Gold to GoldDR  for a failover. You need create a new slack channel to receive the notifications from midas and update the value of slack_webhook variable in midas-probe configmap if you
Basically, Midas checks the IP address of the active cluster or GSLB. If it returns 142.34.64.4 meaning there's a failover and a manual failback from GOLD to GOLDDR is required.

# Patroni

Patroni is an open-source tool written in Python designed for high availability (HA) and automated management of PostgreSQL clusters. Please read [BCGOV database and API management](https://developer.gov.bc.ca/docs/default/component/platform-developer-docs/docs/database-and-api-management/postgres-how-to) documentation to become familiar with patroni tool. Patroni manages its high availability features by using a combination of OpenShift's built-in functionality and the Patroni management software built into the image used to run each database pod. Patroni is managed primarily using the [patronictl](https://patroni.readthedocs.io/en/latest/README.html) command line interface. You can use the terminal on the OpenShift web console or you can use oc ssh to access the command line of any pod in your Patroni cluster to run patronictl. Patroni has three pods: one pod is the master pod or leader, which recieves all communication from the application and two other pods are replica pods, which are in read-only mode and can only be updated by syncing with the master.

# RabbitMQ

[RabbitMQ](https://www.rabbitmq.com/) has been added to the webapi application as the Message Broker to efficiently process message queues, quarante message delivery, ensuring seamless communication between microservices, distributed applications, and large-scale systems. It is based on the AMQP (Advanced Message Queuing Protocol) and is known for its reliability, flexibility, and ease of configuration. As an example, whenevr a party email gets updated, we persist the changes in a message queue using party-email-updated-bc-provider-queue.

[MassTransit](https://masstransit.io/documentation/configuration/transports/rabbitmq) is an open-source distributed application framework for .NET that provides a consistent abstraction on top of the supported message transports.
