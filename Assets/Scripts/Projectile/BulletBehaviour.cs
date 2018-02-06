/*Function: Different types of bullet behaviours (straight shot, homing, spin shot, etc) 
* Status: In progress/ Untested
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour {
	private GameObject player;
	private int bulletSpeed = 10;
	private Vector3 playerPos;
	private Transform playerTransform;


	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player");
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		playerPos = new Vector3 (player.transform.position.x, player.transform.position.y, player.transform.position.z);
		transform.LookAt (playerTransform.position); //Face the player
		transform.Rotate (new Vector3(0,-90,0),Space.Self); //Correct original rotation
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += transform.right * bulletSpeed * Time.deltaTime;
	}

	void OnTriggerEnter2D(Collider2D col){
			Destroy (this.gameObject);
	}
}
