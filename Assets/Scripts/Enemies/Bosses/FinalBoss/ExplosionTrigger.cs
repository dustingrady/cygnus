using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ExplosionTrigger : MonoBehaviour {
	public CinemachineVirtualCamera cam;
	public bool expandingCamera = false;
	private float originalCameraSize;

	[SerializeField]
	private GameObject leftFlame;
	[SerializeField]
	private GameObject rightFlame;
	[SerializeField]
	private GameObject explosionParticles;
	[SerializeField]
	public ButlerBoss finalBoss;

	private bool respawned = false;
	private bool activated = false;

	void OnTriggerEnter2D(Collider2D col) {
		if (col.CompareTag("Player") && activated == false && CheckPartsCollected() == 4 && !GameManager.instance.CheckQuestComplete(999)) {
			cam = GameObject.Find ("Follow CM").gameObject.GetComponent<CinemachineVirtualCamera> ();
			expandingCamera = true;
			originalCameraSize = cam.m_Lens.OrthographicSize;

			Explode ();
		}
	}

	void Update() {
		if (expandingCamera) {
			cam.m_Lens.OrthographicSize += 1.5f * Time.deltaTime;

			if (cam.m_Lens.OrthographicSize > 8.95) {
				expandingCamera = false;
				//this.gameObject.SetActive (false);
			}
		}
	}

	void Explode() {
		activated = true;

		GameObject plr = GameObject.FindGameObjectWithTag ("Player");

		// knock back player
		plr.GetComponent<Rigidbody2D> ().AddForce (new Vector2(1400f, 900f));
		Instantiate (explosionParticles, new Vector3(transform.position.x, transform.position.y - 3, 1), Quaternion.identity);

		// stun player
		StartCoroutine(plr.GetComponent<Player>().StunPlayer(14f));

		leftFlame.SetActive (true);
		rightFlame.SetActive (true);

		finalBoss.Activate ();

		Player.OnDeath += ResetTrigger;
	}

	void ResetTrigger() {
		Debug.Log ("RESETTING TRIGGER");
		Debug.Log (originalCameraSize);
		Debug.Log (cam.m_Lens.OrthographicSize);
		expandingCamera = false;
		cam.m_Lens.OrthographicSize = originalCameraSize;
		activated = false;

		this.gameObject.SetActive (true);
		leftFlame.SetActive (false);
		rightFlame.SetActive (false);
	}

	// Returns the number of ship parts collected
	int CheckPartsCollected() {
		int[] questNums = {199, 299, 399, 499};
		int completed = 0;
		foreach (int quest in questNums) {
			if (GameManager.instance.CheckQuestComplete (quest)) {
				completed++;
			}
		}

		return completed;
	}

}
