/*Function: Controls enemy movement/ interaction with player
* Status: In progress/ Experimental
* Bugs: 
* -Weird patrolling enemy rotation when changing directions (because of lookat)
* -Jerky when resuming patrol after chase
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour {
	private bool chasingPlayer;
	private float delta = 5.0f; //How far we move left and right
	private float patrolSpeed = 0.5f; //How fast we move left and right
	private float chaseSpeed = 2.5f;
	private float chaseRadius = 7.0f; //How far we can see player
	private float escapeRadius = 9.0f; //How far player must be away to break the chase
	private Transform enemyTransform;
	private Transform playerTransform;
	private Vector3 enemyStartingPos;
	private EnemyShooting es;

	private void Awake(){
		enemyStartingPos = transform.position; //Initialize startingPos
		enemyTransform = this.transform; //Reference to current enemy (for testing)
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		es = gameObject.GetComponent<EnemyShooting>();
	}

	void Update () {
		switch (chasingPlayer) {
		case true:
			chase_Player ();
			break;
		case false:
			patrol_Area ();
			break;
		}
	}

	//Normal patrolling behaviour. Using sin function for side to side patrolling (may change)
	void patrol_Area(){
		Vector3 v = enemyStartingPos;
		v.x += delta * Mathf.Sin (Time.time * patrolSpeed);
		transform.position = v;	

		if(Distance() <= chaseRadius){
			chasingPlayer = true;
		}
	}

	//Off with his head!
	void chase_Player(){
		if(Distance() > escapeRadius){
			enemyStartingPos = transform.position; //Where enemy will resume if player escapes
			chasingPlayer = false;
		}
		transform.LookAt (playerTransform.position); //Face the player
		transform.Rotate (new Vector3(0,-90,0),Space.Self); //Correct original rotation

		if (Vector3.Distance (transform.position, playerTransform.position) > 3f) { //Move towards player until we are 3 units away (to avoid collision)
			transform.Translate (new Vector3 (chaseSpeed * Time.smoothDeltaTime, 0, 0));
		} 
		es.shoot_At_Player ();
	}

	//Return distance between player and enemy
	private float Distance(){
		return Vector3.Distance(enemyTransform.position, playerTransform.position);
	}
}
