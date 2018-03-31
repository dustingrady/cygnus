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
	public List<GameObject> children;
	private List<GameObject> offset = new List<GameObject>();
	private int counter = 0;

	private void Start()
	{
		_centre = transform.position;
	}

	private void Update()
	{

		transform.rotation = Quaternion.Euler (0, 0, rotate() - 90);
		if (Clockwise) {
			rotateClockwise ();
		} else {
			rotateCounterClockwise ();
		}
		/**
		if (offset.Count != children.Count) {
			offset.Add (children [counter]);
			counter++;
			for (int i = 0; i < offset.Count; i++) {
				if (offset.Count == 1) {
					float rotation = rotate ();
					offset [0].transform.rotation = Quaternion.Euler (0, 0, rotation - 90);
				} else {
					float rotation = rotate ();
					if (i == 0) {
						offset [0].transform.rotation = Quaternion.Euler (0, 0, rotation - 90);
					} else {
						Quaternion prevRotation = offset [i - 1].transform.rotation;
						offset [i].transform.rotation = prevRotation;//Quaternion.Euler (0, 0, prevRotation - 90);
					}
				}
				if (Clockwise) {
					rotateClockwise ();
				} else {
					rotateCounterClockwise ();
				}
			}
		} else {
			for (int i = 0; i < offset.Count; i++) {
				if (offset.Count == 1) {
					float rotation = rotate ();
					offset [0].transform.rotation = Quaternion.Euler (0, 0, rotation - 90);
					if (Clockwise) {
						rotateClockwise ();
					} else {
						rotateCounterClockwise ();
					}
				} else {
					float rotation = rotate ();

					if (i == 0) {
						offset [0].transform.rotation = Quaternion.Euler (0, 0, rotation - 90);
						if (Clockwise) {
							rotateClockwise ();
						} else {
							rotateCounterClockwise ();
						}
					} else {
						Vector2 position = offset [i - 1].transform.position;
						Quaternion prevRotation = offset [i - 1].transform.rotation;
						offset [i].transform.rotation = prevRotation;//Quaternion.Euler (0, 0, prevRotation - 90);
						offset[i].transform.position = Vector2.Lerp(offset[i].transform.position, position, Time.deltaTime*0.5f);
					}
				}

			}
		}
		/*
		for (int i = 0; i < children.Count; i++) {
			//Vector2 v1 = _centre - new Vector2 (transform.position.x, transform.position.y);
			float rotation = rotate ();
			children [i].transform.rotation = Quaternion.Euler (0, 0, rotation - 90);
			if (Clockwise) {
				rotateClockwise ();
			} else {
				rotateCounterClockwise ();
			}
		}*/
	}

	private float rotate() {
		Vector2 v1 = _centre - new Vector2 (transform.position.x, transform.position.y);
		v1 = v1.normalized;
		return Mathf.Atan2 (v1.y, v1.x) * Mathf.Rad2Deg;
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
