using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour {

    public float speed;    // The bullet speed
    Vector2 dir;    // The bullet direction
    bool isReady;   // To check if bullet direction is set

    public float damage;

    // Setting default values
    void Awake()
    {
        isReady = false;
    }

	// Use this for initialization
	void Start () {

	}

    // Function to set Bullet's direction
    public void SetDirection(Vector2 direction)
    {
        // setting normalised unit vector direction
        dir = direction.normalized;
        isReady = true;
    }

	// Update is called once per frame
	void Update ()
    {
	    if (isReady)
        {
            // get Bullet's current position
            Vector2 position = transform.position;

            // compute new position
            position += dir * speed * Time.deltaTime;

            // update Bullet's position
            transform.position = position;

            // Top screen Vector
            Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

            // Bottom screen Vector
            Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));

            // if Bullet goes outside screen then destroy it

            if ((transform.position.x < min.x) || (transform.position.x > max.x) ||
                (transform.position.y < min.y) || (transform.position.y > max.y))
            {
                Destroy(gameObject);
            }
        }
	}

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            // Destroy the bullet
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
