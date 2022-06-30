using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject[] playerPrefabs;
    public Transform[] spawnPoints;
    
    // Start is called before the first frame update
    void Start()
    {
        // seleccinamos el spawn point dependiendo de si el jugador es atacante o defensor
        // atacante: esquina inferior izquierda, defensa: esquina superior derecha
        Debug.Log("Local player: " + PhotonNetwork.LocalPlayer);
        int whichCharacter = (int)PhotonNetwork.LocalPlayer.CustomProperties["personaje"];
        Transform spawnPoint = spawnPoints[whichCharacter];
        // asignamos el prefab de atacante o defensor según la propiedad del personaje
        // 0: atacante, 1: defensor
        GameObject playerToSpawn = playerPrefabs[whichCharacter];



        // hacemos un spawn para todos los jugadores en el videojuego
        // este es el caso para el atacante
        if(whichCharacter == 0){
            GameObject attacker = PhotonNetwork.Instantiate(playerToSpawn.name,
                spawnPoint.position, Quaternion.identity).transform.GetChild(2).gameObject;
            Text MyscoreText = GameObject.Find("Score").GetComponent<Text>();
            print("score: " + MyscoreText.text);
            attacker.GetComponent<shootArrow>().myScoreText = MyscoreText;
        }
        else {
            GameObject defensor = PhotonNetwork.Instantiate(playerToSpawn.name,
                spawnPoint.position, Quaternion.identity).gameObject;
            Text MyscoreText = GameObject.Find("Score").GetComponent<Text>();
            print("score: " + MyscoreText.text);
            defensor.GetComponent<ScoreManager>().MyscoreText = MyscoreText;
        }
        
    }
}
