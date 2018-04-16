using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour {

	Vector2 initialPos;
	public GameObject target;
	bool goForth = false;
	bool switching = false;

	public float bladeSpeed = 5f;
	public float rotationSpeed = 200f;
	public float waitTime = 3f;

	public int spinDir = 1;

	public bool stationary = false;
	// Use this for initialization
	void Start () {
		initialPos = new Vector2(transform.position.x, transform.position.y);
	}
	
	// Update is called once per frame
	void Update () {
		rotatingBlade ();
		if (target != null && !stationary) {
			moving ();
		}
	}

	void rotatingBlade()
	{
		transform.Rotate (Vector3.back *spinDir, rotationSpeed*Time.deltaTime);
	}

	void moving()
	{
		if (goForth) {
			transform.position = new Vector2 (Mathf.Lerp(transform.position.x, target.transform.position.x, bladeSpeed*Time.deltaTime), Mathf.Lerp (transform.position.y, target.transform.position.y, bladeSpeed * Time.deltaTime));
			if (Mathf.Abs (transform.position.magnitude - target.transform.position.magnitude) < 0.5f && switching) {
				switching = false;
				StartCoroutine (delay ());
			}

		} else {
			transform.position = new Vector2 (Mathf.Lerp(transform.position.x, initialPos.x, bladeSpeed*Time.deltaTime), Mathf.Lerp (transform.position.y, initialPos.y, bladeSpeed * Time.deltaTime));
			if (Mathf.Abs (transform.position.magnitude - initialPos.magnitude) < 0.5f && !switching) {
				switching = true;
				StartCoroutine (delay ());
			}
		}
	}

	IEnumerator delay()
	{
		yield return new WaitForSeconds (waitTime);
		goForth = !goForth;
	}
}
