import socket
import threading
import serverrequests
import dbaccess
import requeststatus
import utils


clients = {}
clients_logged_in = {}


class ClientHandler(threading.Thread):
    def __init__(self, client, address):
        super(ClientHandler, self).__init__()
        self.client = client
        self.address = address
        self.logged_in = False
        self.username = ''
        self.fileTransferMode = False
        self.expectedFileSize = 0
        self.fileData = ''
        self.fileTransferBytesReceived = 0
        self.running = False

    def process_request(self, request):
        if self.fileTransferMode:
            self.fileData += request[0]
            self.fileTransferBytesReceived += len(request[0])
            if self.fileTransferBytesReceived >= self.expectedFileSize:
                self.fileTransferMode = False
            return requeststatus.STATUS_SUCCESS
        if request[0].lower() == 'register':
            if len(request) < 3:
                return requeststatus.STATUS_INVALID_REQUEST_PARAMETERS
            return serverrequests.register(request[1], request[2])
        elif request[0].lower() == 'login':
            if len(request) < 3:
                return requeststatus.STATUS_INVALID_REQUEST_PARAMETERS
            status = serverrequests.login(request[1], request[2], clients_logged_in, clients[self.address])
            if status == requeststatus.STATUS_SUCCESS:
                self.logged_in = True
                self.username = request[1]
            return status
        elif request[0].lower() == 'logout':
            status = serverrequests.logout(self.username, clients_logged_in)
            if status == requeststatus.STATUS_SUCCESS:
                self.logged_in = False
            return status
        elif request[0].lower() == 'sendmsg':
            if len(request) < 4:
                return requeststatus.STATUS_INVALID_REQUEST_PARAMETERS
            return serverrequests.sendmsg(self.username, request[1], request[3:], request[2], clients_logged_in)
        elif request[0].lower() == 'getmessages':
            if len(request) < 3:
                return requeststatus.STATUS_INVALID_REQUEST_PARAMETERS
            return serverrequests.getmessages(request[1], request[2])
        elif request[0].lower() == 'queryonline':
            if len(request) < 2:
                return requeststatus.STATUS_INVALID_REQUEST_PARAMETERS
            return serverrequests.queryonline(self.username, request[1], clients_logged_in)
        elif request[0].lower() == 'friendrequest':
            if len(request) < 2:
                return requeststatus.STATUS_INVALID_REQUEST_PARAMETERS
            return serverrequests.friendrequest(self.username, request[1], clients_logged_in)
        elif request[0].lower() == 'acceptfriendrequest':
            if len(request) < 2:
                return requeststatus.STATUS_INVALID_REQUEST_PARAMETERS
            return serverrequests.acceptfriendrequest(request[1], self.username, clients_logged_in)
        elif request[0].lower() == 'declinefriendrequest':
            if len(request) < 2:
                return requeststatus.STATUS_INVALID_REQUEST_PARAMETERS
            return serverrequests.declinefriendrequest(request[1], self.username, clients_logged_in)
        elif request[0].lower() == 'queryfriendship':
            if len(request) < 2:
                return requeststatus.STATUS_INVALID_REQUEST_PARAMETERS
            return serverrequests.queryfriendship(self.username, request[1], clients_logged_in)
        elif request[0].lower() == 'queryfriendrequestsent':
            if len(request) < 2:
                return requeststatus.STATUS_INVALID_REQUEST_PARAMETERS
            return serverrequests.queryfriendrequestsent(self.username, request[1], clients_logged_in)
        elif request[0].lower() == 'queryfriendrequests':
            return serverrequests.queryfriendrequests(self.username, clients_logged_in)
        elif request[0].lower() == 'queryfriendrequestreceived':
            if len(request) < 2:
                return requeststatus.STATUS_INVALID_REQUEST_PARAMETERS
            return serverrequests.queryfriendrequestreceived(self.username, request[1], clients_logged_in)
        elif request[0].lower() == 'unfriend':
            if len(request) < 2:
                return requeststatus.STATUS_INVALID_REQUEST_PARAMETERS
            return serverrequests.unfriend(self.username, request[1], clients_logged_in)
        elif request[0].lower() == 'blockuser':
            if len(request) < 2:
                return requeststatus.STATUS_INVALID_REQUEST_PARAMETERS
            return serverrequests.blockuser(self.username, request[1], clients_logged_in)
        elif request[0].lower() == 'unblockuser':
            if len(request) < 2:
                return requeststatus.STATUS_INVALID_REQUEST_PARAMETERS
            return serverrequests.unblockuser(self.username, request[1], clients_logged_in)
        elif request[0].lower() == 'doiownthis':
            if len(request) < 2:
                return requeststatus.STATUS_INVALID_REQUEST_PARAMETERS
            return serverrequests.doiownthis(self.username, request[1], clients_logged_in)
        elif request[0].lower() == 'queryblock':
            if len(request) < 3:
                return requeststatus.STATUS_INVALID_REQUEST_PARAMETERS
            elif request[1] == '0':
                return serverrequests.queryblock(self.username, self.username, request[2], clients_logged_in)
            elif request[1] == '1':
                return serverrequests.queryblock(self.username, request[2], self.username, clients_logged_in)
            else:
                return requeststatus.STATUS_INVALID_REQUEST_PARAMETERS
        elif request[0].lower() == 'queryfriends':
            return serverrequests.queryfriends(self.username, clients_logged_in)
        elif request[0].lower() == 'changepassword':
            if len(request) < 3:
                return requeststatus.STATUS_INVALID_REQUEST_PARAMETERS
            return serverrequests.changepassword(self.username, request[1], request[2], clients_logged_in)
        elif request[0].lower() == 'querydescription':
            if len(request) < 2:
                return requeststatus.STATUS_INVALID_REQUEST_PARAMETERS
            return serverrequests.querydescription(self.username, request[1], clients_logged_in)
        elif request[0].lower() == 'setdescription':
            if len(request) < 2:
                return requeststatus.STATUS_INVALID_REQUEST_PARAMETERS
            return serverrequests.setdescription(self.username, request[1:], clients_logged_in)
        elif request[0].lower() == 'queryprofilepic':
            if len(request) < 2:
                return requeststatus.STATUS_INVALID_REQUEST_PARAMETERS
            return serverrequests.queryprofilepic(self.username, request[1], clients_logged_in)
        elif request[0].lower() == 'setprofilepic':
            return serverrequests.setprofilepic(self.username, self.fileData, clients_logged_in)
        elif request[0].lower() == 'queryuserexists':
            if len(request) < 2:
                return requeststatus.STATUS_INVALID_REQUEST_PARAMETERS
            return serverrequests.queryuserexists(request[1])
        elif request[0].lower() == 'sendsong':
            if len(request) < 3:
                return requeststatus.STATUS_INVALID_REQUEST_PARAMETERS
            return serverrequests.sendsong(self.username, request[2:], request[1], clients_logged_in)
        elif request[0].lower() == 'createroom':
            if len(request) < 3:
                return requeststatus.STATUS_INVALID_REQUEST_PARAMETERS
            return serverrequests.createroom(self.username, request[1], request[2], clients_logged_in)
        elif request[0].lower() == 'deleteroom':
            if len(request) < 2:
                return requeststatus.STATUS_INVALID_REQUEST_PARAMETERS
            return serverrequests.deleteroom(self.username, request[1], clients_logged_in)
        elif request[0].lower() == 'addtoroom':
            if len(request) < 3:
                return requeststatus.STATUS_INVALID_REQUEST_PARAMETERS
            return serverrequests.addtoroom(self.username, request[1], request[2], clients_logged_in)
        elif request[0].lower() == 'kickfromroom':
            if len(request) < 3:
                return requeststatus.STATUS_INVALID_REQUEST_PARAMETERS
            return serverrequests.kickfromroom(self.username, request[1], request[2], clients_logged_in)
        elif request[0].lower() == 'broadcastmsg':
            if len(request) < 4:
                return requeststatus.STATUS_INVALID_REQUEST_PARAMETERS
            return serverrequests.broadcastmsg(self.username, request[1], request[3:], request[2], clients_logged_in)
        elif request[0].lower() == 'getrooms':
            return serverrequests.getrooms(self.username, clients_logged_in)
        elif request[0].lower() == 'getroommessages':
            if len(request) < 2:
                return requeststatus.STATUS_INVALID_REQUEST_PARAMETERS
            return serverrequests.getroommessages(self.username, request[1], clients_logged_in)
        elif request[0].lower() == 'getroommembers':
            if len(request) < 2:
                return requeststatus.STATUS_INVALID_REQUEST_PARAMETERS
            return serverrequests.getroommembers(self.username, request[1], clients_logged_in)
        elif request[0].lower() == 'filetransfer':
            if len(request) < 2:
                return requeststatus.STATUS_INVALID_REQUEST_PARAMETERS
            if self.username not in clients_logged_in.keys():
                return requeststatus.STATUS_NOT_LOGGED_IN
            self.expectedFileSize = int(request[1])
            self.fileTransferMode = True
            self.fileData = ''
            self.fileTransferBytesReceived = 0
            return requeststatus.STATUS_SUCCESS
        else:
            return requeststatus.STATUS_UNKNOWN_REQUEST

    def run(self):
        self.running = True
        file_transfer = False
        file_data = ''
        file_bytes_sent = 0
        file_bytes_to_send = 0
        print self.address, 'has connected'
        while self.running:
            try:
                if file_transfer:
                    while file_bytes_sent < file_bytes_to_send:
                        self.client.sendall(file_data[:min(1024, len(file_data))])
                        if len(file_data) <= 1024:
                            file_data = ''
                        else:
                            file_data = file_data[1024:]
                        file_bytes_sent += 1024
                    file_transfer = False
                request = self.client.recv(1024)
                if len(request) == 0:
                    serverrequests.logout(self.username, clients_logged_in)
                    del clients[self.address]
                    print self.address, 'has disconnected'
                    break
                print '[' + str(self.address) + ']: ' + request
                request = request.split()
                response = self.process_request(request)
                if request[0].lower() == 'queryprofilepic':
                    file_data = response[2]
                    file_bytes_sent = 0
                    file_bytes_to_send = len(response[2])
                    response = (response[0], response[1])
                if type(response) is tuple:
                    response = utils.concatlist(response, '\n')
                else:
                    response = str(response)
                self.client.sendall(response)
                if request[0].lower() == 'queryprofilepic':
                    file_transfer = True
            except socket.error as e:
                print e
                serverrequests.logout(self.username, clients_logged_in)
                del clients[self.address]
                print self.address, 'has disconnected'
                break


class Server(threading.Thread):
    def __init__(self, host, port1, port2):
        super(Server, self).__init__()
        self.clients = []
        self.socket1 = socket.socket()
        self.socket1.bind((host, port1))
        self.socket1.listen(5)
        self.socket2 = socket.socket()
        self.socket2.bind((host, port2))
        self.socket2.listen(5)
        self.running = False

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
