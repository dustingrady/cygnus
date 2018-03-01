using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Inventory : MonoBehaviour {


	public bool showInventory = true;

	public int currentScrap = 0;
	public Text scrapCountDisplay;

	public const int inventorySize = 8;

	// Inventory items and quantities
	public Item[] items = new Item[inventorySize];
	public int[] itemQuantity = new int[inventorySize];

	// Inventory UI, should always match up with the items above
	public GameObject inventoryUI;
	public Image[] itemImages = new Image[inventorySize];
	public Text[] itemImageQuantities = new Text[inventorySize];


	public GameObject toolTip;
	public Text toolTipText;

	bool hovering = false;

	Camera cam;

	public Canvas canvas;

	GameObject test;
	Item tempHover;
	int previousSlot;
	int newSlot; 
	int oldSlotQuantity;
	Sprite switching;
	bool onTopOfSlot = false;

	// Collect the item images when the inventory is loaded
	// Needs to happen with each scene change
	public void Start() {
		initializeInventory ();
		cam = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera> ();
		/*canvas = GameObject.Find ("UI").GetComponent<Canvas>();
		toolTip = GameObject.Find("Tooltip");
		toolTipText = GameObject.Find ("TooltipText").GetComponent<Text>();
		toolTip.SetActive (false);
		*/
	}

	public void Update() {
		if (inventoryUI != null) {
			// Temporary solution to display and hide inventory
			if (Input.GetKeyDown (KeyCode.I)) {
				if (!showInventory) {
					hideInventory ();
				} else {
					displayInventory ();
				}
				showInventory = !showInventory;
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
		inventoryUI.transform.localScale = new Vector3 (1f, 1f, 1f);
	}

	// Inventory needs to be initalized every time a new scene is loaded
	public void initializeInventory() {
		inventoryUI = GameObject.FindGameObjectWithTag ("InventoryUI");
		for (int i = 0; i < inventorySize; i++) {
			Transform item = inventoryUI.transform.Find(string.Format("item{0}", i));
			itemImages[i] = item.transform.Find ("ItemImage").GetComponentInChildren<Image> ();
			itemImageQuantities[i] = item.transform.Find ("ItemQuant").GetComponentInChildren<Text> ();
		}

		Transform UITransform = inventoryUI.transform.root.Find ("ScrapElement");
		Transform scrapTransform = UITransform.Find ("ScrapCount");
		scrapCountDisplay = scrapTransform.GetComponent<Text> ();

		setScrapText ();

		hideInventory();
	}

	public void showToolTip(GameObject slot){
		GameObject tempObj = slot;

		int temp = int.Parse(tempObj.name.Substring (4, 1));

		if(tempObj.transform.GetChild (1).GetComponent<Image>().IsActive() && !hovering){
			toolTip.SetActive (true);

			//Debug.Log ("Mouse is over slot " + tempObj.name.Substring(4,1) + " " + items [temp].itemDescription () );
			toolTipText.text = "<b>" + items[temp].name + "</b>\n";
			toolTipText.text += items [temp].itemDescription ();

			float x = inventoryUI.transform.GetChild (0).position.x - inventoryUI.transform.GetComponent<RectTransform>().sizeDelta.x*1.5f;
			float y = inventoryUI.transform.GetChild (0).position.y - inventoryUI.transform.GetComponent<RectTransform>().sizeDelta.y/2;

			toolTip.transform.position = new Vector2 (x, y);
		}
	}

	public void hideToolTip(){
		toolTip.SetActive (false);
		Debug.Log ("Mouse is not over slot ");
	}

	public void moveItem(GameObject slot){

		if (slot.transform.GetChild (1).GetComponent<Image> ().IsActive () && !hovering) {
			test = Instantiate (Resources.Load ("Prefabs/UI/HoverObject"), Input.mousePosition, Quaternion.identity) as GameObject;
			test.GetComponent<SpriteRenderer> ().sprite = slot.transform.GetChild (1).GetComponent<Image> ().sprite;

			previousSlot = int.Parse (slot.name.Substring (4, 1));
			tempHover = items [int.Parse (slot.name.Substring (4, 1))];
			items [int.Parse (slot.name.Substring (4, 1))]	= null;
			oldSlotQuantity = itemQuantity [int.Parse (slot.name.Substring (4, 1))];
			itemQuantity [int.Parse (slot.name.Substring (4, 1))] = 0;
			hovering = true;
		}
	}

	public void dragging(){
		if (hovering) {
			Vector3 position = cam.ScreenToWorldPoint(Input.mousePosition);
			position.z = 0;

			test.transform.position = position;
		}
	}

	public void stopDragging()
	{
		if (!onTopOfSlot) {
			Debug.Log ("previous slot : " + previousSlot);
			items [previousSlot] = tempHover;
			itemQuantity [previousSlot] = oldSlotQuantity;
			hovering = false;
		}

		if (onTopOfSlot) {
			if (items [newSlot] == null) {
				items [newSlot] = tempHover;
				onTopOfSlot = !onTopOfSlot;
				itemQuantity [newSlot] = oldSlotQuantity;
				hovering = false;
			} else {

				Debug.Log ("Previous slot: " + previousSlot + " new slot: " + newSlot + " oldSlotquantity: " + oldSlotQuantity);
				items [previousSlot]  = items [newSlot];
				items [newSlot] = tempHover;

				itemQuantity [previousSlot] = itemQuantity [newSlot];
				itemQuantity [newSlot] = oldSlotQuantity;

				oldSlotQuantity = 0;
				hovering = false;
			}
		}
		Destroy (test);
	}


	public void checkSlot(GameObject slot)
	{
		if (hovering) {
			newSlot = int.Parse (slot.gameObject.name.Substring (4, 1));
			onTopOfSlot = true;
			Debug.Log ("New slot: " + newSlot);
		}
	}

	public void checkExit(GameObject slot)
	{
		if (hovering) {
			onTopOfSlot = false;
		}
	}

	public void addScrap(int quant) {
		currentScrap += quant;
		setScrapText ();
	}

	private void setScrapText() {
		scrapCountDisplay.text = currentScrap.ToString ();
	}
		


		
}
