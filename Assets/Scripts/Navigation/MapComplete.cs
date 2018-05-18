using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapComplete : MonoBehaviour {

	public GameObject ruinsCheck;
	public GameObject forestCheck;
	public GameObject acidCheck;
	public GameObject desertCheck;
	public GameObject phucCheck;

	// Use this for initialization
	void Start () {
		GameManager gm = GameManager.instance;
		GameObject barrier = GameObject.Find ("Barrier");

		// Check to see if fire is out, preventing other level access
		if (gm.CheckQuestComplete (0)) {
			ruinsCheck.SetActive (true);	
		}

		// Check level completes
		if (gm.CheckQuestComplete (0)) {
			ruinsCheck.SetActive (true);	
		}
		if (gm.CheckQuestComplete (199)) {
			forestCheck.SetActive (true);	
		}
		if (gm.CheckQuestComplete (299)) {
			acidCheck.SetActive (true);	
		}
		if (gm.CheckQuestComplete (399)) {
			desertCheck.SetActive (true);	
		}
		if (gm.CheckQuestComplete (499)) {
			phucCheck.SetActive (true);	
		}

		if (gm.CheckQuestComplete (1)) {
			barrier.SetActive (false);
		}
	}
}
