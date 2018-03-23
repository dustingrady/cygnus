using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="PotionItem", menuName="Items/Potion")]
public class Potion : Item {
	public float healAmount;
	public string description;
	private bool consumable = true;

	public override void useItem()
	{
		Debug.Log ("wut");
	}

	public override string itemDescription()
	{
		return description;
	}

	public override float useConsumable()
	{
		return healAmount;
	}

	public override bool checkType()
	{
		return consumable;
	}

	/*
	public void setHeal(float f)
	{
		healAmount = f;
	}
	*/
}
