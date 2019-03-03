using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFloor : MonoBehaviour {
	public List<GameObject> tiles;
	public float spreadTime;
	public float igniteTime = 0;
	public bool ignited = false;
	public int currentBlock = 0;

	void Update() {
		if (ignited) {
			igniteTime += Time.deltaTime;
			if (igniteTime > spreadTime) {
				tiles [currentBlock].SetActive (true);
				if (currentBlock < tiles.Count - 1) {
					currentBlock++;
					igniteTime = 0;
				} else {
					ignited = false;
					currentBlock = 0;
				}
			}
		}
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.CompareTag("BossIgnite")) {
			ignited = true;
			Destroy (col.gameObject);
		}
	}




}
