using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Steam : Element {
	public GameObject burst;
	private float burstStrength = 5f;
	private const float maxForce = 40f;
	private const float flightTime = 2;
	private float flightTimer = 0;
	[SerializeField]
	private float burstCooldown = 1.5f;
	private float timeSinceFire;
	private bool btnReleased = true;

	Vector2 direction = Vector2.zero;
	GameObject steamObject;
	public Image icon;

	private PowerMeter powerMeter;

	public override void UseElement(Vector3 pos, Vector2 dir){
		GameObject ui = GameObject.Find ("UI");
		Transform centerElement = ui.transform.Find ("CenterElement");

		if (Input.GetMouseButtonDown (2) && centerElement.Find ("Icon").transform.Find("IconCD").GetComponent<Image> ().sprite == this.sprite) {
			icon = centerElement.Find ("Icon").transform.Find("IconCD").GetComponent<Image> ();
		} 


		if (timeSinceFire > burstCooldown && btnReleased) {
			steamObject = Instantiate (burst, pos, Quaternion.identity);

			// Change the angle to match the direction.
			float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
			steamObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
			//steamObject.transform.parent = transform.root;
			direction = dir;

			steamObject.GetComponent<ParticleSystem> ().Play ();
			//transform.root.GetComponent<Rigidbody2D> ().AddForce (-dir.normalized * burstStrength);

			btnReleased = false;

			// Set the powermeter color and show the meter
			powerMeter.SetBarColor(Color.white);
			powerMeter.Show ();
		}
	}

	void Start() {
		if (GameObject.FindGameObjectWithTag ("PowerMeter") != null) {
			powerMeter = GameObject.FindGameObjectWithTag ("PowerMeter").GetComponent<PowerMeter> ();
		} else {
			Debug.LogError ("Unable to locate player power meter");
		}
	}

	void Update() {
		direction = GetCursorDirection ();

		timeSinceFire += Time.deltaTime;
		if (icon != null) {
			icon.fillAmount = timeSinceFire / burstCooldown;
		}

		if (!btnReleased) {
			float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
			if (steamObject != null) {
				steamObject.transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);
				steamObject.transform.position = transform.root.position;
			}

			//Debug.Log (transform.root.GetComponent<ConstantForce2D> ().force.magnitude + " " +flightTimer);
			if (transform.root.GetComponent<ConstantForce2D> () != null) {
				if (transform.root.GetComponent<ConstantForce2D> ().force.magnitude <= maxForce) {
					transform.root.GetComponent<ConstantForce2D> ().force += (-direction.normalized * burstStrength);
				} else if (transform.root.GetComponent<ConstantForce2D> ().force.magnitude >= maxForce) {
					transform.root.GetComponent<ConstantForce2D> ().force = new Vector2(transform.root.GetComponent<ConstantForce2D> ().force.x *(-direction.normalized.x), transform.root.GetComponent<ConstantForce2D> ().force.y *(-direction.normalized.y));
				}
			}

			if(flightTimer < flightTime)
				flightTimer += Time.deltaTime;

			// Set the bar to the remaining flightTime
			float percentRemain = (flightTime - flightTimer)/flightTime;
			powerMeter.SetBarValue(percentRemain);

		}

		if (btnReleased || flightTimer >= flightTime) {
			if (transform.root.GetComponent<ConstantForce2D> () != null)
				transform.root.GetComponent<ConstantForce2D> ().force = Vector2.zero;
			if(steamObject != null)
				steamObject.GetComponent<ParticleSystem> ().Stop ();
		}

		if (Input.GetMouseButtonUp (2) == true
			|| Input.GetButtonUp("RightStick")) {
			if (timeSinceFire >= burstCooldown) {
				btnReleased = true;
				timeSinceFire = 0;
				flightTimer = 0;

				powerMeter.Hide();
			}
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