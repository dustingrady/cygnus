using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lava : Element {
	public GameObject lavaJet;
	[SerializeField]
	private float lavaJetCooldown = 0.5f;
	[SerializeField]
	private float jetStrength = 400f;
	[SerializeField]
	private float variability = 0.25f; // Increases the potential offset for the direction
	private float timeSinceFire;
	public Image icon;

	public override void UseElement(Vector3 pos, Vector2 dir){
		if (timeSinceFire > lavaJetCooldown) {
			// Generate some small random floats to offset the projectile
			Vector2 dirOffset = new Vector2(Random.Range(-variability, variability), Random.Range(-variability, variability));
			dir = dir.normalized + dirOffset;

			Vector3 handPos = new Vector3 (dir.normalized.x, dir.normalized.y, 0) * 0.8f;
			GameObject fb = Instantiate (lavaJet, pos + handPos, Quaternion.identity);
			fb.GetComponent<LavaStream> ().Initialize (dir, jetStrength);
			timeSinceFire = 0;
		}
	}

	void Start(){
		GameObject ui = GameObject.Find ("UI");
		Transform centerElement = ui.transform.Find ("CenterElement");
		icon = centerElement.Find ("Icon").transform.Find("IconCD").GetComponent<Image> ();
		if (icon.fillAmount < 1f) {
			icon.fillAmount = 1f;
		}
		Debug.Log (icon.fillAmount);
	}

	void Update() {
		timeSinceFire += Time.deltaTime;
	}
}