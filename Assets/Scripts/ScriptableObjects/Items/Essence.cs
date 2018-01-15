﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="EssenceItem", menuName="Items/Essence")]
public class Essence : Item {
	public enum ElementType {Fire, Water, Earth, Metal};

	public ElementType type;
	public int strength;
	public string description;

	public override void useItem()
	{
		Debug.Log("Used " + this.name);
	}

	public override string itemDescription()
	{
		return description;
	}

}
