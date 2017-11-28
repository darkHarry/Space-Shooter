using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    public GameObject EnemyBullet;
    private float damage;
    public float speed;

    // Use this for initialization
    void Start()
    {
        damage = gameObject.GetComponentInParent<Enemy>().damage;
        speed = 2f;

        InvokeRepeating("FireEnemyBullet", 0.5f, (5f - gameObject.GetComponentInParent<Enemy>().Level()) / 2);
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
            EnemyBullet eBullet = bullet.GetComponent<EnemyBullet>();
            eBullet.damage = damage;
            eBullet.SetDirection(Vector3.down);
            eBullet.speed = speed;

            bullet.transform.position = transform.position;
    }
    }
}
