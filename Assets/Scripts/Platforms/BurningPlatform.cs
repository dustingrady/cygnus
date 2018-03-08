using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningPlatform : MonoBehaviour {
	void Start() {
		StartCoroutine ("remove");
	}

	IEnumerator remove()
	{
		yield return new WaitForSeconds (1);
		Destroy (this.gameObject);
	}
}
