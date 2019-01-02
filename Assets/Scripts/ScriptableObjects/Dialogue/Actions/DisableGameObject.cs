using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "DisableGameObject", menuName = "Dialogue/Actions/DisableGameObject")]
public class DisableGameObject : DialogueAction {
	public string gameObjectName;
	public GameObject effect;

	override public void Activate(GameObject npc) {
		var go = GameObject.Find (gameObjectName);

		if (go != null) {
			// Enable the particle effect for the disabled object
			Instantiate (effect, go.transform.position, Quaternion.identity);

			// disable object
			go.SetActive (false);

		}
	}
}
