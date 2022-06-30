using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdFlying : MonoBehaviour
{
    void SetTransformX(float n)
    {
        transform.position = new Vector3(n, transform.position.y, transform.position.z);
    }
    void Update()
    {
        SetTransformX(7.0f);
    }

}
