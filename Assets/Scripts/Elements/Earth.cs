using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Earth : Element {
	public GameObject earth;
	[SerializeField]
	private float boulderStrength = 500;
	[SerializeField]
	private float earthCooldown = 1.5f;
	[SerializeField]
	private float maxCharge = 2f;

	private float chargeTime;
	private float timeSinceFire;
	private bool shotCharging = false;

	PlayerShooting plrs;

	public GameObject elementPower; // The element power UI element


	public override void UseElement(Vector3 pos, Vector2 dir){
		if (timeSinceFire > earthCooldown) {
			shotCharging = true;

			// enable the graphic
			elementPower.transform.localScale = new Vector3(0.005f, 0.005f, 1f);
		}
	}


	void Start() {
		plrs = transform.root.GetComponent<PlayerShooting> ();

		if (GameObject.Find("Element Power") != null) {
			elementPower = GameObject.Find("Element Power");
		}
	}


	void Update() {
		if (shotCharging) {
			if (chargeTime < maxCharge) {
				chargeTime += Time.deltaTime;

				float chargePercent = Mathf.Round((chargeTime / maxCharge) * 100);
				elementPower.GetComponent<Text> ().text = chargePercent.ToString();
			} else {
				chargeTime = maxCharge;
			}
		} else {
			timeSinceFire += Time.deltaTime;
		}

		if ((Input.GetMouseButtonUp (0) == true || Input.GetMouseButtonUp (1) == true) && shotCharging) {
			FireBoulder ();
		}
	}

	void FireBoulder() {
		shotCharging = false;

		// Get the direction of the cursor relative to the player
		Vector2 dir = plrs.GetCursorDirection ();
		Vector3 pos = plrs.transform.position;

		Debug.Log ("Firing a boulder");

		// Get the scale of the boulder based on the charge time - 1 being full charge
		float scale = Mathf.Clamp((chargeTime / maxCharge), 0.3f, 1.0f);

		GameObject fb = Instantiate (earth, pos, Quaternion.identity);

		fb.transform.localScale = new Vector3 (scale, scale, scale);

		fb.GetComponent<Boulder> ().Initialize (dir, boulderStrength/scale);

		// Start the cooldown
		timeSinceFire = 0;

		// Reset the chargeTime so the next projectile starts at 0 strength
		chargeTime = 0;

		// disable the graphic
		elementPower.transform.localScale = Vector3.zero;
	}
}