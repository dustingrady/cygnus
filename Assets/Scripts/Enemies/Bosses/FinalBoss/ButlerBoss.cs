using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyDamage))]
[RequireComponent(typeof(Rigidbody2D))]
public class ButlerBoss : Enemy {
	public enum Phases {
		Disabled,
		Zero,
		One,
		Two,
		Three,
		Dead
	}

	[Header("Boss Movement")]
	public float speed = 5f;
	public float moveRetargetFreq = 4f;

	[Header("Boss Combat")]
	public float modeTimer;
	public float maxEnergy = 100;
	public bool gainingEnergy;
	public float energyRegenRate = 20;
	public float shotDelay = 1f;

	[Header("Positions")]
	public GameObject phaseZeroPosition;
	public GameObject phaseOneLeftPosition;
	public GameObject phaseOneRightPosition;
	public GameObject phaseTwoTopPosition;
	public GameObject leftFloorTarget;
	public GameObject rightFloorTarget;

	[Header("Tiles")]
	public GameObject[] leftFloorTiles;
	public GameObject[] rightFloorTiles;

	[Header("Assets")]
	public GameObject standardBullet;
	public GameObject igniteBullet;
	public GameObject phaseTwoParticles;

	[Header("Death Cinematic References")]
	public GameObject shield;

	// starting values
	private float startingHealth;
	public float movementTimer;
	public float movementCooldown;

	public float igniteTimer;
	public float igniteCooldown;

	private bool flashed = false;

	private Vector3 currentVel = Vector3.zero;
	private Vector3 target = Vector3.zero;
	private Vector3 targetOffset = Vector3.zero;

	Phases currentPhase = Phases.Disabled;

	// Needed References
	private EnemyShooting es;
	private AudioController ac;

	private BossHealthBar healthBar;

	public void Activate() {
		// Get starting hitpoints
		startingHealth = hitpoints;
		target = phaseOneRightPosition.transform.position;

		currentPhase = Phases.Zero;
		StartCoroutine (IntroDialogOne ());
	}


	void Start() {
		base.Start ();
		es = GetComponent<EnemyShooting> ();

		if (GameManager.instance.CheckQuestComplete(999)) {
			Destroy(this.gameObject);
		}

		Player.OnDeath += Reset;

		rb.constraints = RigidbodyConstraints2D.FreezeRotation;

		// Get starting health and position
		startingPosition = transform.position;
		startingHealth = hitpoints;

		//Reference to Audio Controller
		GameObject camera = GameObject.Find("Main Camera");
		ac = camera.GetComponent<AudioController>();
		if (ac != null) {
			if (ac.audio.Length > 1 && ac.audio [1] != null) {
				ac.source.Stop ();
				ac.source.clip = ac.audio [1]; //Switch to boss music
				ac.source.Play ();
			}
		}

		// Find UI
		healthBar = GameObject.Find("BossHealthBar").GetComponent<BossHealthBar>();
	}

	void OnDestroy() {
		Player.OnDeath -= Reset;
	}


	void Reset() {
		if (transform != null) {
			Debug.Log ("resetting boss!");
			transform.position = startingPosition;
			GetComponent<Rigidbody2D> ().velocity = Vector3.zero;
			hitpoints = startingHealth;
			currentPhase = Phases.Disabled;
			healthBar.Disable ();
			phaseTwoParticles.SetActive (false);

			// RESET THE FLOOR TILES
			foreach (GameObject tile in leftFloorTiles) {
				tile.SetActive (false);
			}

			foreach (GameObject tile in rightFloorTiles) {
				tile.SetActive (false);
			}
		}
	}


	//
	// Phase One
	//
	public void BeginPhaseOne() {
		healthBar.Enable ((int)hitpoints);
		speed = 20;
		currentPhase = Phases.One;
	}

	void PhaseOne() {
		healthBar.SetCurrentHealth (hitpoints);
		EvaluateHealth ();
	}


	//
	// Phase Two
	//
	void BeginPhaseTwo() {
		currentPhase = Phases.Two;
		phaseTwoParticles.SetActive (true);
	}

	void PhaseTwo() {
		healthBar.SetCurrentHealth (hitpoints);
		es.shoot_At_Player ();

		EvaluateHealth ();
	}
		

	void Update() {
		if (currentPhase != Phases.Disabled && currentPhase != Phases.Dead) {
			Movement ();
		}

		if (currentPhase == Phases.One) {
			PhaseOne ();
		} else if (currentPhase == Phases.Two) {
			PhaseTwo ();
		}
	}

	protected void EvaluateHealth() {
		if (hitpoints < 75 && currentPhase == Phases.One) {
			BeginPhaseTwo ();
		}

		if (hitpoints <= 0 && currentPhase != Phases.Dead) {
			currentPhase = Phases.Dead;
			shield.SetActive (false);

			// RESET THE FLOOR TILES
			foreach (GameObject tile in leftFloorTiles) {
				tile.SetActive (false);
			}

			foreach (GameObject tile in rightFloorTiles) {
				tile.SetActive (false);
			}

			// Add gravity during speach
			Rigidbody2D rbTwoDee = GetComponent<Rigidbody2D>();
			rbTwoDee.gravityScale = 2;
			rbTwoDee.constraints = RigidbodyConstraints2D.None;
			rbTwoDee.AddTorque (10);
			healthBar.Disable ();
			StartCoroutine (DeathDialog ());
		}
	}


	void OnTriggerEnter2D(Collider2D col) {
		if (damagingElements.Contains (col.gameObject.tag)) {
			takeDamage (edmg.determine_Damage (col.gameObject.tag, Elements.earth));
		}
	}

	//
	// Rendering
	// 


	//
	// Combat
	// 
	void IgniteFloor() {
		igniteTimer += Time.deltaTime;
		if (igniteTimer > igniteCooldown) {
			Debug.Log ("shooting ignite bullet");
			GameObject blt = (GameObject)Instantiate (igniteBullet, transform.position, Quaternion.identity);

			float random = Random.Range (0f, 1f);
			Debug.Log (random);
			if (random > 0.5) {
				blt.GetComponent<Rigidbody2D> ().velocity = (leftFloorTarget.transform.position - transform.position).normalized * 15;
			} else {
				blt.GetComponent<Rigidbody2D> ().velocity = (rightFloorTarget.transform.position - transform.position).normalized * 15;
			}

			igniteTimer = 0;
		}
	}


	//
	// Movement
	//

	void Movement() {
		if (currentPhase == Phases.Zero) {
			MoveToTarget (phaseZeroPosition.transform.position);
		} else if (currentPhase == Phases.One) {
			PhaseOneTargetShifting (moveRetargetFreq);
			MoveToTarget (target);
			IgniteFloor ();
		} else if (currentPhase == Phases.Two) {
			PhaseTwoTargetShifting (moveRetargetFreq);
			MoveToTarget (target);
			IgniteFloor ();
		}

	}


	void PhaseOneTargetShifting(float frequency) {
		movementTimer += Time.deltaTime;

		if (movementTimer > frequency - 1.5f && !flashed) {
			StartCoroutine(Flash ());
			flashed = true;
		}

		if (movementTimer > frequency) {
			if (target == phaseOneLeftPosition.transform.position) {
				target = phaseOneRightPosition.transform.position;
			} else if (target == phaseOneRightPosition.transform.position) {
				target = phaseOneLeftPosition.transform.position;
			} else {
				// Default to left
				target = phaseOneLeftPosition.transform.position;
			}


			movementTimer = 0;
			flashed = false;
		}
	}


	void PhaseTwoTargetShifting(float frequency) {
		movementTimer += Time.deltaTime;

		if (movementTimer > frequency - 1.5f && !flashed) {
			StartCoroutine(Flash ());
			flashed = true;
		}

		if (movementTimer > frequency) {
			if (target == phaseOneLeftPosition.transform.position) {
				target = phaseTwoTopPosition.transform.position;
			} else if (target == phaseTwoTopPosition.transform.position) {
				target = phaseOneRightPosition.transform.position;
			} else if (target == phaseOneRightPosition.transform.position) {
				target = phaseOneLeftPosition.transform.position;
			} else {
				// Default to left
				target = phaseOneLeftPosition.transform.position;
			}
				
			movementTimer = 0;
			flashed = false;
		}
	}


	void MoveNearPlayer() {
		target = playerTransform.position + targetOffset;
		transform.position = Vector3.SmoothDamp (transform.position, target, ref currentVel, (10 / speed));
	}

	void MoveToTarget(Vector3 target) {
		transform.position = Vector3.SmoothDamp (transform.position, target, ref currentVel, (10 / speed));
	}

	IEnumerator Flash() {
		SpriteRenderer sr = GetComponent<SpriteRenderer> ();
		int elapsed = 0;
		int flashes = 3;
		while(elapsed < flashes){
			sr.color = new Color(1f, 1f, 0.2f);
			yield return new WaitForSeconds(0.10f);
			sr.color = Color.white;
			yield return new WaitForSeconds(0.10f);
			elapsed++;
		}
	}
		

	//
	// Coroutines
	//
/*
	IEnumerator ChangeTargetPos(float time) {
		while (true) {
			if (target == phaseOneLeftPosition.transform.position) {
				target = phaseOneRightPosition.transform.position;
			} else if (target == phaseOneRightPosition.transform.position) {
				target = phaseOneLeftPosition.transform.position;
			}

			yield return new WaitForSeconds (time);
		}
	}
*/

	//
	// Dialog
	//

	IEnumerator IntroDialogOne() {
		DialogPopupController.Initialize();
		DialogPopupController.CreateDialogPopup("Sorry about that...", 3f);
		yield return new WaitForSeconds (3f);
		StartCoroutine (IntroDialogTwo ());
	}

	IEnumerator IntroDialogTwo() {
		DialogPopupController.Initialize();
		DialogPopupController.CreateDialogPopup("I'm not going to depart with you from this hell hole just for you to crash us into another.", 5f);
		yield return new WaitForSeconds (5f);
		StartCoroutine (IntroDialogThree ());
	}

	IEnumerator IntroDialogThree() {
		DialogPopupController.Initialize();
		DialogPopupController.CreateDialogPopup("So why don't you just hand over the last ship part and let me be on my way?", 5f);
		yield return new WaitForSeconds (5f);
		StartCoroutine (IntroDialogFour ());
	}

	IEnumerator IntroDialogFour() {
		DialogPopupController.Initialize();
		DialogPopupController.CreateDialogPopup("Nah, fuck it... PREPARE TO DIE!", 3f);
		yield return new WaitForSeconds (3f);
		BeginPhaseOne ();
	}

	IEnumerator DeathDialog() {
		DialogPopupController.Initialize();
		DialogPopupController.CreateDialogPopup("Yeah, it was worth it.  No regrets.  The ship is yours again kid.  Cheers", 5f);
		yield return new WaitForSeconds (5f);
		Instantiate (deathParticle, transform.position, Quaternion.identity);
		GameObject.Find ("Left Flames").SetActive(false);
		GameObject.Find ("Right Flames").SetActive(false);
		GameManager.instance.CompleteQuest (999);
		Destroy (this.gameObject);
	}

}
