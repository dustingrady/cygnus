using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueType : Enemy {

	private bool chasingPlayer;

	[SerializeField] 
	private float delta = 5.0f; //How far we move left and right
	[SerializeField] 
	private float patrolSpeed = 0.5f; //How fast we move left and right

	[SerializeField] 
	private float chaseRadius = 3.0f; //How far we can see player
	[SerializeField] 
	private float escapeRadius = 15.0f; //How far player must be away to break the chase
	[SerializeField] 
	private float followDistance = 1f; //How close to the player the enemy will get

	private float chaseSpeed;
	[SerializeField] 
	private float chaseBaseSpeed = 5f;
	[SerializeField] 
	private float sprintSpeed = 12f;
	[SerializeField] 
	private float sprintDuration = 0.6f;

	public LayerMask edgeCheck;

	private bool enraged = false; // When the enemy is shot, they persue the player for atleast two seconds
	private bool hidden = true;
	private GameObject smokePuff;

	List<string> avoidedTypes = new List<string> {"WaterElement", "FireElement"}; //Things we are allowed to walk on

	// Reference to coroutine, to refresh it
	private IEnumerator enragedCoroutine;

	void Start(){
		base.Start ();

		// Set the speed to the base speed
		chaseSpeed = chaseBaseSpeed;
		smokePuff = (GameObject)Resources.Load("Prefabs/Particles/SmokePuff");	

		rb.constraints = RigidbodyConstraints2D.FreezeRotation;
		hide_Self(hidden);
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


	bool check_Edge(){
		RaycastHit2D checkEdge = Physics2D.Raycast (new Vector2 (transform.position.x + patrolSpeed*-0.1f, transform.position.y), 
			new Vector2 (patrolSpeed*-1, -1).normalized, 2, edgeCheck);
		
		if (!checkEdge) {
			//Debug.Log ("not hitting something");
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
		hide_Self(false);
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
			reveal_Self(true); 
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

		if ((DistanceToPlayer () > followDistance) && !check_Edge ()) { //Move towards player until we are n unit(s) away unless that results in going over a ledge
			Vector3 oldpos = transform.position;
			transform.position = new Vector3 (Mathf.MoveTowards (transform.position.x, playerTransform.position.x, chaseSpeed * Time.deltaTime), transform.position.y, transform.position.z);
			float dv = transform.position.x - oldpos.x;

			//Debug.Log ((transform.position.x - oldpos.x) + " " + transform.localScale.x);
			if (Mathf.Sign (dv) == Mathf.Sign (transform.localScale.x)) {
				this.transform.localScale = new Vector3 (transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
			}
		}
	}
		

	/*Reveal self once player is in range*/
	private void reveal_Self(bool x){
		if (x) {
			sr.color = new Color (1f, 1f, 1f, 1f); //Change alpha to 1
			GameObject smoke = Instantiate (smokePuff, transform.position, Quaternion.identity); //Instantiate smoke screen
			Destroy (smoke, 2);
			hidden = false;
		}
	}


	/*Hide self once chase has ended*/
	private void hide_Self(bool x){
		if(!x){
			sr.color = new Color (1f, 1f, 1f, .2f); //Sneaky sneak
			hidden = true;
		}
	}


	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "TurnAround") {
			patrolSpeed *= -1;
		}
		if (damagingElements.Contains (col.gameObject.tag)) {

			// If the enemy is hidden, make it come out of stealth and chase the player
			if (hidden) {
				reveal_Self (true);
				chasingPlayer = true;

				// Make the enemy sprint to close the distance gap
				StartCoroutine(Sprint(sprintDuration));

			}

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


	IEnumerator Sprint(float duration) {
		chaseSpeed = sprintSpeed;
		yield return new WaitForSeconds (duration);
		chaseSpeed = chaseBaseSpeed;
	}
}