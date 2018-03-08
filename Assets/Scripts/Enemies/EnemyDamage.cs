using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour {
	public Dictionary<string, string> weaknesses;
	public Dictionary<string, string> resistances;

	void Start(){
		//x is weak to y
		weaknesses = new Dictionary<string,string> ();
		weaknesses.Add("fire", "WaterElement");
		weaknesses.Add("water", "ElectricElement");
		weaknesses.Add("earth", "WaterElement");
		weaknesses.Add("metal", "FireElement");
		weaknesses.Add("electric", "EarthElement");

		//x is resistant to y
		resistances = new Dictionary<string,string> ();
		resistances.Add ("fire", "MetalElement"); 
		resistances.Add ("water", "FireElement");
		resistances.Add ("earth", "ElectricElement");
		resistances.Add ("metal", "MetalElement"); 
		resistances.Add ("electric", "WaterElement");
	}
		
	public float determine_Damage(Collider2D col, string type){
		float dmg = 0f; //Base damage
		switch (col.gameObject.tag) {
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

		if (weaknesses [type] == col.gameObject.tag) {
			dmg *= 2f;
		}
		if(resistances[type] == col.gameObject.tag){
			dmg *= 0.5f;
		}

		//Debug.Log ("Dealt " + dmg + " dmg to " + type + " type enemy");
		return dmg;
	}
}
