﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveFallingPlatform : MonoBehaviour {

	void OnCollisionEnter2D(Collision2D col)
	{
		StartCoroutine (Remove ());
	}

	IEnumerator Remove()
	{
		yield return new WaitForSeconds (2);
		Destroy (this.gameObject);
	}
}
