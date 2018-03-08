using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementUI : MonoBehaviour {


	private Player plr;
	public Image leftElementImg;
	public Image leftElementImgCd;

	public Image rightElementImg;
	public Image rightElementImgCd;

	public Image centerElementImg;
	public Image centerElementImgCd;

	public GameObject powerMeter; // The element power UI element

	void Start () {
		if (GameObject.FindGameObjectWithTag ("Player") != null) {
			plr = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();
		}

		if (GameManager.instance.hasGloves == false) {
			DisableElements ();
		}

		Absorber.OnAbsorb += UpdateElements;

		// Add Cooldown visual delegate here
	}


	void UpdateElements() {
		if (plr.leftElement != null) {
			leftElementImg.sprite = plr.leftElement.sprite;
			leftElementImgCd.sprite =  plr.leftElement.sprite;

			leftElementImg.enabled = true;
			leftElementImgCd.enabled = true;

			leftElementImgCd.type = Image.Type.Filled;
		}

		if (plr.rightElement != null) {
			rightElementImg.sprite = plr.rightElement.sprite;
			rightElementImgCd.sprite = plr.rightElement.sprite;

			rightElementImg.enabled = true;
			rightElementImgCd.enabled = true;

			rightElementImgCd.type = Image.Type.Filled;
		}

		if (plr.centerElement != null) {
			centerElementImg.sprite = plr.centerElement.sprite;
			centerElementImgCd.sprite = plr.centerElement.sprite;

			centerElementImg.enabled = true;
			centerElementImgCd.enabled = true;

			centerElementImgCd.type = Image.Type.Filled;
		} else {
			if (centerElementImg != null) {
				centerElementImg.sprite = null;
				centerElementImg.enabled = false;

				centerElementImgCd.sprite = null;
				centerElementImgCd.enabled = false;
			}
		}
	}


	public void EnableElements() {
		Transform leftElement = transform.Find ("LeftElement");
		leftElementImg = leftElement.Find ("Icon").GetComponent<Image> ();
		leftElementImgCd = leftElement.Find ("Icon").Find ("IconCD").GetComponent<Image> ();

		Transform rightElement = transform.Find ("RightElement");
		rightElementImg = rightElement.Find ("Icon").GetComponent<Image> ();
		rightElementImgCd = leftElement.Find ("Icon").Find ("IconCD").GetComponent<Image> ();

		Transform centerElement = transform.Find ("CenterElement");
		centerElementImg = centerElement.Find ("Icon").GetComponent<Image> ();
		centerElementImgCd = leftElement.Find ("Icon").Find ("IconCD").GetComponent<Image> ();

		leftElement.transform.localScale = Vector3.one;
		rightElement.transform.localScale = Vector3.one;
		centerElement.transform.localScale = Vector3.one;
	}


	public void DisableElements() {
		Transform leftElement = transform.Find ("LeftElement");
		leftElementImg = leftElement.Find ("Icon").GetComponent<Image> ();
		leftElementImgCd = leftElement.Find ("Icon").Find ("IconCD").GetComponent<Image> ();

		Transform rightElement = transform.Find ("RightElement");
		rightElementImg = rightElement.Find ("Icon").GetComponent<Image> ();
		rightElementImgCd = leftElement.Find ("Icon").Find ("IconCD").GetComponent<Image> ();

		Transform centerElement = transform.Find ("CenterElement");
		centerElementImg = centerElement.Find ("Icon").GetComponent<Image> ();
		centerElementImgCd = leftElement.Find ("Icon").Find ("IconCD").GetComponent<Image> ();

		leftElement.transform.localScale = Vector3.zero;
		rightElement.transform.localScale = Vector3.zero;
		centerElement.transform.localScale = Vector3.zero;
	}
}
