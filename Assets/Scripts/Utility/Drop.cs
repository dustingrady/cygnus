using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Drop : MonoBehaviour{

	[System.Serializable]
	//Set the weights for an item to drop. Lowest and highest provide a range for said item to drop. Think of it as a range of percentage probability that the item could drop
	//Careful when setting same range for more than 1 items, the first item on the list that is within range will drop. 
	//Careful not to create holes in your range. ie. if item 1 have 10-20 weight, item 2 have 40-50 weight, there needs to be an item with range 21-39
	public class lootDrop
	{
		public Item item;
		public int lowestWeight;
		public int highestWeight;
	}

	//List<Item> so = new List<Item>();
	public List<lootDrop> lootTable = new List<lootDrop>();
	GameObject drop;
	int totalDropWeight = 0;
	public int dropChance = 70;
	void Start()
	{
		for (int i = 0; i < lootTable.Count; i++) {
			if(totalDropWeight <= lootTable [i].highestWeight)
				totalDropWeight = lootTable [i].highestWeight;
		}

		drop = Resources.Load ("Prefabs/ItemPrefab") as GameObject;
	}

	public void dropItem(int deadEnemy)
	{
		if (deadEnemy > dropChance) {
			Debug.Log ("No drop");
			return;
		}
			
		if (deadEnemy <= dropChance) {
			int chance = Random.Range (0, totalDropWeight);
			Debug.Log ("Drop chance : " + chance);
			for (int i = 0; i < lootTable.Count; i++) {
				if (chance <= lootTable [i].highestWeight && chance >= lootTable [i].lowestWeight) {
					drop.gameObject.name = lootTable [i].item.name;
					drop.gameObject.GetComponent<ItemInteraction> ().item = lootTable[i].item;
					drop.gameObject.GetComponent<SpriteRenderer> ().sprite = lootTable [i].item.sprite;
					Instantiate (drop, transform.position, Quaternion.identity);
					return;
				}
			}
		}
	}

}
