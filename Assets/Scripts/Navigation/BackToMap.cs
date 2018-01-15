using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMap : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player") {
			GameManager.instance.previousLocation = SceneManager.GetActiveScene ().name;
			SceneManager.LoadScene ("Map");
		}
	}
}
