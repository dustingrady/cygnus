using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaftPlatform : MonoBehaviour {


	void Start() {
		GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.FreezePositionX;
	}

	void OnCollisionStay2D(Collision2D col) {
		if (col.gameObject.name == "Player") {
			Vector2 raftVel = gameObject.GetComponent<Rigidbody2D> ().velocity;
			col.gameObject.GetComponent<Rigidbody2D> ().velocity = col.gameObject.GetComponent<Rigidbody2D> ().velocity + raftVel;
			/*
			if ((col.transform.position.y - col.gameObject.GetComponent<BoxCollider2D> ().bounds.extents.y) > transform.position.y) {
				
				Vector2 plrVel = col.gameObject.GetComponent<Rigidbody2D> ().velocity;
				Vector2 raftVel = gameObject.GetComponent<Rigidbody2D> ().velocity;

				col.gameObject.GetComponent<Rigidbody2D> ().AddForce(raftVel * 4f);
				gameObject.GetComponent<Rigidbody2D> ().AddForce(plrVel * -5f);
			}
			*/
		}
	}


	public void UntieRaft() {
		GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.FreezeRotation;
	}
}
