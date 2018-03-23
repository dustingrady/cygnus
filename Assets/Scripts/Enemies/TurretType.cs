using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretType : Enemy {
	[SerializeField] 
	private bool arcLimit = true; //Limits the enemies ability to shoot to ~180 degrees

	private Transform playerTransform;
	private Transform enemyTransform;
	[SerializeField]
	private float turretRadius = 15.0f; //How far our turret enemies can see
	private EnemyShooting es;
	private EnemyDrop edrp;
	private EnemyDamage edmg;
	private LineRenderer line;
	public LayerMask enemySight;

	void Awake(){
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		es = gameObject.GetComponent<EnemyShooting>();
		edrp = gameObject.GetComponent<EnemyDrop> ();
		edmg = gameObject.GetComponent<EnemyDamage> ();
	}

	void Start(){
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
		return up > min && up < max;
	}

	bool within_LoS(){
		Vector2 start = transform.position;
		Vector2 direction = playerTransform.position - transform.position;
		float distance = turretRadius; //Distance in which raycast will check
		//Debug.DrawRay(start, direction, Color.red,2f,false);
		RaycastHit2D sightTest = Physics2D.Raycast (start, direction, distance, enemySight);
		if (sightTest) {
			if (sightTest.collider.CompareTag("Player")) {
				return true;
			}
		}
		return false;
	}

	void draw_And_Shoot(){
		//Debug.DrawRay(transform.position, (playerTransform.position - transform.position), Color.red,2f,false);
		line.SetPosition (0, transform.position);
		line.SetPosition (1, playerTransform.position);
		es.shoot_At_Player (); //Shoot um up
	}

	void guard_Area(){
		if (Distance () < turretRadius) { //If player is in range (distance) of turret
			//Debug.Log("Range: " + within_Arc(playerTransform.position)); //Testing
			if (arcLimit && within_Arc (playerTransform.position) && within_LoS()) {
				line.enabled = true;
				draw_And_Shoot ();
			} else if(!arcLimit && within_LoS()) {
				line.enabled = true;
				draw_And_Shoot ();
			}
			else {
				line.enabled = false;
			}
		} 
	}

	//Return distance between player and enemy
	private float Distance(){
		return Vector3.Distance(transform.position, playerTransform.position);
	}

	void OnTriggerEnter2D(Collider2D col){
		if (damagingElements.Contains (col.gameObject.tag)) {
			takeDamage (edmg.determine_Damage (col.gameObject.tag, getEnemyType ()));
		}
	}

	//particle collision for electricity
	void OnParticleCollision(GameObject other){
		if (other.tag == "ElectricElement") {
			takeDamage (0.5f);
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
}
