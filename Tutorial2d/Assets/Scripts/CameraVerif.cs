using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraVerif : MonoBehaviour
{
    public bool devicesCon;

    void DevConnected()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        if(devices.Length > 0)
            botonInicial.GetComponent<Image>().color = Color.green;
        else
            botonInicial.GetComponent<Image>().color = Color.red;
    }

    public GameObject botonInicial;
    void Start()
    {
        DevConnected();
    }

}  

