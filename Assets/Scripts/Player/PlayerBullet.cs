using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour {

    // Bullet speed
    float speed;
    public float damage = 10f;

    // Use this for initialization
    void Start ()
    {
        speed = 8f;
    }

    // Update is called once per frame
    void Update ()
    {
        // get the bullet's current pos
        Vector2 pos = transform.position;

        // compute bullet's new pos
        pos = new Vector2(pos.x, pos.y + speed * Time.deltaTime);

        // Update bullet's pos
        transform.position = pos;

        // Top screen Vector
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        // if bullet goes out of screen then destroy it
        if (transform.position.y > max.y)
        {
            Destroy(gameObject);
        }
    }
}
