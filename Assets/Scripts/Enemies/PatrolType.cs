using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolType : Enemy {

	private bool chasingPlayer;

	[SerializeField]
	private float delta = 10.0f; //How far we move left and right
	[SerializeField]
	private float patrolSpeed = 1.5f; //How fast we move left and right
	[SerializeField]
	private float chaseSpeed = 4f;
	[SerializeField]
	private float chaseRadius = 6.0f; //How far we can see player
	[SerializeField]
	private float escapeRadius = 12.0f; //How far player must be away to break the chase
	[SerializeField]
	private float followDistance = 1.25f; //How close to the player the enemy will get
	[SerializeField]
	private float turnAroundPoll = 0.5f; //Polling value for checking if enemy is stuck

	[SerializeField] 
	private bool canShoot = false;
	private bool enraged = false; // When the enemy is shot, they persue the player for at least two seconds
	private bool isAlerted = false;
	private bool disengaged = false;
	private GameObject alert;
	private EnemyShooting es;
	public LayerMask edgeCheck;

	private Animator anim;

	// Reference to coroutine, to refresh it
	private IEnumerator enragedCoroutine;

	List<string> avoidedTypes = new List<string> {"WaterElement", "FireElement", "Quicksand", "Acid"}; //Things we are allowed to walk on

	void Start(){
		base.Start ();
		alert = (GameObject)Resources.Load("Prefabs/NPCs/alert");	
		es = gameObject.GetComponent<EnemyShooting>();
		patrolSpeed = Mathf.Sign (Random.Range (-1, 1)) * patrolSpeed;

		anim = GetComponent<Animator> ();
	}
		
	void Update(){
		EvaluateHealth ();
		EvaluateTolerance ();

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
	}


	//THIS IS DEBUG RAY
	void OnDrawGizmosSelected(){
		Gizmos.color = Color.red;
		Gizmos.DrawRay (new Vector3(transform.position.x, transform.position.y, transform.position.z), new Vector3 (patrolSpeed*-1, -1,0).normalized);
		//Gizmos.DrawRay (new Vector3(transform.position.x, transform.position.y, transform.position.z), new Vector3 (patrolSpeed*-1, 0,0).normalized);
	}


	private bool check_Edge(){
		RaycastHit2D checkEdge = Physics2D.Raycast (new Vector2 (transform.position.x + patrolSpeed*-0.1f, transform.position.y), 
			new Vector2 (patrolSpeed*-1, -1).normalized, 25, edgeCheck);
		if (!checkEdge) {
			return true;
		}

		if (avoidedTypes.Contains(checkEdge.collider.transform.gameObject.tag)) { //About to step on something we shouldn't
			//Debug.Log("Hit some " + checkEdge.collider.transform.gameObject.name + " turning around");
			return true;
		}

		// Check if approaching enemy
		if (checkEdge.collider.transform.CompareTag ("Enemy")) {
			return true;
		}
		return false; //No edge
	}


	private bool check_Stuck(){
		RaycastHit2D checkFront = Physics2D.Raycast (new Vector2 (transform.position.x, transform.position.y), new Vector2 (patrolSpeed*-1, 0).normalized, 1, enemySight);
		//Debug.DrawRay (transform.position, new Vector3 (patrolSpeed*-1, 0, 0).normalized, Color.green);
		if(checkFront.collider != null){
			return true;
		}
		return false;
	}
		

	//Normal patrolling behaviour. Using sin function for side to side patrolling (may change)
	private void patrol_Area(){
		Vector3 v = startingPosition;

		if ((Mathf.Abs(transform.position.x - v.x) < delta) && !pause) {
			transform.Translate (new Vector2 (patrolSpeed, 0) * Time.deltaTime);

			if (Mathf.Sign (patrolSpeed) != Mathf.Sign (transform.localScale.x)) {
				this.transform.localScale = new Vector3 (transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
			}
			if ((Mathf.Abs (Mathf.Abs (transform.position.x - v.x) - delta) <= 1.5f) || check_Edge()){
				StartCoroutine (idle ());
				patrolSpeed *= -1;
			}
		}

		if((DistanceToPlayer() <= chaseRadius) && within_LoS() && !check_Edge() && !disengaged){
			alerted(true);
			chasingPlayer = true;
		}

		if (check_Stuck()) { //Turn around if stuck
			StartCoroutine(idle());
			patrolSpeed *= -1;
		}
	}


	private void chase_Player(){
		/*Corrects the patrolSpeed of enemy depending on which side the player is on (Fixes raycast error in check_Edge())*/
		if ((transform.position.x > playerTransform.position.x) && (Mathf.Sign(patrolSpeed)) == -1) {
			patrolSpeed *= -1;
		} 
		if((transform.position.x < playerTransform.position.x) && (Mathf.Sign(patrolSpeed)) == 1) {
			patrolSpeed *= -1;
		}

		if((DistanceToPlayer() > escapeRadius && !enraged) || !within_LoS() || check_Edge()){
			StartCoroutine(break_Contact ());
			startingPosition = transform.position; //Where enemy will resume if player escapes
			chasingPlayer = false;
		}

		if ((DistanceToPlayer () > followDistance) && !check_Edge()) { //Move towards player until we are n unit(s) away unless that results in going over a ledge
			Vector3 oldpos = transform.position;
			transform.position = new Vector3(Mathf.MoveTowards(transform.position.x, playerTransform.position.x, chaseSpeed * Time.deltaTime), transform.position.y, transform.position.z);
			float dv = transform.position.x - oldpos.x;

			//Debug.Log ((transform.position.x - oldpos.x) + " " + transform.localScale.x);
			if (Mathf.Sign (dv) == Mathf.Sign (transform.localScale.x)) {
				this.transform.localScale = new Vector3 (transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
			}
		}
		if (canShoot) {
			if (within_LoS()) {
				es.shoot_At_Player ();
			} 
			followDistance = 4.0f; //Don't get so close when shooting
		}
	}

	/*Display exclamation point above enemy*/
	private void alerted(bool x){
		if (x) {

			float alertHeight = GetComponent<Collider2D>().bounds.extents.y + 0.5f;
			GameObject alertedObj = Instantiate (alert, new Vector2(transform.position.x, transform.position.y + alertHeight), Quaternion.identity); //Instantiate exclamation point
			//SpriteRenderer alertSprite = alertedObj.GetComponent<SpriteRenderer> (); //For fadeout
			StartCoroutine(fade_Out(alertedObj)); 
			alertedObj.transform.parent = this.transform;
			Destroy (alertedObj, 1.25f);
		}
		isAlerted = false;
	}

	void OnTriggerEnter2D(Collider2D col){
		if (damagingElements.Contains (col.gameObject.tag)) {
			takeDamage (edmg.determine_Damage (col.gameObject.tag, elementType));

			// Stop the enrage coroutine and start another
			if (enragedCoroutine != null) {
				StopCoroutine (enragedCoroutine);
			}
			enragedCoroutine = Enrage (2.0f);
			StartCoroutine (enragedCoroutine);
		}
	}


	// Particle collision for electricity
	void OnParticleCollision(GameObject other){
		ElectricShock (other.tag);
	}


	void OnCollisionEnter2D(Collision2D col) {
		float collisionTotal = EvaluatePhysical (col);

		if (collisionTotal > 7) {
			// Stop the enrage coroutine and start another
			if (enragedCoroutine != null) {
				StopCoroutine (enragedCoroutine);
			}
			enragedCoroutine = Enrage (2.0f);
			StartCoroutine (enragedCoroutine);
		}
	}


	//
	// Coroutines 
	//

	/*
	IEnumerator fade_Out(GameObject x){
		SpriteRenderer passed = x.GetComponent<SpriteRenderer> ();

		float time = 1f;
		while(passed.color.a > 0){
			//passed.color.a -= Time.deltaTime / time;
			yield return null;
		}
	}
	*/

	IEnumerator idle(){
		pause = true;
		anim.SetBool ("idle", true);
		yield return new WaitForSeconds (1);
		pause = false;
		anim.SetBool ("idle", false);
	}

	IEnumerator fade_Out(GameObject x){
		SpriteRenderer passed = x.GetComponent<SpriteRenderer> ();
		float time = 1f;
		while(passed.color.a > 0){
			passed.color = new Color(passed.color.r, passed.color.g, passed.color.b, passed.color.a - (Time.deltaTime / time));
			//passed.color -= Time.deltaTime / time;
			yield return null;
		}
	}

	/*Allow enemy a moment to turn away from harmful contact before engaging player again*/
	IEnumerator break_Contact(){
		enraged = false;
		chasingPlayer = false;
		disengaged = true;
		patrolSpeed *= -1;
		yield return new WaitForSeconds (1.25f);
		disengaged = false;
	}

	IEnumerator Enrage(float duration) {
		enraged = true;
		chasingPlayer = true;

		yield return new WaitForSeconds (duration);
		enraged = false;
	}
}
