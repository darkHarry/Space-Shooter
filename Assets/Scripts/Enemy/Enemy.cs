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
    // TODO: Call `Destroy(gameObject)` with some explosions. Also, add some reward.
    Destroy(gameObject);

    if (!parent) {
      // MOTHERSHIP IS DESTROYED, ALL HOPE IS LOST
      Debug.Log("YOU WIN!");
    }

    if (parent && !leftChild && !rightChild) {
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
      } else if (rightChild && !leftChild) {
        // Single Child - Right
        RaiseChild(rightChild);
      } else {
        // Both children
        Enemy leftEnemy = leftChild.GetComponent<Enemy>();
        Enemy rightEnemy = rightChild.GetComponent<Enemy>();

        if (leftEnemy.Height() > rightEnemy.Height()) {
          RaiseChild(leftChild);
          rightEnemy.parent = leftChild;
          if (leftEnemy.rightChild) {
            rightEnemy.AddChildren(leftEnemy.rightChild);
          }
          leftEnemy.rightChild = rightChild;
        } else {
          RaiseChild(rightChild);
          leftEnemy.parent = rightChild;
          if (rightEnemy.leftChild) {
            leftEnemy.AddChildren(rightEnemy.leftChild);
          }
          rightEnemy.leftChild = leftChild;
        }
      }
    }
  }

  public void AddChildren(GameObject child) {
    Enemy childEnemy = child.GetComponent<Enemy>();
    if (!leftChild) {
      leftChild = child;
      childEnemy.parent = gameObject;
      childEnemy.Reposition();
    } else if (!rightChild) {
      rightChild = child;
      childEnemy.parent = gameObject;
      childEnemy.Reposition();
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
    } else {
      parentEnemy.rightChild = child;
    }
    childEnemy.Reposition();
  }

  private void OnCollisionEnter2D(Collision2D other) {
    if (other.gameObject.tag == "Bullet") {
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
      Enemy rightEnemy = rightChild.GetComponent<Enemy>();
      return rightEnemy.Height() + 1;
    } else if (!rightChild) {
      Enemy leftEnemy = leftChild.GetComponent<Enemy>();
      return leftEnemy.Height() + 1;
    } else {
      Enemy rightEnemy = rightChild.GetComponent<Enemy>();
      Enemy leftEnemy = leftChild.GetComponent<Enemy>();
      return Mathf.Max(rightEnemy.Height(), leftEnemy.Height()) + 1;
    }
  }

  private void Reposition() {
    Enemy parentEnemy = parent.GetComponent<Enemy>();
    Vector3 reqPos = parentEnemy.requiredPosition + Vector3.down * offsetFactor;

    if (parentEnemy.leftChild == gameObject) {
      reqPos += Vector3.left * offsetFactor * Mathf.Pow(2, Height() - 2);
    } else {
      reqPos += Vector3.right * offsetFactor * Mathf.Pow(2, Height() - 2);
    }
    requiredPosition = reqPos;

    if (leftChild) {
      leftChild.GetComponent<Enemy>().Reposition();
    }
    if (rightChild) {
      rightChild.GetComponent<Enemy>().Reposition();
    }
  }
}
