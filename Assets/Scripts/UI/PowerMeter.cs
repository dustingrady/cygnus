using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerMeter : MonoBehaviour {

	private Player plr;
	private Image bar;

	// Use this for initialization
	void Start () {
		plr = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();
		bar = GetComponent<Image> ();
		Hide ();
	}

	void Update () {
		transform.position = plr.transform.position;
	}

	public void SetBarValue(float val) {
		bar.fillAmount = val;
	}

	public void SetBarColor(Color clr) {
		bar.color = clr;
	}

	public void Show() {
		transform.localScale = Vector3.one;
	}

	public void Hide() {
		transform.localScale = Vector3.zero;
	}

}
