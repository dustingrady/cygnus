using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AcidBossKiller", menuName = "Items/AcidBossKiller")]
public class AcidBossKiller : Item {

	public string description;
	private bool consumable = false;

	public override void useItem()
	{
		Debug.Log("You picked up Boss Killer!");
	}

	public override string itemDescription()
	{
		return description;
	}

	public override bool checkType()
	{
		return consumable;
	} 
}
