using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CaveGuardian : MonoBehaviour {

	private Vector3 targetPos;
	private CapsuleCollider2D col;
	public float speed = 1.5f;
	private bool activated = false;
	private EnemyShooting es;
	private TurretType turret;
	CinemachineBasicMultiChannelPerlin pn;

	// Use this for initialization
	void Awake () {
		col = GetComponent<CapsuleCollider2D> ();
		targetPos = transform.position + new Vector3 (0f, col.bounds.extents.y * 2, 0f); // Must get the value before the collider is disabled
		col.enabled = false;

		es = GetComponent<EnemyShooting> ();
		es.enabled = false;

		turret = GetComponent<TurretType> ();

		GameObject cam = GameObject.Find ("FollowCM");
		CinemachineVirtualCamera vc = cam.GetComponent<CinemachineVirtualCamera>();
		pn = vc.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin> ();

	}
	
	// Update is called once per frame
	void Update () {
		if (activated == false) {
			if (Vector3.Distance (transform.position, targetPos) > 0.05f) {
				transform.position = Vector3.MoveTowards (transform.position, targetPos, speed * Time.deltaTime);
				pn.m_AmplitudeGain = 0.3f;
				pn.m_FrequencyGain = 8f;

			} else {
				col.enabled = true;
				es.enabled = true;
				activated = true;

				pn.m_AmplitudeGain = 0;
				pn.m_FrequencyGain = 0;
			}
		}
	}
}
