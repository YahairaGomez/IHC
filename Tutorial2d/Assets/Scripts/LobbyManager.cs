using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


public class LobbyManager : MonoBehaviourPunCallbacks
{
    private List<RoomInfo> roomsList = new List<RoomInfo>();
    
    private string defaultRoomName = "ShooterGame";
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.JoinLobby();
    }

    public void OnClickCharacter()
    {
        if (roomsList != )
        {
            
        }
        foreach (RoomInfo room in roomsList)
        {
            if (room.Name == defaultRoomName)
            {
                Debug.Log("This room exists !");
                break;
            }
        }
    }

    
}
