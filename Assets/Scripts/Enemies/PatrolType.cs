using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolType : Enemy {
	float hitpoints;
	string type;

	string[] listOfTypes = {"fire", "water", "earth", "metal"};

	private bool chasingPlayer;
	private float delta = 5.0f; //How far we move left and right
	private float patrolSpeed = 1.5f; //How fast we move left and right
	private float chaseSpeed = 3.5f;
	private float chaseRadius = 5.0f; //How far we can see player
	private float escapeRadius = 10.0f; //How far player must be away to break the chase
	private Transform enemyTransform;
	private Transform playerTransform;
	private Vector3 enemyStartingPos;

	Rigidbody2D rb;

	bool pause = false;

	private void Awake(){
		enemyStartingPos = transform.position; //Initialize startingPos
		enemyTransform = this.transform; //Reference to current enemy (for testing)
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
	}

	void Start()
	{
		//change this temp to randomize type. Random.range for int is exclusive for last interger.
		int temp = Random.Range (0, 4);
		type = listOfTypes [temp];
		Debug.Log (temp + " " + listOfTypes [temp]);

		hitpoints = Mathf.Floor(Random.Range (10f, 21f));

		rb = GetComponent<Rigidbody2D> ();
		rb.constraints = RigidbodyConstraints2D.FreezeRotation;

		patrolSpeed = Mathf.Sign (Random.Range (-1, 1)) * patrolSpeed;
	}

	public override void takeDamage(float amount)
	{
		StartCoroutine (damage (amount));
	}

	public string getEnemyType()
	{
		return type;
	}

	public float getEnemyHitPoints()
	{
		return hitpoints;
	}

	void Update()
	{
		if (hitpoints <= 0)
			Destroy (this.gameObject);
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
		//v.x += delta * Mathf.Sin (Time.time * patrolSpeed);

		//transform.position = v;	

		if ((Mathf.Abs(transform.position.x - v.x) < delta) && !pause) {

			transform.Translate (new Vector2 (patrolSpeed, 0) * Time.deltaTime);

			if (Mathf.Sign (patrolSpeed) != Mathf.Sign (transform.localScale.x)) {
				this.transform.localScale = new Vector3 (transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
			}

			if (Mathf.Abs (Mathf.Abs (transform.position.x - v.x) - delta) <= 0.5)
			{
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

		if (Vector3.Distance (transform.position, playerTransform.position) > 1f) { //Move towards player until we are 1 unit away (to avoid collision)
			//transform.InverseTransformDirection(playerTransform.position);
			//transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, chaseSpeed * Time.deltaTime);

			Vector3 oldpos = transform.position;

			transform.position = new Vector3(Mathf.MoveTowards(transform.position.x, playerTransform.position.x, chaseSpeed * Time.deltaTime), transform.position.y, transform.position.z);

			float dv = transform.position.x - oldpos.x;

			//Debug.Log ((transform.position.x - oldpos.x) + " " + transform.localScale.x);
			if (Mathf.Sign (dv) == Mathf.Sign (transform.localScale.x)) {
				this.transform.localScale = new Vector3 (transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
			}

			/*
			if(transform.position.x < playerTransform.position.x){
				transform.rotation = Quaternion.identity;
			}
			else{
				transform.rotation = Quaternion.Euler (0, 180, 0);
			}
			*/
		} 
	}

	//Return distance between player and enemy
	private float Distance(){
		return Vector3.Distance(enemyTransform.position, playerTransform.position);
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "TurnAround")
			patrolSpeed *= -1;
	}
		
	IEnumerator idle()
	{
		pause = true;
		yield return new WaitForSeconds (1);
		pause = false;
	}

	IEnumerator damage(float amount)
	{
		SpriteRenderer sr = GetComponent<SpriteRenderer> ();
		sr.color = Color.red;
		sr.color = Color.white;
		hitpoints -= amount;
		yield return new WaitForSeconds (1);
	}
}
