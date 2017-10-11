using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public int health;
	public Element leftElement;
	public Element rightElement;
	public Dictionary<string, Element> elements;

	void Start () {
		// Initialize a dictionary of elements that can be found in the world
		elements = new Dictionary<string, Element> ();
		Element fire = GetComponentInChildren<Fire> ();
		elements.Add("fire", fire);

		Element water = GetComponentInChildren<Water> ();
		elements.Add("water", water);
	}
	
	// Update is called once per frame
	void Update () {
			
	}
}
