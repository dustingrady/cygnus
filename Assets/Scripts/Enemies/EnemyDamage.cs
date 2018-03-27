using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour {
	public Dictionary<Elements, string> weaknesses;
	public Dictionary<Elements, string> resistances;

	public float baseFire = 15f;
	public float baseWater = 10f;
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
			dmg = direct;
			break;
		case "ElectricElement":
			dmg = baseElectric;
			break;
		}

		/*Crit*/
		if (weaknesses [enemyType] == attackType) {
			dmg *= 2f;
		}
		/*Resist*/
		if(resistances[enemyType] == attackType){
			dmg *= 0.5f;
		}
		//Debug.Log ("Dealt " + dmg + " dmg to " + enemyType + " type enemy");
		//Debug.Log("Object position: " + this.gameObject.transform.position);
		FloatingTextController.CreateFloatingText (dmg, this.gameObject.transform); //Testing floating damage
		return dmg;
	}
}
