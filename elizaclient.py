import socket


host, port = 'elizaserver.ddns.net', 9999

if __name__ == '__main__':
    sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

    try:
        sock.connect((host, port))
        while True:
            request = raw_input('> ')
            if request == 'exit':
                break
            sock.sendall(request)
            response = sock.recv(1024)
            print response
    except socket.error as err:
        print err
    finally:
        sock.close()
