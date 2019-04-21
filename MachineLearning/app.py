import requests
from sys import argv
import json

script, mode, url = argv

r = requests.get(url)

for o in r.json():
    print(o)

if mode == 'train':
    print('train mode')
else:
    print('predict mode')



