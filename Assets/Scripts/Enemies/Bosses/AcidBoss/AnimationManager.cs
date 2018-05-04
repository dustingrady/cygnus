using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour {
	[SerializeField]
	private Animator snakeAnim;
	private GameObject boss;

	// Use this for initialization
	void Start () {
		snakeAnim = GetComponent<Animator> ();
		boss = GameObject.Find ("AcidBoss");
	}
	
	// Update is called once per frame
	void Update () {
		bool isShooting = boss.GetComponent<AcidBoss> ().isShooting;

		if (isShooting) {
			snakeAnim.SetBool ("Attack Transition", true);
		} else {
			snakeAnim.SetBool ("Attack Transition", false);
		}
	}
}
