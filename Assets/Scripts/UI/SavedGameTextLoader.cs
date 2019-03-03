using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavedGameTextLoader : MonoBehaviour {
	public int saveSlot;

	// Use this for initialization
	void Start () {
		List<int> partQuests = new List<int> { 199, 299, 399, 499 };

		Text buttonText = GetComponent<Text> ();
		SaveFile file = Saver.LoadSaveFile (saveSlot);

		console.log (saveSlot);
		if (saveSlot == 3) {
			console.log (file.questsComplete);
			console.log (file.scene);
		}

		if (file != null) {
			var partsCollected = 0;
			foreach (int quest in file.questsComplete) {
				if (partQuests.Contains (quest)) {
					partsCollected += 1;
				}
			}

			buttonText.text = file.scene + " - Parts collected: " + partsCollected + "/4";
		} else {
			buttonText.text = "No save found!";
		}
	}
}
