/*Function: Controls enemy movement/ interaction with player
* Status: Working/ Tested
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEnemy : MonoBehaviour {
	private Transform playerTransform;
	private Transform enemyTransform;
	private float turretRadius = 10.0f; //How far our turret enemies can see
	private EnemyShooting es;


	void Awake(){
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		es = gameObject.GetComponent<EnemyShooting>();
	}

	// Use this for initialization
	void Start () {
		Rigidbody2D rb = GetComponent<Rigidbody2D> ();
		rb.constraints = RigidbodyConstraints2D.FreezeAll;
	}
	
	// Update is called once per frame
	void Update () {
		guard_Area ();
	}

	void guard_Area(){
		if (Vector3.Distance (transform.position, playerTransform.position) < turretRadius) { //If player is in range of turret
			es.shoot_At_Player (); //Shoot um up
		}
	}

	//Return distance between player and enemy
	private float Distance(){
		return Vector3.Distance(enemyTransform.position, playerTransform.position);
	}
}
