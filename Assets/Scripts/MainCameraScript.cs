/* Purpose: Controls main camera as it follows player throughout environment
 * Status: Working
 * Possible improvements: 
 * -Wait for player to reach given point before panning
 * -Zoom in/out depending on players run speed to give larger FOV
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraScript : MonoBehaviour {

    public GameObject target;
    public float xOffset = 0;
    public float yOffset = 0;
    public float zOffset = 0;

    // Use this for initialization
    void Start () {
		
	}
	
    void Update() {

    }

    // Late Update preferred so that we ensure target has moved before tracking
    void LateUpdate()
    {
        this.transform.position = new Vector3(target.transform.position.x + xOffset,
                                              target.transform.position.y + yOffset,
                                              target.transform.position.z + zOffset);
    }
}
