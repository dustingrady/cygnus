using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electricity : Element {
	public GameObject electProbe;
	bool electricOn = false;

	ParticleSystem.EmissionModule em;

	public override void UseElement(Vector3 pos, Vector2 dir){
		electricOn = true;

		// Change the angle to match the direction.
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		electProbe.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

		//GameObject fb = Instantiate (waterJet, pos, Quaternion.identity);
		//fb.GetComponent<WaterJet> ().Initialize (dir, jetStrength);
	}

	void Start() {
		em = electProbe.GetComponent<ParticleSystem> ().emission;
	}

	void Update() {
		if (electricOn) {
			em.rateOverTime = 50;
		} else {
			em.rateOverTime = 0;
		}


		electricOn = false;
	}
}