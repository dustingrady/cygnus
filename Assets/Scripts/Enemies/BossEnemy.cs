/*Function: Controls boss's interaction with player
* Status: In progress/ Testing
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : Enemy {
	[SerializeField]
	// int health;
	public GameObject bossRagdoll;
	private Transform playerTransform;
	private Transform enemyTransform;
	private BossShooting bs;
	private Vector2 enemyStartingPos;
	private float shootRadius = 15.0f; //How far our turret enemies can see
	private EnemyDamage edmg;


	void Awake(){
		//health.Initalize;
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		enemyStartingPos = transform.position; //Initialize startingPos
		bs = gameObject.GetComponent<BossShooting>();
		edmg = gameObject.GetComponent<EnemyDamage> ();
	}

	// Use this for initialization
	void Start () {
		Rigidbody2D rb = GetComponent<Rigidbody2D> ();
		rb.constraints = RigidbodyConstraints2D.FreezeAll;
	}

	// Update is called once per frame
	void Update () {
		Guard_Area ();
		Check_Health ();
	}
		
	void Guard_Area(){
		if (Vector3.Distance (transform.position, playerTransform.position) < shootRadius) { //If player is in range
			bs.Determine_Attack();
		}
	}

	//Return distance between player and enemy
	private float Distance(){
		return Vector3.Distance(enemyTransform.position, playerTransform.position);
	}

	/*Are we still alive?*/
	private void Check_Health(){
		if(hitpoints <= 0){
			Destroy(this.gameObject);
			Instantiate (bossRagdoll, this.transform.position, Quaternion.identity);	//Instantiate dead boss
		}
	}
		
	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "WaterElement") { //Only damageable by water
			takeDamage (edmg.determine_Damage (col.gameObject.tag, getEnemyType ()));
		}

		/*
		if (col.gameObject.tag == "WaterElement") {
			health -= 2;
		}
		*/

		if (col.gameObject.tag == "Deflected") {
			//health -= 50;
			Destroy (col.gameObject);
		}
	}

	IEnumerator damage(float amount){
		hitpoints -= amount;
		yield return flash ();
		yield return new WaitForSeconds (1);
	}

	public override void takeDamage(float amount){
		StartCoroutine (damage (amount));
	}

}
