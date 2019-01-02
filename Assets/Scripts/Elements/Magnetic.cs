using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnetic : Element {
	public float magnetStrength = 600f;
	public int maxMagnetStrength = 200;

	[SerializeField]
	private float magnetCooldown = 0.2f;

	[SerializeField]
	private const float magneticCapacity = 30f;
	private float currentCapacity = magneticCapacity;

	private float timeSinceFire;
	private bool fireReleased = true;
	public bool pulling = false;
	Vector3 pullPos;

	// UI
	private PowerMeter powerMeter;
	private bool showPower = false;
	public LineRenderer lr;

	public override void UseElement(Vector3 pos, Vector2 dir) {
		if (timeSinceFire > magnetCooldown && fireReleased) {

			RaycastHit2D hit = Physics2D.Raycast (pos, dir, 100, 1 << LayerMask.NameToLayer ("Ground"));

			if (hit.collider != null) {
				if (hit.collider.CompareTag ("MetalElement")) {
					pullPos = hit.point;
					pulling = true;
				}
			}

			timeSinceFire = 0;
			fireReleased = false;

		} else if (pulling && currentCapacity > Time.deltaTime * 20) {
			// Get the transform of the player
			GameObject plr = gameObject.transform.root.gameObject;

			// Find the position difference so you know what direction to apply the force
			Vector3 diff = pullPos - plr.transform.position;

			// Make the force stronger depending on how far away from the object the player is located
			Vector3 pull = ClampVector((magnetStrength / diff.magnitude) * diff.normalized, maxMagnetStrength);
			plr.GetComponent<Rigidbody2D> ().AddForce (pull);

			// Enable the line renderer
			lr.enabled = true;

			// Update the line renderer
			lr.SetPositions (new Vector3[] { plr.transform.position + Vector3.back, pullPos + Vector3.back });

			if (!showPower) {
				// Set the powermeter color and show the meter
				powerMeter.SetBarColor(Color.magenta);
				powerMeter.Show ();
				showPower = true;
			}

			currentCapacity -= Time.deltaTime * 20;
		}
	}

	Vector3 ClampVector(Vector3 vect, int max) {
		return new Vector3 (Mathf.Clamp (vect.x, -max, max), Mathf.Clamp (vect.y, -max, max), Mathf.Clamp (vect.z, -max, max));
	}


	void Start() {
		lr = GetComponent<LineRenderer> ();

		if (GameObject.FindGameObjectWithTag ("PowerMeter") != null) {
			powerMeter = GameObject.FindGameObjectWithTag ("PowerMeter").GetComponent<PowerMeter> ();
		} else {
			Debug.LogError ("Unable to locate player power meter");
		}
	}

	void Update() {
		timeSinceFire += Time.deltaTime;

		if (currentCapacity < magneticCapacity) {
			currentCapacity += Time.deltaTime * 5;
			powerMeter.SetBarValue (currentCapacity/magneticCapacity);
		}

		if (showPower && currentCapacity > magneticCapacity) {
			powerMeter.Hide ();
			showPower = false;
		}

		// UGLY
		if (Input.GetMouseButtonUp (2) == true) {
			fireReleased = true;
			pulling = false;

			// Disable the line renderer
			lr.enabled = false;
		}
	}

	
}




/**
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Magnetic : Element {
	float magnetStrength = 300f;

	public LineRenderer lr;

	private float magnetCooldown = 0f;
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
			// Find the position difference so you know what direction to apply the force
			diff = pullPos - plr.transform.position;
			float strength = Mathf.Abs (diff.magnitude - maxRange) / maxRange; 
			//Make the force stronger depending on how far away from the object the player is located
			//plr.GetComponent<Rigidbody2D> ().AddForce ((magnetStrength / diff.magnitude) * diff.normalized);
			plr.GetComponent<Rigidbody2D> ().AddForce ((magnetStrength/diff.magnitude) * strength*diff.normalized);
			Debug.Log (strength + " " + magnetStrength/diff.magnitude);

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
			
		if (lr.enabled) {
			// Update the line renderer
			lr.SetPositions (new Vector3[] { plr.transform.position + Vector3.back, pullPos + Vector3.back });
		}

		// UGLY
		if (Input.GetMouseButtonUp (2) == true) {
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
*/