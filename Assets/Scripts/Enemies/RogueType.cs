using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueType : Enemy {

	private bool chasingPlayer;
	private float delta = 5.0f; //How far we move left and right
	private float patrolSpeed = 0.5f; //How fast we move left and right
	private float chaseSpeed = 4.5f;
	private float chaseRadius = 3.0f; //How far we can see player
	private float escapeRadius = 10.0f; //How far player must be away to break the chase
	private float followDistance = 1.25f; //How close to the player the enemy will get

	[SerializeField] 
	private bool canShoot = false;
	[SerializeField] 
	private bool arcLimit = true;
	private EnemyShooting es;
	private EnemyDrop edrp;
	private EnemyDamage edmg;
	private Transform enemyTransform;
	private Transform playerTransform;
	private Vector3 enemyStartingPos;

	private Rigidbody2D rb;
	private bool pause = false;
	private bool hidden = true;
	private GameObject smokePuff;
	private GameObject sparks;
	private bool stunned = false;
	private int tolerance = 0;

	private SpriteRenderer sr;

	// When the enemy is shot, they persue the player for atleast two seconds
	private bool enraged = false;
	// Reference to coroutine, to refresh it
	private IEnumerator enragedCoroutine;

	private void Awake(){
		enemyStartingPos = transform.position; //Initialize startingPos
		enemyTransform = this.transform; //Reference to current enemy (for testing)
		es = gameObject.GetComponent<EnemyShooting>();
		edrp = gameObject.GetComponent<EnemyDrop> ();
		edmg = gameObject.GetComponent<EnemyDamage> ();
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		sparks = Resources.Load ("Prefabs/Particles/Sparks") as GameObject;
		smokePuff = (GameObject)Resources.Load("Prefabs/Particles/SmokePuff");	
	}

	void Start(){
		rb = GetComponent<Rigidbody2D> ();
		rb.constraints = RigidbodyConstraints2D.FreezeRotation;
		sr = gameObject.GetComponent<SpriteRenderer> ();
		hide_Self(hidden);
		patrolSpeed = Mathf.Sign (Random.Range (-1, 1)) * patrolSpeed;
	}

	void Update(){
		if (hitpoints <= 0) {
			edrp.determine_Drop (getEnemyType(), this.transform.position);
			Destroy (this.gameObject);
		}

		if (tolerance == 20) {
			stunned = true;
			Instantiate (sparks, this.transform.position, Quaternion.identity);
			StartCoroutine (stunDuration ());
		}

		if (tolerance == 150) {
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
	}

	//Check if player is within ~180 degrees of enemy
	bool within_Arc(Vector3 player){
		float min = -10f; //Give a bit of tolerance incase turret prefab is placed imprecisely 
		float max = 190f;
		Vector3 dirVec = (player - transform.position).normalized;
		float up = Vector3.Dot(transform.up, dirVec) * 90f;
		float down = Vector3.Dot(-transform.up, dirVec) * 90f;
		return up > min && up < max;
	}

	//Normal patrolling behaviour. Using sin function for side to side patrolling (may change)
	void patrol_Area(){
		hide_Self(false);
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

		if(Distance() <= chaseRadius){
			reveal_Self(true); 
			chasingPlayer = true;
		}
	}

	//Off with his head!
	void chase_Player(){
		if(Distance() > escapeRadius && enraged == false){
			enemyStartingPos = transform.position; //Where enemy will resume if player escapes
			chasingPlayer = false;
		}

		if (Distance () > followDistance) { //Move towards player until we are 1 unit away (to avoid collision)
			Vector3 oldpos = transform.position;
			transform.position = new Vector3(Mathf.MoveTowards(transform.position.x, playerTransform.position.x, chaseSpeed * Time.deltaTime), transform.position.y, transform.position.z);
			float dv = transform.position.x - oldpos.x;

			//Debug.Log ((transform.position.x - oldpos.x) + " " + transform.localScale.x);
			if (Mathf.Sign (dv) == Mathf.Sign (transform.localScale.x)) {
				this.transform.localScale = new Vector3 (transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
			}
		}
		if (canShoot) {
			if (arcLimit && within_Arc (playerTransform.position)) {
				es.shoot_At_Player ();
			} else if(!arcLimit) {
				es.shoot_At_Player ();
			}
			followDistance = 4.0f; //Don't get so close when shooting
		}
	}

	//Return distance between player and enemy
	private float Distance(){
		return Vector3.Distance(transform.position, playerTransform.position);
	}

	/*Reveal self once player is in range*/
	void reveal_Self(bool x){
		if (x) {
			sr.color = new Color (1f, 1f, 1f, 1f); //Change alpha to 1
			GameObject smoke = Instantiate (smokePuff, transform.position, Quaternion.identity); //Instantiate smoke screen
			Destroy (smoke, 2);
			hidden = false;
		}
	}

	/*Hide self once chase has ended*/
	void hide_Self(bool x){
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
		if (other.tag == "ElectricElement" && elementType != Elements.earth) {
			tolerance++;
		}

		if (other.tag == "ElectricElement" && elementType == Elements.metal) {
			//Debug.Log ("Particle collision");
			hitpoints -= 0.1f;
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
		//Debug.Log ("now enraged");

		yield return new WaitForSeconds (duration);

		enraged = false;
		//Debug.Log ("no longer enraged");
	}
}
