using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevicesConn : MonoBehaviour
{
    void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        for (int i = 0; i < devices.Length; i++)
            Debug.Log(devices[i].name);
    }

}
