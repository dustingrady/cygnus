using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
	public Vector3 dir;
	public float speed;
	public float ttl = 2f;
	public bool breakable = true;

	// Use this for initialization
	void Start () {
		StartCoroutine (TimeOut (ttl));
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = transform.position + (dir.normalized * speed * Time.deltaTime);
	}

	void OnCollisionEnter2D() {
		Debug.Log ("hittin");
		if (breakable) {
			Destroy (gameObject);
		}
	}

	IEnumerator TimeOut(float t) {
		yield return new WaitForSeconds (t);
		Destroy (gameObject);
	}
}
