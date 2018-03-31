using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidBoss : MonoBehaviour 
{

	public float RotateSpeed = 5f;
	public float Radius = 10.0f;
	public bool Clockwise = true;

	private Vector2 _centre;
	private float _angle;

	private void Start()
	{
		_centre = transform.position;
	}

	private void Update()
	{
		Vector2 v1 = _centre - new Vector2(transform.position.x, transform.position.y);
		v1 = v1.normalized;
		float test = Mathf.Atan2 (v1.y, v1.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler (0, 0, test - 90);
		//transform.LookAt (v1);

		if (Clockwise) {
			rotateClockwise ();
		} else {
			rotateCounterClockwise ();
		}
	}

	private void rotateClockwise() {
		_angle += RotateSpeed * Time.deltaTime;

		var offset = new Vector2(Mathf.Sin(_angle), Mathf.Cos(_angle)) * Radius;
		transform.position = _centre + offset;
	}

	private void rotateCounterClockwise() {
		_angle += RotateSpeed * Time.deltaTime;

		var offset = new Vector2(Mathf.Cos(_angle), Mathf.Sin(_angle)) * Radius;
		transform.position = _centre + offset;
	}
}
