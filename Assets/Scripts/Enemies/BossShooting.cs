/*Function: Perform's various boss type attacks
* Status: In progress/ Testing
* Bugs: Special attack spawns a regular shot too (what bug?)
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShooting : MonoBehaviour {
	[SerializeField]
	private GameObject baseAttackPrefab;
	[SerializeField]
	private GameObject specialAttackPrefab;

	private Transform enemyTransform;
	private int baseAttackCount = 0;
	private int specialAttackCount = 0;
	private int cooldown = 150;
	private int moveCount = 0; //What move are we on again?

	void Awake(){
		enemyTransform = this.transform; //Reference to current enemy
	}

	void Update () {

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
			GameObject bullet = (GameObject)Instantiate (baseAttackPrefab, enemyTransform.position, enemyTransform.rotation);
			Destroy (bullet, 10.0f);
			baseAttackCount = 0;
		}
		baseAttackCount++;
	}

	/*Special attack (in progress)*/
	private void Special_Attack(){
		if(specialAttackCount >= (cooldown + 50)){
			GameObject bullet = (GameObject)Instantiate (specialAttackPrefab, enemyTransform.position, enemyTransform.rotation);
			Destroy (bullet, 10.0f);
			specialAttackCount = 0;
		}
		specialAttackCount++;
	}
}
