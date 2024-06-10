# Midas Probe - Dashboard for Monitoring Silver Services Failover

This service is used to monitor several services running on Openshift (OCP). The main purpose of this dashboard is to:
1. Display the results of the services (running or not) based on the availability of pods for that service. In essence it depends on the OCP deployment readiness probes. It runs on the SILVER cluster and if any of the services monitored by Midas does not have available pods, Midas will return a 500 code for a failover.

## How does it work
The main function of this service is main.py. This python script uses a kubernetes library to look for any services with the tag: "midas=touch". For each of those services it determines if there's active pods for that service. If there are not, it will fail with a 500 code and display the list of service showing which one(s) failed.

Note: There's a environment variable that you can set "notouch" that will allow you to override the results. This is for testing purposes to allow you to manually control the toggle.  Accepted values for notouch are 200 or 500 depending on the return code you wish to force.

## Pipeline
There is a pipeline that builds Midas Probe:
1. uses the docker-compose to build the docker image for release into the OCP imagestream on the -tools namespace.
2. Helm Install/Upgrade Midas Probe: This does a helm install/upgrade. 