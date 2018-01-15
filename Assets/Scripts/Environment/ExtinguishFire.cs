using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtinguishFire : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "WaterElement") { //Catch that shit on fire, yo
			Debug.Log("Water detected");
			Destroy (this.gameObject);
		}
	}
}
