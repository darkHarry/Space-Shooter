using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

  [SerializeField]
  private float offsetFactor = 1f;

  public GameObject parent;
  public GameObject leftChild;
  public GameObject rightChild;

  [SerializeField]
  private float maximumHealth;
  private float currentHealth;
  [SerializeField]
  private float damage;

  // Use this for initialization
  void Start () {
    currentHealth = maximumHealth;
  }

  // Update is called once per frame
  void Update () {

  }

  public void AddLevel(int level, Transform childPrefab) {
    if (level > 0) {
      // TODO: Use different values for childPrefab here
      Vector3 d = Vector3.down * offsetFactor;
      Vector3 l = Vector3.left * offsetFactor;
      Vector3 r = Vector3.right * offsetFactor;

      leftChild = Instantiate(childPrefab, transform.position + d + l, Quaternion.identity).gameObject;
      rightChild = Instantiate(childPrefab, transform.position + d + r, Quaternion.identity).gameObject;

      Enemy lc = leftChild.GetComponent<Enemy>();
      Enemy rc = rightChild.GetComponent<Enemy>();
      lc.AddLevel(level - 1, childPrefab);
      rc.AddLevel(level - 1, childPrefab);

      lc.parent = gameObject;
      rc.parent = gameObject;
    }
  }

  public void TakeDamage(float value) {
    currentHealth -= value;
    if (currentHealth <= 0) {
      Die();
      // TODO: Also balance tree
    }
  }

  private void Die() {
    Debug.Log("DIED");
    // TODO: Call `Destroy(gameObject)` with some explosions. Also, add some reward.
    Destroy(gameObject);
  }

  public void Heal(int value) {
    currentHealth += value;
    if (currentHealth > maximumHealth) {
      currentHealth = maximumHealth;
    }
  }

  void OnCollisionEnter2D(Collision2D other) {
    if (other.gameObject.tag == "Bullet") {
      float dmg = other.gameObject.GetComponent<PlayerBullet>().damage;
      // Destroy the bullet
      // TODO: Make a mini-explosion?
      Destroy(other.gameObject);
      // TODO: Take damage etc.
      TakeDamage(dmg);
    }
  }
}
