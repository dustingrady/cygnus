using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueType : Enemy {
	[SerializeField] 
	private float delta = 5.0f; //How far we move left and right
	[SerializeField] 
	private float patrolSpeed = 0.5f; //How fast we move left and right

	[SerializeField] 
	private float chaseRadius = 3.0f; //How far we can see player
	[SerializeField] 
	private float escapeRadius = 15.0f; //How far player must be away to break the chase

	private float chaseSpeed;
	[SerializeField] 
	private float chaseBaseSpeed = 5f;
	[SerializeField] 
	private float sprintSpeed = 12f;
	[SerializeField] 
	private float sprintDuration = 0.6f;

	public LayerMask edgeCheck;
	private Player p; //Testing

	[SerializeField] 
	private bool canStun = false;
	private bool firstHit = true;
	private bool chasingPlayer;
	private bool enraged = false; // When the enemy is shot, they persue the player for atleast two seconds
	private bool hidden = true;
	private bool disengaged = false;
	private bool fading = false;

	private GameObject smokePuff;

	List<string> avoidedTypes = new List<string> {"WaterElement", "FireElement", "Quicksand", "Acid"}; //Things we are allowed to walk on

	// Reference to coroutine, to refresh it
	private IEnumerator enragedCoroutine;

	void Start(){
		base.Start ();

		// Set the speed to the base speed
		chaseSpeed = chaseBaseSpeed;
		smokePuff = (GameObject)Resources.Load("Prefabs/Particles/SmokePuff");	

		rb.constraints = RigidbodyConstraints2D.FreezeRotation;
		cloak ();
		patrolSpeed = Mathf.Sign (Random.Range (-1, 1)) * patrolSpeed;
	}
		
	void Update(){
		EvaluateHealth ();
		EvaluateTolerance ();
		check_State ();
	}

	void check_State(){
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
		Gizmos.DrawRay (new Vector3(transform.position.x, transform.position.y, transform.position.z), new Vector3 (patrolSpeed*-1, -0.5f,0).normalized);
		//Gizmos.DrawRay (new Vector3(transform.position.x, transform.position.y, transform.position.z), new Vector3 (patrolSpeed*-1, 0,0).normalized);
	}

	bool check_Edge(){
		RaycastHit2D checkEdge = Physics2D.Raycast (new Vector2 (transform.position.x + patrolSpeed*-0.15f, transform.position.y), 
			new Vector2 (patrolSpeed*-1, -0.5f).normalized, 2, edgeCheck);
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
	void patrol_Area() {
		Vector3 v = startingPosition;
		float distanceFromPlayer = DistanceToPlayer ();

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
			
		if ((distanceFromPlayer <= chaseRadius) && within_LoS () && !check_Edge () && !disengaged) {
			// Aggro the player
			reveal_Self (); 
			chasingPlayer = true;
		} else if (distanceFromPlayer < chaseRadius * 2 && !chasingPlayer) {
			// Become visible to the player;
			uncloak ();
		} else if (distanceFromPlayer > chaseRadius * 2 && !chasingPlayer) {
			cloak ();
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

		if((DistanceToPlayer() > escapeRadius && !enraged) || check_Edge()){
			startingPosition = transform.position; //Where enemy will resume if player escapes
			firstHit = true;
			chasingPlayer = false;
		}

		if (!check_Edge ()) { //Move towards player unless that results in going over a ledge
			Vector3 oldpos = transform.position;
			transform.position = new Vector3 (Mathf.MoveTowards (transform.position.x, playerTransform.position.x, chaseSpeed * Time.deltaTime), transform.position.y, transform.position.z);
			float dv = transform.position.x - oldpos.x;

			//Debug.Log ((transform.position.x - oldpos.x) + " " + transform.localScale.x);
			if (Mathf.Sign (dv) == Mathf.Sign (transform.localScale.x)) {
				this.transform.localScale = new Vector3 (transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
			}
		}

		if(check_Edge()){
			if (gameObject.activeInHierarchy){
				StartCoroutine(break_Contact ());
			}
		}
	}
		

	/*Reveal self once player is in range*/
	void reveal_Self(){
		changeOpacity (1f); //Change alpha to 1
		GameObject smokeObj = Instantiate (smokePuff, transform.position, Quaternion.identity); //Instantiate smoke screen
		Destroy (smokeObj, 2);
		hidden = false;
	}


	/*Hide self once chase has ended*/
	void HideSelf() {
		Debug.Log("hiding self");
		changeOpacity (0.2f); //Sneaky sneak
		hidden = true;
	}


	void uncloak() {
		changeOpacity (0.2f);
	}

	void cloak() {
		changeOpacity (0f);
	}

	void changeOpacity(float opacity) {
		sr.color = new Color (sr.color.r, sr.color.g, sr.color.b, opacity);
	}


	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "TurnAround") {
			patrolSpeed *= -1;
		}
			
		if (damagingElements.Contains (col.gameObject.tag)) {

			// If the enemy is hidden, make it come out of stealth and chase the player
			if (hidden) {
				reveal_Self ();
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

	void OnCollisionEnter2D(Collision2D col) {
		float collisionTotal = EvaluatePhysical (col);

		if (canStun &&  firstHit && col.gameObject.tag == "Player") {
			GameObject player = GameObject.FindGameObjectWithTag ("Player");
			Player p = player.GetComponent<Player> ();
			p.stunned = true;
			firstHit = false;
			//p.StartCoroutine (stunDuration());
		}

		if (collisionTotal > 7) {
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
		if (hidden) {
			hidden = false;
			changeOpacity (1f);
			chasingPlayer = true;
		}

		// Make the enemy sprint to close the distance gap
		StartCoroutine(Sprint(sprintDuration));
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
		
	/*Allow enemy a moment to turn away from harmful contact before engaging player again*/
	IEnumerator break_Contact(){
		HideSelf();
		enraged = false;
		chasingPlayer = false;
		disengaged = true;
		patrolSpeed *= -1;
		yield return new WaitForSeconds (2.0f);
		disengaged = false;
	}

	IEnumerator Enrage(float duration) {
		enraged = true;
		chasingPlayer = true;

		yield return new WaitForSeconds (duration);

		enraged = false;
	}

	/*
	IEnumerator SpriteFade(float targetValue, float duration) {
		if (targetValue >= 0f && targetValue <= 1.0f) {
			fading = true;
			var SR = gameObject.GetComponent<SpriteRenderer> ();
			float opacity = SR.color.a;
			Debug.Log (Mathf.Abs (opacity - targetValue));
			while (Mathf.Abs (opacity - targetValue) < 0.01) {
				Debug.Log ("fading sprite in loop");
				opacity = opacity - 0.01f;
				SR.color = new Color (1f, 1f, 1f, opacity);
			}
		}
		yield return new WaitForSeconds (1f);
		fading = false;
	}
	*/

	IEnumerator Sprint(float duration) {
		chaseSpeed = sprintSpeed;
		yield return new WaitForSeconds (duration);
		chaseSpeed = chaseBaseSpeed;
	}
}