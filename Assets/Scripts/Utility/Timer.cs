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

    [SerializeField]
    UnityEvent timerEnd;                    // Add listeners here for when time is up
    Text timerText;

    // Use this for initialization
    void Awake()
    {
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
                timerEnd.Invoke();
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
