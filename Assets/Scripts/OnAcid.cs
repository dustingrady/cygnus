using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnAcid : MonoBehaviour {

	public List<ParticleSystem> ps = new List<ParticleSystem>();
	float timer = 0;
	// Use this for initialization
	void Start () {
		timer = Random.Range (3, 6f);
	}
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		if (timer <= 0) {
			int randomChance = Random.Range (0, ps.Count);
			ps [randomChance].Play ();
			timer = Random.Range (3, 6f);
		}
		Debug.Log (timer);
	}

}
