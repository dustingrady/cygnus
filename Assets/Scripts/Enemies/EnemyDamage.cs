using System.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour {
	public Dictionary<Elements, string> weaknesses;
	public Dictionary<Elements, string> resistances;

	public float baseFire = 15f;
	public float baseWater = 4f;
	public float baseElectric = 0.1f;
	public float baseEarth = 10f;

	void Start(){
		FloatingTextController.Initialize ();
		//x is weak to y
		weaknesses = new Dictionary<Elements, string> ();
		weaknesses.Add(Elements.fire, "WaterElement");
		weaknesses.Add(Elements.water, "ElectricElement");
		weaknesses.Add(Elements.earth, "WaterElement");
		weaknesses.Add(Elements.metal, "FireElement");
		weaknesses.Add(Elements.electric, "EarthElement");

		//x is resistant to y
		resistances = new Dictionary<Elements, string> ();
		resistances.Add (Elements.fire, "MetalElement"); 
		resistances.Add (Elements.water, "FireElement");
		resistances.Add (Elements.earth, "ElectricElement");
		resistances.Add (Elements.metal, "MetalElement"); 
		resistances.Add (Elements.electric, "WaterElement");
	}

	public float determine_Damage(string attackType, Elements enemyType, float direct = 1f){
		float dmg = 0f; //Base damage
		switch (attackType) {
		case "FireElement":
			dmg = baseFire;
			break;
		case "WaterElement":
			dmg = baseWater;
			break;
		case "EarthElement":
			dmg = (int)direct;
			break;
		case "ElectricElement":
			dmg = baseElectric;
			break;
		}

		if (weaknesses [enemyType] == attackType) {
			dmg *= 2f;
		}
		if(resistances[enemyType] == attackType){
			dmg *= 0.5f;
		}
		//Debug.Log ("Dealt " + dmg + " dmg to " + enemyType + " type enemy");

		display_Damage (dmg);
		return dmg;
	}

	// Creates the floating text for the enemy damage
	private void display_Damage(float dmg) {

		// Scale the damage size between 12 and 35 based on how much damage was delt
		float dmgSize = Mathf.Lerp (12, 38, dmg / 80);

		// Reduce the amount of green to make the color more red based on damage
		Color baseClr = Color.yellow;
		baseClr.g -= dmg / 100;

		FloatingTextController.CreateFloatingText (dmg.ToString(), this.gameObject.transform, baseClr, (int)dmgSize);
	}
}
