import socket
import threading


host, port1, port2 = 'elizaserver2.ddns.net', 9999, 9998


class ReceiverThread(threading.Thread):
    def __init__(self, sock):
        super(ReceiverThread, self).__init__()
        self.sock = sock
        self.running = False

    def run(self):
        self.running = True
        while self.running:
            try:
                msg = self.sock.recv(1024)
                print msg
                if len(msg) == 0:
                    break
            except socket.error as err:
                print err
                break


if __name__ == '__main__':
    sock1 = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    sock2 = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    receiver = ReceiverThread(sock2)
    fileTransferMode = False
    expectedFileSize = 0
    fileData = ''
    fileTransferBytesReceived = 0

    try:
        sock1.connect((host, port1))
        sock2.connect((host, port2))

        receiver.start()
        while True:
            if fileTransferMode:
                response = sock1.recv(1024)
                fileTransferBytesReceived += len(response)
                fileData += response
                print response
                if fileTransferBytesReceived >= expectedFileSize:
                    fileTransferMode = False
            else:
                request = raw_input()
                if request == 'exit':
                    break
                sock1.sendall(request)
                response = sock1.recv(1024)
                print response
                if request.startswith('queryprofilepic '):
                    fileTransferMode = True
                    expectedFileSize = int((response.splitlines())[1])
                    fileData = ''
                    fileTransferBytesReceived = 0
    except socket.error as err:
        print err
    finally:
        sock1.close()
        sock2.close()
        receiver.running = False
        receiver.join()
