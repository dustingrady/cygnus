using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMPGrenade : MonoBehaviour {
	float speed;
	public Vector3 direction;

	public void Initialize(Vector2 direction, float speed) {
		this.speed = speed;
		this.direction = direction.normalized;

		// Change the angle to match the direction.
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}

	void Update() {
		transform.position += direction * speed * Time.deltaTime;
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.tag != "Player" && col.gameObject.tag != "EMP") {
			DestroyObject (this.gameObject);
		}
	}

	public Vector2 GetCursorDirection() {
		if (GameManager.instance.controllerConnected) {
			GameObject ret = GameObject.Find ("Reticle");
			Vector3 dirV3 = ret.transform.position - transform.position;
			Vector2 dir = new Vector2 (dirV3.x, dirV3.y);

			return dir;
		} else {
			Vector3 dirV3 = Camera.main.ScreenToWorldPoint (Input.mousePosition) - transform.position;
			Vector2 dir = new Vector2 (dirV3.x, dirV3.y);

			return dir;
		}
	}
}
