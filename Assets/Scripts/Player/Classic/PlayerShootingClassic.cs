using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootingClassic : MonoBehaviour {
	[SerializeField]
	private GameObject playerBullet;
	[SerializeField]
	private float shootCooldown = 1f;
	private float shootTimer = 0f;

	public delegate void ShootAction();
	public static event ShootAction OnShoot;

	void Update () {
		shootTimer += Time.deltaTime;

		if (Input.GetKeyDown(KeyCode.RightControl) && shootTimer > shootCooldown) {
			Shoot ();

			if (OnShoot != null) {
				OnShoot ();
			}
		}
	}

	void Shoot() {
		if (transform.localScale.x > 0) {
			Instantiate (playerBullet, transform.position, transform.rotation);
		} else {
			Instantiate (playerBullet, transform.position, transform.rotation);
		}
		shootTimer = 0f;
	}
}
