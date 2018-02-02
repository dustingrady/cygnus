using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metal : Element {
	public GameObject metalShield;
	private GameObject shieldInstance;
	private PlayerShooting plrs;
	[SerializeField]
	private float shieldStrength = 500;
	[SerializeField]
	private float distance = 1f;
	private bool metalReleased = true;

	// Checks for controller release
	private bool leftFireDown = false;
	private bool rightFireDown = false;

	public override void UseElement(Vector3 pos, Vector2 dir){
		leftFireDown = plrs.leftFireDown;
		rightFireDown = plrs.rightFireDown;

		if (metalReleased) {
			shieldInstance = Instantiate (metalShield, pos, Quaternion.identity);
			metalReleased = false;
		}
	}

	void Start() {
		plrs = transform.root.GetComponent<PlayerShooting> ();
	}

	void Update() {
		if (ShotRelease()) {
			metalReleased = true;
			Destroy (shieldInstance);
		}

		if (!metalReleased) {
			// Get the direction of the cursor relative to the player
			Vector2 dir = plrs.GetCursorDirection ();

			// Set the angle of the shield
			float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
			shieldInstance.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

			// Position at a specific distance from the player
			shieldInstance.transform.position = plrs.transform.position + (new Vector3(dir.x, dir.y, 0).normalized * distance);
		}
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