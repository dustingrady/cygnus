/*Function: Controls enemy movement/ interaction with player
* Status: In progress
* Bugs: 
* -Enemy refuses to be locked to y axis, resulting in some bouncing
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingEnemy : MonoBehaviour {
	private bool chasingPlayer;
	private float delta = 5.0f; //How far we move left and right
	private float patrolSpeed = 1.5f; //How fast we move left and right
	private float chaseSpeed = 3.5f;
	private float chaseRadius = 5.0f; //How far we can see player
	private float escapeRadius = 10.0f; //How far player must be away to break the chase
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

	private void Start(){
		Rigidbody2D rb = GetComponent<Rigidbody2D> ();
		rb.constraints = RigidbodyConstraints2D.FreezeRotation;
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

		if (Vector3.Distance (transform.position, playerTransform.position) > 1f) { //Move towards player until we are 1 unit away (to avoid collision)
			//transform.InverseTransformDirection(playerTransform.position);
			transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, chaseSpeed * Time.deltaTime);
		
		
			if(transform.position.x < playerTransform.position.x){
				transform.rotation = Quaternion.identity;
			}
			else{
				transform.rotation = Quaternion.Euler (0, 180, 0);
			}
		
		
		} 
		es.shoot_At_Player ();
	}

	//Return distance between player and enemy
	private float Distance(){
		return Vector3.Distance(enemyTransform.position, playerTransform.position);
	}
}
