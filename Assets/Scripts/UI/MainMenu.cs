using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public GameObject saveMenu;
	private bool fadingOut;
	private Image fader;
	public AudioSource audio;

	public void Start() {
		Cursor.visible = true;
		fader = GameObject.Find ("Fader").GetComponent<Image>();
	}

	void Update() {
		if (fadingOut) {
			Debug.Log (fader.color.a);
			fader.color = new Color(fader.color.r, fader.color.g, fader.color.b, fader.color.a + Time.deltaTime);
		}
	}


	public void NewGame() {
		StartCoroutine (NewGameRoutine ());
	}

	public void OpenSaveMenu() {
		saveMenu.GetComponent<Animator> ().enabled = true;
	}

	public void SaveOne() {
		Debug.Log ("boo");
		Load (1);
	}

	public void SaveTwo() {
		Load (2);
	}

	public void SaveThree() {
		Load (3);
	}

	public void Load(int slot){
		StartCoroutine (LoadGameRoutine (slot));
	}

	public void Quit()
	{
		Application.Quit ();
	}
		

	IEnumerator NewGameRoutine() {
		audio.Stop ();
		fadingOut = true;
		yield return new WaitForSeconds (1f);
		SceneManager.LoadScene("Ship");
	}

	IEnumerator LoadGameRoutine(int slot) {
		audio.Stop ();
		fadingOut = true;
		yield return new WaitForSeconds (1f);
		try {
			Debug.Log ("Load game: " + slot);
			SaveMan.Load((SaveSlot)slot);
		} catch {
			GameObject.Find ("UnableToLoad").transform.localScale = new Vector3 (1.5f, 1.5f, 1f);
			fader.color = new Color(fader.color.r, fader.color.g, fader.color.b, 0f);
			fadingOut = false;
		}
	}
}
