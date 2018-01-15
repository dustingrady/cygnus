using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueProjectile : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine (deleteRogueProjectiles ());	
	}

	IEnumerator deleteRogueProjectiles()
	{
		yield return new WaitForSeconds (2);
		Destroy (this.gameObject);
	}
}
