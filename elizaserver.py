import socket
import threading
import serverrequests


clients = {}


class ClientHandler(threading.Thread):
    running = False

    def __init__(self, client, address):
        super(ClientHandler, self).__init__()
        self.client = client
        self.address = address

    @staticmethod
    def process_request(request):
        if request[0] == 'register':
            if len(request) < 3:
                return 'Invalid request parameters'
            return serverrequests.register(request[1], request[2])
        else:
            return 'Unknown request'

    def run(self):
        self.running = True
        print self.address, 'has connected'
        while self.running:
            try:
                request = self.client.recv(1024).lower()
                if len(request) == 0:
                    print self.address, 'has disconnected'
                    break
                print '[' + str(self.address) + ']: ' + request
                response = self.process_request(request.split())
                self.client.sendall(response)
            except socket.error as err:
                print err
                break


class Server(threading.Thread):
    running = False

    def __init__(self, host, port1, port2):
        super(Server, self).__init__()
        self.clients = []
        self.socket1 = socket.socket()
        self.socket1.bind((host, port1))
        self.socket1.listen(5)
        self.socket2 = socket.socket()
        self.socket2.bind((host, port2))
        self.socket2.listen(5)

    def run(self):
        self.running = True
        while self.running:
            client, address = self.socket1.accept()
            client2, address2 = self.socket2.accept()
            if address[0] != address2[0]:
                continue
            clients[address] = (client2, '')
            new_client = ClientHandler(client, address)
            new_client.start()
            self.clients.append(new_client)
        for c in self.clients:
            c.running = False
        for c in self.clients:
            c.join()


if __name__ == '__main__':
    server = Server('elizaserver.ddns.net', 9999, 9998)
    server.start()

    while True:
        prompt = raw_input()
        if prompt == 'exit':
            break

    server.running = False
    server.join()
