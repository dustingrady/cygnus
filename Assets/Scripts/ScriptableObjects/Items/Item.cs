using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Item : ScriptableObject {
	public string itemName;
	public Sprite sprite;
	public abstract void useItem();
	public abstract string itemDescription ();
	public abstract bool checkType();
}
