import http.client


class Client:
    def __init__(self, host='localhost', port=5000):
        self.connection = http.client.HTTPConnection(host, port)

    def get(self, url=''):
        self.connection.connect()
        self.connection.request(method='GET', url=url)
        response = self.connection.getresponse()
        self.connection.close()
        return response

    def post(self, url='', body=None):
        self.connection.connect()
        self.connection.request(method='POST', url=url, body=body)
        response = self.connection.getresponse()
        self.connection.close()
        return response
