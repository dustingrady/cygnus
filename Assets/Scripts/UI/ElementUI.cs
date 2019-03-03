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

	Sprite prevLeftE;
	Sprite prevRightE;
	Sprite prevCenterE;
	public bool lhandCD = false;
	public bool rhandCD = false;

	void Start () {
		if (GameObject.FindGameObjectWithTag ("Player") != null) {
			plr = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();
		}

		if (GameManager.instance.hasGloves == false) {
			DisableElements ();
		}

		Absorber.OnAbsorb += UpdateElements;
	}

	void OnDestroy() {
		Absorber.OnAbsorb -= UpdateElements;
	}

	void Update()	{
		//Stop the CD fill when swapped to different element
		if (prevLeftE != null) {
			if (prevLeftE != leftElementImg.sprite) {
				prevLeftE = leftElementImg.sprite;
				lhandCD = true;
				leftElementImgCd.fillAmount = 1;
			}
		}

		if (prevRightE != null) {
			if (prevRightE != rightElementImg.sprite) {
				prevRightE = rightElementImg.sprite;
				rhandCD = true;
				rightElementImgCd.fillAmount = 1;
			}
		}
	}

	public void UpdateElements() {
		if (GameObject.FindGameObjectWithTag ("Player") != null) {
			plr = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();
		}

		if (plr.leftElement != null) {
			prevLeftE = leftElementImgCd.sprite;

			if (leftElementImg.enabled == false) {
				Debug.Log ("component is disabled");
			}

			leftElementImg.sprite = plr.leftElement.sprite;
			leftElementImgCd.sprite =  plr.leftElement.sprite;

			leftElementImg.enabled = true;
			leftElementImgCd.enabled = true;

			leftElementImgCd.type = Image.Type.Filled;
		}

		if (plr.rightElement != null) {
			prevRightE = rightElementImgCd.sprite;

			rightElementImg.sprite = plr.rightElement.sprite;
			rightElementImgCd.sprite = plr.rightElement.sprite;

			rightElementImg.enabled = true;
			rightElementImgCd.enabled = true;

			rightElementImgCd.type = Image.Type.Filled;
		}

		if (plr.centerElement != null) {
			prevCenterE = centerElementImgCd.sprite;

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
		Transform rightElement = transform.Find ("RightElement");
		Transform centerElement = transform.Find ("CenterElement");

		if (leftElement != null) {
			leftElement.transform.localScale = Vector3.one;
		}

		if (rightElement != null) {
			rightElement.transform.localScale = Vector3.one;
		}

		if (centerElement != null) {
			centerElement.transform.localScale = Vector3.one;
		}

		UpdateElements ();
	}


	public void DisableElements() {
		Transform leftElement = transform.Find ("LeftElement");
		Transform rightElement = transform.Find ("RightElement");
		Transform centerElement = transform.Find ("CenterElement");

		if (leftElement != null) {
			leftElement.transform.localScale = Vector3.zero;
		}

		if (rightElement != null) {
			rightElement.transform.localScale = Vector3.zero;
		}

		if (centerElement != null) {
			centerElement.transform.localScale = Vector3.zero;
		}

		UpdateElements ();
	}
}
