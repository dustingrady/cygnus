using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidBoss : Enemy {
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
		}
	}

	void OnTriggerEnter2D(Collider2D col) {
		if(damagingElements.Contains (col.gameObject.tag))
			takeDamage(edmg.determine_Damage (col.gameObject.tag, elementType));
	}

	void OnCollisionEnter2D(Collision2D col) {
		EvaluatePhysical (col);
	}

	void OnParticleCollision(GameObject other){
		ElectricShock (other.tag);
	}

}

