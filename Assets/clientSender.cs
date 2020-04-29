using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System;
using System.Threading;
using System.Threading.Tasks;
using TMPro;

public class clientSender : MonoBehaviour
{





    //Funções de conexão
    private bool connected;
    public void ConnectButton()
    {
        if (connected)
        {
            print("already ready");
            return;
        }
        print("not ready");

        string host = "127.0.0.1";
        int port = 6666;


        string[] h = GameObject.Find("InputAddres").GetComponent<InputField>().text.Split(':');
        if (h.Length > 1)
        {
            print("setting custom Addres");
            host = h[0];
            port = Int32.Parse(h[1]);
        }

        Task.Run(() => connect(host, port));
        Thread.Sleep(2);



    }

    private TcpClient socket;
    public async Task connect(string host, int port)
    {
        if (connected) return;
        this.socket = new TcpClient();
        print("Connecting");

        try
        {
            await this.socket.ConnectAsync(host, port);
            connected = true;
        }
        catch (Exception e)
        {

            print("Socket error " + e.Message);
        }

        readDataAsync(socket);


    }
    public void disconnect()
    {
        board = new int[4,4];
        points = 0;
        newData = true;
        sendDataServerAsync("7:");
        socket.GetStream().Close();
        socket.Close();
        connected = false;
    }
    //Funções de recepção
    private async void readDataAsync(TcpClient socket)
    {
        try
        {
            StreamReader clientStreamReader = new StreamReader(socket.GetStream());
            char[] buffer = new char[64];
            int byteCount = 0;

            while (true)
            {
                byteCount = await clientStreamReader.ReadAsync(buffer, 0, buffer.Length);
                if (byteCount <= 0)
                {
                    print("Disconnecting....");
                    socket.Close();
                    break;
                }
                string data = new string(buffer);
                processData(data);
                Array.Clear(buffer, 0, buffer.Length);

            }

        }
        catch(ObjectDisposedException a)
        {
            print("Closed socket error: "+a.Message);
        }
        catch (Exception e)
        {

            print("Socket read error: " + e.Message);
        }
    }
    private void processData(string data)
    {
        try
        {
            if (data[0] == '1')
            {
                print(data);
                string[] output = data.Split(':');
                for (int i = 1; i < 5; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        board[i - 1, j] = output[i][j] - '0';
                    }
                }
                points = Convert.ToInt32(output[5]);
                newData = true;
            }
            else if (data[0] == '0')
            {
                print(data.Split(':')[1]);
            }
            else if (data[0]=='7'){
                print("Other side disconnected");
                board = new int[4,4];
                points = 0;
                newData = true;
            }
            /*else if (data[0] == '9')
            {
                print(data.Split(':')[1]);
                protocolMatrix();
            }*/
        }
        catch (Exception e)
        {
            print("Data protocol error: " + e.Message);
        }
    }

    //Funções de envio
    public string protocolBoard(int[,]rawBoard,int pts)
    {
        string stringBoard = "2";
        for (int i = 0; i < 4; i++)
        {
            string line = ":";
            for (int j = 0; j < 4; j++)
            {
                line += rawBoard[i, j].ToString();
            }
            stringBoard += line;

        }
        stringBoard += ":" + pts.ToString();
        sendDataServerAsync(stringBoard);
        return stringBoard;
    }
    public async void sendDataServerAsync(string data)
    {
        if (this.socket != null && this.socket.Connected)
        {
            StreamWriter clientStreamWriter = new StreamWriter(socket.GetStream());
            clientStreamWriter.AutoFlush = true;

            await clientStreamWriter.WriteLineAsync(data);
        }
    }

    //Funções do inimigo

    Color[] colorList = new Color[20];
    private void Start()
    {
        manager = managerObj.GetComponent<gameManager>();


        for (int i = 0; i < colorList.Length; i++)
        {
            colorList[i] = new Color(
            UnityEngine.Random.Range(0.1f, 1f),
            UnityEngine.Random.Range(0.1f, 1f),
            UnityEngine.Random.Range(0.1f, 1f)
                                );
        }
    }


    public GameObject managerObj;
    private gameManager manager;
    public GameObject enemy;

    bool newData = false;

    int points = 0;
    int[,] board = new int[4, 4];
    private void Update()
    {

        if (newData)
        {
            manager.renderBoard(board, enemy.transform);
            manager.updatePts(points, GameObject.Find("EnemyPts").GetComponent<Text>());
            newData = false;
        }
    }
}

