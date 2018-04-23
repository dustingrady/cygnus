using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerEvent : MonoBehaviour {

	int hp = 3;
	public bool startSequence = false;
	public bool endSequence;
	public GameObject Timer;
	public ParticleSystem exp;
	public GameObject tp;
	bool addingTime = true;
	public GameObject trTrigger;
	TrapRoom tr;

	// Use this for initialization
	void Start () {
		if (trTrigger != null) {
			tr = trTrigger.GetComponent<TrapRoom> ();
		} else {
			tr = gameObject.GetComponent<TrapRoom> ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (hp <= 0) {
			if (startSequence && !endSequence) {
				Timer.GetComponent<Timer> ().Activate (180f);
			} else if (!startSequence && !endSequence) {
				addTime (180f);
			} else if (!startSequence && endSequence) {
				Timer.GetComponent<Timer> ().Active = false;
			}
			if (tp != null) {
				tp.SetActive (true);
			}

			if (tr != null) {
				tr.summon = true;
			}
			exp.Play ();

			Destroy (gameObject, 0.1f);
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.name != "Player" && col.gameObject.tag != "EnemyProjectile") {
			hp -= 1;
		}

	}

	void addTime(float time)
	{
		if (addingTime) {
			Timer.GetComponent<Timer> ().timeLeft += time;
		}
		addingTime = false;
	}
}
