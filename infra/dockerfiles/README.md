# Dockerfiles README

This readme is to explain any Dockerfiles in this directory.

## Dockerfile.sonarscan.test

This Dockerfile is the base image for the scanner portion of SonarQube (updated to install `dotnet-sdk-10.0` for .NET 10). It is stored here as a backup in case it needs to be redeployed.

> **Legacy Note:** Historical Tekton/SonarScan pipelines referenced `dotnet-sonarscanner:6.0-bcgov` (.NET 6). All backend services now target **.NET 10** (`dotnet-sdk-10.0`), so if rebuilding this image, tag and reference it accordingly for .NET 10.