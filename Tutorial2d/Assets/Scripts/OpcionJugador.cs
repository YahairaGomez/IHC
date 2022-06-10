using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class OpcionJugador : MonoBehaviour
{
    public Image backgroundImage;
    public Button seleccionButton;


    // esta función se llama cuando se selecciona un personaje del juego
    [PunRPC]
    public void OnSelected(Color highlightColor)
    {
        // cambiamos el background color de una imagen
        backgroundImage.color = highlightColor;
    }
    
    // desabilitamos la seleccion del boton ya que el otro personaje fue escogido
    [PunRPC]
    public void disableSelection()
    {
        seleccionButton.interactable = false;
    }
}
