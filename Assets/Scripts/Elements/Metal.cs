using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Metal : Element {
	public GameObject metalShield;
	private GameObject shieldInstance;
	private PlayerShooting plrs;

	public Image licon;
	public Image ricon;
	bool lbutton = false;
	bool rbutton = false;

	private const float maxStrength = 3;
	private float shieldStrength = maxStrength;

	[SerializeField]
	private float distance = 1f;
	private bool metalReleased = true;

	private bool destroyed = false;
	private float cooldown = 5;
	private float cdCounter = 0;
	// Checks for controller release
	private bool leftFireDown = false;
	private bool rightFireDown = false;

	ElementUI eUI;
	Transform leftElement;
	Transform rightElement;

	public override void UseElement(Vector3 pos, Vector2 dir){
		leftFireDown = plrs.leftFireDown;
		rightFireDown = plrs.rightFireDown;

		if (metalReleased && !destroyed) {
			shieldInstance = Instantiate (metalShield, pos, Quaternion.identity);
			metalReleased = false;

			GameObject ui = GameObject.Find ("UI");
			leftElement = ui.transform.Find ("LeftElement");
			rightElement = ui.transform.Find ("RightElement");

			eUI = ui.GetComponent<ElementUI> ();

			if (leftElement.Find ("Icon").transform.Find("IconCD").GetComponent<Image> ().sprite == this.sprite) {
				licon = leftElement.Find ("Icon").transform.Find("IconCD").GetComponent<Image> ();
			} 

			if (rightElement.Find ("Icon").transform.Find("IconCD").GetComponent<Image> ().sprite == this.sprite) {
				ricon = rightElement.Find ("Icon").transform.Find("IconCD").GetComponent<Image> ();
			}
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

		if (!metalReleased && shieldInstance != null) {
			// Get the direction of the cursor relative to the player
			Vector2 dir = plrs.GetCursorDirection ();

			// Set the angle of the shield
			float angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;
			shieldInstance.transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);

			// Position at a specific distance from the player
			shieldInstance.transform.position = plrs.transform.position + (new Vector3 (dir.x, dir.y, 0).normalized * distance);
		}

		if (shieldStrength <= 0) {
			Destroy (shieldInstance);
			shieldStrength = maxStrength;
			destroyed = true;
		}

		if (destroyed) {
			cdCounter += Time.deltaTime;
			if(!eUI.lhandCD && lbutton)
				licon.fillAmount = cdCounter / cooldown;
			if (!eUI.rhandCD && rbutton)
				ricon.fillAmount = cdCounter / cooldown;
		}


		//continue the cd fill when swapped back to element
		if (destroyed && leftElement != null) {
			if (leftElement.Find ("Icon").GetComponent<Image> ().sprite == this.sprite) {
				licon.fillAmount = cdCounter / cooldown;
			}
		}
		//continue the cd fill when swapped back to element
		if (destroyed && rightElement != null) {
			if (rightElement.Find ("Icon").GetComponent<Image> ().sprite == this.sprite) {
				ricon.fillAmount = cdCounter / cooldown;
			}
		}

		if (cdCounter >= cooldown) {
			eUI.lhandCD = false;
			eUI.rhandCD = false;
			destroyed = false;
			cdCounter = 0;
		}
		if (eUI != null) {
			//Debug.Log ("Metal update " + eUI.lhandCD + " " + eUI.rhandCD);
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

	public void reduceStrength(float damage)
	{
		shieldStrength -= damage;
		Debug.Log ("shield strength: " + shieldStrength);
	}
}