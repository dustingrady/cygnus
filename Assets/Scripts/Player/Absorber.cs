using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Absorbs different materials for player to use
public class Absorber : MonoBehaviour {
	public static bool fireInLeftHand;
	public static bool fireInRightHand;

	//public bool iceInHand;
	//...
	public float speed;
	public Vector3 direction;

	public void Initialize(Vector2 direction, float speed) {
		this.speed = speed;
		this.direction = direction.normalized;
	}

	void Update() {
		transform.position += direction * speed * Time.deltaTime;
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.tag != "Player") {
			DestroyObject (this.gameObject);
		}

		if (col.gameObject.tag == "FireElement") {
			//Check if we obtained fire with left or right hand
			if (PlayerShooting.shotFromLeft) {
				fireInLeftHand = true; //We now have fire to use
				Debug.Log("That was fired from the left hand");

			}
			if(PlayerShooting.shotFromRight){
				fireInRightHand = true; //We now have fire to use
				Debug.Log("That was fired from the right hand");
			}
				
			DestroyObject (this.gameObject);
			DestroyObject (col.gameObject);
			PlayerShooting.shotFromRight = false;
			PlayerShooting.shotFromLeft = false;
		}
	}
}
