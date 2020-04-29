import socket                
  
# Create a socket object 
s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)      
  
# Define the port on which you want to connect 
port = 80                
# connect to the server on local computer 
print(s.connect(('server2048wars.herokuapp.com', port)) )
print("connected")
while True:
    a = s.recv(1024)
    print(a.decode("utf-8"))
    pass