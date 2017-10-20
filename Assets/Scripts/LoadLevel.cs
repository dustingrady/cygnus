using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour {

    bool onTop = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (onTop)
        {
            if (Input.GetKeyDown("e"))
            {
                Debug.Log("Entering level " + this.name);
                SceneManager.LoadScene(this.name);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            Debug.Log(col.gameObject.name + " is on top of " + this.name);

            onTop = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            Debug.Log("exiting");
            onTop = false;
        }
    }
}
