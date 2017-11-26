using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

  [SerializeField] private float offsetFactor = 1f;
  [SerializeField] private float moveRate = 0.05f;

  public GameObject parent;
  public GameObject leftChild;
  public GameObject rightChild;

  [SerializeField] private float maximumHealth;
  private float currentHealth;
  [SerializeField] private float damage;

  public Vector3 requiredPosition;

  void Start () {
    currentHealth = maximumHealth;
    requiredPosition = transform.position;
  }

  void Update () {
    if (transform.rotation != Quaternion.identity) {
      transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, 0.1f);
    }
    if (transform.position != requiredPosition) {
      transform.position = Vector3.Slerp(transform.position, requiredPosition, moveRate);
    }
  }

  public void AddLevel(int level, Transform childPrefab) {
    if (level > 0) {
      // TODO: Use different values for childPrefab here
      Vector3 d = Vector3.down * offsetFactor;
      Vector3 l = Vector3.left * offsetFactor * Mathf.Pow(2, level - 2);
      Vector3 r = Vector3.right * offsetFactor * Mathf.Pow(2, level - 2);

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
    }
  }

  public void Die() {
    if (!parent) {
      // MOTHERSHIP IS DESTROYED, ALL HOPE IS LOST
      Debug.Log("YOU WIN!");
      Application.Quit();
    }

    // LEAF NODE, EZ PZ
    if (!leftChild && !rightChild) {
      Enemy parentEnemy = parent.GetComponent<Enemy>();
      if (parentEnemy.leftChild == gameObject) {
        parentEnemy.leftChild = null;
      } else {
        parentEnemy.rightChild = null;
      }
    }

    // TODO: Work on placement
    if (leftChild || rightChild) {
      if (leftChild && !rightChild) {
        // Single Child - Left
        RaiseChild(leftChild);
        leftChild.GetComponent<Enemy>().Reposition();
      } else if (rightChild && !leftChild) {
        // Single Child - Right
        RaiseChild(rightChild);
        rightChild.GetComponent<Enemy>().Reposition();
      } else {
        // Both children
        Enemy leftEnemy = leftChild.GetComponent<Enemy>();
        Enemy rightEnemy = rightChild.GetComponent<Enemy>();

        if (leftEnemy.Height() > rightEnemy.Height()) {
          RaiseChild(leftChild);
          leftEnemy.AddChildren(rightChild);
          leftEnemy.Reposition();
        } else {
          RaiseChild(rightChild);
          rightEnemy.AddChildren(leftChild);
          rightEnemy.Reposition();
        }
      }
    }

    Destroy(gameObject);
  }

  public void AddChildren(GameObject child) {
    if (gameObject == child) {
      Application.Quit();
    }

    Enemy childEnemy = child.GetComponent<Enemy>();
    if (!leftChild) {
      leftChild = child;
      childEnemy.parent = gameObject;
    } else if (!rightChild) {
      rightChild = child;
      childEnemy.parent = gameObject;
    } else {
      Enemy leftEnemy = leftChild.GetComponent<Enemy>();
      Enemy rightEnemy = rightChild.GetComponent<Enemy>();

      if (leftEnemy.Height() > rightEnemy.Height()) {
        rightEnemy.AddChildren(child);
      } else {
        leftEnemy.AddChildren(child);
      }
    }
  }

  private void RaiseChild(GameObject child) {
    Enemy childEnemy = child.GetComponent<Enemy>();
    Enemy parentEnemy = parent.GetComponent<Enemy>();
    childEnemy.parent = parent;
    if (parentEnemy.leftChild == gameObject) {
      parentEnemy.leftChild = child;
    } else if (parentEnemy.rightChild == gameObject) {
      parentEnemy.rightChild = child;
    } else {
      Application.Quit();
    }
  }

  private void OnCollisionEnter2D(Collision2D other) {
        Debug.Log("VEE");
    if (other.gameObject.tag == "Bullet") {
            Debug.Log("Hello");
      float dmg = other.gameObject.GetComponent<PlayerBullet>().damage;
      // Destroy the bullet
      // TODO: Make a mini-explosion?
      Destroy(other.gameObject);
      // TODO: Take damage etc.
      TakeDamage(dmg);
    }
  }

  public int Height() {
    if (!leftChild && !rightChild) {
      return 1;
    } else if (!leftChild) {
      return rightChild.GetComponent<Enemy>().Height() + 1;
    } else if (!rightChild) {
      return leftChild.GetComponent<Enemy>().Height() + 1;
    } else {
      return Mathf.Max(rightChild.GetComponent<Enemy>().Height(), leftChild.GetComponent<Enemy>().Height()) + 1;
    }
  }

  public void Reposition() {
    if (parent) {
      Vector3 d = Vector3.down * offsetFactor;
      Vector3 l = Vector3.left * offsetFactor * Mathf.Pow(2, Level() - 2);
      Vector3 r = Vector3.right * offsetFactor * Mathf.Pow(2, Level() - 2);

      Enemy parentEnemy = parent.GetComponent<Enemy>();
      Vector3 pp = parentEnemy.requiredPosition;

      if (parentEnemy.leftChild == gameObject) {
        requiredPosition = pp + d + l;
      } else if (parentEnemy.rightChild == gameObject) {
        requiredPosition = pp + d + r;
      }
    }

    // UGLY HACK --- SORRY
    if (leftChild && leftChild != gameObject) {
      leftChild.GetComponent<Enemy>().Reposition();
    }
    // UGLY HACK --- SORRY
    if (rightChild && rightChild != gameObject) {
      rightChild.GetComponent<Enemy>().Reposition();
    }
  }

  public int Level() {
    if (!parent) {
      return GameObject.Find("Spawner").GetComponent<Spawner>().levels;
    } else {
      return parent.GetComponent<Enemy>().Level() - 1;
    }
  }
}
