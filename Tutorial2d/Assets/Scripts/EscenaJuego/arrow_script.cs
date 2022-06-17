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

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Choque" || col.tag == "destroy_arrow" || col.tag == "ShieldCollision")
        {
            // Destroy(this.gameObject);
            // Destruyendo objeto en photon
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
