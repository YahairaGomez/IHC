using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class arrow_script : MonoBehaviour
{
    Rigidbody2D rb;
    bool hasHit = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // solo el atacante puede trackear el movimiento
        if ((int)PhotonNetwork.LocalPlayer.CustomProperties["personaje"] == 0)
        {
            trackMovement();
        }
    }

    void trackMovement()
    {
        Vector2 direction = rb.velocity;
        // Debug.Log(direction);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        
        Debug.Log(collision.tag);
        if (collision.tag == "Choque")
        {
            // se tocó un ave
            GameObject MyScoreManager = GetComponent<ScoreManager>().gameObject;

            if (MyScoreManager.GetComponent<PhotonView>().IsMine)
            {
                print("pajarito herido");
                MyScoreManager.GetComponent<PhotonView>().RPC("AddPoint", RpcTarget.AllBuffered);
            }
            
            PhotonNetwork.Destroy(gameObject);
            //AddPoint();
        }
        // colisiono con un escudo
        if (collision.tag == "ShieldCollision")
        {
            // Destroy(collision.gameObject);
            // se tocó una barrera
            GameObject MyScoreManager = GetComponent<ScoreManager>().gameObject;
            if (MyScoreManager.GetComponent<PhotonView>().IsMine)
            {
                MyScoreManager.GetComponent<PhotonView>().RPC("AddBarrierCollision", RpcTarget.AllBuffered);
            }
            PhotonNetwork.Destroy(gameObject);
        }
        // colisiono con el borde del mapa
        if (collision.tag == "destroy_arrow")
        {
            // Destroy(collision.gameObject);
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
