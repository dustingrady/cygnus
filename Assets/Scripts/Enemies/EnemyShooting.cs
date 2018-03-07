using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour {
	[SerializeField]
	private GameObject bulletPrefab;
	private Transform enemyTransform;

	private int count = 0;
	private int cooldown = 50;

	void Awake(){
		enemyTransform = this.transform; //Reference to current enemy
	}

	public void shoot_At_Player(){
		if(count >= cooldown){
			GameObject bullet = (GameObject)Instantiate (bulletPrefab, enemyTransform.position, enemyTransform.rotation);
			count = 0;
		}
		count++;
	}
}
