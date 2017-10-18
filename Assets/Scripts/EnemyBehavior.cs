using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour {
	private Vector3 startingPos;


	public float delta = 15f; //How far we move left and right
	public float speed = 2.0f; //How fast we move left and right
	// Use this for initialization
	void Start () {
		startingPos = transform.position; //Initialize startingPos
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 v = startingPos;
		v.x += delta * Mathf.Sin (Time.time * speed); //Sin gives better movement
		transform.position = v;
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.tag == "Fireball") {
			Debug.Log ("You hit an enemy with a fireball");
			FireEffect();
		}
		if (col.gameObject.tag == "WaterJet") {
			Debug.Log ("You hit an enemy with a water jet");
			IceEffect ();
		}
	}

	//These could be status effects on/ damage to the enemy (unless we want to put that somewhere else)
	private void FireEffect(){
		GetComponent<SpriteRenderer> ().color = Color.red;
		//yield return new WaitForSeconds (1); 
		//renderer.material.color = 
	}

	private void IceEffect(){
		GetComponent<SpriteRenderer> ().color = Color.blue;
	}
}
