using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {
     private void OnTriggerEnter2D(Collider2D collision)
    {
        SendMessage("StartDialogue");
    }

	/*
    public void TurnRed()
    {
        GameObject expld = Instantiate(sfx, transform);
        expld.transform.parent = null;
        Destroy(gameObject);
    }
    */
}
