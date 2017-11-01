using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterEssence : Item {
    public override void useItem()
    {
        Debug.Log("Used " + this.name);
    }
}
