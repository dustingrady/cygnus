using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour {
	[SerializeField]
	private GameObject absorber;
	[SerializeField]
	private GameObject fire;
	[SerializeField]
	private float shootCooldown = 0.25f;
	[SerializeField]
	private float absorberSpeed = 10f;
	[SerializeField]
	private float fireSpeed = 10f;
	[SerializeField]
	private float absorberCooldown = 0.25f;
	private float shootTimer = 0f;
	private float absorbTimer = 0f;

	public static bool shotFromLeft;
	public static bool shotFromRight;

	private bool secondaryBtnRelease = true;
	private bool primaryBtnRelease = true;
	
	public delegate void ShootAction();
	public static event ShootAction OnShoot;

	GameManager gm;
	void Start() {
		gm = GameManager.instance;
	}

	void Update () {
		shootTimer += Time.deltaTime;
		absorbTimer += Time.deltaTime;

		if (gm.controllerConnected) {
			if (Input.GetAxisRaw ("SecondaryFireGamePad") <= 0.1) {
				secondaryBtnRelease = true;
			}
			if (Input.GetAxisRaw ("PrimaryFireGamePad") <= 0.1) {
				primaryBtnRelease = true;
			}

			if (Input.GetAxis("PrimaryFireGamePad") > 0.5f && shootTimer > shootCooldown && primaryBtnRelease) {
				Debug.Log ("Triggered primary fire on controller!");
				// Shoot goes here
			}

			if (Input.GetAxis("SecondaryFireGamePad") > 0.5f && absorbTimer > absorberCooldown && secondaryBtnRelease) {
				secondaryBtnRelease = false;
				Absorb ();

				// Broadcast the shoot event to anyone who cares (PlayerAnimations)
				if (OnShoot != null) {
					OnShoot ();
				}
			}
		} else {
			//if (Input.GetButton("PrimaryFire") && shootTimer > shootCooldown) {
			if (Input.GetButton("PrimaryFire") && absorbTimer > absorberCooldown) {
				DetermineLeftShot ();
			}

			if (Input.GetButton("SecondaryFire") && absorbTimer > absorberCooldown) {
				DetermineRightShot();

				// Broadcast the shoot event to anyone who cares (PlayerAnimations)
				if (OnShoot != null) {
					OnShoot ();
				}
			}
		}
	}

	//Determine what we should shoot (absorb or element)
	void DetermineLeftShot(){
		if (Absorber.fireInLeftHand) {
			Fire();
			Absorber.fireInLeftHand = false; //Mark false after used
		} else{
			shotFromLeft = true;
			Absorb ();
		}
	}

	//Determine what we should shoot (absorb or element)
	void DetermineRightShot(){
		if (Absorber.fireInRightHand) {
			Fire();
			Absorber.fireInRightHand = false; //Mark false after used
		} else{
			shotFromRight = true;
			Absorb ();
		}
	}

	void Absorb() {
		// No controller detected, use mouse and keyboard
		if (!gm.controllerConnected) {
			// Get the location of the mouse relative to the player
			Vector3 dirV3 = Camera.main.ScreenToWorldPoint (Input.mousePosition) - transform.position;

			// Convert this into a Vector2
			Vector2 dir = new Vector2 (dirV3.x, dirV3.y);

			// Instantiate the Sorb Orb with a direction and speed
			GameObject blt = Instantiate (absorber, transform.position, transform.rotation);
			blt.GetComponent<Absorber> ().Initialize (dir, absorberSpeed);

			// restart the clock on the shoot cooldown
			absorbTimer = 0f;
		} else {
			Debug.Log ("shooting absorb with controller");
			float x = Input.GetAxis ("RightHorizontal");
			float y = Input.GetAxis ("RightVertical");

			Vector2 dir = new Vector2 (x, -y).normalized;

			// Instantiate the Sorb Orb with a direction and speed
			GameObject blt = Instantiate (absorber, transform.position, transform.rotation);
			blt.GetComponent<Absorber> ().Initialize (dir, absorberSpeed);

			// restart the clock on the shoot cooldown
			absorbTimer = 0f;	
		}
	}


	void Fire(){
		// Get the location of the mouse relative to the player
		Vector3 dirV3 = Camera.main.ScreenToWorldPoint (Input.mousePosition) - transform.position;

		// Convert this into a Vector2
		Vector2 dir = new Vector2 (dirV3.x, dirV3.y);

		// Instantiate the Sorb Orb with a direction and speed
		GameObject blt = Instantiate (fire, transform.position, transform.rotation);
		blt.GetComponent<Fire> ().Initialize (dir, absorberSpeed);

		// restart the clock on the shoot cooldown
		absorbTimer = 0f;
	}
}
