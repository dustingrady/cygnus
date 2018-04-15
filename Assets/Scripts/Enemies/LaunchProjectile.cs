using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchProjectile : MonoBehaviour {
	public Vector3 dir;
	public float strength;
	public float launchFrequency;
	public float launchOffset;
	public float projectileDuration;
	public bool physicsObject = true;

	[SerializeField]
	GameObject proj;

	// Use this for initialization
	void Start () {
		if (launchOffset != 0) {
			StartCoroutine (Delay ());
		} else {
			if (physicsObject)
				StartCoroutine (PhysicsLaunch ());
			else
				StartCoroutine (Launch ());
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator PhysicsLaunch() {
		GameObject go = Instantiate (proj, transform.position, Quaternion.identity);
		Rigidbody2D rb = go.GetComponent<Rigidbody2D> ();

		if (rb != null) {
			go.GetComponent<Rigidbody2D> ().AddForce (dir.normalized * strength);
		}

		yield return new WaitForSeconds (launchFrequency);
	
		StartCoroutine (PhysicsLaunch ());
	}

	IEnumerator Launch() {
		GameObject go = Instantiate (proj, transform.position, Quaternion.identity);
		Projectile projectileComponent = go.GetComponent<Projectile> ();
		if (projectileComponent != null) {
			projectileComponent.dir = this.dir;
			projectileComponent.speed = strength;
			projectileComponent.ttl = projectileDuration;
		}

		yield return new WaitForSeconds (launchFrequency);

		StartCoroutine (Launch ());
	}

	IEnumerator Delay() {
		yield return new WaitForSeconds (launchOffset);
		if (physicsObject)
			StartCoroutine (PhysicsLaunch ());
		else
			StartCoroutine (Launch ());
	}
}
