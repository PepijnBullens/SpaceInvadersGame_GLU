using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // game
    [SerializeField] 
    private GameManager game;

    // can attack?
    [SerializeField] 
    private bool attacks;

    // health
    [SerializeField]
    private int health = 1;

    // bullet to spawn
    [SerializeField]
    private GameObject bulletPrefab;

    // shoot attack interval variables
    [SerializeField]
    private float minAttackTime = 1f;
    [SerializeField]
    private float maxAttackTime = 5f;
    private float attackTimer;
    private float attackInterval;

    // where to spawn bullet and what rotation
    [SerializeField]
    private Transform bulletSpawnPoint;

    // amount of points gained
    [SerializeField]
    private int points;


    // awake
    private void Awake()
    {
        // find game manager
        game = FindObjectOfType<GameManager>();
    }

    // start
    private void Start()
    {
        // set random interval to shoot
        attackInterval = Random.Range(minAttackTime, maxAttackTime);
    }

    private void Update()
    {
	    //if enemy cant attack return and stop going further in update
        if (!attacks) return;
        
        // add to attack timer
        attackTimer += Time.deltaTime;

        // if timer reached goal shoot and set timer back to zero
        if(attackTimer >= attackInterval)
        {
            Shoot();
            attackTimer = 0;
        }
    }

    // function to shoot
    private void Shoot()
    {
        // create bullet instance at bullet spawn point
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        // set bullet layer to enemies layer so we can disable colliding between enemy and bullet
        bullet.layer = gameObject.layer;

        // again set a random interval to shoot
        attackInterval = Random.Range(minAttackTime, maxAttackTime);
    }

    // 2D collision detecting function
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // remove health
        health--;  

        // if health is zero or less add points to palyer score and remove enemy this script is attached to
        if(health <= 0)
        {
            game.AddScore(points);
            game.RemoveEnemy(this);
        }
    }
}
