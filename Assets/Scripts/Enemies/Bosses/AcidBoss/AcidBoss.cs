using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidBoss : Enemy {
	private float radius = 35.0f; //How far we can see player
	private bool shootingPlayer;
	private EnemyShooting es;
	public bool isShooting = false;

	public int maxHP = 200;
	private Vector3 originalPosition;
	[SerializeField]
	private GameObject leftTail;
	private Vector3 originalLeftTailPosition;
	[SerializeField]
	private GameObject rightTail;
	private Vector3 originalRightTailPosition;

	[SerializeField]
	private GameObject endDoor;

	private BossHealthBar healthBar;

	public bool activated;
	public bool raised;

	private void Awake(){
		es = gameObject.GetComponent<EnemyShooting> ();
		Player.OnDeath += ResetBoss;
	}

	// Use this for initialization
	void Start () {
		base.Start (); // Call the based enemy Start() function
		hitpoints = maxHP;

		// find ui
		healthBar = GameObject.Find("BossHealthBar").GetComponent<BossHealthBar>();

		originalPosition = transform.position;
		originalLeftTailPosition = leftTail.transform.position;
		originalRightTailPosition = rightTail.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (activated) {
			EvaluateHealth ();
			EvaluateTolerance ();

			if (!raised) {
				Activate ();
			} else if (hitpoints > maxHP * 0.75) {
				PhaseOne ();
			} else if (hitpoints < maxHP * 0.75) {
				PhaseTwo ();
			}
		}
	}

	void OnCollisionEnter2D(Collision2D col) {
		//EvaluatePhysical (col);
	}

	public void TakeDamage(string tag) {
		ElectricShock (tag);
	}
				
	private void Activate() {
		if (transform.localPosition.y < -13) {
			transform.Translate (new Vector3 (-Time.deltaTime * 3, 0, 0));
		} else {
			raised = true;
			healthBar.Enable (maxHP);
		}
	}

	private void PhaseOne() {
		if (leftTail.transform.localPosition.y < -13) {
			leftTail.transform.Translate (new Vector3 (Time.deltaTime * 3, 0, 0));
		}

		ShootAtPlayer ();
	}

	private void PhaseTwo() {
		if (leftTail.transform.localPosition.y > -30) {
			leftTail.transform.Translate (new Vector3 (-Time.deltaTime * 3, 0, 0));
		} else if (rightTail.transform.localPosition.y < -13) {
			rightTail.transform.Translate (new Vector3 (Time.deltaTime * 3, 0, 0));
		}

		ShootAtPlayer ();
	}

	private void ShootAtPlayer() {
		if (raised && DistanceToPlayer () <= radius && !stunned) {
			es.shoot_At_Player ();
		}
	}

	protected void EvaluateHealth() {
		// Remove the door if the boss dies
		if (hitpoints <= 0) {
			// remove the door
			endDoor.SetActive (false);

			// disable bar
			healthBar.Disable ();

			// disable tail
			leftTail.SetActive(false);
			rightTail.SetActive(false);
		}

		healthBar.SetCurrentHealth (hitpoints);

		base.EvaluateHealth ();

	}

	void ResetBoss() {
		healthBar.Disable ();

		hitpoints = maxHP;

		transform.position = originalPosition;
		leftTail.transform.position = originalLeftTailPosition;
		rightTail.transform.position = originalRightTailPosition;

		activated = false;
		raised = false;
	}
}

