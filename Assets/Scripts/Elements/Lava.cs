using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : Element{
	public GameObject lava;

	private bool btnReleased = true;
	private GameObject plr;

	public override void UseElement(Vector3 pos, Vector2 dir){
		if (btnReleased){
			GameObject steamObject = Instantiate(lava, pos, Quaternion.identity);

			steamObject.GetComponent<ParticleSystem>().Play();

			// Destroy the system after it finishes
			//Destroy(steamObject, 1f);

			Debug.Log("Shooting lava");
			btnReleased = false;
		}
	}

	void Start(){

	}

	void Update(){
		if (Input.GetMouseButtonUp(2) == true){
			btnReleased = true;
		}
	}
}
