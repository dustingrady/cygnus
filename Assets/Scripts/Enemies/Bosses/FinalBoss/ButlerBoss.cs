using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyDamage))]
[RequireComponent(typeof(Rigidbody2D))]
public class ButlerBoss : Enemy {
	public enum Phases {
		Zero,
		One,
		Two,
		Three
	}

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
	public GameObject ignitionBullet;
	public GameObject standardBullet;

	private Vector3 currentVel = Vector3.zero;
	private Vector3 target = Vector3.zero;
	private Vector3 targetOffset = Vector3.zero;

	private bool activated = false;
	Phases currentPhase = Phases.One;

	// Needed References
	private EnemyShooting es;
	private LineRenderer lr;
	public GameObject endingIsland;

	private AudioController ac;

	private BossHealthBar healthBar;

	public void Activate() {
		healthBar.Enable ((int)hitpoints);
		activated = true;
	}

	void Start() {
		base.Start ();
		rb.constraints = RigidbodyConstraints2D.FreezeRotation;

		//Reference to Audio Controller
		GameObject camera = GameObject.Find("Main Camera");
		ac = camera.GetComponent<AudioController>();
		if (ac.audio [1] != null) {
			ac.source.Stop ();
			ac.source.clip = ac.audio [1]; //Switch to boss music
			ac.source.Play ();
		}

		// Find UI
		healthBar = GameObject.Find("BossHealthBar").GetComponent<BossHealthBar>();
	}


	void PhaseZero() {
	
	}

	void PhaseOne() {
	
	}

	void PhaseTwo() {
	
	}

	void Update() {
		if (activated) {
			healthBar.SetCurrentHealth (hitpoints);
			EvaluateHealth ();
			Movement ();
		}
	}


	void OnTriggerEnter2D(Collider2D col) {

	}

	//
	// Rendering
	// 


	//
	// Combat
	// 


	//
	// Movement
	//

	void Movement() {
		MoveNearPlayer();
		Bobble (bobbleFrequency, bobbleAmplitude);
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
}
