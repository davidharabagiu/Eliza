import socket


host, port = 'localhost', 9999

if __name__ == '__main__':
    sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

    try:
        sock.connect((host, port))
        while True:
            data = raw_input('> ')
            if data == '/exit':
                break
            sock.sendall(data)
    except socket.error, msg:
        print msg
    finally:
        sock.close()
