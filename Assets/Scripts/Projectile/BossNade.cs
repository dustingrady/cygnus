using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossNade : MonoBehaviour {
	GameObject gnade;
	bool exploding = false;

	void Start(){
		gnade = Resources.Load ("Prefabs/Particles/Acid Grenade") as GameObject;
	}

	void Update(){
	}

	void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.tag == "Player" || col.gameObject.tag == "Shield") {
			explodeSequence ();
		}
		else{
			StartCoroutine (delay ());
		}
	}

	IEnumerator delay(){
		yield return new WaitForSeconds (0.2f);
		explodeSequence ();
	}

	void explodeSequence(){
		if (!exploding) {
			gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
			Instantiate (gnade, gameObject.transform.position, gameObject.transform.rotation);
			exploding = true;
		}
		Destroy (gameObject);
	}
}
