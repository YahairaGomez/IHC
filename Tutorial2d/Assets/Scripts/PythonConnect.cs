using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
using UnityEngine.Windows.Speech;

public class PythonConnect : MonoBehaviour
{
    Thread mThread;
    public string connectionIP = "127.0.0.1";
    public int connectionPort = 25001;
    IPAddress localAdd;
    TcpListener listener;
    TcpClient client;
    
    Vector3 receivedPos;
    float vertExtentProjection;  
    float horzExtentProjection;
    private float horzExtentAruco = 640.0f; // horizontal width of aruco marker
    private float vertExtentAruco = 480.0f; // vertical height of aruco marker
    private float pendiente_W;
    private float pendiente_H;
    
    bool running;

    private KeywordRecognizer keywordRecognizer;
    private List<string> actions = new List<string>();
    public GameObject Shield;
    private int currShields = 0; // cantidad actual de escudos en el juego
    [Range(0, 5)]
    public int maxShields = 3; // la máxima cantidad de escudos posibles en el juego
    [Range(2, 5)] 
    public float destroyDelay = 3; // el tiempo en segundos que demoran en destruirse los escudos por defecto 3s
    
    private void Update()
    {
        Vector3 currPos = new Vector3(pendiente_W * receivedPos.x - horzExtentProjection, 
            pendiente_H * receivedPos.y - vertExtentProjection, 0);
        transform.position = currPos; //assigning receivedPos in SendAndReceiveData()
    }

    private void Start()
    {
        receivedPos = new Vector3(horzExtentAruco / 2, vertExtentAruco / 2, 0);
            
        ThreadStart ts = new ThreadStart(GetInfo);
        mThread = new Thread(ts);
        mThread.Start();

        vertExtentProjection = Camera.main.orthographicSize;
        horzExtentProjection = vertExtentProjection * Screen.width / Screen.height;
        pendiente_W = 2 * (horzExtentProjection / horzExtentAruco);
        pendiente_H = 2 * (vertExtentProjection / vertExtentAruco);
        
        Debug.Log("horzExtent: " + horzExtentProjection);
        Debug.Log("vertExtent: " + vertExtentProjection);

        Debug.Log("screen width: " + Screen.width);
        Debug.Log("screen height: " + Screen.height);
        
        // Estas son las tres palabras que podemos decir para poner un escudo
        actions.Add("escudo");
        actions.Add("defensa");
        actions.Add("barrera");

        keywordRecognizer = new KeywordRecognizer(actions.ToArray());
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        keywordRecognizer.Start();
    }
    private void RecognizedSpeech(PhraseRecognizedEventArgs speech)
    {
        Debug.Log("Keyword: "+ speech.text);
        if (currShields < maxShields)
        {
            StartCoroutine(generateBarrier());
        }
    }
    
    IEnumerator generateBarrier()
    {
        GameObject ShieldIns = Instantiate(Shield, transform.position, transform.rotation);
        currShields++; // aumentamos la cantidad de escudos en el juego
        Debug.Log("Current shields: " + currShields);
        // esperar 3 segundos antes de destruir un objeto
        yield return new WaitForSeconds(destroyDelay);
        Destroy(ShieldIns);
        currShields--; // decrementamos los escudos en el juego una vez destruido
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
            receivedPos = StringToVector3(dataReceived); //<-- assigning receivedPos value from Python
            print("received pos data, and placed the shield!");

            //---Sending Data to Host----
            byte[] myWriteBuffer = Encoding.ASCII.GetBytes("Hey I got your message Python! Do You see this message?"); //Converting string to byte data
            nwStream.Write(myWriteBuffer, 0, myWriteBuffer.Length); //Sending the data in Bytes to Python
        }
    }

    public static Vector3 StringToVector3(string sVector)
    {
        // Remove the parentheses
        if (sVector.StartsWith("(") && sVector.EndsWith(")"))
        {
            sVector = sVector.Substring(1, sVector.Length - 2);
        }

        // split the items
        string[] sArray = sVector.Split(',');

        // store as a Vector3
        Vector3 result = new Vector3(
            float.Parse(sArray[0]),
            float.Parse(sArray[1]),
            float.Parse(sArray[2]));

        return result;
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