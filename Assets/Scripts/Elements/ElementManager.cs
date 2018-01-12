using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementManager : MonoBehaviour {

	public Dictionary<string, Element> elements;
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

		Element electric = GetComponent<Electricity> ();
		elements.Add ("electric", electric);

		Element magnetic = GetComponent<Magnetic> ();
		elements.Add ("magnetic", magnetic);

		// Get a reference to the player
		plr = transform.root.GetComponent<Player>();
	}
	
	public void AssignToHand(string hand, string itemTag) {
		if (itemTag == "FireElement") {
			if (hand == "left") {
				plr.leftElement = elements["fire"];
			} else {
				plr.rightElement = elements["fire"];
			}
		}

		if (itemTag == "WaterElement" || itemTag == "Ice") {
			if (hand == "left") {
				plr.leftElement = elements["water"];
			} else {
				plr.rightElement = elements["water"];
			}
		}

		if (itemTag == "EarthElement") {
			if (hand == "left") {
				plr.leftElement = elements["earth"];
			} else {
				plr.rightElement = elements["earth"];
			}
		}

		if (itemTag == "MetalElement") {
			if (hand == "left") {
				plr.leftElement = elements["metal"];
			} else {
				plr.rightElement = elements["metal"];
			}
		}

		if (itemTag == "ElectricElement") {
			if (hand == "left") {
				plr.leftElement = elements["electric"];
			} else {
				plr.rightElement = elements["electric"];
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
			Debug.Log ("Adding Steam as combo");
			plr.centerElement = elements["steam"];

		}

		// Fire and water crossover
		else if ((le == "electric" && re == "metal") || (le == "metal" && re == "electric")) {
			plr.centerElement = elements["magnetic"];

		}

		// Fire and earth crossover
		else if ((le == "fire" && re == "earth") || (le == "earth" && re == "fire")) {
			plr.centerElement = null;
		}

		// No crossover
		else {
			Debug.Log ("Not a valid combo");
			plr.centerElement = null;
		}

	}
}
