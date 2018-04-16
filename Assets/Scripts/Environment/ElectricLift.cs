using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricLift : MonoBehaviour {

	public GameObject target;

	bool playerOnTop = false;

	float liftSpeed = 3f;

	public bool electrifying = false;
	public bool goingRight = true;
	public bool goingDown = true;

	int dirx;
	int diry;
	int particlesCount = 0;
	int prevParticleCount = 0;

	float timer = 0;

	public enum state{
		vertical,
		horizontal
	}

	public state orientation;

	// Use this for initialization
	void Start () {
		if (goingRight) {
			dirx = 1;
		}
		else if(!goingRight){
			dirx = -1;
		}

		if (goingDown) {
			diry = -1;
		} else if (!goingDown) {
			diry = 1;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (prevParticleCount != particlesCount) {
			prevParticleCount = particlesCount;
			timer = 0;
			electrifying = true;
		}

		if (prevParticleCount == particlesCount) {
			timer += Time.deltaTime;
			//electrifying = false;
			//particlesCount = 0;
		}

		if (timer >= 3f) {
			electrifying = false;
			particlesCount = 0;
		}
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
			particlesCount++;
			if (orientation == state.horizontal) {
				if (Mathf.Abs (this.transform.position.x - target.transform.position.x) >= 0.1) {
					transform.position = new Vector2 (transform.position.x + liftSpeed * Time.deltaTime*dirx, transform.position.y);
				}
			}

			if (orientation == state.vertical) {
				if (Mathf.Abs (this.transform.position.y - target.transform.position.y) >= 0.1) {
					transform.position = new Vector2 (transform.position.x, transform.position.y + liftSpeed *diry* Time.deltaTime);
				}
			}
		}
	}

}
