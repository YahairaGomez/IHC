using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        Transform spawnPoint = spawnPoints[(int)PhotonNetwork.LocalPlayer.CustomProperties["personaje"]];
        // asignamos el prefab de atacante o defensor según la propiedad del personaje
        // 0: atacante, 1: defensor
        GameObject playerToSpawn = playerPrefabs[(int)PhotonNetwork.LocalPlayer.CustomProperties["personaje"]];
        
        // hacemos un spawn para todos los jugadores en el videojuego
        PhotonNetwork.Instantiate(playerToSpawn.name, spawnPoint.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
