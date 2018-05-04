using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ExitReveal : MonoBehaviour {
	public GameObject room;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (GameObject.Find ("AcidBoss") == null)
			room.SetActive (false);
	}
}
