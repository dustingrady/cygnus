using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricLift : MonoBehaviour {

	public GameObject target;

	bool playerOnTop = false;

	float liftSpeed = 3f;

	public enum state{
		vertical,
		horizontal,
		diagonal
	}

	public state orientation;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.name == "Player") {
			playerOnTop = true;
			//Vector3 s = col.gameObject.transform.localScale;
			col.transform.parent = transform;

			//col.gameObject.transform.localScale = s;
		}
	}

	void OnCollisionExit2D(Collision2D col){
		if (col.gameObject.name == "Player") {
			playerOnTop = false;

			col.transform.parent = null;
		}
	}

	void OnParticleCollision(GameObject other){
		if (other.tag == "ElectricElement" && playerOnTop) {
			if (orientation == state.horizontal) {
				if (Mathf.Abs (this.transform.position.x - target.transform.position.x) >= 0.5) {
					transform.position = new Vector2 (transform.position.x + liftSpeed * Time.deltaTime, transform.position.y);
				}
			}

			if (orientation == state.vertical) {
				if (Mathf.Abs (this.transform.position.y - target.transform.position.y) >= 0.5) {
					transform.position = new Vector2 (transform.position.x, transform.position.y + liftSpeed * Time.deltaTime);
				}
			}

			if (orientation == state.diagonal) {
				if (Mathf.Abs(transform.position.magnitude - target.transform.position.magnitude) >= 0.5) {
					transform.position = new Vector2 (transform.position.x + liftSpeed * Time.deltaTime, transform.position.y + liftSpeed * Time.deltaTime);
				}
			}
		}
			
	}
}
