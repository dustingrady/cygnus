using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchProjectile : MonoBehaviour {
	public Vector3 dir;
	public float strength;
	public float launchFrequency;

	[SerializeField]
	GameObject proj;

	// Use this for initialization
	void Start () {
		StartCoroutine (Launch ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator Launch() {
		GameObject go = Instantiate (proj, transform.position, Quaternion.identity);
		go.GetComponent<Rigidbody2D> ().AddForce (dir.normalized * strength);

		yield return new WaitForSeconds (launchFrequency);
	
		StartCoroutine (Launch ());
	}
}
