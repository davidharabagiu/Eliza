import socket
import threading
import serverrequests


clients = {}
clients_logged_in = {}


class ClientHandler(threading.Thread):
    running = False
    logged_in = False
    username = ''

    def __init__(self, client, address):
        super(ClientHandler, self).__init__()
        self.client = client
        self.address = address

    def process_request(self, request):
        if request[0] == 'register':
            if len(request) < 3:
                return 'Invalid request parameters'
            return serverrequests.register(request[1], request[2])
        elif request[0] == 'login':
            if len(request) < 3:
                return 'Invalid request parameters'
            status = serverrequests.login(request[1], request[2], clients_logged_in, clients[self.address])
            if status == 'Login successful':
                self.logged_in = True
                self.username = request[1]
            return status
        elif request[0] == 'logout':
            if self.username == '':
                return 'User not logged in'
            else:
                status = serverrequests.logout(self.username, clients_logged_in)
                if status == 'Logout successful':
                    self.logged_in = False
                return status
        elif request[0] == 'sendmsg':
            if len(request) < 3:
                return 'Invalid request parameters'
            return serverrequests.sendmsg(self.username, request[2], request[1], clients_logged_in)
        elif request[0] == 'queryonline':
            if len(request) < 2:
                return 'Invalid request parameters'
            return serverrequests.queryonline(request[1], clients_logged_in)
        else:
            return 'Unknown request'

    def run(self):
        self.running = True
        print self.address, 'has connected'
        while self.running:
            try:
                request = self.client.recv(1024).lower()
                if len(request) == 0:
                    serverrequests.logout(self.username, clients_logged_in)
                    del clients[self.address]
                    print self.address, 'has disconnected'
                    break
                print '[' + str(self.address) + ']: ' + request
                response = self.process_request(request.split())
                self.client.sendall(response)
            except socket.error:
                serverrequests.logout(self.username, clients_logged_in)
                del clients[self.address]
                print self.address, 'has disconnected'
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
            clients[address] = client2
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
