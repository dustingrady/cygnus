﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesertBoss : Enemy {

	private bool chasingPlayer;
	private bool stunned = false;
	private int tolerance = 0;
	private float delta = 5.0f; //How far we move left and right
	private float patrolSpeed = 1.5f; //How fast we move left and right
	private float chaseSpeed = 2.5f;
	private float chaseRadius = 5.0f; //How far we can see player
	private float escapeRadius = 30.0f; //How far player must be away to break the chase
	private EnemyShooting es;
	private EnemyDrop edrp;
	private EnemyDamage edmg;
	private GameObject sparks;
	private GameObject ebarrier;

	private Transform enemyTransform;
	private Transform playerTransform;
	private Vector3 enemyStartingPos;
	public LayerMask enemySight;

	// When the enemy is shot, they persue the player for atleast two seconds
	private bool enraged = false;
	// Reference to coroutine, to refresh it
	private IEnumerator enragedCoroutine;

	Rigidbody2D rb;
	CircleCollider2D cc;
	bool pause = false;

	private void Awake(){
		enemyStartingPos = transform.position; //Initialize startingPos
		enemyTransform = this.transform; //Reference to current enemy
		es = gameObject.GetComponent<EnemyShooting>();
		edrp = gameObject.GetComponent<EnemyDrop> ();
		edmg = gameObject.GetComponent<EnemyDamage> ();
		ebarrier = GameObject.Find ("DesertBossBarrier");
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		sparks = Resources.Load ("Prefabs/Particles/Sparks") as GameObject;
	}

	void Start(){
		rb = GetComponent<Rigidbody2D> ();
		cc = ebarrier.GetComponent<CircleCollider2D> ();
		rb.constraints = RigidbodyConstraints2D.FreezeRotation;
		patrolSpeed = Mathf.Sign (Random.Range (-1, 1)) * patrolSpeed;
	}

	void Update(){
		if (hitpoints <= 0) {
			edrp.determine_Drop (getEnemyType (), this.transform.position);
			Destroy (this.gameObject);
		}

		if (stunned == false) {
			switch (chasingPlayer) {
			case true:
				chase_Player ();
				break;
			case false:
				patrol_Area ();
				break;
			}
		}

		if (tolerance == 20) {
			stunned = true;
			Instantiate (sparks, this.transform.position, Quaternion.identity);
			StartCoroutine (stunDuration ());
		}

		if (tolerance == 200) {
			tolerance = 0;
		}

		if (stunned == false) {
			switch (chasingPlayer) {
			case true:
				chase_Player ();
				break;
			case false:
				patrol_Area ();
				break;
			}
		}

		Barrier ();
	}

	void Barrier() {
		if (GameObject.Find ("DesertBossBarrier").GetComponent<BossBarrier> ().godlike) {
			
		}
	}

	bool within_LoS(){
		Vector2 start = transform.position;
		Vector2 direction = playerTransform.position - transform.position;
		float distance = chaseRadius; //Distance in which raycast will check
		//Debug.DrawRay(start, direction, Color.red,2f,false);
		RaycastHit2D sightTest = Physics2D.Raycast (start, direction, distance, enemySight);
		if (sightTest) {
			if (sightTest.collider.CompareTag("Player")) {
				return true;
			}
		}
		return false;
	}

	//Normal patrolling behaviour. Using sin function for side to side patrolling (may change)
	void patrol_Area(){
		Vector3 v = enemyStartingPos;
		if ((Mathf.Abs(transform.position.x - v.x) < delta) && !pause) {
			transform.Translate (new Vector2 (patrolSpeed, 0) * Time.deltaTime);

			if (Mathf.Sign (patrolSpeed) != Mathf.Sign (transform.localScale.x)) {
				this.transform.localScale = new Vector3 (transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
			}

			if (Mathf.Abs (Mathf.Abs (transform.position.x - v.x) - delta) <= 0.5){
				StartCoroutine (idle ());
				patrolSpeed *= -1;
			}
		}

		if(Distance() <= chaseRadius && within_LoS()){
			chasingPlayer = true;
		}
	}

	//Off with his head!
	void chase_Player(){
		if(Distance() > escapeRadius && enraged == false || !within_LoS()){
			enemyStartingPos = transform.position; //Where enemy will resume if player escapes
			chasingPlayer = false;
		}
		//Follow player in x-axis
		if (Distance () > 3f) { //Move towards player until we are n unit(s) away (to avoid collision)
			Vector3 oldpos = transform.position;
			transform.position = new Vector3(Mathf.MoveTowards(transform.position.x, playerTransform.position.x, chaseSpeed * Time.deltaTime), transform.position.y, transform.position.z);
			float dx = transform.position.x - oldpos.x;

			if (Mathf.Sign (dx) == Mathf.Sign (transform.localScale.x)) {
				this.transform.localScale = new Vector3 (transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
			}
		}
		//Follow player in y-axis
		if(Mathf.Abs(enemyTransform.position.y - playerTransform.position.y) > 2f){
			//Debug.Log ("Y distance trigger"); //Testing
			transform.position = new Vector3(transform.position.x, Mathf.MoveTowards(transform.position.y, playerTransform.position.y, chaseSpeed * Time.deltaTime), transform.position.z);
		}
		if (within_LoS ()) {
			//es.shoot_At_Player ();
		}
	}

	//Return distance between player and enemy
	private float Distance(){
		return Vector3.Distance(transform.position, playerTransform.position);
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "TurnAround") {
			patrolSpeed *= -1;
		}
		if (damagingElements.Contains (col.gameObject.tag)) {
			takeDamage (edmg.determine_Damage (col.gameObject.tag, getEnemyType ()));

			// Stop the enrage coroutine and start another
			if (enragedCoroutine != null) {
				StopCoroutine (enragedCoroutine);
			}

			enragedCoroutine = Enrage (2.0f);
			StartCoroutine (enragedCoroutine);
		}
	}

	//particle collision for electricity
	void OnParticleCollision(GameObject other){
		if (other.tag == "ElectricElement") {
			takeDamage (0.5f);
		}
	}

	IEnumerator idle(){
		pause = true;
		yield return new WaitForSeconds (1);
		pause = false;
	}

	IEnumerator damage(float amount){
		hitpoints -= amount;
		yield return flash ();
		yield return new WaitForSeconds (1);
	}

	public override void takeDamage(float amount){
		StartCoroutine (damage (amount));
	}

	IEnumerator stunDuration(){
		yield return new WaitForSeconds (2);
		stunned = false;
	}

	IEnumerator Enrage(float duration) {
		enraged = true;
		chasingPlayer = true;
		Debug.Log ("now enraged");

		yield return new WaitForSeconds (duration);

		enraged = false;
		Debug.Log ("no longer enraged");
	}
}