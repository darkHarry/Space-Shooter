using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{

    public GameObject EnemyBullet;

    public float speed;

    // Use this for initialization
    void Start()
    {
        speed = 10f - 2*gameObject.GetComponentInParent<Enemy>().Level();

        InvokeRepeating("FireEnemyBullet", speed*0.5f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Function to fire enemy bullet
    void FireEnemyBullet()
    {
        // Get a reference to Player's ship
        GameObject playerShip = GameObject.Find("PlayerGO");

        if (playerShip != null) // If player not dead
        {
            // Instantiate Enemy Bullet
            GameObject bullet = Instantiate(EnemyBullet).gameObject;

            // Set bullet's initial pos to enemy gun pos
            bullet.transform.position = transform.position;

            // Compute bullet's downward direction
            Vector2 direction = Vector3.down;

            // set bullet's direction
            bullet.GetComponent<EnemyBullet>().SetDirection(direction);

            // set bullet's speed
            bullet.GetComponent<EnemyBullet>().speed = speed;
        }
    }
}
