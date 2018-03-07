/* Function: Determines what the enemy will drop when they die
 * Status: Working/ In development
 * Bugs: 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDrop : MonoBehaviour {
	private GameObject metalDrop, fireDrop, waterDrop, earthDrop, electricDrop;

	void Start(){
		metalDrop = (GameObject)Resources.Load("Prefabs/Items/Pickups/MetalDrop");
		fireDrop = (GameObject)Resources.Load("Prefabs/Items/Pickups/FireDrop");	
		waterDrop = (GameObject)Resources.Load("Prefabs/Items/Pickups/WaterDrop");	
		earthDrop = (GameObject)Resources.Load("Prefabs/Items/Pickups/EarthDrop");	
		electricDrop = (GameObject)Resources.Load("Prefabs/Items/Pickups/ElectricDrop");	
	}

	public void determine_Drop(string type, Vector3 pos){
		if(type == "metal"){
			Instantiate (metalDrop, pos, Quaternion.identity);
		}
		if(type == "fire"){
			Instantiate (fireDrop, pos, Quaternion.identity);
		}
		if(type == "water"){
			Instantiate (waterDrop, pos, Quaternion.identity);
		}
		if(type == "earth"){
			Instantiate (earthDrop, pos, Quaternion.identity);
		}
		if(type == "electric"){
			Instantiate (electricDrop, pos, Quaternion.identity);
		}
	}

	/*
	private void drop_Epic_Item(){
		//Drop twin blades of azzinoth
	}
	*/
}
