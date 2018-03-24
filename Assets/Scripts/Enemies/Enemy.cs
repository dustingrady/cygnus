using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enums are safer than strings, no chance of a typo and can be easily set in the editor
public enum Elements { fire, water, earth, metal, electric };

public abstract class Enemy : MonoBehaviour {
	// No need to construct the list with an array, you can do it this way
	protected List<string> damagingElements = new List<string> {
		"FireElement",
		"WaterElement",
		"EarthElement",
		"MetalElement",
		"ElectricElement"
	};

	[SerializeField]
	protected Elements elementType;
	[SerializeField]
	protected bool assignRandomType; // Check this if you want a random element

	[SerializeField]
	protected float hitpoints = 100;
	[SerializeField]
	protected int energy = 100;

	public void Start() {
		
		// Assigns a random element at the start
		if (assignRandomType) {
			int elementCount = System.Enum.GetValues (typeof(Elements)).Length;
			elementType = (Elements)Random.Range (0, elementCount);
		}
	}

	protected IEnumerator flash(){
		SpriteRenderer sr = GetComponent<SpriteRenderer> ();
		int elapsed = 0;
		int flashes = 3;
		while(elapsed < flashes){
			sr.color = Color.red;
			yield return new WaitForSeconds(0.10f);
			sr.color = Color.white;
			yield return new WaitForSeconds(0.10f);
			elapsed++;
		}
	}

	public Elements getEnemyType(){
		return elementType;
	}

	public float getEnemyHitPoints(){
		return hitpoints;
	}

	public float CalculatePhysicalImpact(Vector2 contactNorm, Vector2 vel, float mass) {
		return Vector3.Dot (contactNorm, vel) * mass;
	}

	public abstract void takeDamage(float amount);
}
