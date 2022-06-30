using System;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    
    public Text MyscoreText;
    public int scoreAtacante;
    public SystemController controlador_del_sistema;

    private void Awake()
    {
        controlador_del_sistema = GameObject.Find("SystemController").GetComponent<SystemController>();
    }
    
    private void Start()
    {
        scoreAtacante = 0;
        MyscoreText = GameObject.Find("Score").GetComponent<Text>();
    }
    

    [PunRPC]
    void AddPoint()
    {
        scoreAtacante = PlayerPrefs.GetInt("puntaje_atacante");
        scoreAtacante++;
        print("Puntaje " + scoreAtacante);
        MyscoreText.text = "Puntaje: " + scoreAtacante;
        PlayerPrefs.SetInt("puntaje_atacante", scoreAtacante);
        // ahora actualizamos los datos del controlador del sistema
        controlador_del_sistema.updateScoresPuntajeAtacante();
    }

    [PunRPC]
    void AddDisparoAtacante()
    {
        // actualizamos los datos del controlador del sistema
        // controlador_del_sistema = GameObject.Find("SystemController").GetComponent<SystemController>();
        controlador_del_sistema.updateScoresDisparosAtacante();
    }
    
    [PunRPC]
    void AddBarrierCollision()
    {
        // actualizamos los datos del controlador del sistema
        // controlador_del_sistema = GameObject.Find("SystemController").GetComponent<SystemController>();
        controlador_del_sistema.updateScoresBarrerasDestruidas();
    }
 
    [PunRPC]
    void AddBarrierDefensor()
    {
        // actualizamos los datos del controlador del sistema
        // controlador_del_sistema = GameObject.Find("SystemController").GetComponent<SystemController>();
        controlador_del_sistema.updateScoresBarrerasDefensor();
    }
    
    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     Debug.Log(collision.tag);
    //     if (collision.tag == "Choque")
    //     {
    //         print("Choque score manager");
    //         PhotonView photonView = PhotonView.Get(this);
    //         photonView.RPC("AddPoint", RpcTarget.All);
    //         
    //         PhotonNetwork.Destroy(gameObject);
    //         //AddPoint();
    //     }
    //     // colisiono con un escudo
    //     if (collision.tag == "ShieldCollision")
    //     {
    //         // Destroy(collision.gameObject);
    //         PhotonNetwork.Destroy(gameObject);
    //     }
    //     // colisiono con el borde del mapa
    //     if (collision.tag == "destroy_arrow")
    //     {
    //         // Destroy(collision.gameObject);
    //         PhotonNetwork.Destroy(gameObject);
    //     }
    //
    //    
    // }
}