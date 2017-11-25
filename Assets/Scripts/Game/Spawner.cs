using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

  [SerializeField]
  private Transform motherShipPrefab;
  [SerializeField]
  private Transform enemyShipPrefab;
  [SerializeField]
  private int levels;
  [SerializeField]
  private Vector3 motherShipPosition;

  // Use this for initialization
  void Start () {
    GameObject motherShip = Instantiate(motherShipPrefab, motherShipPosition, Quaternion.identity).gameObject;
    Enemy motherEnemy = motherShip.GetComponent<Enemy>();
    motherEnemy.AddLevel(levels - 1, enemyShipPrefab);
  }

  // Update is called once per frame
  void Update () {

  }

}
