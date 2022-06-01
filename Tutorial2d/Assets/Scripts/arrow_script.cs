using System.Collections;
using System.Collections.Generic;
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
       
       trackMovement();
        
        
    }

    void trackMovement()
    {
        Vector2 direction = rb.velocity;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Choque" || col.tag == "destroy_arrow")
            Destroy(this.gameObject);
    }
}
