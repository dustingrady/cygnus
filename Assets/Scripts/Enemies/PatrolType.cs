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
	private EnemyShooting es;
	public LayerMask edgeCheck;

	private bool enraged = false; // When the enemy is shot, they persue the player for at least two seconds

	// Reference to coroutine, to refresh it
	private IEnumerator enragedCoroutine;

	List<string> avoidedTypes = new List<string> {"WaterElement", "FireElement"}; //Things we are allowed to walk on

	void Start(){
		base.Start ();

		es = gameObject.GetComponent<EnemyShooting>();
		patrolSpeed = Mathf.Sign (Random.Range (-1, 1)) * patrolSpeed;
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


	bool check_Edge(){
		RaycastHit2D checkEdge = Physics2D.Raycast (new Vector2 (transform.position.x + patrolSpeed*-0.1f, transform.position.y), 
			new Vector2 (patrolSpeed*-1, -1).normalized, 2, edgeCheck);
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


	bool check_Stuck(){
		RaycastHit2D checkFront = Physics2D.Raycast (new Vector2 (transform.position.x, transform.position.y), new Vector2 (patrolSpeed*-1, 0).normalized, 1, enemySight);
		//Debug.DrawRay (transform.position, new Vector3 (patrolSpeed*-1, 0, 0).normalized, Color.green);
		if(checkFront.collider != null){
			return true;
		}
		return false;
	}
		

	//Normal patrolling behaviour. Using sin function for side to side patrolling (may change)
	void patrol_Area(){
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
		if((DistanceToPlayer() <= chaseRadius) && within_LoS() && !check_Edge()){
			chasingPlayer = true;
		}

		if (check_Stuck()) { //Turn around if stuck
			StartCoroutine(idle());
			patrolSpeed *= -1;
		}
	}


	void chase_Player(){
		/*Corrects the patrolSpeed of enemy depending on which side the player is on (Fixes raycast error in check_Edge())*/
		if ((transform.position.x > playerTransform.position.x) && (Mathf.Sign(patrolSpeed)) == -1) {
			patrolSpeed *= -1;
		} 
		if((transform.position.x < playerTransform.position.x) && (Mathf.Sign(patrolSpeed)) == 1) {
			patrolSpeed *= -1;
		}

		if((DistanceToPlayer() > escapeRadius && enraged == false) || !within_LoS() || check_Edge()){
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
		Rigidbody2D collisionRB = col.gameObject.GetComponent<Rigidbody2D> ();
		if (collisionRB != null) {
			float colForce = CalculatePhysicalImpact (col.contacts [0].normal, col.relativeVelocity, collisionRB.mass);

			if (colForce > 3) {
				float dmg = edmg.determine_Damage ("EarthElement", elementType, colForce);
				takeDamage (dmg);

				// Stop the enrage coroutine and start another
				if (enragedCoroutine != null) {
					StopCoroutine (enragedCoroutine);
				}
				enragedCoroutine = Enrage (2.0f);
				StartCoroutine (enragedCoroutine);
			}
		}
	}


	//
	// Coroutines 
	//


	IEnumerator idle(){
		pause = true;
		yield return new WaitForSeconds (1);
		pause = false;
	}


	IEnumerator Enrage(float duration) {
		enraged = true;
		chasingPlayer = true;

		yield return new WaitForSeconds (duration);
		enraged = false;
	}
}
