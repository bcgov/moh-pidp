FROM python:3.11-slim-buster

RUN apt-get update && \
    apt-get -y upgrade && \
    apt-get -y install \
      python3-pip \
      curl \
      vim && \
    rm -rf /var/lib/apt/list/*

COPY requirements.txt requirements.txt
RUN pip3 install --no-cache-dir -r requirements.txt

SHELL ["/bin/bash", "-c"]

COPY main.py main.py

EXPOSE 8080

ENTRYPOINT ["python", "main.py"]