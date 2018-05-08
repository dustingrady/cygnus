using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowBossEntrance : MonoBehaviour {

	public GameObject area1;
	public GameObject area2;

	public GameObject bossEntrance;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (area1.GetComponent<Triggered> ().touched && area2.GetComponent<Triggered> ().touched) {
			if (!bossEntrance.activeInHierarchy) {
				bossEntrance.SetActive (true);
			}
		}
	}
}
