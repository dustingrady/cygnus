using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EMP", menuName = "Items/EMP")]
public class EMP : Item {

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
