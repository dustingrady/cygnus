using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour {
	public Dictionary<Elements, string> weaknesses;
	public Dictionary<Elements, string> resistances;

	void Start(){
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
		
	public float determine_Damage(string attackType, Elements enemyType){
		float dmg = 0f; //Base damage
		switch (attackType) {
		case "FireElement":
			dmg = 15f;
			break;
		case "WaterElement":
			dmg = 10f;
			break;
		case "EarthElement":
			dmg = 20f; //Need to figure out how to change this based on boulder size
			break;
		case "ElectricElement":
			dmg = 0.2f;
			break;
		}

		if (weaknesses [enemyType] == attackType) {
			dmg *= 2f;
		}
		if(resistances[enemyType] == attackType){
			dmg *= 0.5f;
		}

		//Debug.Log ("Dealt " + dmg + " dmg to " + type + " type enemy");
		return dmg;
	}
}
