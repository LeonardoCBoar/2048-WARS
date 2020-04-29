import socket
import threading
import time
import os


clients = [False,False]
connections = []
address = []




def createSocket():
    # inicializa o socket, atribuindo ip e porta
    global host
    global port
    global server
    host = ""
    port = 6666
    print(port)
    try:
        server = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        server.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
        print("Binding the Port: " + str(port))

        server.bind((host, int(port)))
        server.listen(10)

    except socket.error as msg:
        print("Socket creating error" + str(msg))


def listeningConnections():
    # thread principal - aguardar conexões
    print("Waiting connections at:" +host+str(port))
    global connections
    global address
    global count 
    count = 0

    while True:
        print("listeniiing now")
        try:
            conn, addr = server.accept()
            server.setblocking(1)
            connections.append(conn)
            address.append(addr)
            print(addr[0]+" has connected")
            count+=1
            print(count)
            if not clients[0]:
                clients[0] = True
                connections[0] = conn
                print("Client one")
                conn.send('0:You\'re client one'.encode())
                tTwo = threading.Thread(target=listenClientOne)
                tTwo.daemon = True
                tTwo.start()
            elif not clients[1]:
                clients[1] = True
                connections[1] = conn
                print("Client two")
                conn.send('0:You\'re client two'.encode())
                tThree = threading.Thread(target=listenClientTwo)
                tThree.daemon = True
                tThree.start()
            print(clients)
                

        except:
            print("Error in connection")
        time.sleep(0.5)

        limite = min(2,len(connections))
        try:
            for i in(0,limite-1):
                connections[i].send(("9:"+addr[0]+" has connected").encode())
        except:
            pass
def listenClientOne():
    #thread 2- tratar dados do cliente 1
    print("Thread two started")
    while True:
        #loop-recebe dados do cliente 1, troca o identificador e envia para o cliente 2
        conn = connections[0]
        a = conn.recv(1024).decode("UTF-8")
        if a == "":
            #gerenciamento de desconexão
            print(address[0][0] + "(1)has Disconnected")
            conn.close()
            clients[0] = False
            print(clients)
            return
        elif(a[0] == "2"):
            a =a.replace("2:","1:")
            if(len(clients)>1 and clients[1]):
                connections[1].send(a.encode("utf-8"))
        elif(a[0]=="7"):
            connections[1].send("7:".encode("utf-8"))
        print("C1="+a)

def listenClientTwo():
    #thread 2- tratar dados do cliente 1
    print("Thread three started")
    while True:
        #loop-recebe dados do cliente 1, troca o identificador e envia para o cliente 1
        conn = connections[1]
        a = conn.recv(1024).decode("UTF-8")
        print(a)
        if a == "":
            #gerenciamento de desconexão
            print(address[1][0] + "(2)has Disconnected")
            conn.close()
            clients[1] = False
            print(clients)
            return
        elif(a[0] == "2"):
            a =a.replace("2:","1:")
            if(len(clients)>0 and clients[0]):
                connections[0].send(a.encode("utf-8"))
        elif(a[0]=="7"):
            print("disco")
            connections[0].send("7:".encode("utf-8"))
        print("C2="+a)

def startServer():
    createSocket()
    listeningConnections()






startServer()