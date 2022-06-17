import socket
import time

# socket global para el envío de datos


def connect_to_server(host="127.0.0.1", port=25001):
    sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    sock.connect((host, port))
    return sock


# enviamos el socket al cual enviaremos tales datos
def send_positions(sock, positions, character, debug=True):
    # character define hacia quien estará dirigido el mensaje, lo agregaremos
    # al string que enviamos 'a' para el atacante y 'd' para el defensor

    posStr = "" + character + ","
    for pos in positions:
        posStr += ','.join(map(str, pos)) + "," #Converting Vector3 to a string, example "0,0,0" + "," to separate coords
    # print(posStr)
    # enviamos todas las posiciones de nuestros vectores
    sock.sendall(posStr[:-1].encode("UTF-8")) #Converting string to Byte, and sending it to C#
    if debug:
        receivedData = sock.recv(1024).decode("UTF-8") #receiveing data in Byte fron C#, and converting it to String
        print(receivedData)
