using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditScroll : MonoBehaviour {

	public bool scrolling;
	public GameObject cam;
	public GameObject MenuUI;
	public float endPosX = 101;
	public float speed = 2;

	void Start () {
		cam = Camera.main.gameObject;
	}
	
	void Update () {
		if (scrolling) {
			cam.transform.Translate (new Vector3 (Time.deltaTime * speed, 0, 0));

			if (cam.transform.position.x >= endPosX) {
				scrolling = false;
				StartCoroutine (ReloadScene ());
			}
		}
	}

	public void RollCredits() {
		MenuUI.SetActive (false);
		scrolling = true;
	}

	IEnumerator ReloadScene() {
		yield return new WaitForSeconds (5f);
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

}
