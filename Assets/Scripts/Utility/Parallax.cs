using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour {

    public List<Transform> background;
    public List<float> scale;
    public float smoothing = 1f;

    private Vector3 prevCameraPos;

    void Awake()
    {

        GameObject bg = GameObject.Find("Expanded");
        foreach (Transform child in bg.transform)
            {
                background.Add(child);
            }

    }

	// Use this for initialization
	void Start () {
        prevCameraPos = this.transform.position;

        foreach (Transform bg in background)
            scale.Add(bg.position.z * -1);
	}
	
	// Update is called once per frame
	void Update () {
		for(int i = 0; i < background.Count; i++)
        {
            float parallax = (prevCameraPos.x - this.transform.position.x) * scale[i];

            float newBackgroundPosX = background[i].position.x + parallax;

            Vector3 newBackgroundPos = new Vector3(newBackgroundPosX, background[i].position.y, background[i].position.z);

            background[i].position = Vector3.Lerp(background[i].position, newBackgroundPos, smoothing * Time.deltaTime);
        }

        prevCameraPos = this.transform.position;
	}
}
