using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Magnetic : Element {
	float magnetStrength = 1000f;

	public LineRenderer lr;

	private float magnetCooldown = 3f;
	private float timeSinceFire;
	private bool fireReleased = true;
	public bool pulling = false;
	private const float maxRange = 10f;

	public Image icon;
	GameObject ui;
	Transform centerElement;
	bool iconCd = false;
	bool timerTick = false;

	Vector3 pullPos;
	GameObject plr;
	Vector3 diff;


	public override void UseElement(Vector3 pos, Vector2 dir) {


		if (fireReleased && !iconCd) {
			RaycastHit2D hit = Physics2D.Raycast (pos, dir, maxRange, 1 << LayerMask.NameToLayer ("Ground"));

			if (hit.collider != null) {
				if (hit.collider.CompareTag ("MetalElement")) {
					pullPos = hit.point;
					pulling = true;
				}
			}

			timeSinceFire = 0;
			fireReleased = false;

		} else if (pulling && diff.magnitude <= maxRange) {

			//Make the force stronger depending on how far away from the object the player is located
			plr.GetComponent<Rigidbody2D> ().AddForce ((magnetStrength / diff.magnitude) * diff.normalized);

			// Enable the line renderer
			lr.enabled = true;

		}
	}

	void Start() {
		ui = GameObject.Find ("UI");
		centerElement = ui.transform.Find ("CenterElement");
		icon = centerElement.Find ("Icon").transform.Find("IconCD").GetComponent<Image> ();
		// Get the transform of the player
		plr = gameObject.transform.root.gameObject;
		lr = GetComponent<LineRenderer> ();
	}

	void Update() {
		if (timerTick) {
			timeSinceFire += Time.deltaTime;
			//Debug.Log (timeSinceFire);
		}

		if (icon != null && iconCd) {
			icon.fillAmount = timeSinceFire / magnetCooldown;
		}
			
		// Find the position difference so you know what direction to apply the force
		diff = pullPos - plr.transform.position;
		if (lr.enabled) {
			// Update the line renderer
			lr.SetPositions (new Vector3[] { plr.transform.position + Vector3.back, pullPos + Vector3.back });
		}

		// UGLY
		if (Input.GetMouseButtonUp (2) == true) {
			Debug.Log ("test");
			fireReleased = true;
			pulling = false;
			timerTick = true;
			iconCd = true;

			// Disable the line renderer
			lr.enabled = false;
		}
		if (timeSinceFire >= magnetCooldown) {
			timerTick = false;
			timeSinceFire = 0;
			iconCd = false;
		}

		if (diff.magnitude > maxRange) {
			lr.enabled = false;
		}
	}
}
