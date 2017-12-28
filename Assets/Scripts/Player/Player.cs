using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

    [SerializeField]
    private Stat health;
	public Element leftElement;
	public Element rightElement;
    public Element centerElement;
	public Dictionary<string, Element> elements;

    Inventory inventory;

	bool onFire = false;

	bool standingInFire = false;

    private void Awake()
    {
        health.Initalize();
    }

    void Start () {
		// Initialize a dictionary of elements that can be found in the world
		elements = new Dictionary<string, Element> ();
		Element fire = GetComponentInChildren<Fire> ();
		elements.Add("fire", fire);

		Element water = GetComponentInChildren<Water> ();
		elements.Add("water", water);

		Element earth = GetComponentInChildren<Earth> ();
		elements.Add ("earth", earth);

		Element metal = GetComponentInChildren<Metal> ();
		elements.Add ("metal", metal);

		// Inventory references need to be reset each time a scene loads
        inventory = GameObject.Find("Game Manager").GetComponent<Inventory>();
		inventory.initializeInventory ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Q)) {
            health.CurrentVal -= 10;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            health.CurrentVal += 10;
        }

		if (health.CurrentVal <= 0) {
			inventory.emptyInventory ();
			SceneManager.LoadScene (SceneManager.GetActiveScene().name);
		}
    }
		
	void OnTriggerEnter2D(Collider2D col) {
		
		// Test for the Playground, if you hit Lava reload
		if (col.gameObject.name == "Lava") {
			Debug.Log (col.gameObject.name);
			inventory.emptyInventory ();
			SceneManager.LoadScene (SceneManager.GetActiveScene().name);
		}

        if (col.gameObject.tag == "Item")
        {
            //Debug.Log("Collided with item");
            //string path = "Items/" + col.gameObject.name;
			//Item temp = Resources.Load(path) as Item;
			Item item = col.gameObject.GetComponent<ItemInteraction>().item;

			if (item != null) {
				inventory.GetComponent<Inventory>().addItem(item);
			} else {
				Debug.LogError ("There was no item on that object!");
			}
        }

		/*
		 * DAMAGE SECTION
		 */

		//TODO:
		//Enemies projectiles
		//Collision with enemies

		if (col.gameObject.name == "Fire" && !standingInFire) {
			StartCoroutine(singularDamage(5));
		}
    }

	void OnTriggerStay2D(Collider2D col)
	{
		if (col.gameObject.name == "Fire"  && !standingInFire) {
			Debug.Log (health.CurrentVal + " " + onFire);
			StartCoroutine(singularDamage(5));
		}
	}

	void OnTriggerExit2D(Collider2D col)
	{
		if (col.gameObject.name == "Fire" && !onFire) {
			StartCoroutine(damageOverTime (5, 1));
		}
	}


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
}
