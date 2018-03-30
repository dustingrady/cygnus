using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementManager : MonoBehaviour {

	public Dictionary<string, Element> elements;
	public Dictionary<string, string> tagToElement;
	private Player plr;

	void Start () {
		// Initialize a dictionary of elements that can be found in the world
		elements = new Dictionary<string, Element> ();
		Element fire = GetComponent<Fire> ();
		elements.Add("fire", fire);

		Element water = GetComponent<Water> ();
		elements.Add("water", water);

		Element earth = GetComponent<Earth> ();
		elements.Add ("earth", earth);

		Element metal = GetComponent<Metal> ();
		elements.Add ("metal", metal);

		Element steam = GetComponent<Steam> ();
		elements.Add ("steam", steam);

		Element lava = GetComponent<Lava>();
		elements.Add("lava", lava);

		Element electric = GetComponent<Electricity> ();
		elements.Add ("electric", electric);

		Element magnetic = GetComponent<Magnetic> ();
		elements.Add ("magnetic", magnetic);

		tagToElement = new Dictionary<string, string> () {
			{ "FireElement", "fire" },
			{ "WaterElement", "water" },
			{ "EarthElement", "earth" },
			{ "MetalElement", "metal" },
			{ "ElectricElement", "electric" },
		};

		// Get a reference to the player
		plr = transform.root.GetComponent<Player>();
	}
	
	public void AssignToHand(string hand, string itemTag) {

		foreach (var element in tagToElement) {
			if (itemTag == element.Key) {
				if (hand == "left") {
					if (plr.leftElement != null)
						plr.leftElement.active = false;			// Disable the previous element
					plr.leftElement = elements[element.Value];	// Set the new element to the hand
					plr.leftElement.active = true;				// Enable that new element
				} else {
					if (plr.rightElement != null)
						plr.rightElement.active = false;
					plr.rightElement = elements[element.Value];
					plr.rightElement.active = true;
				}
			}
		}

		// If either hand is empty, don't attempt to combine them
		if (plr.leftElement != null && plr.rightElement != null) {
			elementCrossover ();
		}
	}

	void elementCrossover() {
		string le = plr.leftElement.elementType;
		string re = plr.rightElement.elementType;

		// Fire and water crossover
		if ((le == "fire" && re == "water") || (le == "water" && re == "fire")) {
			if (plr.centerElement != null)
				plr.centerElement.active = false;			
			plr.centerElement = elements["steam"];
			plr.centerElement.active = true;
		}

		// Metal and electric crossover
		else if ((le == "electric" && re == "metal") || (le == "metal" && re == "electric")) {
			if (plr.centerElement != null)
				plr.centerElement.active = false;
			plr.centerElement = elements["magnetic"];
			plr.centerElement.active = true;
		}

		// Fire and earth crossover
		else if ((le == "fire" && re == "earth") || (le == "earth" && re == "fire")) {
			if (plr.centerElement != null)
				plr.centerElement.active = false;
			plr.centerElement = elements["lava"];
			plr.centerElement.active = true;
		}

		// No crossover
		else {
			Debug.Log ("Not a valid combo");
			plr.centerElement = null;
		}

	}
}
