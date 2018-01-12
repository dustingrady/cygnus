using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementUI : MonoBehaviour {

	private Player plr;
	public Image leftElementImg;
	public Image rightElementImg;
	public Image centerElementImg;

	public GameObject powerMeter; // The element power UI element

	void Start () {
		if (GameObject.FindGameObjectWithTag ("Player") != null) {
			plr = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();
		}

		Transform leftElement = transform.Find ("LeftElement");
		leftElementImg = leftElement.Find ("Icon").GetComponent<Image> ();

		Transform rightElement = transform.Find ("RightElement");
		rightElementImg = rightElement.Find ("Icon").GetComponent<Image> ();

		Transform centerElement = transform.Find ("CenterElement");
		centerElementImg = centerElement.Find ("Icon").GetComponent<Image> ();

		if (GameManager.instance.hasGloves == false) {
			leftElement.transform.localScale = Vector3.zero;
			rightElement.transform.localScale = Vector3.zero;
			centerElement.transform.localScale = Vector3.zero;
		}

		Absorber.OnAbsorb += UpdateElements;

		// Add Cooldown visual delegate here
	}
		

	void UpdateElements() {
		if (plr.leftElement != null) {
			leftElementImg.sprite = plr.leftElement.sprite;
			leftElementImg.enabled = true;
		}

		if (plr.rightElement != null) {
			rightElementImg.sprite = plr.rightElement.sprite;
			rightElementImg.enabled = true;
		}

		if (plr.centerElement != null) {
			centerElementImg.sprite = plr.centerElement.sprite;
			centerElementImg.enabled = true;
		} else {
			if (centerElementImg != null) {
				centerElementImg.sprite = null;
				centerElementImg.enabled = false;
			}
		}
	}
}
