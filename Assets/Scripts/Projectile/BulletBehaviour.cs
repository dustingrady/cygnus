/*Function: Different types of bullet behaviours (straight shot, homing, spin shot, etc) 
* Status: In progress/ Untested
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR 
using UnityEditor; 
#endif

public class BulletBehaviour : MonoBehaviour {
	private GameObject player;
	private int bulletSpeed = 10;
	private Vector3 playerPos;
	private Transform playerTransform;

	public string typeOfBullet;
	public Vector3 omDir;


	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player");
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		playerPos = new Vector3 (player.transform.position.x, player.transform.position.y, player.transform.position.z);
		if(typeOfBullet == "single" || typeOfBullet == "homing")
			transform.LookAt (playerTransform.position); //Face the player
		//if(typeOfBullet == "spread")
			//transform.LookAt (new Vector3(playerTransform.position.x + Random.Range(-0.7f, 0.7f), playerTransform.position.y + Random.Range(-0.5f, 0.5f), 0));

		transform.Rotate (new Vector3(0,-90,0),Space.Self); //Correct original rotation
	}

	// Update is called once per frame
	void Update () {
		if (typeOfBullet == "homing") {
			Vector3 dir = new Vector3 (playerTransform.position.x - transform.position.x, playerTransform.position.y - transform.position.y, 0);
			transform.position += (bulletSpeed / 2) * dir.normalized * Time.deltaTime;
			Destroy (this.gameObject, 10f);
		} else if (typeOfBullet == "omnidirectional") {
			transform.position += omDir.normalized * bulletSpeed * Time.deltaTime;
			Destroy (this.gameObject, 2f);
		} else {
			transform.position += transform.right * bulletSpeed * Time.deltaTime;
			Destroy(this.gameObject, 5f);
		}
	}

	void OnTriggerEnter2D(Collider2D col){
			Destroy (this.gameObject);
	}

	public void setBullet(string s)
	{
		typeOfBullet = s;
	}
}
