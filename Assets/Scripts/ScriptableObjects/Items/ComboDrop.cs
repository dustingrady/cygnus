using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="ComboItem", menuName="Items/Combo")]
public class ComboDrop : Item {

	public string comboElement;
	public string description;
	private bool consumable = true;
	ElementManager EM;
	ElementUI EU;

	public override void useItem()
	{
		EM = GameObject.Find ("Elements").GetComponent<ElementManager> ();
		EU = GameObject.Find ("UI").GetComponent<ElementUI> ();

		if (comboElement == "lava") {
			EM.AssignToHand ("left", "FireElement");
			EM.AssignToHand ("right", "EarthElement");
		}
		if (comboElement == "steam") {
			EM.AssignToHand ("left", "WaterElement");
			EM.AssignToHand ("right", "FireElement");
		}
		if (comboElement == "magnet") {
			EM.AssignToHand ("left", "MetalElement");
			EM.AssignToHand ("right", "ElectricElement");
		}

		EU.UpdateElements ();
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
