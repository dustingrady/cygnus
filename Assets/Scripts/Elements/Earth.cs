using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Earth : Element {
	public GameObject earth;
	[SerializeField]
	private float maxBoulderStrength = 45f;
	private float boulderStrength = 0f;
	[SerializeField]
	private float maxMass = 1.4f;
	[SerializeField]
	private float earthCooldown = 0.4f;
	[SerializeField]
	private float maxCharge = 2.5f;
	[SerializeField]
	private float chargeRate = 1.5f;

	private float chargeTime;
	private float timeSinceFire;
	private bool shotCharging = false;

	// Checks for controller release
	private bool leftFireDown = false;
	private bool rightFireDown = false;

	PlayerShooting plrs;

	public PowerMeter powerMeter; // The element power UI element


	public override void UseElement(Vector3 pos, Vector2 dir){
		leftFireDown = plrs.leftFireDown;
		rightFireDown = plrs.rightFireDown;

		if (timeSinceFire > earthCooldown) {
			if (shotCharging == false) {
				// Start charging the projectile
				shotCharging = true;

				// Reset value of bar from previous shot
				powerMeter.SetBarValue(0f);

				// Reset color from previous shot
				powerMeter.SetBarColor(new Color(0.8f, 0.4f, 0f, 1f));

				// enable the graphic
				powerMeter.Show();
			}
		}
	}


	void Start() {
		plrs = transform.root.GetComponent<PlayerShooting> ();

		if (GameObject.FindGameObjectWithTag ("PowerMeter") != null) {
			powerMeter = GameObject.FindGameObjectWithTag ("PowerMeter").GetComponent<PowerMeter> ();
		} else {
			Debug.LogError ("Unable to locate player power meter");
		}
	}


	void Update() {
		if (shotCharging) {
			if (chargeTime < maxCharge) {
				chargeTime += Time.deltaTime * chargeRate;

				float chargePercent = chargeTime / maxCharge;

                if (active)
				    powerMeter.SetBarValue (chargePercent);

			} else {
				chargeTime = maxCharge;
			}
		} else {
			timeSinceFire += Time.deltaTime;
		}

		if (ShotRelease() && shotCharging) {
			FireBoulder ();
		}
	}

	void FireBoulder() {
		shotCharging = false;

		// Get the direction of the cursor relative to the player
		Vector2 dir = plrs.GetCursorDirection ();
		Vector3 pos = plrs.transform.position;

		// Get the scale of the boulder based on the charge time - 1 being full charge
		float scale = Mathf.Clamp((chargeTime / maxCharge), 0.3f, 1.0f);
		float scaleMass = Mathf.Clamp((chargeTime / maxCharge), 0.1f, 1.0f);
		boulderStrength = maxBoulderStrength * scale;

		Vector3 handPos = new Vector3 (dir.normalized.x, dir.normalized.y, 0) * 0.8f;
		GameObject fb = Instantiate (earth, pos + handPos, Quaternion.identity);

		fb.transform.localScale = new Vector3 (scale, scale, scale);

		fb.GetComponent<Boulder> ().Initialize (dir, Mathf.Pow(boulderStrength, 2));

		// Setting the mass of the boulder based on the scale
		fb.GetComponent<Rigidbody2D>().mass = scaleMass * maxMass;

		// Start the cooldown
		timeSinceFire = 0;

		// Reset the chargeTime so the next projectile starts at 0 strength
		chargeTime = 0;

		// disable the graphic
		powerMeter.Hide();

        //Debug.Log("boulder fired");
	}

	bool ShotRelease() {
		if (GameManager.instance.controllerConnected) {
			if (plrs.leftFireDown == false && leftFireDown == true
				|| plrs.rightFireDown == false && rightFireDown == true) {
				return true;
			}
		} else if ((Input.GetMouseButtonUp (0) == true || Input.GetMouseButtonUp (1) == true)) {
			return true;
		}

		return false;
	}
}