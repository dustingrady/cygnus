using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Inventory : MonoBehaviour {

	public bool showInventory = true;

	public const int inventorySize = 8;

	// Inventory items and quantities
	public Item[] items = new Item[inventorySize];
	public int[] itemQuantity = new int[inventorySize];

	// Inventory UI, should always match up with the items above
	public GameObject inventoryUI;
	public Image[] itemImages = new Image[inventorySize];
	public Text[] itemImageQuantities = new Text[inventorySize];

	// Collect the item images when the inventory is loaded
	public void Start() {
		initializeInventory ();
	}

	public void Update() {
		if (inventoryUI != null) {
			// Temporary solution to display and hide inventory
			if (Input.GetKeyDown (KeyCode.I)) {
				if (showInventory == true) {
					hideInventory ();
                    showInventory = !showInventory;
                } else {
					displayInventory ();
                    showInventory = !showInventory;
                }
			}
		}
	}

	public void addItem(Item item)
    {
        for(int i = 0; i < items.Length; i++)
        {
			if (items [i] == item) 
			{
				itemQuantity [i]++;
				return;
			}
			else if (items [i] == null) {
				items [i] = item;
				itemQuantity [i]++;
				return;
			} 
        }
        Debug.Log(item.name + " added");
    }

	public void removeItem(Item item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == item)
            {
                items[i] = null;
				itemQuantity [i]--;
                return;
            }
        }
        //Debug.Log(item.name + " removed");
    }

	// Only update the Inventory GUI when the 
    void OnGUI()
    {
        if (!isEmpty() && inventoryUI != null)
        {
            for (int i = 0; i < items.Length; i++)
            {
				if (items [i] != null) {
					itemImageQuantities [i].enabled = true;
					itemImageQuantities [i].text = itemQuantity [i].ToString();

					itemImages [i].enabled = true;
					itemImages [i].sprite = items [i].sprite;
				} else {
					itemImageQuantities [i].enabled = false;
					itemImages [i].enabled = false;
				}
            }
        }
    }

    public bool isEmpty()
    {
        for(int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
                continue;
            else if (items[i] != null)
                return false;
        }
        return true;
    }

	public void emptyInventory() {
		items = new Item[inventorySize];
		itemQuantity = new int[inventorySize];
	}

	public void hideInventory() {
		inventoryUI.transform.localScale = Vector3.zero;
	}

	public void displayInventory() {
		inventoryUI.transform.localScale = new Vector3(1f, 1f, 1f);
	}

	// Inventory needs to be initalized every time a new scene is loaded
	public void initializeInventory() {
		inventoryUI = GameObject.FindGameObjectWithTag ("InventoryUI");
		for (int i = 0; i < inventorySize; i++) {
			Transform item = inventoryUI.transform.Find(string.Format("item{0}", i));
			itemImages[i] = item.transform.Find ("ItemImage").GetComponentInChildren<Image> ();
			itemImageQuantities[i] = item.transform.Find ("ItemQuant").GetComponentInChildren<Text> ();
		}



	}
	/*
	public void enableImage(Image img, Sprite sprt)
	{
		img.sprite = sprt;
		img.enabled = true;
	}
		
	public void disableImage(Image img)
	{
		img.sprite = null;
		img.enabled = false;
	}	
	*/
		
}
