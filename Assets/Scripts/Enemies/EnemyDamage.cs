﻿using System.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour {
	public Dictionary<Elements, string> weaknesses;
	public Dictionary<Elements, string> resistances;
	public Dictionary<Elements, string> immunities;

	public float baseFire = 12f;
	public float baseWater = 3f;
	public float baseElectric = 0.1f;
	public float earthMultiplier = 2f;
	public float baseLava = 20f;

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
		resistances.Add (Elements.electric, "WaterElement");

		//x is immune
		immunities = new Dictionary<Elements, string> ();
		immunities.Add (Elements.fire, "FireElement"); 
		immunities.Add (Elements.water, "WaterElement");
		immunities.Add (Elements.earth, "EarthElement");
		immunities.Add (Elements.metal, "MetalElement"); 
		immunities.Add (Elements.electric, "ElectricElement");
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
			dmg = (int)(direct * earthMultiplier);
			break;
		case "ElectricElement":
			dmg = baseElectric;
			break;
		case "Lava":
			dmg = baseLava;
			break;
		}

		if (weaknesses.ContainsKey (enemyType)) {
			if (weaknesses [enemyType] == attackType) {
				dmg *= 2f;
			}
		}

		if (resistances.ContainsKey (enemyType)) {
			if (resistances [enemyType] == attackType) {
				dmg *= 0.5f;
			}
		}

		// Immune to damage from it's own element
		if (immunities.ContainsKey (enemyType)) {
			if (immunities [enemyType] == attackType) {
				dmg = 0;
			}
		}

		Debug.Log ("Dealt " + dmg + " " + attackType + " type dmg to " + enemyType + " type enemy");

		display_Damage (dmg);
		return dmg;
	}

	// Creates the floating text for the enemy damage
	private void display_Damage(float dmg) {

		// Scale the damage size between 12 and 35 based on how much damage was delt
		float dmgSize = Mathf.Lerp (18, 35, dmg / 80);
		float height;
		// Reduce the amount of green to make the color more red based on damage
		Color baseClr = Color.yellow;
		baseClr.g -= dmg / 100;

		//Get y axis offset for where text should be spawned
		if (this.gameObject.GetComponent<BoxCollider2D> () != null) {//If there is a box collider
			height = GetComponent<BoxCollider2D> ().size.y; 
		} 
		else if (this.gameObject.GetComponent<CapsuleCollider2D> () != null) { //If there is a capsule collider
			height = GetComponent<CapsuleCollider2D> ().size.y-2; //Capsule collider seems to be acting taller than it is, offsetting by -2
		} 
		else {
			height = 1; //Default
		}

		if (dmg > 0) {
			FloatingTextController.CreateFloatingText (dmg.ToString (), this.gameObject.transform, height, baseClr, (int)dmgSize);
		} else {
			FloatingTextController.CreateFloatingText ("immune", this.gameObject.transform, height, Color.white, 20);
		}
	}
}
