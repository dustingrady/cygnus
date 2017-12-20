/*Function: Controls enemy movement/ interaction with player
* Status: In progress/ Untested
* Bugs:
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {
	private Vector3 enemyStartingPos; //We will need to return here if we lose line of sight with player
	private bool chasingPlayer;
	private float delta = 3.0f; //How far we move left and right
	private float patrolSpeed = 2.0f; //How fast we move left and right
	private float chaseSpeed = 3.0f;
	private float chaseRadius = 3.0f;
	private float escapeRadius = 7.0f;
	private Transform t;
	private Transform player;


	private void Awake(){
		enemyStartingPos = transform.position; //Initialize startingPos
		t = this.transform; //Reference to current enemy (for testing)
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}

	void Update () {
		test_Function ();

		switch(chasingPlayer){
			case true:
				chase_Player ();
				break;
			case false:
				patrol_Area ();
				break;
		}
	}

	//Normal patrolling behaviour
	void patrol_Area(){
		//Using sin function for side to side patrolling, may want to change
		Vector3 v = enemyStartingPos;
		v.x += delta * Mathf.Sin (Time.time * patrolSpeed);
		transform.position = v;	

		if(Distance() <= chaseRadius){
			chasingPlayer = true;
		}
	}

	//Off with his head!
	void chase_Player(){
		Debug.Log ("Initiate chase protocol!");

		if(Distance() > escapeRadius){
			chasingPlayer = false;
		}
		transform.LookAt (player.position); //Face the player
		transform.Rotate (new Vector3(0,-90,0),Space.Self); //Correct original rotation
		if (Vector3.Distance (transform.position, player.position) > 2f) { //If we are more than 1 unit away from player
			transform.Translate (new Vector3 (chaseSpeed * Time.smoothDeltaTime, 0, 0)); //Move towards player
		} 
		//Else we can attack
	}

	//Go back to spawn point if we lose track of player
	void go_Home(){

	}

	private float Distance(){
		return Vector3.Distance(t.position, player.position);
	}

	//For testing purposes
	void test_Function(){
		if (player)
			print (player.name + " is " + Distance ().ToString () + " units from " + t.name);
		else {
			print ("Player not found!");
		}
	}
}
