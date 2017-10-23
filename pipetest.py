import struct

try:
    fh = open(r'\\.\pipe\elizapipe', 'r+b', 0)
    reqlen = struct.unpack('I', fh.read(4))[0]
    req = fh.read(reqlen)
    fh.seek(0)
    print(req)

    resp = 'e totu bine coae'
    fh.write(struct.pack('I', len(resp)) + resp)
    fh.seek(0)
except Exception as err:
    print err
finally:
    raw_input('press any key to exit')
