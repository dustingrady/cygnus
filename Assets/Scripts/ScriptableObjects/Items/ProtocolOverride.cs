using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProtocolOverride", menuName = "Items/ProtocolOverride")]
public class ProtocolOverride : Item {
	
	public string description;
	private bool consumable = true;
	Player player;
	GameObject protocolGrenade;
	GameObject protocolHolder;

	public override void useItem () {
		protocolGrenade = Resources.Load ("Prefabs/Projectiles/ProtocolGrenade") as GameObject;
		player = GameObject.Find ("Player").GetComponent<Player> ();
		protocolHolder = Instantiate (protocolGrenade, player.transform.position, Quaternion.identity) as GameObject;
		protocolHolder.GetComponent<ProtocolGrenade> ().Initialize (GetCursorDirection (), 20f);
	}

	public override string itemDescription()
	{
		return description;
	}

	public override bool checkType()
	{
		return consumable;
	} 

	public Vector2 GetCursorDirection() {
		if (GameManager.instance.controllerConnected) {
			GameObject ret = GameObject.Find ("Reticle");
			Vector3 dirV3 = ret.transform.position - player.transform.position;
			Vector2 dir = new Vector2 (dirV3.x, dirV3.y);

			return dir;
		} else {
			Vector3 dirV3 = Camera.main.ScreenToWorldPoint (Input.mousePosition) - player.transform.position;
			Vector2 dir = new Vector2 (dirV3.x, dirV3.y);

			return dir;
		}
	}
}
