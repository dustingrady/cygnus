using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sandstorm : MonoBehaviour {

    public ParticleSystem Storm;
    private AreaEffector2D sandForce;
    float timer = 3;
    

    // Use this for initialization
    void Start () {
        var force = GameObject.FindWithTag("Sandstorm");
        sandForce = force.GetComponent<AreaEffector2D>();
        sandForce.forceMagnitude = 0;
        Debug.Log(" Its not hitting this ");
        timer = Random.Range(20.0f, 40.0f);
        Debug.Log(" It is hitting this " + " " + timer);
    }

    // Update is called once per frame
    void Update() {
        if (!Storm.isEmitting) {
            
            sandForce.forceMagnitude = 0;
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                Storm.Play();
                timer = Random.Range(20.0f,40.0f);
            }
        }

        if (Storm.isEmitting)
        {
            if (sandForce.forceMagnitude > -50)
            {
                sandForce.forceMagnitude -= Time.deltaTime * 7;
            }
            else if (sandForce.forceMagnitude <= -50)
            {
                sandForce.forceMagnitude = -50;
            }
        }
    }
}