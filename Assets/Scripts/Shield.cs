using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {

	private Metal metal;
	private GameObject Player;

	// Use this for initialization
	void Start () {
		Player = GameObject.FindGameObjectWithTag ("Player");
		metal = Player.gameObject.GetComponentInChildren<Metal> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "BossBullet" || col.gameObject.tag == "EnemyProjectile") {
			metal.reduceStrength (1f);
		}

		if (col.gameObject.tag == "BossSpecial" || col.gameObject.tag == "Deflected") {
			metal.reduceStrength (5f);
		}
	}
}
