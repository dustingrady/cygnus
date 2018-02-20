using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleShellsRemoval : MonoBehaviour {

	private ParticleSystem particles;

	// Use this for initialization
	void Start () {
		particles = GetComponent<ParticleSystem> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!particles.IsAlive ()) {
			Destroy (this.gameObject);
		}
	}
}
