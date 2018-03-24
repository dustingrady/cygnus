using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="ComboItem", menuName="Items/Combo")]
public class ComboDrop : Item {

	public string comboElement;
	public string description;
	private bool consumable = true;

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
		return comboElement;
	}

	public override bool checkType()
	{
		return consumable;
	}
}
