using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretType : Enemy {
	float hitpoints;
	string type;

	string[] listOfTypes = {"fire", "water", "earth", "metal"};

	private Transform playerTransform;
	private Transform enemyTransform;
	private float turretRadius = 10.0f; //How far our turret enemies can see
	private EnemyShooting es;
	LineRenderer line;

	GameObject sparks;
	bool stunned = false;
	int tolerance = 0;

	void Awake(){
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		es = gameObject.GetComponent<EnemyShooting>();
		sparks = Resources.Load ("Prefabs/Particles/Sparks") as GameObject;
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

	void Start()
	{
		//change this temp to randomize type. Random.range for int is exclusive for last interger.
		int temp = Random.Range (0, 4);
		type = listOfTypes [temp];

		hitpoints = Mathf.Floor(Random.Range (5f, 11f));


		Rigidbody2D rb = GetComponent<Rigidbody2D> ();
		line = this.gameObject.GetComponent<LineRenderer>();
		rb.constraints = RigidbodyConstraints2D.FreezeAll;

	}

	void Update()
	{
		if (tolerance == 20) {
			stunned = true;
			Instantiate (sparks, this.transform.position, Quaternion.identity);
			line.enabled = false;
			StartCoroutine (stunDuration ());
		}

		if (tolerance == 200) {
			tolerance = 0;
		}

		if (hitpoints <= 0)
			Destroy (this.gameObject);
		
		if (stunned == false) {
			guard_Area ();
		}
	}

	void guard_Area(){
		if (Vector3.Distance (transform.position, playerTransform.position) < turretRadius) { //If player is in range of turret
			//line.positionCount (2);
			line.enabled = true;
			line.SetPosition (0, transform.position);
			line.SetPosition (1, playerTransform.position);
			es.shoot_At_Player (); //Shoot um up
		} else {
			line.enabled = false;
		}
	}

	//Return distance between player and enemy
	private float Distance(){
		return Vector3.Distance(enemyTransform.position, playerTransform.position);
	}

	//particle collision for electricity
	void OnParticleCollision(GameObject other)
	{
		if (other.tag == "ElectricElement" && this.type != "earth") {
			tolerance++;
		}
			
		if (other.tag == "ElectricElement" && this.type == "metal") {
			Debug.Log ("Particle collision");
			hitpoints -= 0.1f;
		}
	}

	IEnumerator damage(float amount)
	{
		hitpoints -= amount;
		yield return new WaitForSeconds (1);
	}

	IEnumerator stunDuration()
	{
		yield return new WaitForSeconds (2);
		stunned = false;
	}
}
