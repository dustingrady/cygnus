using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour {

    [SerializeField]
    public Stat health;
	public Element leftElement;
	public Element rightElement;
    public Element centerElement;

	bool onFire = false;

	bool standingInFire = false;

	bool takingDamage = false;

    public Inventory inventory;

    void Awake () {
		health.Initalize();
		// Inventory references need to be reset each time a scene loads
        inventory = GameObject.Find("Game Manager").GetComponent<Inventory>();
		inventory.initializeInventory ();
	}
	
	// Update is called once per frame
	void Update () {
		if (health.CurrentVal <= 0) {
			inventory.emptyInventory ();
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
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
	
	public void healWounds(float amount)
	{
		this.health.CurrentVal += amount;
		Debug.Log ("healing for: " + amount);
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

					} else if (tilemap.GetTile (tilemap.WorldToCell (rightBounds)) != null) {
                        //GetComponent<PlayerController> ().StartGrapple ("left");
                        GetComponent<PlayerController>().Grapple = PlayerController.GrappleState.Left;

                        // Disable the magnetic pull
                        centerElement.gameObject.GetComponent<Magnetic>().pulling = false;
					}

				}
			}	
		}
			
		if (col.gameObject.tag == "Scrap") {
			inventory.addScrap (1);
			Destroy (col.gameObject);
		}

		if (col.gameObject.tag == "Enemy" && !takingDamage) {
			// Trigger knockback and flash
			TouchedEnemy(col.gameObject);

			StartCoroutine (enemyOnContact (20));
		}
			
	}

	void OnCollisionStay2D(Collision2D col)
	{
		if (col.gameObject.tag == "Enemy" && !takingDamage) {
			StartCoroutine (enemyOnContact (20));
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
		if (col.gameObject.name == "Lava" || col.gameObject.tag == "BossSpecial") {
			inventory.emptyInventory ();
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		}

		if (col.gameObject.tag == "Acid") {
			StartCoroutine (acidContact (1));
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
	}

	void OnTriggerStay2D(Collider2D col){
		if ((col.gameObject.tag == "LavaPlatform" && !standingInFire)) {
			StartCoroutine(singularDamage(5));
			StartCoroutine (flash());
		}

		if ((col.gameObject.tag == "FireElement" && !standingInFire)) {
			StartCoroutine(singularDamage(5));
			StartCoroutine (flash());
		} if (col.gameObject.tag == "Acid") {
			StartCoroutine (acidContact (1));
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
            health.CurrentVal -= damageAmount;
            yield return new WaitForSeconds (1);
            currentTick++;
        }

        onFire = false;
    }

    IEnumerator singularDamage(int damageAmount)
    {
        standingInFire = true;
        health.CurrentVal -= damageAmount;
        yield return new WaitForSeconds (2);
        standingInFire = false;
    }

	//Damage from enemy's projectiles
	IEnumerator enemyProjectiles(int damageAmount)
	{
		takingDamage = true;
		health.CurrentVal -= damageAmount;
		yield return new WaitForSeconds (2);
		takingDamage = false;
	}

	//Damage from contact with enemies
	IEnumerator enemyOnContact(int damageAmount)
	{
		takingDamage = true;
		health.CurrentVal -= damageAmount;
		yield return new WaitForSeconds (1);
		takingDamage = false;
	}

	IEnumerator acidContact(int damageAmount) 
	{
		health.CurrentVal -= damageAmount;
		yield return new WaitForSeconds (0.3f);
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
}
