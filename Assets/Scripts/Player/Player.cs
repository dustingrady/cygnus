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

	bool onFire = false;
	bool inAcid = false;
	private Coroutine acidDamageCoroutine;

	bool standingInFire = false;

	bool takingDamage = false;

    public Inventory inventory;

	private Vector3 checkpointPos;

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
    }
	
	// Update is called once per frame
	void Update () {
		if (health.CurrentVal <= 0) {
			CheckHealth ();
		}

		//if(Input.GetKeyDown("space")) {
		//	StopCoroutine(acidDamageCoroutine);
		//}
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
	
	public void healWounds(float amount)
	{
		this.health.CurrentVal += amount;
		Debug.Log ("healing for: " + amount);
	}
	

	void CheckHealth() {
		if (health.CurrentVal <= 0) {
			if (checkpointPos != null) {
				//Debug.Log ("going to checkpoint");
				health.CurrentVal = 100;
				transform.position = checkpointPos;
			} else {
				inventory.emptyInventory ();
				//Debug.Log ("Resetting scene");
				StopCoroutine (acidDamageCoroutine);
				SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
			}
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
			/*
			if (col.gameObject.tag == "Item") {
				Debug.Log ("ITEMS");
				//string path = "Items/" + col.gameObject.name;
				//Item temp = Resources.Load(path) as Item;

				Item item = col.gameObject.GetComponent<ItemInteraction>().item;

				if (item != null) {
					Debug.Log ("TEST");
					//inventory.GetComponent<Inventory>().addItem(item);
					//Debug.Log(inventory.GetComponent<Inventory> ().checkSlot(item));
					if (inventory.GetComponent<Inventory> ().checkSlot (item)) 
					{
						inventory.GetComponent<Inventory> ().stackItem (item);
					} 
					else if (!inventory.GetComponent<Inventory> ().checkSlot (item)) 
					{
						inventory.GetComponent<Inventory> ().addItem (item);
						Debug.Log ("TEST add");
					}
				} else {
					Debug.LogError ("There was no item on that object!");
				}
			}*/
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
			this.health.CurrentVal -= 1f;
		}

		if (other.tag == "Lava") {
			this.health.CurrentVal -= 5f;
			//StartCoroutine(damageOverTime(5,1));
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
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		}

		if (col.gameObject.tag == "BossBullet") {
			Debug.Log ("ouch, a fuckin bossbullet");
			StartCoroutine(enemyProjectiles(1));
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
			FloatingTextController.CreateFloatingText ("Checkpoint!", this.gameObject.transform, height, Color.blue, 20);
			checkpointPos = col.transform.position;
		}
	}

	void OnTriggerStay2D(Collider2D col){
		if ((col.gameObject.tag == "LavaPlatform" && !standingInFire)) {
			StartCoroutine(singularDamage(5));
			StartCoroutine (flash());
		}

        if (col.gameObject.tag == "QuicksandXD" && !standingInFire) {
            StartCoroutine(singularDamage(25));
        }

		if ((col.gameObject.tag == "FireElement" && !standingInFire)) {
			StartCoroutine(singularDamage(5));
			StartCoroutine (flash());
		} 

		if (col.gameObject.tag == "Acid" && !inAcid) {
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


    //FOR FIRE ONLY
    IEnumerator damageOverTime(int ticks, int damageAmount)
    {
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

	void ReducePlayerHealth(int dmg) {
		float height = GetComponent<BoxCollider2D>().size.y; 
		FloatingTextController.CreateFloatingText (dmg.ToString(), transform, height, Color.red, 15);
		health.CurrentVal -= dmg;
	}
}
