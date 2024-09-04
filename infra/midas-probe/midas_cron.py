#!/usr/local/bin/python

# This user the main.py midas webservice (in JSON format) to discover the health of all the services.
# It was designed to work on Gold and GoldDR and has several conditions to process depending on a fail or failback.

from os import environ
import redis
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


def get_redis_client():
  # Connect to Redis server
  redis_host = f'{environ.get("appPrefix")}-redis-main'
  redis_port = 6379  # Replace with your Redis server port
  redis_db = 3  # Replace with your Redis database number
  redis_client = redis.StrictRedis(host=redis_host, port=redis_port, db=redis_db)
  return redis_client


def get_key_value(key):
  redis_client = get_redis_client()

  # Check if the key value exists and if so return it
  if redis_client.exists(key):
    inhand_value = redis_client.get(key).decode('utf-8')
    return inhand_value
  else:
    return None


def set_key_value(key, value):
  redis_client = get_redis_client()
  redis_client.set(key, value)


def unset_key(key):
  redis_client = get_redis_client()
  redis_client.delete(key)


def rebuild_cache(url,params):
  print('Call the external rebuild cache function')
  try:
    # The timeout param along with the except forces a trigger of the event but not wait for a response.
    requests.get(url, params)
  except:
    raise

def check_service():
  # do a self check to determine my location "what cluster am I running on?".
  where_am_i = environ.get('cluster')
  print(f"I'm on: {where_am_i}")

  # contact an json endpint called "midas" and pull the value for key "cluster"
  who_is_active = get_json_value_from_url(f'https://{environ.get("appPrefix")}-mydomain/midas/json','cluster')
  print(f"Who has the ball: {who_is_active}")

  #This can be set to override the external call and force a cluster.  Choose either "gold" or "golddr" to override
  #who_is_active = "gold"

  # Contact the redis database and see if there's a key "inhand" and find it's value.
  inhand_value = get_key_value('inhand')
  print(f"Inhand value is: {inhand_value}")

  now = datetime.now().replace(microsecond=0)
  print(f"Now is: {str(now)}")

  cache_refresh_endpoint = f"http://{environ.get('appPrefix')}-mydomain/cache-refresh/"
  cache_refresh_params = {'processAsyncFlag': 'true'}

  if inhand_value is not None:
    if where_am_i == who_is_active:
      print("I've got the ball")
      inhand_datetime_object = datetime.strptime(inhand_value, "%Y-%m-%d %H:%M:%S")
      five_mins_ago = now - timedelta(minutes=5)
      if inhand_datetime_object < five_mins_ago:
        print("The inhand value is old. Rebuild the cache.")
        set_key_value('inhand', str(now))
        rebuild_cache(cache_refresh_endpoint,cache_refresh_params)
      else:
        # this is the normal use case - cache is being maintained
        print(f"All is well, update inhand value to: {str(now)}")
        set_key_value('inhand', str(now))
    else:
      # something has gone wrong, I should not be inhand
      print("I do not have the ball, but I have an inhand value. Purge it.")
      unset_key('inhand')
  else:
    if where_am_i == who_is_active:
      # there's no inhand entry in the DB
      print("I have the ball, but there's no inhand. Must have recently switched to me. Rebuild the cache and set inhand.")
      set_key_value('inhand', str(now))
      rebuild_cache(cache_refresh_endpoint,cache_refresh_params)
    else:
      print("I do not have the ball and there's no inhand - the other has the ball. Standing by.")

if __name__ == '__main__':
  while True:
    print("-------------------------------------")
    print("Started Timer")
    time.sleep(60)
    check_service()
    # The console was updating every 30 minutes...this forces a console output right away.
    sys.stdout.flush()