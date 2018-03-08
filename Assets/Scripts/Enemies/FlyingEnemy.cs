using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : Enemy {
	[SerializeField]
	private float hitpoints;
	string type;
	string[] listOfTypes = {"fire", "water", "earth", "metal", "electric"};
	List<string> damagingElements = new List<string> (new string[] {
		"FireElement",
		"WaterElement",
		"EarthElement",
		"MetalElement",
		"ElectricElement"
	});

	private bool chasingPlayer;
	private float delta = 5.0f; //How far we move left and right
	private float patrolSpeed = 1.5f; //How fast we move left and right
	private float chaseSpeed = 2.5f;
	private float chaseRadius = 5.0f; //How far we can see player
	private float escapeRadius = 10.0f; //How far player must be away to break the chase
	private EnemyShooting es;
	private EnemyDrop edrp;
	private EnemyDamage edmg;
	private Transform enemyTransform;
	private Transform playerTransform;
	private Vector3 enemyStartingPos;

	Rigidbody2D rb;
	bool pause = false;

	private void Awake(){
		enemyStartingPos = transform.position; //Initialize startingPos
		enemyTransform = this.transform; //Reference to current enemy
		es = gameObject.GetComponent<EnemyShooting>();
		edrp = gameObject.GetComponent<EnemyDrop> ();
		edmg = gameObject.GetComponent<EnemyDamage> ();
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
	}

	void Start(){
		//change this temp to randomize type. Random.range for int is exclusive for last interger.
		int temp = Random.Range (0, 5);
		type = listOfTypes [temp];
		Debug.Log (temp + " " + listOfTypes [temp]);

		rb = GetComponent<Rigidbody2D> ();
		rb.constraints = RigidbodyConstraints2D.FreezeRotation;

		patrolSpeed = Mathf.Sign (Random.Range (-1, 1)) * patrolSpeed;
	}

	void Update(){
		if (hitpoints <= 0) {
			edrp.determine_Drop (getEnemyType (), this.transform.position);
			Destroy (this.gameObject);
		}
		switch (chasingPlayer) {
		case true:
			chase_Player ();
			break;
		case false:
			patrol_Area ();
			break;
		}
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

		if(Distance() <= chaseRadius){
			chasingPlayer = true;
		}
	}

	//Off with his head!
	void chase_Player(){
		if(Distance() > escapeRadius){
			enemyStartingPos = transform.position; //Where enemy will resume if player escapes
			chasingPlayer = false;
		}
		//Follow player in x-axis
		if (Vector3.Distance (transform.position, playerTransform.position) > 3f) { //Move towards player until we are n unit(s) away (to avoid collision)
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
		es.shoot_At_Player ();
	}

	//Return distance between player and enemy
	private float Distance(){
		return Vector3.Distance(enemyTransform.position, playerTransform.position);
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "TurnAround") {
			patrolSpeed *= -1;
		}
		if (damagingElements.Contains (col.gameObject.tag)) {
			takeDamage (edmg.determine_Damage (col, getEnemyType ()));
		}
	}

	//particle collision for electricity
	void OnParticleCollision(GameObject other){
		if (other.tag == "ElectricElement") {
			takeDamage (0.5f);
		}
	}

	IEnumerator flash(){
		SpriteRenderer sr = GetComponent<SpriteRenderer> ();
		int elapsed = 0;
		int flashes = 3;
		while(elapsed < flashes){
			sr.color = Color.red;
			yield return new WaitForSeconds(0.10f);
			sr.color = Color.white;
			yield return new WaitForSeconds(0.10f);
			elapsed++;
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

	public string getEnemyType(){
		return type;
	}

	public float getEnemyHitPoints(){
		return hitpoints;
	}
}
