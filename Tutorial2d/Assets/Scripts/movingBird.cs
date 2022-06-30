using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingBird : MonoBehaviour
{
    private float speed = 2.5f;
    private int state = 1;
    private Rigidbody2D rigidbody;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void moveUp()
    {
        Vector2 positionPlayer = transform.position;
        positionPlayer.y += speed * Time.deltaTime;
        transform.position = positionPlayer;
    }

    void moveDown()
    {
        Vector2 positionPlayer = transform.position;
        positionPlayer.y -= speed * Time.deltaTime;
        transform.position = positionPlayer;
    }
    // Update is called once per frame
    void Update()
    {
        if (state == 1)
        {
            moveUp();
        }
        else
        {
            moveDown();
        }
        
        
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        state = state * -1;
        Debug.Log(state);
    }
}
