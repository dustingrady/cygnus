using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolType : Enemy {

	private bool chasingPlayer;
	private float delta = 10.0f; //How far we move left and right
	private float patrolSpeed = 1.5f; //How fast we move left and right
	private float chaseSpeed = 4f;
	private float chaseRadius = 6.0f; //How far we can see player
	private float escapeRadius = 12.0f; //How far player must be away to break the chase
	private float followDistance = 1.25f; //How close to the player the enemy will get
	private float turnAroundPoll = 0.5f; //Polling value for checking if enemy is stuck

	[SerializeField] 
	private bool canShoot = false;
	private EnemyShooting es;
	private EnemyDrop edrp;
	private EnemyDamage edmg;
	private Transform enemyTransform;
	private Transform playerTransform;
	private Vector3 enemyStartingPos;
	public LayerMask enemySight;
	public LayerMask edgeCheck;

	private Rigidbody2D rb;
	private bool pause = false;

	private GameObject sparks;
	private bool stunned = false;
	private int tolerance = 0;

	// When the enemy is shot, they persue the player for at least two seconds
	private bool enraged = false;
	// Reference to coroutine, to refresh it
	private IEnumerator enragedCoroutine;

	private Drop dr;

	private void Awake(){
		enemyStartingPos = transform.position; //Initialize startingPos
		enemyTransform = this.transform; //Reference to current enemy (for testing)
		es = gameObject.GetComponent<EnemyShooting>();
		edmg = gameObject.GetComponent<EnemyDamage> ();
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

		if (gameObject.GetComponent<Drop>() != null) 
			dr = gameObject.GetComponent<Drop>();
		if (gameObject.GetComponent<EnemyDrop> () != null)
			edrp = gameObject.GetComponent<EnemyDrop> ();

		sparks = Resources.Load ("Prefabs/Particles/Sparks") as GameObject;
	}

	void Start(){
		base.Start ();

		rb = GetComponent<Rigidbody2D> ();
		//rb.constraints = RigidbodyConstraints2D.FreezeRotation;
		patrolSpeed = Mathf.Sign (Random.Range (-1, 1)) * patrolSpeed;
	}
		
	void Update(){
		if (hitpoints <= 0) {
			if (edrp != null)
				edrp.determine_Drop (getEnemyType(), this.transform.position);

			if (dr != null) {
				int chance = Random.Range (0, 100);
				Debug.Log("Dead Drop Chance: " + chance);
				dr.dropItem (chance);
			}
			Destroy (this.gameObject);
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
	}

	bool check_Edge(){
		RaycastHit2D checkEdge = Physics2D.Raycast (new Vector2 (transform.position.x, transform.position.y), new Vector2 (patrolSpeed*-2, -1).normalized, 3, edgeCheck);
		Debug.DrawRay (transform.position, new Vector3 (patrolSpeed*-2, -1, 0).normalized, Color.green);
		if(checkEdge){ //Null check
			if(checkEdge.collider.transform.gameObject.name != "Foreground"){ //Can no longer see ground
				//Debug.Log("Hit some " + checkEdge.collider.transform.gameObject.name + " turning around");
				return false;
			}
		}
		return true;
	}

	bool check_Stuck(){
		RaycastHit2D checkFront = Physics2D.Raycast (new Vector2 (transform.position.x, transform.position.y), new Vector2 (patrolSpeed*-1, 0).normalized, 1, enemySight);
		Debug.DrawRay (transform.position, new Vector3 (patrolSpeed*-1, 0, 0).normalized, Color.green);
		//Debug.Log (checkFront.collider); 
		if(checkFront.collider != null){
			return true;
		}
		return false;
	}

	bool within_LoS(){
		Vector2 start = transform.position;
		Vector2 direction = playerTransform.position - transform.position;
		float distance = chaseRadius; //Distance in which raycast will check
		if (enraged) {
			distance = 100f;
		}
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
			if ((Mathf.Abs (Mathf.Abs (transform.position.x - v.x) - delta) <= 1.5f) || !check_Edge()){
				StartCoroutine (idle ());
				patrolSpeed *= -1;
			}
		}
		if((Distance() <= chaseRadius) && within_LoS() && check_Edge()){
			chasingPlayer = true;
		}

		if (check_Stuck()) { //Turn around if stuck
			StartCoroutine(idle());
			patrolSpeed *= -1;
		}
	}

	//Off with his head!
	void chase_Player(){
		/*Corrects the patrolSpeed of enemy depending on which side the player is on (Fixes raycast error in check_Edge())*/
		if ((transform.position.x > playerTransform.position.x) && (Mathf.Sign(patrolSpeed)) == -1) {
			patrolSpeed *= -1;
		} 
		if((transform.position.x < playerTransform.position.x) && (Mathf.Sign(patrolSpeed)) == 1) {
			patrolSpeed *= -1;
		}

		if((Distance() > escapeRadius && enraged == false) || !within_LoS() || !check_Edge()){
			enemyStartingPos = transform.position; //Where enemy will resume if player escapes
			chasingPlayer = false;
		}

		if ((Distance () > followDistance) && check_Edge()) { //Move towards player until we are n unit(s) away unless that results in going over a ledge
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

	//Return distance between player and enemy
	private float Distance(){
		return Vector3.Distance(transform.position, playerTransform.position);
	}

	void OnTriggerEnter2D(Collider2D col){
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

		if (other.tag == "ElectricElement") {
			takeDamage (0.5f);
		}
	}

	void OnCollisionEnter2D(Collision2D col) {
		Rigidbody2D collisionRB = col.gameObject.GetComponent<Rigidbody2D> ();
		if (collisionRB != null) {
			float colForce = CalculatePhysicalImpact (col.contacts [0].normal, col.relativeVelocity, collisionRB.mass);

			if (colForce > 3) {
				float dmg = edmg.determine_Damage ("EarthElement", getEnemyType (), colForce);
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

		yield return new WaitForSeconds (duration);
		enraged = false;
	}
}
