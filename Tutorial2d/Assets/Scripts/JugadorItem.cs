using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class JugadorItem : MonoBehaviourPunCallbacks
{
    public Text nombreJugador;
    private ExitGames.Client.Photon.Hashtable jugadorPropiedades = new ExitGames.Client.Photon.Hashtable();
    private Player jugador;
    private string[] personajes = { "atacante", "defensor"};
    
    public void SetPlayerInfo(Player _jugador)
    {
        nombreJugador.text = _jugador.NickName;
        jugador = _jugador;
        UpdateJugadorItem(jugador);
    }

    // con esta función asignaremos un personaje "defensor" o "atacante" a nuestro jugador
    // con un índice, 0 para atacante y 1 para defensor
    public void SelectCharacter(int index)
    {
        jugadorPropiedades["personaje"] = index;
        PhotonNetwork.SetPlayerCustomProperties(jugadorPropiedades);
    }

    // esta función se llama cada vez que actualizamos las propiedades de nuestros jugadores
    public override void OnPlayerPropertiesUpdate(Player targetJugador, ExitGames.Client.Photon.Hashtable propiedades)
    {
        // hacemos esto solo para el jugador de cual queremos modificar su avatar
        if (jugador == targetJugador)
        {
            UpdateJugadorItem(targetJugador);
        }
    }

    void UpdateJugadorItem(Player jugador)
    {
        if (jugador.CustomProperties.ContainsKey("personaje"))
        {
            // jugadorPropiedades["personaje"] = (int)jugador.CustomProperties["personaje"];
            
            if (PhotonNetwork.IsMasterClient)
            {
                // por defecto la propiedad es 0:atacante para el cliente master
                jugadorPropiedades["personaje"] = 0;
            }
            else
            {
                // y 1:defensor para el cliente visitante 
                jugadorPropiedades["personaje"] = 1;
            }
            Debug.Log(personajes[(int)jugadorPropiedades["personaje"]]);
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
            {
                // por defecto la propiedad es 0:atacante para el cliente master
                jugadorPropiedades.Add("personaje", 0);
            }
            else
            {
                // y 1:defensor para el cliente visitante 
                jugadorPropiedades.Add("personaje", 1);
            }
            Debug.Log(personajes[(int)jugadorPropiedades["personaje"]]);
        }
        
        PhotonNetwork.SetPlayerCustomProperties(jugadorPropiedades);
    }
}
