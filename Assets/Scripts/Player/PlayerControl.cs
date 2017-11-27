using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    public GameObject PlayerBulletGO;
    public GameObject bulletPosition01;
    public GameObject bulletPosition02;

    [SerializeField] private float maximumHealth;
    private float currentHealth;

    // player speed
    public float speed;

    // Use this for initialization
    void Start ()
    {
      currentHealth = maximumHealth;
    }

    // Update is called once per frame
    void Update ()
    {
        // fire bullet when space key pressed
        if (Input.GetKeyDown("space"))
        {
            // Instantiate the first bullet
            GameObject bullet01 = (GameObject)Instantiate(PlayerBulletGO);
            bullet01.transform.position = bulletPosition01.transform.position;  // set pos of bullet01

            // Instantiate the second bullet
            GameObject bullet02 = (GameObject)Instantiate(PlayerBulletGO);
            bullet02.transform.position = bulletPosition02.transform.position;  // set pos of bullet02
        }


        float x = Input.GetAxisRaw("Horizontal");   // -1, 0, 1 : left, no input, right
        float y = Input.GetAxisRaw("Vertical");     // -1, 0, 1 : down, no input, up

        // Compute Direction vector from Input and normalise it to get Unit Vector
        Vector2 direction = new Vector2(x, y).normalized;

        // Move player towards the direction
        Move(direction);
    }

    void Move (Vector2 direction)
    {
        // Screen Limits
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        // For Player Sprite to remain in screen

        max.x -= 0.225f;    // Subtract half width of player sprite from max.x
        min.x += 0.225f;    // Add half widht of player sprite to min.x

        max.y -= 0.285f;    // Subtract half height of player sprite from max.y
        min.y += 0.285f;    // Add half height of player sprite to min.y

        // Get player's current position
        Vector2 pos = transform.position;

        // Calculate new position
        pos += direction * speed * Time.deltaTime;

        // Make sure new pos is inside the screen
        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(pos.y, min.y, max.y);

        // Update player's position
        transform.position = pos;
    }

    public void TakeDamage(float value) {
      currentHealth -= value;
      if (currentHealth <= 0) {
        Die();
      }
    }

    public void Die() {
      Debug.Log("YOU LOSE!");
      Destroy(gameObject);
      UnityEditor.EditorApplication.isPlaying = false;
      return;
    }

    private void OnCollisionEnter2D(Collision2D other) {
      if (other.gameObject.tag == "EnemyBullet") {
        float dmg = other.gameObject.GetComponent<EnemyBullet>().damage;
        Debug.Log(dmg);
        Destroy(other.gameObject);
        TakeDamage(dmg);
      }

      if (other.gameObject.tag == "EnemyShip") {
        Destroy(other.gameObject);
        Die();
      }
    }
}
