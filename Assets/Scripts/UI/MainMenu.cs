using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : EventTrigger {

	public override void OnPointerClick(PointerEventData data)
	{
		switch (this.gameObject.name) {
		case "Start":
			startGame ();
			break;
		case "Load":
			load ();
			break;
		case "Options":
			options ();
			break;
		case "Exit":
			exitGame ();
			break;
		}

	}

	void startGame()
	{
		SceneManager.LoadScene ("Ship");
	}

	void load()
	{
		Debug.Log ("Loading save.");
	}

	void options()
	{
		Debug.Log ("No options here.");
	}

	void exitGame()
	{
		Debug.Log ("Exiting.");
		Application.Quit ();
	}
}
