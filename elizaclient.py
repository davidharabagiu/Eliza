import socket
import threading
import guiconnection
import sys


host, port1, port2 = '127.0.0.1', 9999, 9998


def receive_file(length, sock, pipe):
    bytes_received = 0
    while bytes_received < length:
        file_data = sock.recv(1024)
        bytes_received += len(file_data)
        print 'File data received: ' + file_data
        pipe.send(file_data)


class ReceiverThread(threading.Thread):
    def __init__(self, sock, instance_id):
        super(ReceiverThread, self).__init__()
        self.sock = sock
        self.running = False
        self.gui_pipe_msg = guiconnection.GuiPipe('elizapipemsg', 'r+b', instance_id)

    def run(self):
        self.running = True
        while self.running:
            try:
                msg = self.sock.recv(1024)
                if msg.startswith('/song'):
                    song_length = msg.split()[1]
                    print 'Receiving song with length', song_length
                    self.gui_pipe_msg.send('/song')
                    self.gui_pipe_msg.send(song_length)
                    receive_file(int(song_length), self.sock, self.gui_pipe_msg)
                else:
                    self.gui_pipe_msg.send(msg)
                print 'Message received: ' + msg
                if len(msg) == 0:
                    break
            except socket.error as err:
                print err
                break


if __name__ == '__main__':
    sock1 = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    sock2 = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    receiver = ReceiverThread(sock2, sys.argv[1])
    fileTransferMode = False
    expectedFileSize = 0
    fileData = ''
    fileTransferBytesReceived = 0
    gui_pipe = guiconnection.GuiPipe('elizapipe', 'r+b', sys.argv[1])

    if sys.executable.endswith('pythonw.exe'):
        sys.stdout = open('elizaclient.log', 'w')
        sys.stderr = sys.stdout
        redirect_stdout = True
    else:
        redirect_stdout = False

    try:
        sock1.connect((host, port1))
        sock2.connect((host, port2))

        receiver.start()
        while True:
            if fileTransferMode:
                response = sock1.recv(1024)
                print 'File data received: ' + response
                fileTransferBytesReceived += len(response)
                fileData += response
                gui_pipe.send(response)
                if fileTransferBytesReceived >= expectedFileSize:
                    fileTransferMode = False
            else:
                request = gui_pipe.recv()
                if request == 'exit':
                    break
                sock1.sendall(request)
                print 'Sent: ' + request
                response = sock1.recv(10240)
                print 'Received: ' + response
                gui_pipe.send(response)
                if request.startswith('queryprofilepic '):
                    fileTransferMode = True
                    expectedFileSize = int((response.splitlines())[1])
                    fileData = ''
                    fileTransferBytesReceived = 0
    except socket.error as err:
        print err
        raw_input()
    finally:
        if redirect_stdout:
            sys.stdout.close()
        sock1.close()
        sock2.close()
        receiver.running = False
        receiver.join()
