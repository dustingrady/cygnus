/*Function: Controls boss's interaction with player
* Status: In progress/ Testing
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : Enemy {
	public GameObject bossRagdoll;
	private Transform enemyTransform;
	private Area1Boss bs;
	public float shootRadius = 15.0f; //How far our turret enemies can see

	// Use this for initialization
	void Start () {
		base.Start (); // Call the based enemy Start() function
		FloatingTextController.Initialize ();
		bs = gameObject.GetComponent<Area1Boss>();
		rb.constraints = RigidbodyConstraints2D.FreezeAll;
	}


	void Update () {
		if (within_LoS()) {
			Guard_Area ();
		}
		Check_Health ();
	}

	protected override bool within_LoS(){
		Vector2 start = transform.position;
		Vector2 direction = playerTransform.position - transform.position;
		float distance = shootRadius; //Distance in which raycast will check

		RaycastHit2D sightTest = Physics2D.Raycast (start, direction, distance, enemySight);
		if (sightTest) {
			if (sightTest.collider.CompareTag("Player")) {
				return true;
			}
		}
		return false;
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
		if (damagingElements.Contains(col.gameObject.tag)) { //Only damageable by water
			takeDamage (edmg.determine_Damage (col.gameObject.tag, elementType));
		}

		if (col.gameObject.tag == "Deflected") {
			hitpoints -= 50;

			Color baseClr = Color.yellow;
			baseClr.g -= 50 / 100;
			float dmgSize = Mathf.Lerp (18, 35, 50 / 80);
			FloatingTextController.CreateFloatingText ("50", this.gameObject.transform,dmgSize, Color.yellow, 20);

			Destroy (col.gameObject);
		}
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		float collisionTotal = EvaluatePhysical (col);
	}

	public float getHealth()
	{
		return hitpoints;
	}
}
