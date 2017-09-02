using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCamScript : MonoBehaviour
{
    GameObject target;
    [SerializeField]
    float xMax;
    [SerializeField]
    float yMax;
    [SerializeField]
    float xMin;
    [SerializeField]
    float yMin;


    float leftCameraBound = 0.47f;
    float rightCameraBound = 0.53f;

    float leftbound, rightbound;

    float smoothing = 0.5f;
    Vector2 velocity = Vector2.zero;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        rightbound =Camera.main.ViewportToWorldPoint(new Vector3(Mathf.Abs(0.5f - rightCameraBound), 0, 0)).x;
        Debug.Log(rightbound);
    }

    void Update()
    {
        //Debug.Log(Camera.main.WorldToViewportPoint(target.transform.position).x + " "+ leftCameraBound);
        if(Camera.main.WorldToViewportPoint(target.transform.position).x <= (leftCameraBound - 0.01f))
        {
            float camPos = target.transform.position.x + Camera.main.ViewportToWorldPoint(new Vector3(Mathf.Abs(Camera.main.WorldToViewportPoint(target.transform.position).x - rightCameraBound), 0, 0)).x;
            float smoothx = Mathf.SmoothDamp(transform.position.x, target.transform.position.x, ref velocity.x, smoothing);
            transform.position = new Vector3(Mathf.Clamp(smoothx, xMin, xMax), Mathf.Clamp(target.transform.position.y, yMin, yMax), transform.position.z);
        }

        else if (Camera.main.WorldToViewportPoint(target.transform.position).x >= (rightCameraBound + 0.01f))
        {
            float camPos = target.transform.position.x - Camera.main.ViewportToWorldPoint(new Vector3(Mathf.Abs(Camera.main.WorldToViewportPoint(target.transform.position).x - leftCameraBound), 0, 0)).x;
            float smoothx = Mathf.SmoothDamp(transform.position.x, target.transform.position.x, ref velocity.x, smoothing);
            transform.position = new Vector3(Mathf.Clamp(smoothx, xMin, xMax), Mathf.Clamp(target.transform.position.y, yMin, yMax), transform.position.z);
        }
    }

}
