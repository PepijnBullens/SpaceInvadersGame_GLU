using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // speed
    [SerializeField]
    private float speed = 10;

    // reference to the hit effect
    [SerializeField]
    private GameObject hitEffect;

    // start
    private void Start()
    {
        // destroy the object this script is attached to in 4 seconds
        Destroy(gameObject, 4);
    }

    // update
    private void Update()
    {
        // move bullet up
        transform.Translate(0f, speed * Time.deltaTime, 0f);
    }

    // 2D collision detecting function
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // create explosion effect
        Instantiate(hitEffect, transform.position, transform.rotation);
        // destroy bullet this script is attached to
        Destroy(gameObject);
    }
}
