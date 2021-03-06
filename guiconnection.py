import struct


class GuiPipe:
    def __init__(self, pipe_name, mode, instance_id):
        self.fh = open(r'\\.\pipe\\' + pipe_name + instance_id, mode, 0)

    def send(self, resp):
        self.fh.write(struct.pack('I', len(resp)) + resp)
        self.fh.seek(0)

    def recv(self):
        resplen = struct.unpack('I', self.fh.read(4))[0]
        resp = self.fh.read(resplen)
        self.fh.seek(0)
        return resp
