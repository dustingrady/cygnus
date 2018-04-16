using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EMP", menuName = "Items/EMP")]
public class EMP : Item {

    public string description;
    private bool consumable = true;
	Player player;
	GameObject nade;

    public override void useItem()
    {
		nade = Resources.Load ("Prefabs/Projectiles/EMPGrenade") as GameObject;
		player = GameObject.Find ("Player").GetComponent<Player> ();
		Instantiate (nade, player.transform.position, Quaternion.identity);
		Debug.Log ("Used " + this.name);
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
