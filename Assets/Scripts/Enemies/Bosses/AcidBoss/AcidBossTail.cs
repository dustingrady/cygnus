using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidBossTail : MonoBehaviour {

	private AcidBoss head;

	// Use this for initialization
	void Start () {
		head = GameObject.Find ("AcidBoss").GetComponent<AcidBoss> ();
	}

	void OnParticleCollision(GameObject other){
		head.TakeDamage (other.tag);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
