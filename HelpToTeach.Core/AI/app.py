##from RestClient import Client
##
##client = Client(port=4000)
##response = client.get('api/Students')
import sys
arg = sys.argv[1]
if arg == 'train':
	print('Training mode')
else:
	print("Prediction mode")