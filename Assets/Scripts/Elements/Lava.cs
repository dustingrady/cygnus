using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : Element {
	public GameObject lavaJet;
	[SerializeField]
	private float lavaJetCooldown = 0.5f;
	[SerializeField]
	private float jetStrength = 500;
	private float timeSinceFire;

	public override void UseElement(Vector3 pos, Vector2 dir){
		if (timeSinceFire > lavaJetCooldown) {
			GameObject fb = Instantiate (lavaJet, pos, Quaternion.identity);
			fb.GetComponent<LavaStream> ().Initialize (dir, jetStrength);
			timeSinceFire = 0;
		}
	}

	void Update() {
		timeSinceFire += Time.deltaTime;
	}
}