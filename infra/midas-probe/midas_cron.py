#!/usr/local/bin/python

# This user the main.py midas webservice (in JSON format) to discover the health of all the services.
# It was designed to work on Gold and GoldDR and has several conditions to process depending on a fail or failback.

from os import environ
import requests
import time
import sys
from datetime import datetime, timedelta

def get_json_value_from_url(url, index):
  try:
    response = requests.get(url)
    if response.status_code == 200:
      json_data = response.json()
      if index in json_data:
        return json_data[index]
      else:
        return f"Index '{index}' not found in JSON response"
    else:
      return f"Failed to retrieve data from the URL. Status Code: {response.status_code}"
  except Exception as e:
      return f"An error occurred: {e}"


def check_service():
  # do a self check to determine my location "what cluster am I running on?".
  where_am_i = environ.get('cluster')
  return_code = "200"
  print(f"I'm on: {where_am_i}")

  # contact an json endpint called "midas" and pull the value for key "cluster"
  who_is_active = get_json_value_from_url(f'https://{environ.get("application_url")}/midas/json','cluster')
  if who_is_active:
    print(f"The GSLB points traffic to: {who_is_active}")
    if where_am_i.upper()=="GOLD" and who_is_active.upper()=="GOLD":  
      return_code = "200"
    elif where_am_i.upper()=="GOLD" and who_is_active.upper()=="GOLDDR":
      print("The GSLB is point traffic to golddr")
      return_code = "500"
  else:
    return_code = "500"
  
  return return_code


if __name__ == '__main__':
  while True:
    print("-------------------------------------")
    print("Started Timer")
    time.sleep(60)
    return_code = check_service()
    # The console was updating every 30 minutes...this forces a console output right away.
    sys.stdout.flush()