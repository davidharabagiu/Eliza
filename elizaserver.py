# import mysql.connector
import socket
import threading


connection_config = {
    'user': 'root',
    'password': 'oprisa',
    'host': '127.0.0.1',
    'database': 'eliza',
}


class ClientHandler(threading.Thread):
    running = False

    def __init__(self, client, address):
        super(ClientHandler, self).__init__()
        self.client = client
        self.address = address

    def run(self):
        self.running = True
        while self.running:
            msg = self.client.recv(1024)
            print '[' + self.address[0] + ':' + str(self.address[1]) + ']: ' + msg


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
    # connection = mysql.connector.connect(**connection_config)
    # print 'Connection to database established successfully'

    #server = SocketServer.TCPServer((host, port), serverhandler.RequestHandler)
    #server.serve_forever()

    server = Server('localhost', 9999)
    server.start()
    server.join()


    # connection.close()
