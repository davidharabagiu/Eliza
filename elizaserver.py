import socket
import threading


class ClientHandler(threading.Thread):
    running = False

    def __init__(self, client, address):
        super(ClientHandler, self).__init__()
        self.client = client
        self.address = address

    def run(self):
        self.running = True
        user_name = self.client.recv(1024)
        print user_name + ' has connected'
        while self.running:
            msg = self.client.recv(1024)
            if len(msg) == 0:
                print user_name + ' has disconnected'
                break
            print '[' + user_name + ']: ' + msg


class Server(threading.Thread):
    running = False

    def __init__(self, host, port):
        super(Server, self).__init__()
        self.clients = []
        self.socket = socket.socket()
        self.socket.bind((host, port))
        self.socket.listen(5)

    def run(self):
        self.running = True
        while self.running:
            client, address = self.socket.accept()
            new_client = ClientHandler(client, address)
            new_client.start()
            self.clients.append(new_client)
        for c in self.clients:
            c.running = False
        for c in self.clients:
            c.join()


if __name__ == '__main__':
    server = Server('elizaserver.ddns.net', 9999)
    server.start()
    server.join()
