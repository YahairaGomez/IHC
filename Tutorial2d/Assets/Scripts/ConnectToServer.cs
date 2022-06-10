using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    public InputField usernameInput;
    public Text buttonText;

    // se llama cuando damos click al botón "conectarse"
    public void OnClickConnect()
    {
        if (usernameInput.text.Length >= 1)
        {
            // guardamos el nombre de usuario dentro de Photon
            PhotonNetwork.NickName = usernameInput.text;
            buttonText.text = "Conectando...";
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    // Photon llama automáticamente a esta función cuando nos conectamos exitosamente al servidor
    public override void OnConnectedToMaster()
    {
        // una vez ingresados al servidor cargamos la siguiente escena Lobby
        SceneManager.LoadScene("Lobby");
    }
}
