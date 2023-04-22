# Dockerfiles README

This readme is to explain any Dockerfiles in this directory.

## Dockerfile.sonarscan.test

This Dockerfile is the base image for the scanner portion of SonarQube. It is stored here as a backup in case it needs to be redeployed. 

There is a slight bug with this image. The dotnet-sonarscanner images for webapi and plr-intake rely on the dotnet-sonarscanner:6.0-bcgov tag. 

This image was missing, and thus, this Dockerfile must be built and tagged as dotnet-sonarscanner:6.0-bcgov to ensure that the SonarScan for webapi and plr-intake images continue to function. This was a quick fix to ensure that we could actually use SonarScan for webapi and plr-intake. 