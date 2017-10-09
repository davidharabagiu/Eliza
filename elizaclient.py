import socket
import sys
import random


host, port = 'elizaserver.ddns.net', 9999

if __name__ == '__main__':
    sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

    try:
        sock.connect((host, port))
        if len(sys.argv) < 2:
            username = 'User ' + str(random.randint(10000, 100000))
        else:
            username = sys.argv[1]
        sock.sendall(username)
        while True:
            data = raw_input('> ')
            if data == '/exit':
                break
            sock.sendall(data)
    except socket.error, msg:
        print msg
    finally:
        sock.close()
