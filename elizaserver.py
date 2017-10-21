import socket
import threading
import serverrequests
import dbaccess


clients = {}
clients_logged_in = {}


class ClientHandler(threading.Thread):
    running = False
    logged_in = False
    username = ''
    fileTransferMode = False
    expectedFileSize = 0
    fileData = ''
    fileTransferBytesReceived = 0

    def __init__(self, client, address):
        super(ClientHandler, self).__init__()
        self.client = client
        self.address = address

    def process_request(self, request):
        if self.fileTransferMode:
            self.fileData += request[0]
            self.fileTransferBytesReceived += len(request[0])
            if self.fileTransferBytesReceived >= self.expectedFileSize:
                self.fileTransferMode = False
            return 'Bytes received successfully'
        if request[0].lower() == 'register':
            if len(request) < 3:
                return 'Invalid request parameters'
            return serverrequests.register(request[1], request[2])
        elif request[0].lower() == 'login':
            if len(request) < 3:
                return 'Invalid request parameters'
            status = serverrequests.login(request[1], request[2], clients_logged_in, clients[self.address])
            if status == 'Login successful':
                self.logged_in = True
                self.username = request[1]
            return status
        elif request[0].lower() == 'logout':
            if self.username == '':
                return 'User not logged in'
            else:
                status = serverrequests.logout(self.username, clients_logged_in)
                if status == 'Logout successful':
                    self.logged_in = False
                return status
        elif request[0].lower() == 'sendmsg':
            if len(request) < 3:
                return 'Invalid request parameters'
            return serverrequests.sendmsg(self.username, request[2:], request[1], clients_logged_in)
        elif request[0].lower() == 'queryonline':
            if len(request) < 2:
                return 'Invalid request parameters'
            return serverrequests.queryonline(request[1], clients_logged_in)
        elif request[0].lower() == 'friendrequest':
            if len(request) < 2:
                return 'Invalid request parameters'
            return serverrequests.friendrequest(self.username, request[1], clients_logged_in)
        elif request[0].lower() == 'acceptfriendrequest':
            if len(request) < 2:
                return 'Invalid request parameters'
            return serverrequests.acceptfriendrequest(request[1], self.username, clients_logged_in)
        elif request[0].lower() == 'queryfriendship':
            if len(request) < 2:
                return 'Invalid request parameters'
            return serverrequests.queryfriendship(self.username, request[1], clients_logged_in)
        elif request[0].lower() == 'queryfriendrequestsent':
            if len(request) < 2:
                return 'Invalid request parameters'
            return serverrequests.queryfriendrequestsent(self.username, request[1], clients_logged_in)
        elif request[0].lower() == 'queryfriendrequestreceived':
            if len(request) < 2:
                return 'Invalid request parameters'
            return serverrequests.queryfriendrequestreceived(self.username, request[1], clients_logged_in)
        elif request[0].lower() == 'unfriend':
            if len(request) < 2:
                return 'Invalid request parameters'
            return serverrequests.unfriend(self.username, request[1], clients_logged_in)
        elif request[0].lower() == 'blockuser':
            if len(request) < 2:
                return 'Invalid request parameters'
            return serverrequests.blockuser(self.username, request[1], clients_logged_in)
        elif request[0].lower() == 'unblockuser':
            if len(request) < 2:
                return 'Invalid request parameters'
            return serverrequests.unblockuser(self.username, request[1], clients_logged_in)
        elif request[0].lower() == 'queryblock':
            if len(request) < 3:
                return 'Invalid request parameters'
            elif request[1] == '0':
                return serverrequests.queryblock(self.username, self.username, request[2], clients_logged_in)
            elif request[1] == '1':
                return serverrequests.queryblock(self.username, request[2], self.username, clients_logged_in)
            else:
                return 'Invalid request parameters'
        elif request[0].lower() == 'queryfriends':
            return serverrequests.queryfriends(self.username, clients_logged_in)
        elif request[0].lower() == 'changepassword':
            if len(request) < 3:
                return 'Invalid request parameters'
            return serverrequests.changepassword(self.username, request[1], request[2], clients_logged_in)
        elif request[0].lower() == 'querydescription':
            if len(request) < 2:
                return 'Invalid request parameters'
            return serverrequests.querydescription(self.username, request[1], clients_logged_in)
        elif request[0].lower() == 'setdescription':
            if len(request) < 2:
                return 'Invalid request parameters'
            return serverrequests.setdescription(self.username, request[1:], clients_logged_in)
        elif request[0].lower() == 'queryprofilepic':
            if len(request) < 2:
                return 'Invalid request parameters'
            return serverrequests.queryprofilepic(self.username, request[1], clients_logged_in)
        elif request[0].lower() == 'setprofilepic':
            return serverrequests.setprofilepic(self.username, self.fileData, clients_logged_in)
        elif request[0].lower() == 'filetransfer':
            if len(request) < 2:
                return 'Invalid request parameters'
            if self.username not in clients_logged_in.keys():
                return 'Not logged in'
            self.expectedFileSize = int(request[1])
            self.fileTransferMode = True
            self.fileData = ''
            self.fileTransferBytesReceived = 0
            return 'Initialized file transfer mode successfully'
        else:
            return 'Unknown request'

    def run(self):
        self.running = True
        print self.address, 'has connected'
        while self.running:
            try:
                request = self.client.recv(1024)
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
    dbaccess.set_all_users_offline()

    server = Server('0.0.0.0', 9999, 9998)
    server.start()

    while True:
        prompt = raw_input()
        if prompt == 'exit':
            break

    server.running = False
    server.join()
