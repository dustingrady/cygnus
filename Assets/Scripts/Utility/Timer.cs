using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Timer : MonoBehaviour {
    bool active = false;
    public float timeLeft = 0.0f;           // Time to end
    public float alertTime = 30.0f;         // Start flashing when time is reached
    public Color hightlight = Color.red;    // Flash color
    public Color standard = Color.green;    // Regular color

    Text timerText;

	Player player;

	void OnLevelWasLoaded(){
		player = GameObject.Find ("Player").GetComponent<Player> ();
	}

    // Use this for initialization
    void Awake()
    {
		player = GameObject.Find ("Player").GetComponent<Player> ();
        timerText = gameObject.GetComponent<Text>();
        timerText.color = standard;
        timerText.text = timeLeft.ToString("0.00");
        Active = active;
    }

    // Get/set state of timer
    public bool Active
    {
        get { return active; }
        set
        {
            timerText.color = standard;
            timerText.enabled = value;
            active = value;
        }
    }

    // Start timer with specified time
    public void Activate(float time)
    {
        timeLeft = time;
        Active = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (active)
        {
            if (timeLeft <= 0.0f)
            {
                timeLeft = 0.0f;
				player.backToCheckPoint ();
                Active = false;
            }
            else
            {
                timerText.text = timeLeft.ToString("0.00");
                timeLeft -= Time.deltaTime;

                if (timeLeft < alertTime)
                {
                    int sec = (int)timeLeft;
                    timerText.color = sec % 2 == 0 ? standard : hightlight;
                }
            }
        }
	}
}
