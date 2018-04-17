/*Function: Controls boss's interaction with player
* Status: In progress/ Testing
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : Enemy {
	public GameObject bossRagdoll;
	private Transform enemyTransform;
	private BossShooting bs;
	private float shootRadius = 15.0f; //How far our turret enemies can see

	// Use this for initialization
	void Start () {
		base.Start (); // Call the based enemy Start() function

		bs = gameObject.GetComponent<BossShooting>();
		rb.constraints = RigidbodyConstraints2D.FreezeAll;
	}


	void Update () {
		Guard_Area ();
		Check_Health ();
	}


	void Guard_Area() {
		if (Vector3.Distance (transform.position, playerTransform.position) < shootRadius) { //If player is in range
			bs.Determine_Attack();
		}
	}
		

	/*Are we still alive?*/
	private void Check_Health() {
		if(hitpoints <= 0){
			Destroy(this.gameObject);
			Instantiate (bossRagdoll, this.transform.position, Quaternion.identity);	//Instantiate dead boss
		}
	}
		

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.tag == "WaterElement") { //Only damageable by water
			takeDamage (edmg.determine_Damage (col.gameObject.tag, elementType));
		}
			

		if (col.gameObject.tag == "Deflected") {
			//health -= 50;
			Destroy (col.gameObject);
		}
	}

}
