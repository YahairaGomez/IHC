using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class timer : MonoBehaviour
{   
    [SerializeField] GameObject panelAtacante;
    [SerializeField] GameObject panelDefensor;
    [SerializeField] GameObject controladorDelSistema;
    [SerializeField] Image timeImage;
    [SerializeField] Text timeText;
    [SerializeField] GameObject regresarMenuButton;
    [SerializeField] float duration, currentTime;
    [SerializeField] GameObject textRecordatorio;
    private GameObject defensorPlayer, atacantePlayer;
    private int minScoreToWin;

    private void Start()
    {
        minScoreToWin = 10;
        defensorPlayer = GameObject.Find("Defensor").gameObject;
        atacantePlayer = GameObject.Find("Atacante").gameObject;
    }

    private void Update()
    {
        // solo cuando el usuario presiona la tecla "e" y es el atacante(roomhost) se inicia el juego
        if (Input.GetKeyUp("e") && 
            (int)PhotonNetwork.LocalPlayer.CustomProperties["personaje"] == 0)
        {
            panelAtacante.SetActive(false);
            panelDefensor.SetActive(false);
            regresarMenuButton.SetActive(false);
            textRecordatorio.SetActive(false);
            currentTime = duration;
            
            timeText.text = (currentTime < 10)? "00:0"+ currentTime.ToString() : "00:"+ currentTime.ToString();
            StartCoroutine(TimeIEn());
        }
    }

    IEnumerator TimeIEn()
    {
        while(currentTime >= 0)
        {
            yield return new WaitForSeconds(1f);

            // solo el atacante (HOST) actualiza el timer
            if ((int)PhotonNetwork.LocalPlayer.CustomProperties["personaje"] == 0 && GetComponent<PhotonView>().IsMine)
            {
                GetComponent<PhotonView>().RPC("updateTimer", RpcTarget.AllBuffered);
            }
        }

        if ((int)PhotonNetwork.LocalPlayer.CustomProperties["personaje"] == 0 && GetComponent<PhotonView>().IsMine)
        {
            GetComponent<PhotonView>().RPC("endTheGame", RpcTarget.AllBuffered);
        }
    }

    public void OnReturnToMenuClick()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.Disconnect(); // nos desconectamos del juego
        SceneManager.LoadScene("menuInicial");
    }

    [PunRPC]
    void updateTimer()
    {
        timeImage.fillAmount = Mathf.InverseLerp(0, duration, currentTime);
        //textDisplay.GetComponent<Text>().text = "00:0" + currentTime;
        timeText.text = (currentTime < 10)? "00:0"+ currentTime.ToString() : "00:"+ currentTime.ToString();
        currentTime--;
    }
    
    [PunRPC]
    void endTheGame() {
        if (controladorDelSistema.GetComponent<SystemController>().puntaje_atacante >= minScoreToWin)
        {
            timeText.text = "";
            panelAtacante.SetActive(true);
        }
        else
        {
            timeText.text = "";
            panelDefensor.SetActive(true);
        }
        regresarMenuButton.SetActive(true);    
    }
}
