from RestClient import Client

client = Client(port=4000)
response = client.get('api/Students')

print(response)
