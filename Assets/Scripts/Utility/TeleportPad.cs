using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPad : MonoBehaviour {

	public GameObject destinationPad;
	float teleportTimer = 0;
	GameObject player;

	public Texture2D fadeOutTexture;
	float fadeSpeed = 0.5f; //low number = slow fade

	int drawDepth = -10;

	//if alpha = 0 and fadedir = 1 then it'll fade in else it'll fade out 
	float alpha = 0f;
	int fadeDir = 1;

	bool fading = false;

	const float tpCharge = 3.0f;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
		if (teleportTimer >= tpCharge) {
			fading = true;
			StartCoroutine (teleport (player));
		}
	}

	void OnTriggerStay2D(Collider2D col)
	{
		if (col.tag == "Player") {
			if (destinationPad != null) {
				teleportTimer += Time.deltaTime;
				player = col.gameObject;
			}
		}
	}

	void OnTriggerExit2D(Collider2D col)
	{
		if (col.tag == "Player") {
			teleportTimer = 0;
		}
	}

	void OnGUI()
	{
		if (fading) {
			alpha += fadeDir * fadeSpeed * Time.deltaTime;
			alpha = Mathf.Clamp01 (alpha);

			if (alpha == 1) {
				StartCoroutine (hold ());
			}

			GUI.color = new Color (GUI.color.r, GUI.color.g, GUI.color.b, alpha);
			GUI.depth = drawDepth;
			GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), fadeOutTexture);

			StartCoroutine (fadingTimer ());
		}
	}

	IEnumerator teleport(GameObject g)
	{
		yield return new WaitForSeconds (1);
		g.transform.position = destinationPad.transform.position;
	}

	IEnumerator fadingTimer()
	{
		yield return new WaitForSeconds (tpCharge + 1 / 2);
		fadeDir = 1;
		fading = false;
		alpha = 0f;
	}

	IEnumerator hold()
	{
		yield return new WaitForSeconds (0.7f);
		fadeDir = -1;
	}
}
