using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenu : MonoBehaviour {

	GameObject inGameMenu;
	private bool display = false;

	void OnLevelWasLoaded(){
		if (GameObject.Find ("UI") != null) {
			inGameMenu = GameObject.Find ("UI").transform.Find ("InGameMenu").gameObject;
			hideMenu ();
		}
	}

	// Use this for initialization
	void Start () {
		if (GameObject.Find ("UI") != null) {
			inGameMenu = GameObject.Find ("UI").transform.Find ("InGameMenu").gameObject;
			hideMenu ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape)){
			if (!display) {
				showMenu ();
				Time.timeScale = 0;
				display = true;
			} else if (display) {
				hideMenu ();
				Time.timeScale = 1;
				display = false;
			}
		}
	}

	void showMenu()
	{
		inGameMenu.transform.localScale = new Vector3(1,1,1);
	}

	void hideMenu()
	{
		inGameMenu.transform.localScale = new Vector3 (0, 0, 0);
	}

	public void Resume()
	{
		hideMenu ();
		Time.timeScale = 1;
		display = false;
	}

	public void Save()
	{
		Debug.Log ("Saving");
        Resume();
        SaveMan.Save();
	}

	public void Load(){
		Debug.Log ("Load");
        Resume();
        SaveMan.Load();
	}

	public void Quit()
	{
		Debug.Log ("Quitting");
	}
}
