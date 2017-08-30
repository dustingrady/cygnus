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

    [SerializeField]
    GameObject target;
    [SerializeField]
    float xOffset = 0;
    [SerializeField]
    float yOffset = 0;
    [SerializeField]
    float zOffset = 0;
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

    Vector2 velocity = Vector2.zero;
    

    // Use this for initialization
    void Start () {
        target = GameObject.FindGameObjectWithTag("Player");
	}
	
    void Update() {

    }

    // Late Update preferred so that we ensure target has moved before tracking
    void LateUpdate()
    {
        float smoothx = Mathf.SmoothDamp(transform.position.x, target.transform.position.x, ref velocity.x, smoothing);
        float smoothy = Mathf.SmoothDamp(transform.position.y, target.transform.position.y, ref velocity.y, smoothing);

        transform.position = new Vector3(Mathf.Clamp(smoothx, xMin, xMax), Mathf.Clamp(smoothy, yMin, yMax), transform.position.z);
        //transform.position = new Vector3(Mathf.Clamp(target.transform.position.x, xMin, xMax), Mathf.Clamp(target.transform.position.y, yMin, yMax), transform.position.z);
        /*this.transform.position = new Vector3(target.transform.position.x + xOffset,
                                              target.transform.position.y + yOffset,
                                              target.transform.position.z + zOffset);*/
    }
}
