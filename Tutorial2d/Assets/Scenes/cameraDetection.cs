using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cameraDetection : MonoBehaviour
{
    
    // Start is called before the first frame update
    WebCamTexture webCamTexture;
    public string path;
    public RawImage imgDisplay;
    void Start()
    {
        webCamTexture = new WebCamTexture();

        GetComponent<Renderer>().material.mainTexture = webCamTexture;

        webCamTexture.Play();
    }
}
