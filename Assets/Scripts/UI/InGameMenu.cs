using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour {

	GameObject inGameMenu;
    public Dropdown drop;
	private bool display = false;
    private int slot = 1;

	void OnLevelWasLoaded(){
		if (GameObject.Find ("UI") != null) {
			inGameMenu = GameObject.Find ("UI").transform.Find ("InGameMenu").gameObject;
			drop = GameObject.Find ("File").GetComponent<Dropdown> ();
			hideMenu ();
		}
	}

	// Use this for initialization
	void Start () {
		if (GameObject.Find ("UI") != null) {
			inGameMenu = GameObject.Find ("UI").transform.Find ("InGameMenu").gameObject;
			drop = GameObject.Find ("File").GetComponent<Dropdown> ();
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

    public void ChangeSlot() {
        slot = drop.value + 1;
        Debug.Log("New Slot: " + slot);
    }

	void showMenu()
	{
		inGameMenu.transform.localScale = new Vector3(1,1,1);
	}

	void hideMenu()
	{
		inGameMenu.transform.localScale = new Vector3 (0, 0, 0);
	}

    public void NewGame() {
        SceneManager.LoadScene("Ship");
    }

	public void Resume()
	{
		hideMenu ();
		Time.timeScale = 1;
		display = false;
	}

	public void Save()
	{
		Saver.SaveGame (drop.value + 1);
        Resume();
	}

	public void Load(){
		Debug.Log ("Load");
		Saver.LoadGame (drop.value + 1);
        Resume();
	}

	public void VolumeSlider(float val) {
		GameManager.instance.backgroundMusicVolume = val;

	}

	public void Quit()
	{
		Application.Quit ();
	}
}
