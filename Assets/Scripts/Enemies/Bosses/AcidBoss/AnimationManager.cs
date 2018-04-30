using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour {
	[SerializeField]
	private Animator snakeAnim;

	AcidBoss boss;

	// Use this for initialization
	void Start () {
		snakeAnim = GetComponent<Animator> ();
		boss = GameObject.Find ("AcidBoss").GetComponent<AcidBoss> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (boss.isShooting) {
			snakeAnim.SetBool ("Attack Transition", true);
		} else {
			snakeAnim.SetBool ("Attack Transition", false);
		}

		if (boss.isTakingDamage) {
			snakeAnim.SetBool ("Damage Transition", true);
		} else {
			snakeAnim.SetBool ("Damage Transition", false);
		}
	}
}
