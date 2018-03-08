using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sandstorm : MonoBehaviour {

    public ParticleSystem particleSystem;
    private Rigidbody2D playerRigidbody2D;

    // Use this for initialization
    void Start () {
        var player = GameObject.FindWithTag("Player");
        playerRigidbody2D = player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        if (particleSystem.isStopped) {
            //playerRigidbody2D.AddForce(new Vector2(-20, 0));
        }
    }
}