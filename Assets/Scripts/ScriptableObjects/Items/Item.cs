﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Item : ScriptableObject {
	public Sprite sprite;
	public abstract void useItem();
	public abstract float useConsumable ();
	public abstract string itemDescription ();
	public abstract bool checkType();
}
