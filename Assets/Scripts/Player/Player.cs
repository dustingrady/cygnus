using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public struct SerPosition {
    public float x, y, z;

    public SerPosition(float x, float y, float z) { this.x = x; this.y = y; this.z = z; }
    public static implicit operator SerPosition(Vector3 rvalue) {
        return new SerPosition(rvalue.x, rvalue.y, rvalue.z);
    }
    
    public static implicit operator Vector3(SerPosition rvalue) {
        return new Vector3(rvalue.x, rvalue.y, rvalue.z);
    }
}


public class Player : MonoBehaviour {
    [SerializeField]
    public Stat health;
	public Element leftElement;
	public Element rightElement;
    public Element centerElement;

	public Sprite stunnedSprite;

	public bool stunned = false;
	bool onFire = false;
	bool inAcid = false;
	private Coroutine acidDamageCoroutine;

	bool standingInFire = false;
	bool takingDamage = false;
	public bool respawning = false;

    public Inventory inventory;

	private Vector3 checkpointPos;

	private GameObject stunSwirl;
	private GameObject deathParticle;

	private AudioController ac;
	public AudioClip deathSound;

	// Death broadcast event
	public delegate void PlayerDeath();
	public static event PlayerDeath OnDeath;

    public void OnSaveGame(Dictionary<SaveType, object> dict) {
        dict.Add(SaveType.PLAYER, (SerPosition)checkpointPos);
    }
    
    public void OnLoadGame(Dictionary<SaveType, object> dict) {
        checkpointPos = (Vector3)dict[SaveType.PLAYER];
    }

    void Awake () {
		health.Initalize();
		// Inventory references need to be reset each time a scene loads
        inventory = GameObject.Find("Game Manager").GetComponent<Inventory>();
		inventory.initializeInventory ();
	}

    void Start() {
        checkpointPos = transform.position;
		stunSwirl = (GameObject)Resources.Load("Prefabs/NPCs/stunned");	
		deathParticle = (GameObject)Resources.Load("Prefabs/Particles/DeathEffect");	

		//Reference to Audio Controller
		GameObject camera = GameObject.Find("Main Camera");
		ac = camera.GetComponent<AudioController>();

		// Ensure the game manager has the correct reference
		GameManager.instance.player = this.gameObject;

		// Check to see if the Saver was used to load and set player position;
		if (Saver.loading == true) {
			transform.position = Saver.position;
			Saver.loading = false;
		} else {
			Debug.Log ("started without loading");
		}

    }
	
	// Update is called once per frame
	void Update () {
		CheckHealth ();

		if (stunned) {
			StartCoroutine (StunPlayer(1f));
		}
    }

	public void Knockback (float strength, Vector2 dir) {
		Rigidbody2D rb = GetComponent<Rigidbody2D> ();
		rb.AddForce (dir * strength);

		StartCoroutine (flash());
	}

	void TouchedEnemy(GameObject enemy) {
		Vector2 dir = Vector2.zero;
		if (transform.position.x < enemy.transform.position.x) {
			dir = new Vector2 (-0.5f, 0.5f);
		} else {
			dir = new Vector2 (0.5f, 0.5f);
		}

		Knockback(1200f, dir);
	}
	
	public void healWounds(float amount){
		this.health.CurrentVal += amount;
		Debug.Log ("healing for: " + amount);
	}
	

	void CheckHealth() {
		if (health.CurrentVal <= 0 && !respawning) {
			if (GameObject.Find ("CameraSwapTrigger") != null || GameObject.Find("Boss Wall") != null) {
				GameObject.Find ("CameraSwapTrigger").GetComponent<CameraSwitch> ().playerCam = true;
				//GameObject.Find ("Wall Trigger").GetComponent<BossWallTrigger> ().wallOn = false;
			}

			// set respawning flag to prevent update
			respawning = true;
			if (acidDamageCoroutine != null)
				StopCoroutine (acidDamageCoroutine);
			
			StartCoroutine(playerDeath ());
		}
	}
		
	public void backToCheckPoint()	{
		if (OnDeath != null) {
			OnDeath ();
		}

		if (checkpointPos != null) {
			Debug.Log ("Going back to checkpoint");
			inAcid = false;
			health.CurrentVal = 100;
			transform.position = checkpointPos;
		} else {
			Debug.Log ("reloading scene?");
			inventory.emptyInventory ();
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		}
	}
	/*
	 * 
	 * START OF COLLISION STUFF
	 * 
	*/

	void OnCollisionEnter2D(Collision2D col) {
		if (centerElement != null) {
			if (col.transform.CompareTag("MetalElement") && centerElement.elementType == "magnetic") {

				// Check to see if the metal object hit is a tilemap
				Tilemap tilemap = col.gameObject.GetComponent<Tilemap> ();

				if (tilemap != null) {
					BoxCollider2D bc = GetComponent<BoxCollider2D> ();

					// Get a position to left and right of the player to test for a valid tile
					Vector3 leftBounds = new Vector3 (transform.position.x - bc.bounds.extents.x - 0.1f,
						transform.position.y, 
						transform.position.z);

					Vector3 rightBounds = new Vector3 (transform.position.x + bc.bounds.extents.x + 0.1f,
						transform.position.y, 
						transform.position.z);

					// If a tile is found, the player is hugging the left or right wall, start grapple
					if (tilemap.GetTile (tilemap.WorldToCell (leftBounds)) != null) {
                        //GetComponent<PlayerController> ().StartGrapple ("right");
                        GetComponent<PlayerController>().Grapple = PlayerController.GrappleState.Right;

                        // Disable the magnetic pull
                        centerElement.gameObject.GetComponent<Magnetic>().pulling = false;
						centerElement.gameObject.GetComponent<Magnetic> ().lr.enabled = false;

					} else if (tilemap.GetTile (tilemap.WorldToCell (rightBounds)) != null) {
                        //GetComponent<PlayerController> ().StartGrapple ("left");
                        GetComponent<PlayerController>().Grapple = PlayerController.GrappleState.Left;

                        // Disable the magnetic pull
                        centerElement.gameObject.GetComponent<Magnetic>().pulling = false;
						centerElement.gameObject.GetComponent<Magnetic> ().lr.enabled = false;
					}

				}
			}	
		}
			
		if (col.gameObject.tag == "Scrap") {
			inventory.addScrap (1);
			Destroy (col.gameObject);
		}

		if ((col.gameObject.tag == "Enemy" && !takingDamage) || (col.gameObject.tag == "Obstacle" && !takingDamage)) {
			// Trigger knockback and flash
			TouchedEnemy(col.gameObject);

			StartCoroutine (enemyOnContact (20));
		}
			
			
	}

	void OnCollisionStay2D(Collision2D col)
	{
		if ((col.gameObject.tag == "Enemy" && !takingDamage) || (col.gameObject.tag == "Obstacle" && !takingDamage)) {
			StartCoroutine (enemyOnContact (20));
		}
			
	}

	void OnParticleCollision(GameObject other){
		if (other.tag == "Acid") {
			Debug.Log ("touched acid");
			this.health.CurrentVal -= 5f;
			StartCoroutine(damageOverTime(5,1));
		}

		if (other.tag == "Lava") {
			this.health.CurrentVal -= 5f;
			StartCoroutine(damageOverTime(5,1));
		}
	}

	/*
	 * 
	 * END OF COLLISION STUFF 
	 * 
	 */



	/* 
	 *
	 * START OF TRIGGER STUFF 
	 *
	 */

	void OnTriggerEnter2D(Collider2D col) {

		// Test for the Playground, if you hit Lava or enemy projectile reload
		if (col.gameObject.tag == "BossSpecial") {
			inventory.emptyInventory ();
			StartCoroutine (enemyProjectiles (50));
			StartCoroutine (flash ());
		}

		if (col.gameObject.tag == "BossBullet") {
			Debug.Log ("ouch, a fuckin bossbullet");
			StartCoroutine(enemyProjectiles(10));
			StartCoroutine (flash ());
		}

		if (col.gameObject.tag == "Item") {

			//string path = "Items/" + col.gameObject.name;
			//Item temp = Resources.Load(path) as Item;

			Item item = col.gameObject.GetComponent<ItemInteraction>().item;

			if (item != null) {
				//inventory.GetComponent<Inventory>().addItem(item);
				//Debug.Log(inventory.GetComponent<Inventory> ().checkSlot(item));
				if (inventory.GetComponent<Inventory> ().checkSlot (item)) 
				{
					inventory.GetComponent<Inventory> ().stackItem (item);
				} 
				else if (!inventory.GetComponent<Inventory> ().checkSlot (item)) 
				{
					inventory.GetComponent<Inventory> ().addItem (item);
				}
			} else {
				Debug.LogError ("There was no item on that object!");
			}
		}

		if ((col.gameObject.tag == "EnemyProjectile" && !takingDamage)) {
			StartCoroutine (enemyProjectiles(10));
			StartCoroutine (flash());
		}

		if (col.gameObject.name == "Fire" && !standingInFire) {
			StartCoroutine (singularDamage (5));
			StartCoroutine (flash());
		}

		// Collision with checkpoint trigger
		if (col.CompareTag("Checkpoint")) {
			float height = GetComponent<BoxCollider2D>().size.y; 
			FloatingTextController.CreateFloatingText ("Checkpoint", this.gameObject.transform, height, Color.yellow, 20);
			checkpointPos = col.transform.position;
		}
	}

	void OnTriggerStay2D(Collider2D col){
		if ((col.gameObject.tag == "LavaPlatform" && !standingInFire)) {
			StartCoroutine(singularDamage(5));
			StartCoroutine (flash());
		}

        if (col.gameObject.tag == "QuicksandXD" && !standingInFire) {
			StartCoroutine(singularDamage(5));
        }

		if ((col.gameObject.tag == "FireElement" && !standingInFire)) {
			StartCoroutine(singularDamage(5));
			StartCoroutine (flash());
		} 

		if (col.gameObject.tag == "Acid" && !inAcid) {
			Debug.Log ("touched acid");
			acidDamageCoroutine = StartCoroutine (acidOverTime (3, 10));
		}
	}

	void OnTriggerExit2D(Collider2D col){
		if ((col.gameObject.tag == "LavaPlatform" && !onFire)) {
			StartCoroutine(damageOverTime (5, 1));
		}

		if ((col.gameObject.name == "Fire" && !onFire)) {
			StartCoroutine(damageOverTime (5, 1));
		}
	}

	/* 
	 *
	 * END OF TRIGGER STUFF 
	 *
	 */

	public IEnumerator StunPlayer (float duration) {
		var animator = GetComponent<Animator> ();
		var controller = GetComponent<PlayerController> ();
		var shooting = GetComponent<PlayerShooting> ();

		float height = GetComponent<Collider2D>().bounds.extents.y + 0.5f;
		GameObject swirlObj = Instantiate (stunSwirl, new Vector2(transform.position.x, transform.position.y + height), Quaternion.identity); //Instantiate exclamation point
		swirlObj.transform.parent = this.transform;

		// stun the player
		controller.Stun ();
		animator.SetInteger ("State", 0);
		animator.enabled = false;
		shooting.enabled = false;
		GetComponent<SpriteRenderer> ().sprite = stunnedSprite;

		// set timeout
		yield return new WaitForSeconds (duration);

		// unstun the player
		animator.enabled = true;
		shooting.enabled = true;
		controller.Unstun ();
		Destroy (swirlObj);
		stunned = false;
	}

	IEnumerator fade_Out(GameObject x){
		SpriteRenderer passed = x.GetComponent<SpriteRenderer> ();
		float time = 0.5f;
		while(passed.color.a > 0){
			passed.color = new Color(passed.color.r, passed.color.g, passed.color.b, passed.color.a - (Time.deltaTime / time));
			//passed.color -= Time.deltaTime / time;
			yield return null;
		}
	}

    //FOR FIRE ONLY
    IEnumerator damageOverTime(int ticks, int damageAmount){
        onFire = true;

        int currentTick = 0;
        while (currentTick < ticks) {
			ReducePlayerHealth (damageAmount);
            yield return new WaitForSeconds (1);
            currentTick++;
        }

        onFire = false;
    }

	//Dot for being in Acid
	IEnumerator acidOverTime(int ticks, int damageAmount) {
		inAcid = true;

		int currentTick = 0;
		while (currentTick < ticks) {
			ReducePlayerHealth (damageAmount);
			yield return new WaitForSeconds (1);
			currentTick++;
		}

		inAcid = false;
	}

    IEnumerator singularDamage(int damageAmount)
    {
        standingInFire = true;
		ReducePlayerHealth (damageAmount);
        yield return new WaitForSeconds (0.5f);
        standingInFire = false;
    }

	//Damage from enemy's projectiles
	IEnumerator enemyProjectiles(int damageAmount)
	{
		takingDamage = true;
		ReducePlayerHealth (damageAmount);
		yield return new WaitForSeconds (0.5f);
		takingDamage = false;
	}

	//Damage from contact with enemies
	IEnumerator enemyOnContact(int damageAmount)
	{
		takingDamage = true;
		ReducePlayerHealth (damageAmount);
		yield return new WaitForSeconds (1);
		takingDamage = false;
	}

	IEnumerator flash(){
		SpriteRenderer sr = GetComponent<SpriteRenderer> ();
		int elapsed = 0;
		int flashes = 3;
		while(elapsed < flashes){
			sr.color = new Color(1f, 0.8f, 0.8f);
			yield return new WaitForSeconds(0.10f);
			sr.color = Color.white;
			yield return new WaitForSeconds(0.10f);
			elapsed++;
		}
	}

	IEnumerator playerDeath() {
		// Display particle effect
		Instantiate(deathParticle, transform.position, Quaternion.identity);

		if (deathSound != null)
			ac.source.PlayOneShot (deathSound);
		
		// Disable character sprite and controller
		GetComponent<PlayerController>().enabled = false;
		GetComponent<SpriteRenderer>().enabled = false;
		GetComponent<Collider2D>().enabled = false;
		GetComponent<Rigidbody2D> ().simulated = false;

		yield return new WaitForSeconds (1.5f);

		GetComponent<PlayerController>().enabled = true;
		GetComponent<SpriteRenderer>().enabled = true;
		GetComponent<Rigidbody2D> ().simulated = true;
		GetComponent<Collider2D>().enabled = true;

		respawning = false;

		if (ac.source.clip != ac.audio [0]) {
			ac.source.clip = ac.audio [0]; //Switch to default music
			ac.source.Play ();
		}

		backToCheckPoint ();
	}

	void ReducePlayerHealth(int dmg) {
		float height = GetComponent<BoxCollider2D>().size.y; 
		FloatingTextController.CreateFloatingText (dmg.ToString(), transform, height, Color.red, 15);
		health.CurrentVal -= dmg;
	}
}
