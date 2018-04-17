using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour {
	[SerializeField]
	private GameObject bulletPrefab;
	private Transform enemyTransform;
	private Transform playerTransform;

	private int count = 0;
	public int cooldown;

	public enum shootPattern
	{
		spread,
		single,
		homing,
		omnidirectional
	}

	public shootPattern sp;

	void Awake(){
		enemyTransform = this.transform; //Reference to current enemy
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		Debug.Log (cooldown);
	}

	public void shoot_At_Player(){
		if(count >= cooldown){
			if (sp == shootPattern.single) {
				GameObject bullet = (GameObject)Instantiate (bulletPrefab, enemyTransform.position, enemyTransform.rotation);
				bullet.GetComponent<BulletBehaviour> ().setBullet ("single");
			}
			if (sp == shootPattern.homing) {
				GameObject bullet = (GameObject)Instantiate (bulletPrefab, enemyTransform.position, enemyTransform.rotation);
				bullet.GetComponent<BulletBehaviour> ().setBullet ("homing");
			}
			if (sp == shootPattern.spread) {
				for (int i = -1; i <= 1; i++) {
					GameObject bullet = (GameObject)Instantiate (bulletPrefab, enemyTransform.position, enemyTransform.rotation);
					bullet.GetComponent<BulletBehaviour> ().setBullet ("spread");
					bullet.transform.LookAt (new Vector3(playerTransform.position.x + i*0.7f, playerTransform.position.y + i*0.7f, 0));
				}
			}
			if (sp == shootPattern.omnidirectional) {
				for (float i = -1; i <= 1; i += 1f) {
					for (float j = -1; j <= 1; j += 0.5f) {
						if (j != 0 || i !=0) {
							GameObject bullet = (GameObject)Instantiate (bulletPrefab, enemyTransform.position, enemyTransform.rotation);
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
