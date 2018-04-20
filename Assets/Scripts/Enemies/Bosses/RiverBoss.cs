using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyDamage))]
[RequireComponent(typeof(Rigidbody2D))]
public class RiverBoss : Enemy {

	public bool fireMode = false;

	[Header("Boss Movement")]
	public float speed = 5f;
	public float bobbleAmplitude = 1.5f;
	public float bobbleFrequency = 0.2f;
	public float moveRetargetFreq = 3f;

	[Header("Boss Combat")]
	public float modeTimer;
	public float maxEnergy = 100;
	public bool gainingEnergy;
	public float energyRegenRate = 20;
	public float shotDelay = 1f;

	[Header("Assets")]
	public Sprite fireSprite;
	public Sprite waterSprite;
	public GameObject bulletPrefab;

	private Vector3 currentVel = Vector3.zero;
	private Vector3 target = Vector3.zero;
	private Vector3 targetOffset = Vector3.zero;

	// Needed References
	private EnemyShooting es;
	private LineRenderer lr;

	void Start() {
		base.Start ();
		rb.constraints = RigidbodyConstraints2D.FreezeRotation;

		// Get References
		es = GetComponent<EnemyShooting>();
		lr = GetComponent<LineRenderer> ();

		// Begin target shifting
		StartCoroutine (ChangeTargetPos(moveRetargetFreq));
		StartCoroutine (ChangeModeRoutine (modeTimer));
	}


	void Update() {
		EvaluateEnergy ();
		EvaluateHealth ();
		UpdateLine ();
		DrawLine ();

		Movement ();
	}


	void OnTriggerEnter2D(Collider2D col) {
		if (damagingElements.Contains (col.gameObject.tag)) {
			if (fireMode) {
				takeDamage (edmg.determine_Damage (col.gameObject.tag, Elements.fire));
			} else {
				takeDamage (edmg.determine_Damage (col.gameObject.tag, Elements.water));
			}
		}
	}
		
	//
	// Rendering
	// 

	void DrawLine() {
		if (Vector3.Distance(transform.position, playerTransform.position) < 15) {
			lr.SetPosition (0, transform.position);
			lr.SetPosition (1, playerTransform.position);
		}
	}


	void ChangeColors(bool fireMode) {
		if (fireMode) {
			lr.startColor = Color.red;
			lr.endColor = Color.red;
			sr.sprite = fireSprite;
		} else {
			lr.startColor = Color.cyan;
			lr.endColor = Color.cyan;
			sr.sprite = waterSprite;
		}
	}


	void UpdateLine() {
		float maxLineSize = 0.2f;
		float lineMulti = energy / maxEnergy;
		lr.endWidth = maxLineSize - maxLineSize * lineMulti;
		lr.startWidth = maxLineSize - maxLineSize * lineMulti;
	}

		
	//
	// Combat
	// 

	void ChangeModes() {
		Debug.Log ("Changing modes");
		fireMode = !fireMode;
		ChangeColors (fireMode);
	}


	void EvaluateEnergy() {
		if (gainingEnergy) {
			energy += energyRegenRate * Time.fixedDeltaTime;
		}

		if (energy >= 100) {
			Shoot ();
			StartCoroutine (DelayShoot (shotDelay));
		}
	}


	void Shoot() {
		if (fireMode) {
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
		} else {
			GameObject bullet = (GameObject)Instantiate (bulletPrefab, transform.position, transform.rotation);
			bullet.GetComponent<BulletBehaviour> ().setBullet ("single");
		}
	}
		
	//
	// Movement
	//

	void Movement() {
		MoveNearPlayer();
		//Bobble (bobbleFrequency, bobbleAmplitude);
	}


	void MoveNearPlayer() {
		target = playerTransform.position + targetOffset;

		transform.position = Vector3.SmoothDamp (transform.position, target, ref currentVel, (10 / speed));
	}


	void Bobble(float peroid, float amplitude) {
		float theta = Time.timeSinceLevelLoad / peroid;
		float distance = amplitude * Mathf.Sin (theta) * Time.deltaTime;
		transform.position = new Vector3 (transform.position.x, transform.position.y + distance, 0f);
	}

	//
	// Coroutines
	//

	IEnumerator ChangeTargetPos(float time) {
		while (true) {

			// Change base height based on phase
			float baseHeight = 0;
			if (fireMode) {
				baseHeight = 4f;
			} else {
				baseHeight = 6f;
			}
			float offsetX = Random.Range (-3, 9); // Lower bottom end as the raft is moving right
			float offsetY = Random.Range (-1, 1);

			targetOffset = new Vector3 (offsetX, baseHeight + offsetY, 0f);

			yield return new WaitForSeconds (time);
		}
	}

	IEnumerator ChangeModeRoutine(float time) {
		while (true) {
			yield return new WaitForSeconds (time);
			ChangeModes ();
		}
	}

	IEnumerator DelayShoot(float time) {
		energy = 0;
		gainingEnergy = false;
		lr.enabled = false;
		yield return new WaitForSeconds (time);
		lr.enabled = true;
		gainingEnergy = true;
	}
}
