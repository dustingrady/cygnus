using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCamScript : MonoBehaviour {

    public Transform target;

    private Vector3 tempPos;

    [SerializeField]
    float xMax;
    [SerializeField]
    float yMax;
    [SerializeField]
    float xMin;
    [SerializeField]
    float yMin;

    [SerializeField]
    float camSpeed = .5f;
    [SerializeField]
    float x;
    [SerializeField]
    float y;

    [SerializeField]
    float xThreshhold = 3f;
    [SerializeField]
    float yThreshold = 8f;

    Vector2 velocity = Vector2.zero;

	// Use this for initialization
	void Start () {
        tempPos.z = this.transform.position.z;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        moveCam();
    }

    void moveCam()
    {

        if (target.transform.position.x > this.transform.position.x)
        {
            x = target.transform.position.x - this.transform.position.x;
        }
        else
        {
            x = this.transform.position.x - target.transform.position.x;
        }

        if (target.transform.position.y > this.transform.position.y)
        {
            y = target.transform.position.y - this.transform.position.y;
        }
        else
        {
            y = this.transform.position.y - target.transform.position.y;
        }

        if (x >= xThreshhold || y >= yThreshold)
        {
            tempPos = new Vector3(Mathf.Clamp(target.transform.position.x, xMin, xMax), Mathf.Clamp(target.transform.position.y, yMin, yMax), tempPos.z);

            if (Camera.main.WorldToViewportPoint(target.position).y < 0.25)
            {
                
                Vector3 tempYPos = new Vector3(target.transform.position.x, target.position.y, tempPos.z);
                Vector3 smooth = new Vector3(Mathf.SmoothDamp(transform.position.x, tempYPos.x, ref velocity.x, 1f),
                                             Mathf.SmoothDamp(transform.position.y, tempYPos.y, ref velocity.y, 1f),
                                             tempYPos.z);
                transform.position = new Vector3(Mathf.Clamp(smooth.x, xMin, xMax), Mathf.Clamp(smooth.y, yMin, yMax), tempYPos.z);

                //Vector3 tempYPos = new Vector3(Mathf.Clamp(target.transform.position.x, xMin, xMax), Mathf.Clamp(target.position.y, yMin, yMax), tempPos.z);
                //this.transform.position = Vector3.MoveTowards(this.transform.position, tempYPos, Mathf.Lerp(camSpeed, camSpeed*(-target.GetComponent<Rigidbody2D>().velocity.y), 1f) * Time.deltaTime);
            }
            else
                this.transform.position = Vector3.MoveTowards(this.transform.position, tempPos, camSpeed * Time.deltaTime);
            //this.transform.position = Vector3.Lerp(this.transform.position, tempPos, camSpeed * Time.deltaTime);
        }

    }

}
