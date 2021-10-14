using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public void Awake()
    {
        Destroy(this, 2f);
    }

    void OnTriggerEnter(Collider collision)
    {
        Debug.Log("HIT");
    }
}
