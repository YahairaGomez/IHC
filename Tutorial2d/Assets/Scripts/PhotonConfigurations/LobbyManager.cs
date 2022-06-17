using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


public class LobbyManager : MonoBehaviourPunCallbacks
{
    public GameObject lobbyPanel;
    public GameObject roomPanel;
    public Text ayudaText;
    public Text jugadoresCounter;
    public GameObject jugarButton;

    public List<JugadorItem> jugadoresItemsList = new List<JugadorItem>();
    public JugadorItem jugadorItemPrefab;
    public Transform jugadorItemParent;
    
    public InputField createRoomInput;
    public InputField joinRoomInput;

    
    private string defaultRoomName = "ShooterGame";
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.JoinLobby();
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount >= 2)
        {
            jugarButton.SetActive(true);
        }
        else
        {
            jugarButton.SetActive(false);   
        }
    }

    public void OnClickCreate()
    {
        if (createRoomInput.text.Length >= 1)
        {
            // creamos la sala con el nombre especificado en el inputField "crear"
            // ¡importante! cuando se crea una sala automáticamente nos unimos (join) a ella
            PhotonNetwork.CreateRoom(createRoomInput.text, new RoomOptions()
            {
                // maximo serán dos jugadores el defensor y el atacante
                MaxPlayers = 2,
                BroadcastPropsChangeToAll = true
            });
        }
    }

    public void OnClickJoin()
    {
        if (joinRoomInput.text.Length >= 1)
        {
            // ingresamos a la sala con el nombre especificado en el inputField "join"
            PhotonNetwork.JoinRoom(joinRoomInput.text);
        }
    }

    // esta función es de Photon, se llama automáticamente cuando ingresamos a una sala
    public override void OnJoinedRoom()
    {
        // una vez ingresados a la sala elegiremos uno de los dos personajes: atacante o defensor
        // ocultamos el panel del lobby y activamos el del room
        lobbyPanel.SetActive(false);
        roomPanel.SetActive(true);
        UpdateJugadoresList();        
    }
    
    // esta función se ejecuta cuando damos click en salir de sala
    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        roomPanel.SetActive(false);
        lobbyPanel.SetActive(true);
    }
    
    // Esta función se llama después de que un jugador entre a una sala
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdateJugadoresList();
    }
    
    // Esta función se llama después de que un jugador deja una sala
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdateJugadoresList();
    }


    void UpdateJugadoresList()
    {
        // no solo actualizamos la lista de jugadores sino su cantidad
        jugadoresCounter.text = "Jugadores: " + PhotonNetwork.CurrentRoom.PlayerCount + " (max: 2)";
        
        foreach (JugadorItem item in jugadoresItemsList)
        {
            Destroy(item.gameObject);
        }
        jugadoresItemsList.Clear();

        // chequear que estamos en una sala para evitar bugs
        if (PhotonNetwork.CurrentRoom == null)
        {
            return;
        }

        foreach (KeyValuePair<int, Player> jugador in PhotonNetwork.CurrentRoom.Players)
        {
            JugadorItem newJugadorItem = Instantiate(jugadorItemPrefab, jugadorItemParent);
            newJugadorItem.SetPlayerInfo(jugador.Value);
            jugadoresItemsList.Add(newJugadorItem);
        }
    }

    public void OnClickJugarButton()
    {
        PhotonNetwork.LoadLevel("Nivel1");
    }
}
