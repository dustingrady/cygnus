/* Purpose: Controls main camera as it follows player throughout environment
 * Status: Working
 * Possible improvements: 
 * -Wait for player to reach given point before panning
 * -Zoom in/out depending on players run speed to give larger FOV
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraScript : MonoBehaviour {

    GameObject target;
    [SerializeField]
    float xMax;
    [SerializeField]
    float yMax;
    [SerializeField]
    float xMin;
    [SerializeField]
    float yMin;
    [SerializeField]
    float smoothing = 0.5f;
    [SerializeField]

    Camera reference;

    Vector3 oldPos;
    Vector2 currentVelocity;

    float allottedMovement = 100f;
    bool testx, testy = false;

    Vector2 velocity = Vector2.zero;
    

    // Use this for initialization
    void Start () {
        target = GameObject.FindGameObjectWithTag("Player");
        reference = GameObject.FindGameObjectWithTag("Reference").GetComponent<Camera>();
        oldPos = Camera.main.WorldToScreenPoint(target.transform.position);
        currentVelocity = target.GetComponent<Rigidbody2D>().velocity;
        //Debug.Log(oldPos);
	}
	
    void Update() {
        currentVelocity = target.GetComponent<Rigidbody2D>().velocity;
        Debug.Log(Camera.main.WorldToViewportPoint(target.transform.position));
        if (currentVelocity.x == 0)
        {
            oldPos = Camera.main.WorldToScreenPoint(target.transform.position);
        }

        if ((Mathf.Abs(currentVelocity.x) >= 0 && (Mathf.Abs(currentVelocity.y) == 0)) &&
            Mathf.Abs((Camera.main.WorldToScreenPoint(target.transform.position).x - oldPos.x)) >= allottedMovement)
        {
            //testx = true;
            //testy = false;
            float smoothx = Mathf.SmoothDamp(transform.position.x, target.transform.position.x, ref velocity.x, smoothing);
            //float smoothy = Mathf.SmoothDamp(transform.position.y, target.transform.position.y, ref velocity.y, smoothing);

            //transform.position = new Vector3(Mathf.Clamp(smoothx, xMin, xMax), Mathf.Clamp(smoothy, yMin, yMax), transform.position.z);
            transform.position = new Vector3(Mathf.Clamp(smoothx, xMin, xMax), Mathf.Clamp(target.transform.position.y, yMin, yMax), transform.position.z);
        }
        else if (((Mathf.Abs(currentVelocity.x) >= 0 || (Mathf.Abs(currentVelocity.x) == 0 )) 
            && (Mathf.Abs(currentVelocity.y) > 0)))
        {
            //testy = true;
            //testx = false;
            float smoothx = Mathf.SmoothDamp(transform.position.x, target.transform.position.x, ref velocity.x, smoothing);
            float smoothy = Mathf.SmoothDamp(transform.position.y, target.transform.position.y, ref velocity.y, smoothing);

            transform.position = new Vector3(Mathf.Clamp(smoothx, xMin, xMax), Mathf.Clamp(smoothy, yMin, yMax), transform.position.z);
        }

        //Debug.Log((Camera.main.WorldToScreenPoint(target.transform.position).x - oldPos.x) + " " + currentVelocity.x);
    }

    // Late Update preferred so that we ensure target has moved before tracking
    void LateUpdate()
    {
        /*
        if (testx)
        {
            float smoothx = Mathf.SmoothDamp(transform.position.x, target.transform.position.x, ref velocity.x, smoothing);
            //float smoothy = Mathf.SmoothDamp(transform.position.y, target.transform.position.y, ref velocity.y, smoothing);

            //transform.position = new Vector3(Mathf.Clamp(smoothx, xMin, xMax), Mathf.Clamp(smoothy, yMin, yMax), transform.position.z);
            transform.position = new Vector3(Mathf.Clamp(smoothx, xMin, xMax), Mathf.Clamp(target.transform.position.y, yMin, yMax), transform.position.z);
        }
        if(testy)
        {
            float smoothx = Mathf.SmoothDamp(transform.position.x, target.transform.position.x, ref velocity.x, smoothing);
            float smoothy = Mathf.SmoothDamp(transform.position.y, target.transform.position.y, ref velocity.y, smoothing);

            transform.position = new Vector3(Mathf.Clamp(smoothx, xMin, xMax), Mathf.Clamp(smoothy, yMin, yMax), transform.position.z);
        }
        //transform.position = new Vector3(Mathf.Clamp(target.transform.position.x, xMin, xMax), Mathf.Clamp(target.transform.position.y, yMin, yMax), transform.position.z);
        /*this.transform.position = new Vector3(target.transform.position.x + xOffset,
                                              target.transform.position.y + yOffset,
                                              target.transform.position.z + zOffset);*/
        }
}
