using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="PotionItem", menuName="Items/Potion")]
public class Potion : Item {
	Player player;
	public float healAmount;
	public string description;
	private bool consumable = true;

	public override void useItem()
	{
		player = GameObject.Find ("Player").GetComponent<Player> ();
		player.healWounds (healAmount);
		Debug.Log (player.health.CurrentVal);
	}

	public override string itemDescription()
	{
		return description;
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
