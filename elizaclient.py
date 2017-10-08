import socket
import sys


host, port = '192.168.0.113', 9999

if __name__ == '__main__':
    sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

    try:
        sock.connect((host, port))
        if len(sys.argv) < 2:
            username = 'handicapat'
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
