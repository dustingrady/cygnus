using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidBossTest2 : Enemy {
	private float radius = 40.0f; //How far we can see player
	private bool shootingPlayer;
	private EnemyShooting es;

	private CameraSwitch cs;

	private void Awake(){
		es = gameObject.GetComponent<EnemyShooting> ();
	}

	// Use this for initialization
	void Start () {
		cs = GameObject.Find ("CameraSwapTrigger").GetComponent<CameraSwitch> ();
		base.Start (); // Call the based enemy Start() function	
	}
	
	// Update is called once per frame
	void Update () {
		if (this.hitpoints <= 0) {
			cs.playerCam = true;
		}
		EvaluateHealth ();

		if (DistanceToPlayer() <= radius) {
			es.shoot_At_Player ();
			//takeDamage (0.01f);
		}
	}

}

