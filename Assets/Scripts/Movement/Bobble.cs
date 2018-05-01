using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bobble : MonoBehaviour {

	public float amplitude = 1.5f;
	public float frequency = 0.2f;

	void Update () {
		float theta = Time.timeSinceLevelLoad / frequency;
		float distance = amplitude * Mathf.Sin (theta) * Time.deltaTime;
		transform.position = new Vector3 (transform.position.x, transform.position.y + distance, 0f);
	}
}
