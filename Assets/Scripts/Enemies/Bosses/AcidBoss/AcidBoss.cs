using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidBoss : Enemy {
	private float radius = 35.0f; //How far we can see player
	private bool shootingPlayer;
	private EnemyShooting es;
	public bool isShooting = false;
	public bool protocolOn = true;
	private bool healBool = true;

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

		if (DistanceToPlayer () <= radius) {
			es.shoot_At_Player ();
			isShooting = true;
		} else {
			isShooting = false;
		}

		if (protocolOn && healBool) {
			healBool = false;
			StartCoroutine (heal (10.0f));
		}
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (damagingElements.Contains (col.gameObject.tag)) {
			takeDamage (edmg.determine_Damage (col.gameObject.tag, elementType));
		}

		if (col.gameObject.tag == "ProtocolGrenade")
			protocolOn = false;
	}

	void OnCollisionEnter2D(Collision2D col) {
		EvaluatePhysical (col);
	}

	void OnParticleCollision(GameObject other){
		ElectricShock (other.tag);
	}
		
	IEnumerator heal(float amount) {
		if(hitpoints < maxHitPoints)
			hitpoints += amount;

		yield return new WaitForSeconds (2);
		healBool = true;
	}
}

