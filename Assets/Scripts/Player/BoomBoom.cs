using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomBoom : MonoBehaviour {
    public GameObject boom;
    public Timer timer;

	// Use this for initialization
	void Start () {
        timer.Activate(8.0f);
	}

    public void OnTimerEnd() {
        for(int i = 0; i < 10; i++)
            Instantiate(boom, transform.position, Quaternion.identity).GetComponent<ParticleSystem>().Play();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
