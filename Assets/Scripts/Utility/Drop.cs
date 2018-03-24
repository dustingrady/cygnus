using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Drop : MonoBehaviour{
	List<Item> so = new List<Item>();
	GameObject drop;

	void Start()
	{
		//so = Resources.Load ("ScriptableObjects/Items/Lesser Potion of Healing") as Item;
		foreach (Object i in Resources.LoadAll("ScriptableObjects/Items/Usable")) {
			Item temp = i as Item;
			so.Add (temp);
			//Debug.Log (temp);
		}
			
		//Debug.Log(Resources.LoadAll("ScriptableObjects/Items/Usable").Length);

		drop = Resources.Load ("Prefabs/Potion") as GameObject;
	}

	public void dropItem(int i)
	{
		if (i <= 2) {
			drop.gameObject.GetComponent<ItemInteraction> ().item = so [2];
			drop.gameObject.GetComponent<SpriteRenderer> ().sprite = so [2].sprite;
			Instantiate (drop, transform.position, Quaternion.identity);
		} else if (i > 2 && i <= 4) {
			drop.gameObject.GetComponent<ItemInteraction> ().item = so [1];
			drop.gameObject.GetComponent<SpriteRenderer> ().sprite = so [1].sprite;
			Instantiate (drop, transform.position, Quaternion.identity);
		} else if (i > 4 && i <= 6) {
			drop.gameObject.GetComponent<ItemInteraction> ().item = so [0];
			drop.gameObject.GetComponent<SpriteRenderer> ().sprite = so [0].sprite;
			Instantiate (drop, transform.position, Quaternion.identity);
		} else if (i > 6 && i <= 7) {
			drop.gameObject.GetComponent<ItemInteraction> ().item = so [3];
			drop.gameObject.GetComponent<SpriteRenderer> ().sprite = so [3].sprite;
			Instantiate (drop, transform.position, Quaternion.identity);
		} else if (i > 7 && i <= 8) {
			drop.gameObject.GetComponent<ItemInteraction> ().item = so [4];
			drop.gameObject.GetComponent<SpriteRenderer> ().sprite = so [4].sprite;
			Instantiate (drop, transform.position, Quaternion.identity);
		} else if (i > 8 && i <= 9) {
			drop.gameObject.GetComponent<ItemInteraction> ().item = so [5];
			drop.gameObject.GetComponent<SpriteRenderer> ().sprite = so [5].sprite;
			Instantiate (drop, transform.position, Quaternion.identity);
		} else if (i > 9 ) {
			drop.gameObject.GetComponent<ItemInteraction> ().item = so [6];
			drop.gameObject.GetComponent<SpriteRenderer> ().sprite = so [6].sprite;
			Instantiate (drop, transform.position, Quaternion.identity);
		}
	}

}
