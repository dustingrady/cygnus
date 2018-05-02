using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWallTrigger : MonoBehaviour {
	public GameObject wall;
	public bool wallOn = false;
	private GameObject boss;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (wallOn == false || GameObject.Find ("AcidBoss") == null)
			wall.SetActive(false);
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.tag == "Player") {
			wall.SetActive(true);
			wallOn = true;
		}
	}
}
