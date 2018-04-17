using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area1Boss : MonoBehaviour {
	[SerializeField]
	private GameObject baseAttackPrefab;
	[SerializeField]
	private GameObject specialAttackPrefab;
	[SerializeField]
	private GameObject nade;

	private Transform enemyTransform;
	private int baseAttackCount = 0;
	private int specialAttackCount = 0;
	private int cooldown = 50;
	private int moveCount = 0; //What move are we on again?
	public GameObject player;
	private float maxHealth;

	enum attackState{
		healthy,
		halfhealth,
		neardeath
	}

	attackState atkState;
	void Awake(){
		enemyTransform = this.transform; //Reference to current enemy
		maxHealth = gameObject.GetComponent<BossEnemy> ().getHealth ();
	}

	void Update () {
		Debug.Log (maxHealth + " " + gameObject.GetComponent<BossEnemy> ().getHealth ());
		if ((gameObject.GetComponent<BossEnemy> ().getHealth () / maxHealth) > 0.7f) {
			atkState = attackState.healthy;
		}
		if ((gameObject.GetComponent<BossEnemy> ().getHealth () / maxHealth) <= 0.7f && (gameObject.GetComponent<BossEnemy> ().getHealth () / maxHealth) > 0.3f) {
			atkState = attackState.halfhealth;
		}
		if ((gameObject.GetComponent<BossEnemy> ().getHealth () / maxHealth) <= 0.3f) {
			atkState = attackState.neardeath;
		}
	}

	/*Determine which attack to use*/
	public void Determine_Attack(){
		moveCount++;
		if (moveCount % 5 == 0) {
			Special_Attack ();
		} 
		Base_Attack ();
	}

	/*Base attack (in progress)*/
	private void Base_Attack(){
		if(baseAttackCount >= cooldown){
			if (atkState == attackState.healthy) {
				cooldown = 75;
				GameObject bullet = (GameObject)Instantiate (baseAttackPrefab, enemyTransform.position, enemyTransform.rotation);
				bullet.GetComponent<BulletBehaviour> ().setBullet ("single");
				Destroy (bullet, 10.0f);
				baseAttackCount = 0;
			}
			if (atkState == attackState.halfhealth) {
				cooldown = 75;
				GameObject grenade = Instantiate(nade, enemyTransform.position, enemyTransform.rotation) as GameObject;
				Vector2 dir = (player.transform.position - transform.position);
				dir = new Vector2 (dir.x, dir.y + 2).normalized;
				grenade.GetComponent<Rigidbody2D> ().AddForce (dir * 10f);
				baseAttackCount = 0;
			}
			if (atkState == attackState.neardeath) {
				cooldown = 100;
				for (float i = -2; i <= 2; i += 1f) {
					for (float j = -2; j <= 2; j += 0.5f) {
						if (j != 0 || i !=0) {
							GameObject bullet = (GameObject)Instantiate (baseAttackPrefab, enemyTransform.position, enemyTransform.rotation);
							bullet.GetComponent<BulletBehaviour> ().setBullet ("omnidirectional");
							bullet.transform.LookAt (new Vector3 (i, j, 0));
							bullet.GetComponent<BulletBehaviour> ().omDir = new Vector3 (i, j, 0);
						}
					}
				}
				baseAttackCount = 0;
			}
		}
		baseAttackCount++;
	}

	/*Special attack (in progress)*/
	private void Special_Attack(){
		if(specialAttackCount >= (cooldown + 150)){
			GameObject bullet = (GameObject)Instantiate (specialAttackPrefab, enemyTransform.position, enemyTransform.rotation);
			Destroy (bullet, 10.0f);
			specialAttackCount = 0;
		}
		specialAttackCount++;
	}
}
