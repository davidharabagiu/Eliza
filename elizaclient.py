import socket
import threading
import requeststatus


host, port1, port2 = 'elizaserver.ddns.net', 9999, 9998


class ReceiverThread(threading.Thread):
    running = False

    def __init__(self, sock):
        super(ReceiverThread, self).__init__()
        self.sock = sock

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


if __name__ == '__main__':
    sock1 = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    sock2 = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    receiver = ReceiverThread(sock2)

    try:
        sock1.connect((host, port1))
        sock2.connect((host, port2))

        receiver.start()
        while True:
            request = raw_input()
            if request == 'exit':
                break
            sock1.sendall(request)
            response = sock1.recv(1024).splitlines()
            print requeststatus.status_messages[int(response[0])]
            for i in range(1, len(response)):
                print response[i]
    except socket.error as err:
        print err
    finally:
        sock1.close()
        sock2.close()
        receiver.running = False
        receiver.join()
