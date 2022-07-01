using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class bow : MonoBehaviour
{
    public Vector2 direction;
    // solo se usara photon en la escena del juego
    string sceneName;

    private void Awake()
    {
        sceneName = SceneManager.GetActiveScene().name;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (sceneName == "Nivel1")
        {
            // solo el atacante puede mover la dirección del arco
            if ((int)PhotonNetwork.LocalPlayer.CustomProperties["personaje"] == 0)
            {
                Vector2 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                // Vector2 MousePos = scriptParent.receivedRot;
                Vector2 bowPos = transform.position;
                direction = MousePos - bowPos;
                FaceMouse();            
            }                
        }
        else
        {                
            Vector2 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Vector2 MousePos = scriptParent.receivedRot;
            Vector2 bowPos = transform.position;
            direction = MousePos - bowPos;
            FaceMouse();       
        }
    }
    void FaceMouse()
    {
        transform.right = direction;
    }
}
