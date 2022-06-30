using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Text;
using System.Threading;
using Photon.Pun;
using UnityEngine.Windows.Speech;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class AtacanteConnect : MonoBehaviour
{
    Thread mThread;
    public string connectionIP = "127.0.0.1";
    public int connectionPort = 25005; // el atacante recibirá los datos por el puerto 25002 diferente al defensors
    IPAddress localAdd;
    TcpListener listener;
    TcpClient client;
    
    // receivedPos es para la posición recibida y recivedRot para la rotación recibida
    public Vector3 receivedPos, receivedRot;
    
    Vector2 ArX, ArY, UnPosX, UnPosY, UnRotX, UnRotY;
    private float horzExtentAruco = 640.0f; // horizontal width of aruco marker
    private float vertExtentAruco = 480.0f; // vertical height of aruco marker
    private float mPos_W, mPos_H, mRot_W, mRot_H;
    private float bPos_W, bPos_H, bRot_W, bRot_H;
    
    
    bool running;

    private KeywordRecognizer keywordRecognizer;
    private List<string> actions = new List<string>();
   
    private void Update()
    {
        // solo el atacante puede actualizar el vector de posición
        if ((int)PhotonNetwork.LocalPlayer.CustomProperties["personaje"] == 0)
        {
            Vector3 currPos = new Vector3(mPos_W * receivedPos.x + bPos_W, 
                mPos_H * receivedPos.y + bPos_H, 0);
            transform.position = currPos; //assigning receivedPos in SendAndReceiveData()
        
            receivedRot = new Vector3(mRot_W * receivedRot.x + bRot_W, 
                mRot_H * receivedRot.y + bRot_H, 0);            
        }
    }

    private void Start()
    {
        receivedRot = new Vector3(horzExtentAruco / 2, 0, 0); // llano al inicio
        receivedPos = new Vector3(horzExtentAruco / 2, vertExtentAruco / 2, 0);

        ThreadStart ts = new ThreadStart(GetInfo);
        mThread = new Thread(ts);
        mThread.Start();

        // los límites para los marcadores Aruco
        ArX = new Vector2(0.0f, horzExtentAruco);
        ArY = new Vector2(0.0f, vertExtentAruco);
        // los límites para el espacio de MOVIMIENTO en unity
        UnPosX = new Vector2(-11.52f, -6.26f);
        UnPosY = new Vector2(-2.61f, 3.08f);

        // obteniendo las pendientes de la relación lineal MOVIMIENTO
        mPos_W = (UnPosX.y - UnPosX.x) / (ArX.y - ArX.x); // pendiente para las coordenadas horizontales
        mPos_H = (UnPosY.y - UnPosY.x) / (ArY.y - ArY.x); // pendiente para las coordenadas verticales

        // obteniendo las pendientes de la relación lineal ROTACION
        mRot_W = (UnRotX.y - UnRotX.x) / (ArX.y - ArX.x); // pendiente para las coordenadas horizontales
        mRot_H = (UnRotY.y - UnRotY.x) / (ArY.y - ArY.x); // pendiente para las coordenadas verticales

        // obteniendo las constantes de las relaciones lineales MOVIMIENTO
        bPos_W = UnPosX.y - mPos_W * ArX.y; // horizontal
        bPos_H = UnPosY.y - mPos_H * ArY.y; // vertical

        // obteniendo las constantes de las relaciones lineales ROTACIÓN
        bRot_W = UnRotX.y - mRot_W * ArX.y; // horizontal
        bRot_H = UnRotY.y - mRot_H * ArY.y; // vertical
    }


    void GetInfo()
    {
        localAdd = IPAddress.Parse(connectionIP);
        listener = new TcpListener(IPAddress.Any, connectionPort);
        listener.Start();

        client = listener.AcceptTcpClient();

        running = true;
        while (running)
        {
            SendAndReceiveData();
        }
        listener.Stop();
    }

    void SendAndReceiveData()
    {
        NetworkStream nwStream = client.GetStream();
        byte[] buffer = new byte[client.ReceiveBufferSize];

        //---receiving Data from the Host----
        int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize); //Getting data in Bytes from Python
        string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead); //Converting byte data to string

        
        if (dataReceived != null)
        {
            //---Using received data---
            List<Vector3> positionsFromPython = StringToVector3(dataReceived);
            receivedPos = positionsFromPython[0]; //<-- assigning receivedPos value from Python
            if (positionsFromPython.Count() > 1)
            {
                receivedRot = positionsFromPython[1];
            }
            
            print("received pos data, and placed the shield!");

            //---Sending Data to Host----
            byte[] myWriteBuffer = Encoding.ASCII.GetBytes("Hey I got your message Python! Do You see this message?"); //Converting string to byte data
            nwStream.Write(myWriteBuffer, 0, myWriteBuffer.Length); //Sending the data in Bytes to Python
        }
    }

    public static List<Vector3> StringToVector3(string sVector)
    {
        // Remove the parentheses
        if (sVector.StartsWith("(") && sVector.EndsWith(")"))
        {
            sVector = sVector.Substring(1, sVector.Length - 2);
        }

        // split the items
        string[] sArray = sVector.Split(',');
        
        // Aquí solo se recibe un vector para el defensor
        // store as a Vector3
        Vector3 result = new Vector3(), rotVec = new Vector3();
        
        // si el string empieza con "a" entonces es para el atacante
        
        if (sArray[0] == "a")
        {
            try
            {
                result.x = float.Parse(sArray[1]);
                result.y = float.Parse(sArray[2]);
                result.z = float.Parse(sArray[3]);
                // significa que también recibimos el vector de rotación
                if (sArray.Length > 4)
                {
                    rotVec.x = float.Parse(sArray[4]);
                    rotVec.y = float.Parse(sArray[5]);
                    rotVec.z = float.Parse(sArray[6]);
                }
            }
            catch (FormatException e)
            {
                Console.WriteLine(e);
            }
        }

        List<Vector3> ans = new List<Vector3>();
        ans.Add(result);
        if(sArray.Length > 4)
            ans.Add(rotVec);
        return ans;
    }
    /*
    public static string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new System.Exception("No network adapters with an IPv4 address in the system!");
    }
    */
}
