using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : Enemy {

	private bool chasingPlayer;

	[SerializeField]
	private float delta = 5.0f; //How far we move left and right
	[SerializeField]
	private float patrolSpeed = 1.5f; //How fast we move left and right
	[SerializeField]
	private float chaseSpeed = 7.5f;
	[SerializeField]
	private float chaseRadius = 12.0f; //How far we can see player
	[SerializeField]
	private float escapeRadius = 20.0f; //How far player must be away to break the chase
	public float moveRetargetFreq = 2f;

	private GameObject alert;
	private EnemyShooting es;

	private Vector3 currentVel = Vector3.zero;
	private Vector3 target = Vector3.zero;
	private Vector3 targetOffset = Vector3.zero;

	private bool isAlerted = false;
	private bool enraged = false; // When the enemy is shot, they persue the player for atleast two seconds

	// Reference to coroutine, to refresh it
	private IEnumerator enragedCoroutine;
	List<string> avoidedTypes = new List<string> {"WaterElement", "FireElement", "ElectricElement", "MetalElement", "EarthElement", "Ice"}; //Things we are allowed to collide with

	void Start(){
		base.Start ();
		StartCoroutine (ChangeTargetPos(moveRetargetFreq));
		rb.constraints = RigidbodyConstraints2D.FreezeRotation;
		alert = (GameObject)Resources.Load("Prefabs/NPCs/alert");	
		es = gameObject.GetComponent<EnemyShooting>();
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


	//Normal patrolling behaviour. Using sin function for side to side patrolling (may change)
	void patrol_Area(){
		Vector3 v = startingPosition;
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

		if(DistanceToPlayer() <= chaseRadius && within_LoS()){
			//alerted(true);
			chasingPlayer = true;
		}
	}


	void chase_Player(){
		if(DistanceToPlayer() > escapeRadius && enraged == false || !within_LoS()){
			//startingPosition = transform.position; //Where enemy will resume if player escapes
			chasingPlayer = false;
		}

		if (within_LoS ()) {
			es.shoot_At_Player ();
		}

		target = playerTransform.position + targetOffset;
		transform.position = Vector3.SmoothDamp (transform.position, target, ref currentVel, (chaseSpeed));
	}

	/*Display exclamation point above enemy*/
	private void alerted(bool x){
		if (x) {
			GameObject alertedObj = Instantiate (alert, new Vector2(transform.position.x, transform.position.y + 1), Quaternion.identity); //Instantiate exclamation point
			SpriteRenderer alertSprite = alertedObj.GetComponent<SpriteRenderer> (); //For fadeout
			alertedObj.transform.parent = this.transform;
			Destroy (alertedObj, 1.25f);
		}
		isAlerted = false;
	}


	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "TurnAround") {
			patrolSpeed *= -1;
		}
		if (damagingElements.Contains (col.gameObject.tag)) {
			takeDamage (edmg.determine_Damage (col.gameObject.tag, elementType));

			// Stop the enrage coroutine and start another
			if (enragedCoroutine != null) {
				StopCoroutine (enragedCoroutine);
			}

			enragedCoroutine = Enrage (2.0f);
			StartCoroutine (enragedCoroutine);
		}
		//Testing for collision with objects
		if (avoidedTypes.Contains (col.transform.gameObject.tag)) {
			patrolSpeed *= -1;
			/*
			Debug.Log ("Hit some: " + col.transform.gameObject.tag);
			Collider2D collider = col.collider;
			Vector3 contactPoint = col.GetContacts [0];
			Vector3 center = collider.bounds.center;

			bool right = contactPoint.x > center.x;
			bool top = contactPoint.y > center.y;
			*/
		}
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

	IEnumerator ChangeTargetPos(float time) {
		while (true) {
			float baseHeight = 5f;
			float offsetX = Random.Range (-3, 4);
			float offsetY = Random.Range (-1, 2);
			targetOffset = new Vector3 (offsetX, baseHeight + offsetY, 0f);
			yield return new WaitForSeconds (time);
		}
	}
}
