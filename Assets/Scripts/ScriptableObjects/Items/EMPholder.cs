using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "EMPholder", menuName = "Items/EMPholder")]
public class EMPholder : Item {

    public string description;
    private bool consumable = false;

    public override void useItem()
    {
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
