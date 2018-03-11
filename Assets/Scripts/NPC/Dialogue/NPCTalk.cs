using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class NPCTalk : MonoBehaviour
{
    public GameObject uiRoot;
    public TalkTree tree;
    public Sprite portrait;
    public GameObject windowPrefeb;
    bool inDialogue = false;

    public void EndDialogue()
    {
        inDialogue = false;

		// Re-enable shooting now that dialog ended
		EnableShooting();
    }

    public void StartDialogue()
    {
        if (!inDialogue)
        {
			// Disable shooting component from player
			DisableShooting();

            inDialogue = true;
			GameObject win = Instantiate(windowPrefeb, uiRoot.transform);
			win.transform.SetAsFirstSibling ();

			// Position the window closer to the top of the screen
			win.transform.position = win.transform.position + new Vector3(0, Camera.main.pixelHeight/4f, 0);

            NPCDialogue d = win.GetComponent<NPCDialogue>();

            tree.Reset();
            d.signal = EndDialogue;
            d.dialogue = tree;
            d.potrait.sprite = portrait;
			d.npc = this.gameObject;

            d.UpdateDialogue();
        }
    }

	private void DisableShooting() {
		GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerShooting> ().enabled = false;
	}

	private void EnableShooting() {
		GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerShooting> ().enabled = true;
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.CompareTag("Player")) {
			StartDialogue ();
		}
	}
}
