using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Steam : Element {
	public GameObject burst;
	public float burstStrength = 5f;
	private const float maxForce = 40f;

	private const float steamCapacity = 20f;
	private float currentCapacity = steamCapacity;
	private bool outOfSteam = false;
	private bool iconCd = false;
	private bool timerTick = false;

	private float burstCooldown = 10f;
	private float timeSinceFire;
	private bool btnReleased = true;

	GameObject steamObject;
	public Image icon;

	private PowerMeter powerMeter;
	float percentRemain;
	GameObject ui;
	Transform centerElement;
	bool showPower = false;

	public override void UseElement(Vector3 pos, Vector2 dir){
		if (Input.GetMouseButtonDown (2) && centerElement.Find ("Icon").transform.Find("IconCD").GetComponent<Image> ().sprite == this.sprite) {
			icon = centerElement.Find ("Icon").transform.Find("IconCD").GetComponent<Image> ();
		} 
			
		if(currentCapacity >= 0 && btnReleased && !iconCd){
			steamObject = Instantiate (burst, pos, Quaternion.identity);

			// Change the angle to match the direction.
			float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
			steamObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

			steamObject.GetComponent<ParticleSystem> ().Play ();

			btnReleased = false;

			// Set the powermeter color and show the meter
			powerMeter.SetBarColor(Color.white);
			powerMeter.Show ();
			showPower = true; // Tracks if the meter is in a show state
		}
	}

	void Start() {
		ui = GameObject.Find ("UI");
		centerElement = ui.transform.Find ("CenterElement");

		if (GameObject.FindGameObjectWithTag ("PowerMeter") != null) {
			powerMeter = GameObject.FindGameObjectWithTag ("PowerMeter").GetComponent<PowerMeter> ();
		} else {
			Debug.LogError ("Unable to locate player power meter");
		}

	}

	void Update() {
		Vector2 direction = GetCursorDirection ();
		// Set the bar to the remaining flightTime
		//float percentRemain = (flightTime - flightTimer)/flightTime;
		percentRemain = (currentCapacity)/steamCapacity;

        if (active)
        {
            powerMeter.SetBarValue(percentRemain);
        }

		if (percentRemain > 1f && showPower == true) {
			powerMeter.Hide ();
			showPower = false;
		}

		if (!btnReleased && currentCapacity >= 0) {
			float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
			if (steamObject != null) {
				steamObject.transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);

				Vector3 handPos = new Vector3 (direction.normalized.x, direction.normalized.y, 0) * 0.5f;
				steamObject.transform.position = transform.root.position + handPos;
			}
				
			if (transform.root.GetComponent<ConstantForce2D> () != null) {
				if (transform.root.GetComponent<ConstantForce2D> ().force.magnitude <= maxForce) {
					transform.root.GetComponent<ConstantForce2D> ().force += (-direction.normalized * burstStrength);
				} else if (transform.root.GetComponent<ConstantForce2D> ().force.magnitude >= maxForce) {
					transform.root.GetComponent<ConstantForce2D> ().force = new Vector2(transform.root.GetComponent<ConstantForce2D> ().force.x *(-direction.normalized.x), transform.root.GetComponent<ConstantForce2D> ().force.y *(-direction.normalized.y));
				}
			}

			currentCapacity -= Time.deltaTime*15;
		}
			
		if(btnReleased || currentCapacity <= 0){
			if (transform.root.GetComponent<ConstantForce2D> () != null)
				transform.root.GetComponent<ConstantForce2D> ().force = Vector2.zero;
			if(steamObject != null)
				steamObject.GetComponent<ParticleSystem> ().Stop ();
		}

		// DISABLE THE COOLDOWN for gameplay testing!
		/*
		if (currentCapacity <= 0) {
			timerTick = true;
			iconCd = true;
		}
		*/

		if (timerTick) {
			timeSinceFire += Time.deltaTime;

			if (icon != null && iconCd && active) {
				icon.fillAmount = timeSinceFire / burstCooldown;
			} else {
				icon.fillAmount = 1;
			}
		}


		if (currentCapacity <= steamCapacity) {
			currentCapacity += Time.deltaTime*5;
			Mathf.Clamp (currentCapacity, 0, steamCapacity - 2f);

            if (active)
            {
                powerMeter.Show();
                showPower = true; // Tracks if the meter is in a show state
            }
		}

		if (Input.GetMouseButtonUp (2) == true
			|| Input.GetButtonUp("RightStick")) {
			btnReleased = true;
			if (timeSinceFire >= burstCooldown) {
				timerTick = false;
				timeSinceFire = 0;
				iconCd = false;
				//powerMeter.Hide();
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