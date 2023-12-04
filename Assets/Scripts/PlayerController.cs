using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 1f;

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField] 
    private GameManager game;

    private void Awake()
    {
        game = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        float direction = Input.GetAxisRaw("Horizontal");

        transform.Translate(direction * Time.deltaTime * movementSpeed, 0f, 0f);

        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        bullet.layer = gameObject.layer;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        game.ReportPlayerDeath();
        Destroy(gameObject);
    }
}
