import requests
import json


r = requests.get('http://localhost:4000/api/Values')


for value in json.loads(r.content):
    print(value)
