using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="EssenceItem", menuName="Items/Essence")]
public class Essence : Item {
	public enum ElementType {Fire, Water, Earth, Metal};

	public ElementType type;
	public int strength;
	public string description;
	private bool consumable = false;

	public override void useItem()
	{
		Debug.Log("Used " + this.name);
	}

	public override string itemDescription()
	{
		return description;
	}


	public override float useConsumable ()
	{
		return 0;
	}

	public override string consumeCombo ()
	{
		return "";
	}

	public override bool checkType()
	{
		return consumable;
	}
}
