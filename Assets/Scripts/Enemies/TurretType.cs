using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretType : Enemy {
	[SerializeField]
	private float hitpoints;
	[SerializeField]
	string type;

	string[] listOfTypes = {"fire", "water", "earth", "metal", "electric"};
	List<string> damagingElements = new List<string> {
		"FireElement",
		"WaterElement",
		"EarthElement",
		"MetalElement",
		"ElectricElement"
	};


	[SerializeField] 
	private bool arcLimit = true; //Limits the enemies ability to shoot to ~180 degrees

	private Transform playerTransform;
	private Transform enemyTransform;
	[SerializeField]
	private float turretRadius = 15.0f; //How far our turret enemies can see
	private EnemyShooting es;
	private EnemyDrop edrp;
	private EnemyDamage edmg;
	LineRenderer line;

	void Awake(){
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		es = gameObject.GetComponent<EnemyShooting>();
		edrp = gameObject.GetComponent<EnemyDrop> ();
		edmg = gameObject.GetComponent<EnemyDamage> ();
	}

	void Start(){
		// Randomly assign a type if no type is given
		if (type == "") {
			//change this temp to randomize type. Random.range for int is exclusive for last interger.
			int temp = Random.Range (0, 5);
			type = listOfTypes [temp];
		}

		Rigidbody2D rb = GetComponent<Rigidbody2D> ();
		line = this.gameObject.GetComponent<LineRenderer>();
		rb.constraints = RigidbodyConstraints2D.FreezeAll;
	}

	void Update(){
		if (hitpoints <= 0) {
			edrp.determine_Drop (getEnemyType(), this.transform.position);
			Destroy (this.gameObject);
		}
		guard_Area ();
	}

	//Check if player is within ~180 degrees of enemy
	bool within_Arc(Vector3 player){
		float min = -10f; //Give a bit of tolerance incase turret prefab is placed imprecisely 
		float max = 190f;
		Vector3 dirVec = (player - transform.position).normalized;
		float up = Vector3.Dot(transform.up, dirVec) * 90f;
		float down = Vector3.Dot(-transform.up, dirVec) * 90f;
		//Debug.Log ("up: " + up + " min: " + min);
		//Debug.Log ("down: " + down + " max: " + max);
		return up > min && up < max;
	}

	void draw_And_Shoot(){
		line.enabled = true;
		line.SetPosition (0, transform.position);
		line.SetPosition (1, playerTransform.position);
		es.shoot_At_Player (); //Shoot um up
	}

	void guard_Area(){
		if (Vector3.Distance (transform.position, playerTransform.position) < turretRadius) { //If player is in range (distance) of turret
			//Debug.Log("Range: " + within_Arc(playerTransform.position)); //Testing
			if (arcLimit && within_Arc (playerTransform.position)) {
				draw_And_Shoot ();
			} else if(!arcLimit) {
				draw_And_Shoot ();
			}
		} else {
			line.enabled = false;
		}
	}

	//Return distance between player and enemy
	private float Distance(){
		return Vector3.Distance(enemyTransform.position, playerTransform.position);
	}

	void OnTriggerEnter2D(Collider2D col){
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
