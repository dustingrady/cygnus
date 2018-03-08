using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaftTrigger : MonoBehaviour {

	public GameObject raft;

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.CompareTag("FireElement")) {
			raft.GetComponent<RaftPlatform> ().UntieRaft ();
		}
	}
}
