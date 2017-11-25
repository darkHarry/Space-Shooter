using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : MonoBehaviour {

    public GameObject EnemyBullet;

	// Use this for initialization
	void Start ()
    {
        InvokeRepeating("FireEnemyBullet", 1.0f, 0.5f);	
	}
	
	// Update is called once per frame
	void Update () {
		
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

            // Compute bullet's direction towards player
            Vector2 direction = playerShip.transform.position - bullet.transform.position;

            // set bullet's direction
            bullet.GetComponent<EnemyBullet>().SetDirection(direction);
        }
    }
}
