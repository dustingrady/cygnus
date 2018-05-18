using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enums are safer than strings, no chance of a typo and can be easily set in the editor
public enum Elements { fire, water, earth, metal, electric };

public abstract class Enemy : MonoBehaviour {
	// No need to construct the list with an array, you can do it this way
	protected List<string> damagingElements = new List<string> {
		"FireElement",
		"WaterElement",
		"EarthElement",
		"MetalElement",
		"ElectricElement",
		"Lava"
	};

	[SerializeField]
	protected Elements elementType;
	[SerializeField]
	protected bool assignRandomType; // Check this if you want a random element

	[SerializeField]
	protected float hitpoints = 100;
	[SerializeField]
	protected float energy = 100;

	protected Color elementTint;

	// Initialize enemy components and rigidbody refernece
	protected EnemyDamage edmg;
	protected EnemyDrop edrp;
	protected Player player;
	protected Drop dr;
	protected SpriteRenderer sr;
	protected Rigidbody2D rb;

	// Particle Effects
	protected GameObject sparks;

	// Start position and player reference
	protected Vector3 startingPosition;
	protected Transform playerTransform;

	// Visibilty mask for the enemy
	public LayerMask enemySight;

	// For overloading the enemy with electricity
	protected int tolerance = 0;

	protected bool pause = false;
	protected bool stunned = false;

	protected float maxHitPoints;

	protected void Start() {
		// Getting references to the drop and damage classes and player
		playerTransform = playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

		// Get references to the enemy drops, if they exist
		if (gameObject.GetComponent<Drop>() != null) 
			dr = gameObject.GetComponent<Drop>();
		if (gameObject.GetComponent<EnemyDrop> () != null)
			edrp = gameObject.GetComponent<EnemyDrop> ();

		edmg = GetComponent<EnemyDamage> ();
		rb = GetComponent<Rigidbody2D> ();
		sr = gameObject.GetComponent<SpriteRenderer> ();

		// Particle effects
		sparks = Resources.Load ("Prefabs/Particles/Sparks") as GameObject;

		// Set random if type if 'assignRandomType' is checked
		if (assignRandomType) {
			int elementCount = System.Enum.GetValues (typeof(Elements)).Length;
			elementType = (Elements)Random.Range (0, elementCount);
		}

		// Setting default position and getting player reference
		startingPosition = transform.position;
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		maxHitPoints = hitpoints;

		SetElementColor ();
	}


	// Changes the tint of the sprite on the enemy to match their type
	public void SetElementColor() {
		Dictionary<Elements, Color> elementColors = new Dictionary<Elements, Color> {
			{ Elements.fire, new Color (1, 0.7f, 0.7f, 1f) },
			{ Elements.water, new Color (0.7f, 0.7f, 1f, 1f) },
			{ Elements.earth, new Color (0.7f, 0.7f, 0.5f, 1f) },
			{ Elements.metal, new Color (1, 1, 1, 1f) },
			{ Elements.electric, new Color (1, 1, 0.7f, 1f) }
		};

		SpriteRenderer sr = GetComponent<SpriteRenderer> ();
		elementTint = elementColors [elementType];
		sr.color = elementTint;
	}
		

	protected virtual bool within_LoS(){
		Vector2 start = transform.position;
		Vector2 direction = playerTransform.position - transform.position;
		//Debug.DrawRay(start, direction, Color.red,2f,false);
		RaycastHit2D sightTest = Physics2D.Raycast (start, direction, enemySight);
		if (sightTest) {
			if (sightTest.collider.CompareTag("Player")) {
				if (sightTest.collider.CompareTag ("EnemyProjectile")) {
					return true;
				}
				return true;
			}
		}
		return false;
	}


	protected void ElectricShock(string tag){
		if (tag == "ElectricElement" && elementType != Elements.earth) {
			tolerance++;
		}

		if (tag == "ElectricElement") {
			FloatingTextController.Initialize();
			FloatingTextController.CreateFloatingText ("1", this.gameObject.transform, GetComponent<Collider2D>().bounds.extents.y + 0.1f, Color.yellow, 20);
			takeDamage (1f);
		}
	}


	protected float DistanceToPlayer(){
		return Vector3.Distance(transform.position, playerTransform.position);
	}


	public float CalculatePhysicalImpact(Vector2 contactNorm, Vector2 vel, float mass) {
		return Vector3.Dot (contactNorm, vel) * mass;
	}
		

	protected virtual void takeDamage(float amount){
		StartCoroutine (damage (amount));
	}


	protected void EvaluateTolerance() {
		if (tolerance == 20) {
			stunned = true;

			// Display stunned
			FloatingTextController.Initialize();
			FloatingTextController.CreateFloatingText ("Stunned", this.gameObject.transform, GetComponent<Collider2D>().bounds.extents.y + 0.1f, Color.yellow, 20);

			Instantiate (sparks, this.transform.position, Quaternion.identity);
			StartCoroutine (stunDuration ());
		}

		if (tolerance == 200) {
			tolerance = 0;
		}
	}


	protected void EvaluateHealth() {
		if (hitpoints <= 0) {
			if (edrp != null)
				edrp.determine_Drop (elementType, this.transform.position);

			if (dr != null) {
				int chance = Random.Range (0, 100);
				Debug.Log("Dead Drop Chance: " + chance);
				dr.dropItem (chance);
			}
			//Destroy (this.gameObject);
			this.gameObject.SetActive(false);
		}
	}

	public void respawnActive(){
		hitpoints = maxHitPoints;
		this.gameObject.transform.position = startingPosition;
		this.SetElementColor ();
		if (!this.gameObject.activeInHierarchy) {
			this.gameObject.SetActive (true);
		}
	}

	public void respawnHidden(){
		hitpoints = maxHitPoints;
		this.gameObject.transform.position = startingPosition;
		this.SetElementColor ();
	}

	protected float EvaluatePhysical(Collision2D col) {
		float colForce = 0;
		Rigidbody2D collisionRB = col.gameObject.GetComponent<Rigidbody2D> ();
		if (collisionRB != null && col.contacts.Length > 0) {
			colForce = CalculatePhysicalImpact (col.contacts [0].normal, col.relativeVelocity, collisionRB.mass);

			if (colForce > 5) {
				float dmg = edmg.determine_Damage ("EarthElement", elementType, colForce);
				takeDamage (dmg);
			}
		}

		return colForce;
	}

	public float getHP() {
		return(hitpoints);
	}

	//
	// Coroutines 
	//


	protected IEnumerator flash(){
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

		sr.color = elementTint;
	}


	IEnumerator damage(float amount){
		hitpoints -= amount;
		if (amount > 0)
			yield return flash ();
		yield return new WaitForSeconds (1);
	}


	protected IEnumerator stunDuration() {
		yield return new WaitForSeconds (2);
		stunned = false;
	}
}
