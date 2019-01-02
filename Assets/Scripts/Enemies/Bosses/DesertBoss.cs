using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesertBoss : Enemy {

	public float moveRetargetFreq = 2f;
	public float speed = 6f;
	public int phaseTwoStartHealth = 300;
	public bool inPhaseTwo = false;
	private float baseHeight = 7;
	private Vector3 startPos;

	private Vector3 currentVel = Vector3.zero;
	private Vector3 target = Vector3.zero;
	private Vector3 targetOffset = Vector3.zero;

	public GameObject shipPiece;
	public GameObject teleporter;

	private EnemyShooting es;

	private GameObject ebarrier;
	public bool godLike;
	CircleCollider2D cc;

	bool activated = true;

	private void Awake(){
		es = gameObject.GetComponent<EnemyShooting>();
		ebarrier = GameObject.Find ("DesertBossBarrier");

		startPos = transform.position;
	}

	void Start(){
		base.Start (); // Call the based enemy Start() function
		startPos = transform.position;

		// Setting the godLike boolean so that the boss starts unkillable
		godLike = true;

		// Begin target shifting
		StartCoroutine (ChangeTargetPos(moveRetargetFreq));
	}

	void Update(){
		if (activated == true) {
			EvaluateHealth ();
			CheckPhaseChange ();
			Movement ();
			es.shoot_At_Player ();
		}
	}


	void CheckPhaseChange() {
		if (hitpoints < phaseTwoStartHealth) {
			GetComponent<SpriteRenderer> ().color = new Color(1f, 0.8f, 0.8f, 1.0f);
			baseHeight = 3;
			es.cooldown = 75;

			GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.None;
			GetComponent<Rigidbody2D> ().AddTorque (5f);

			inPhaseTwo = true;
		}
	}

	void Movement() {
		target = playerTransform.position + targetOffset;
		transform.position = Vector3.SmoothDamp (transform.position, target, ref currentVel, (10 / speed));
	}
		
		

	void OnTriggerEnter2D(Collider2D col){
		activated = true;
		// Check if godlike is disabled before allowing this 
		if (godLike == false) {
			if (damagingElements.Contains (col.gameObject.tag)) {
				takeDamage (edmg.determine_Damage (col.gameObject.tag, elementType));
			}
		}
	}


	IEnumerator damage(float amount) {
		hitpoints -= amount;

		if (hitpoints <= 0) {
			Instantiate (shipPiece, transform.position, Quaternion.identity);
			teleporter.SetActive (true);
		}

		yield return flash (Color.red);
		yield return new WaitForSeconds (1);
	}


	IEnumerator ChangeTargetPos(float time) {
		while (true) {
			float offsetX = Random.Range (-3, 9); // Lower bottom end as the raft is moving right
			float offsetY = Random.Range (-1, 1);

			targetOffset = new Vector3 (offsetX, baseHeight + offsetY, 0f);

			yield return new WaitForSeconds (time);
		}
	}


	void ResetBoss() {
		transform.position = new Vector3 (232, -19, 0); // bad
		transform.rotation = Quaternion.identity;
		hitpoints = 1000;
		currentVel = Vector3.zero;
		GetComponent<Rigidbody2D>().velocity = Vector3.zero;
		inPhaseTwo = false;
		activated = false;
	}

	protected override void takeDamage(float amount) {
		StartCoroutine (damage (amount));
	}
}