using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnAbsorb : MonoBehaviour {
	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "SorbOrb") {
			Destroy (this.gameObject);
		}
	}
}
