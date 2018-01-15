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
	LineRenderer line;


	void Awake(){
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		es = gameObject.GetComponent<EnemyShooting>();
	}

	// Use this for initialization
	void Start () {
		Rigidbody2D rb = GetComponent<Rigidbody2D> ();
		line = this.gameObject.GetComponent<LineRenderer>();
		rb.constraints = RigidbodyConstraints2D.FreezeAll;


	}
	
	// Update is called once per frame
	void Update () {
		guard_Area ();
	}

	void guard_Area(){
		if (Vector3.Distance (transform.position, playerTransform.position) < turretRadius) { //If player is in range of turret
			//line.positionCount (2);
			line.enabled = true;
			line.SetPosition (0, transform.position);
			line.SetPosition (1, playerTransform.position);
			es.shoot_At_Player (); //Shoot um up
		} else {
			line.enabled = false;
		}
	}

	//Return distance between player and enemy
	private float Distance(){
		return Vector3.Distance(enemyTransform.position, playerTransform.position);
	}
		
}
