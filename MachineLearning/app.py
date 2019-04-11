import requests
import json


r = requests.get('http://localhost:4000/api/Values')
for value in json.loads(r.content):
    print(value)

from sklearn.datasets import load_wine

wine = load_wine()

print(wine.feature_names)
print(wine.target)
print(wine.data)
