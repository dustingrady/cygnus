using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class EngageBoss : MonoBehaviour {
	public bool activated; // If the trigger has been hit

	public CinemachineVirtualCamera cam;
	public bool expandingCamera = false;
	private float originalCameraSize;
	public AcidBoss boss;

	void Start() {
		Player.OnDeath += ResetBoss;
	}

	void Update() {
		if (expandingCamera) {
			cam.m_Lens.OrthographicSize += 1.5f * Time.deltaTime;

			if (cam.m_Lens.OrthographicSize > 12) {
				expandingCamera = false;
			}
		}
	
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.CompareTag ("Player") && !activated) {
			activated = true; // activate the switch
			boss.activated = true; // activate the boss

			cam = GameObject.Find ("Follow CM").gameObject.GetComponent<CinemachineVirtualCamera> ();
			expandingCamera = true;
			originalCameraSize = cam.m_Lens.OrthographicSize;
		}
	}

	void ResetBoss() {
		if (activated) {
			Debug.Log ("RESETTING TRIGGER");
			Debug.Log (originalCameraSize);
			Debug.Log (cam.m_Lens.OrthographicSize);
			expandingCamera = false;
			cam.m_Lens.OrthographicSize = originalCameraSize;
			activated = false;
		}
	}
}
