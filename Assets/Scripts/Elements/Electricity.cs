using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electricity : Element {
	public GameObject electProbe;
	bool electricOn = false;

	public int particleRate = 50;

	ParticleSystem.EmissionModule em;

	public override void UseElement(Vector3 pos, Vector2 dir){
		electricOn = true;

		// Change the angle to match the direction.
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		electProbe.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}

	void Start() {
		em = electProbe.GetComponent<ParticleSystem> ().emission;
	}

	void Update() {
		if (electricOn) {
			em.rateOverTime = particleRate;
		} else {
			em.rateOverTime = 0;
		}


		electricOn = false;
	}
}