using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class bow : MonoBehaviour
{
    public Vector2 direction;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
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
    void FaceMouse()
    {
        transform.right = direction;
    }
}
