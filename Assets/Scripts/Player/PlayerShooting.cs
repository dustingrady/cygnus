using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour {
	[SerializeField]
	private GameObject absorber;
	[SerializeField]
	private float absorberSpeed = 10f;
	[SerializeField]
	private float absorberCooldown = 0.25f;
	private float absorbTimer = 0f;

	public bool leftFireDown = false;
	public bool rightFireDown = false;

	// Event broadcast
	public delegate void ShootAction();
	public static event ShootAction OnShoot;

	private Player plr;

	// Left + Right combo for keyboard
	// Example
	// KeyCombo leftRight = new KeyCombo(new string[] {"PrimaryFire", "SecondaryFire"});

	GameManager gm;
	void Start() {
		gm = GameManager.instance;
		plr = GetComponent<Player> ();
	}

	void Update () {
		absorbTimer += Time.deltaTime;

		if (gm.controllerConnected) {
			if (Input.GetAxis ("SecondaryFireGamePad") < 0.1f) {
				leftFireDown = false;
			}

			if (Input.GetAxis ("PrimaryFireGamePad") < 0.1) {
				rightFireDown = false;
			}

			if (Input.GetButton ("LeftBumper") && absorbTimer > absorberCooldown && gm.hasGloves) {
				Absorb ("left");
			}
			else if (Input.GetButton ("RightBumper") && absorbTimer > absorberCooldown && gm.hasGloves) {
				Absorb ("right");
			}
				
			if (Input.GetAxis ("SecondaryFireGamePad") > 0.5f) {
				leftFireDown = true;
				if (plr.leftElement != null) {
					useElement ("left");
				}
			}

			if (Input.GetAxis ("PrimaryFireGamePad") > 0.5f) {
				rightFireDown = true;
				if (plr.rightElement != null) {
					useElement ("right");
				}

				// Broadcast the shoot event to anyone who cares (PlayerAnimations)
				if (OnShoot != null) {
					OnShoot ();
				}
			}

			if (Input.GetButton("RightStick")) {
				if (plr.centerElement != null) {
					useElement ("both");
				}
			}

		} else {
			 if (Input.GetButton ("PrimaryFire") && (Input.GetButton ("LeftCtrl")) && absorbTimer > absorberCooldown
				&& gm.hasGloves) {
				Absorb ("left");
			}
				
			else if (Input.GetButton ("SecondaryFire") && (Input.GetButton ("LeftCtrl")) && absorbTimer > absorberCooldown
				&& gm.hasGloves) {
				Absorb ("right");
			}

			if (Input.GetMouseButton(2)) {
				if (plr.centerElement != null) {
					useElement ("both");
				}
			}
				
			if (Input.GetButton ("PrimaryFire") && !(Input.GetButton ("LeftCtrl"))) {
				if (plr.leftElement != null) {
					useElement ("left");
				}
			}

			if (Input.GetButton ("SecondaryFire") && !(Input.GetButton ("LeftCtrl"))) {
				if (plr.rightElement != null) {
					useElement ("right");
				}
			}
			
		}
	}

	void Absorb(string hand) {

		Vector2 dir = GetCursorDirection ();

		// Instantiate the Sorb Orb with a direction and speed
		GameObject blt = Instantiate (absorber, transform.position, transform.rotation);
		blt.GetComponent<Absorber> ().Initialize (dir, absorberSpeed, hand);

		// restart the clock on the shoot cooldown
		absorbTimer = 0f;	
	}

	void useElement(string hand) {
		Vector2 dir = GetCursorDirection ();

		if (hand == "left") {
			plr.leftElement.UseElement (transform.position, dir);
		} else if (hand == "right") {
			plr.rightElement.UseElement (transform.position, dir);
		} else {
			plr.centerElement.UseElement (transform.position, dir);
		}
	}


	public Vector2 GetCursorDirection() {
		if (GameManager.instance.controllerConnected) {
			GameObject ret = GameObject.Find ("Reticle");
			Vector3 dirV3 = ret.transform.position - transform.position;
			Vector2 dir = new Vector2 (dirV3.x, dirV3.y);

			return dir;
		} else {
			Vector3 dirV3 = Camera.main.ScreenToWorldPoint (Input.mousePosition) - transform.position;
			Vector2 dir = new Vector2 (dirV3.x, dirV3.y);

			return dir;
		}
	}
}
