using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ForestRespawner : MonoBehaviour {

	public List<GameObject> originalMaps;
	public List<GameObject> clonedMaps;

	// Use this for initialization
	void Start () {
		MakeClones ();
	}


	void OnEnable() {
		Player.OnDeath += Respawn;
	}

	void OnDestroy() {
		Player.OnDeath -= Respawn;
	}


	void MakeClones() {
		foreach (GameObject map in originalMaps) {
			// Create a clone and make sure it's active
			GameObject clone = Instantiate (map);
			clone.transform.parent = gameObject.transform;
			clone.SetActive (true);

			// Deactivate original
			map.SetActive (false);

			// Add it to the list to be tracked
			clonedMaps.Add (clone);
		}
	}


	void Respawn() {
		foreach (GameObject map in clonedMaps) {
			Destroy (map);
		}
		clonedMaps.Clear ();
		MakeClones ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
