using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Element : MonoBehaviour {
	public string elementType;
	public Sprite sprite;
	public bool active = false;

	public abstract void UseElement(Vector3 pos, Vector2 dir);
}
