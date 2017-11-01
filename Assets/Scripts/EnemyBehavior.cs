using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour {
	private Vector3 startingPos;

	public float delta = 15f; //How far we move left and right
	public float speed = 2.0f; //How fast we move left and right
	public int health = 100;
	// Use this for initialization
	void Start () {
		startingPos = transform.position; //Initialize startingPos
	}

	void Update () {
		Behavior ();
	}

	void Behavior(){
		//---------Movement---------
		Vector3 v = startingPos;
		v.x += delta * Mathf.Sin (Time.time * speed);
		transform.position = v;	

		//-------Check if dead-------
		if (health <= 0) {
			Destroy(this.gameObject);
		}
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

	//These could be status effects/ damage to the enemy (unless we want to put that somewhere else)
	private void FireEffect(){
		GetComponent<SpriteRenderer> ().color = Color.red;
		speed = 2.0f; //Modify speed
		health -= 20; //Deal damage
		Debug.Log ("Enemy Health: " + health);
	}

	private void IceEffect(){
		GetComponent<SpriteRenderer> ().color = Color.blue; //Change sprite to blue
		speed = 1.0f; //Modify speed
		health -= 10; //Deal damage
		Debug.Log ("Enemy Health: " + health);
	}
}
