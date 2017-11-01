﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

	public int health;
	public Element leftElement;
	public Element rightElement;
	public Dictionary<string, Element> elements;

    Inventory inventory;


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

        inventory = GameObject.Find("Game Manager").GetComponent<Inventory>();
	}
	
	// Update is called once per frame
	void Update () {
			
	}

	// Test for the Playground, if you hit Lava reload
	void OnTriggerEnter2D(Collider2D col) {
		Debug.Log (col.gameObject.name);
		if (col.gameObject.name == "Lava") {
			SceneManager.LoadScene (SceneManager.GetActiveScene().name);
		}

        if (col.gameObject.tag == "Item")
        {
            Debug.Log("Collided with item");
            string path = "Prefabs/Items/" + col.gameObject.name;
            GameObject temp = Resources.Load(path) as GameObject;
            inventory.GetComponent<Inventory>().addItem(temp);
        }
    }
}
