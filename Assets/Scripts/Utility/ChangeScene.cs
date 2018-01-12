using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

	[SerializeField]
	string sceneName;

	void OnTriggerEnter2D(Collider2D col) {
		if (col.transform.CompareTag("Player")) {
			GameManager.instance.previousLocation = SceneManager.GetActiveScene ().name;
			SceneManager.LoadScene (sceneName);	
		}
	}
}
