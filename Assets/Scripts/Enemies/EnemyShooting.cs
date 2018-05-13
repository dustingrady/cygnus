using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour {
	[SerializeField]
	private GameObject bulletPrefab;
	private Transform playerTransform;

	private int count = 0;
	[SerializeField]
	public int cooldown = 150; // Fire rate

	public enum shootPattern
	{
		spread,
		single,
		homing,
		omnidirectional
	}

	public shootPattern sp;

	void Awake(){
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
	}

	public void shoot_At_Player(){
		if(count >= cooldown){
			if (sp == shootPattern.single) {
				GameObject bullet = (GameObject)Instantiate (bulletPrefab, transform.position, transform.rotation);
				bullet.GetComponent<BulletBehaviour> ().setBullet ("single");
			}
			if (sp == shootPattern.homing) {
				GameObject bullet = (GameObject)Instantiate (bulletPrefab, transform.position, transform.rotation);
				bullet.GetComponent<BulletBehaviour> ().setBullet ("homing");
			}
			if (sp == shootPattern.spread) {
				for (int i = 0; i < 3; i++) {
					GameObject bullet = (GameObject)Instantiate (bulletPrefab, transform.position, transform.rotation);
					bullet.GetComponent<BulletBehaviour> ().setBullet ("spread");
					bullet.transform.LookAt (new Vector3(playerTransform.position.x + i*0.7f, playerTransform.position.y + i*0.7f, 0));
				}
			}
			if (sp == shootPattern.omnidirectional) {
				for (float i = -2; i <= 2; i += 1f) {
					for (float j = -2; j <= 2; j += 0.5f) {
						if (j != 0) {
							GameObject bullet = (GameObject)Instantiate (bulletPrefab, transform.position, transform.rotation);
							bullet.GetComponent<BulletBehaviour> ().setBullet ("omnidirectional");
							bullet.transform.LookAt (new Vector3 (i, j, 0));
							bullet.GetComponent<BulletBehaviour> ().omDir = new Vector3 (i, j, 0);
						}
					}
				}
			}
			count = 0;
		}
		count++;
	}
}
