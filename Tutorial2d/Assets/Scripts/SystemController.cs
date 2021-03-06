using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class SystemController : MonoBehaviour
{
    private static Mutex mutex = new Mutex();
    // variables para el atacante
    public int disparos_atacante, puntaje_atacante;

    // variables para el defensor
    public int barreras_defensor, barreras_destruidas;
    
    // TCP connection
    Thread mThread;
    public string connectionIP = "127.0.0.1";
    public int connectionPort = 25010;
    IPAddress localAdd;
    TcpListener listener;
    TcpClient client;
    private NetworkStream nwStream;
    private bool running, sendDataFlag; // sendDataFlag es un booleano para indicar que se enviarán datos

    
    // Start is called before the first frame update
    void Start()
    {
        disparos_atacante = puntaje_atacante = 0;
        barreras_defensor = barreras_destruidas = 0;
        
        // TCP threads for connection
        ThreadStart ts = new ThreadStart(GetInfo);
        mThread = new Thread(ts);
        mThread.Start();
    }

    void GetInfo()
    {
        localAdd = IPAddress.Parse(connectionIP);
        listener = new TcpListener(IPAddress.Any, connectionPort);
        listener.Start();
        client = listener.AcceptTcpClient();
        running = sendDataFlag = true;
        while (running)
        {
            // solo si este flag se activa, enviaremos datos
            if (sendDataFlag)
            {
                nwStream = client.GetStream();
                string controllerData = disparos_atacante + "," + puntaje_atacante + "," 
                                        + barreras_defensor + "," + barreras_destruidas;
        
                byte[] myWriteBuffer =
                    Encoding.ASCII.GetBytes(controllerData); //Converting string to byte data
                nwStream.Write(myWriteBuffer, 0, myWriteBuffer.Length); //Sending the data in Bytes to Python                            
                mutex.WaitOne();
                sendDataFlag = false;
                mutex.ReleaseMutex();
            }
        }
        listener.Stop();
    }

    
    public void updateScoresDisparosAtacante()
    {
        disparos_atacante++;
        print("Disparos: " + disparos_atacante);
        
        // enviamos las estadísticas del juego al controlador del sistema
        mutex.WaitOne();
        sendDataFlag = true;
        mutex.ReleaseMutex();
    }
    public void updateScoresPuntajeAtacante()
    {
        
        puntaje_atacante = PlayerPrefs.GetInt("puntaje_atacante");
        print("Puntaje: " + puntaje_atacante);

        // enviamos las estadísticas del juego al controlador del sistema
        mutex.WaitOne();
        sendDataFlag = true;
        mutex.ReleaseMutex();
    }
    public void updateScoresBarrerasDefensor()
    {
        barreras_defensor++;
        print("Barreras: " + barreras_defensor);

        // enviamos las estadísticas del juego al controlador del sistema
        mutex.WaitOne();
        sendDataFlag = true;
        mutex.ReleaseMutex();
    }
    public void updateScoresBarrerasDestruidas()
    {
        barreras_destruidas++;
        print("Barreras destruidas: " + barreras_destruidas);

        // enviamos las estadísticas del juego al controlador del sistema
        mutex.WaitOne();
        sendDataFlag = true;
        mutex.ReleaseMutex();
    }
}
