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
using Photon.Pun;
using UnityEngine.SceneManagement;


public class DefensorConnect : MonoBehaviour
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
    public int currShields; // cantidad actual de escudos en el juego
    public int maxShields = 3; // la máxima cantidad de escudos posibles en el juego
    
    [Range(2, 5)]
    public float destroyDelay = 3; // el tiempo en segundos que demoran en destruirse los escudos por defecto 3s

    // solo se usara photon en la escena del juego
    string sceneName;
    
    private void Awake()
    {
        sceneName = SceneManager.GetActiveScene().name;
        currShields = 0;
    }

    private void Start()
    {
        if (sceneName == "Nivel1" || sceneName == "Instruction1")
        {
            receivedPos = new Vector3(horzExtentAruco / 2, vertExtentAruco / 2, 0);

            ThreadStart ts = new ThreadStart(GetInfo);
            mThread = new Thread(ts);
            mThread.Start();

            vertExtentProjection = Camera.main.orthographicSize;
            horzExtentProjection = vertExtentProjection * Screen.width / Screen.height;
            pendiente_W = 2 * (horzExtentProjection / horzExtentAruco);
            pendiente_H = 2 * (vertExtentProjection / vertExtentAruco);

            // Debug.Log("horzExtent: " + horzExtentProjection);
            // Debug.Log("vertExtent: " + vertExtentProjection);
            //
            // Debug.Log("screen width: " + Screen.width);
            // Debug.Log("screen height: " + Screen.height);

            // Estas son las tres palabras que podemos decir para poner un escudo
            actions.Add("escudo");
            actions.Add("defensa");
            actions.Add("barrera");

            keywordRecognizer = new KeywordRecognizer(actions.ToArray());
            keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
            keywordRecognizer.Start();
        }
    }

    private void Update()
    {
        // para el juego y para el tutorial se podrá mover al defensor
        if (sceneName == "Nivel1")
        {
            // solo si el jugador tiene asignado el defensor se puede mover
            if ((int)PhotonNetwork.LocalPlayer.CustomProperties["personaje"] == 1)
            {
                Vector3 currPos = new Vector3(pendiente_W * receivedPos.x - horzExtentProjection,
                    pendiente_H * receivedPos.y - vertExtentProjection, 0);
                transform.position = currPos; //assigning receivedPos in SendAndReceiveData()

                // con la tecla "b" puedes poner un escudo
                if (Input.GetKeyUp("b") && currShields < maxShields)
                {
                    StartCoroutine(generateBarrier());
                }
            }
        }
        else if (sceneName == "Instruction1")
        {
            Vector3 currPos = new Vector3(pendiente_W * receivedPos.x - horzExtentProjection,
                pendiente_H * receivedPos.y - vertExtentProjection, 0);
            transform.position = currPos; //assigning receivedPos in SendAndReceiveData()

            // con la tecla "b" puedes poner un escudo
            if (Input.GetKeyUp("b") && currShields < maxShields)
            {
                StartCoroutine(generateBarrier());
            }
        }
    }
    
    private void RecognizedSpeech(PhraseRecognizedEventArgs speech)
    {
        if (currShields < maxShields)
        {
            StartCoroutine(generateBarrier());
        }
    }

    IEnumerator generateBarrier()
    {
        // para el juego
        if (sceneName == "Nivel1")
        {
            //GameObject ShieldIns = Instantiate(Shield, transform.position, transform.rotation);
            GameObject ShieldIns = PhotonNetwork.Instantiate(Shield.name, transform.position, Quaternion.identity);
            currShields++;
            Debug.Log("Current shields: " + currShields);
            // actualizamos las barreras colocadas por el defensor
            ScoreManager myScoreManager = GetComponent<ScoreManager>();
            if (myScoreManager.GetComponent<PhotonView>().IsMine)
            {
                myScoreManager.GetComponent<PhotonView>().RPC("AddBarrierDefensor", RpcTarget.AllBuffered);
            }
            // aumentamos la cantidad de escudos en el juego
            

            // esperar destroyDelay segundos antes de destruir un objeto
            yield return new WaitForSeconds(destroyDelay);

            if (currShields > 0)
            {
                currShields--;
            }
            PhotonNetwork.Destroy(ShieldIns); // decrementamos los escudos en el juego una vez destruido
        }
        else if (sceneName == "Instruction1") // para la escena del tutorial
        {
            GameObject ShieldIns = Instantiate(Shield, transform.position, transform.rotation);
            // actualizamos las barreras colocadas por el defensor
            currShields++; // aumentamos la cantidad de escudos en el juego

            Debug.Log("Current shields: " + currShields);
            // esperar destroyDelay segundos antes de destruir un objeto
            yield return new WaitForSeconds(destroyDelay);

            if (currShields > 0)
            {
                currShields--;
            }
            Destroy(ShieldIns); // decrementamos los escudos en el juego una vez destruido
        }
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
            receivedPos = StringToVectors3(dataReceived); //<-- assigning receivedPos value from Python

            //---Sending Data to Host----
            byte[] myWriteBuffer =
                Encoding.ASCII.GetBytes(
                    "Hey I got your message Python! Do You see this message?"); //Converting string to byte data
            nwStream.Write(myWriteBuffer, 0, myWriteBuffer.Length); //Sending the data in Bytes to Python
        }
    }

    public static Vector3 StringToVectors3(string sVector)
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
        Vector3 result = new Vector3();
        // si el string empieza con "d" entonces si es para el defensor
        if (sArray[0] == "d")
        {
            try
            {
                result.x = float.Parse(sArray[1]);
                result.y = float.Parse(sArray[2]);
                result.z = float.Parse(sArray[3]);
            }
            catch (FormatException e)
            {
                Console.WriteLine(e);
            }
        }
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