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

	public override void UseElement(Vector3 pos, Vector2 dir){
		if (metalReleased) {
			shieldInstance = Instantiate (metalShield, pos, Quaternion.identity);
			metalReleased = false;
		}
	}

	void Start() {
		plrs = transform.root.GetComponent<PlayerShooting> ();
	}

	void Update() {
		if (Input.GetMouseButtonUp (0) == true || Input.GetMouseButtonUp (1) == true ) {
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
}