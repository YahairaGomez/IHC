import socket
import time

host, port = "127.0.0.1", 25001
sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
sock.connect((host, port))

# time.sleep(0.5)  # sleep 0.5 sec


def send_position(pos, debug=True):
    posString = ','.join(map(str, pos)) #Converting Vector3 to a string, example "0,0,0"
    print(posString)

    sock.sendall(posString.encode("UTF-8")) #Converting string to Byte, and sending it to C#
    if debug:
        receivedData = sock.recv(1024).decode("UTF-8") #receiveing data in Byte fron C#, and converting it to String
        print(receivedData)
