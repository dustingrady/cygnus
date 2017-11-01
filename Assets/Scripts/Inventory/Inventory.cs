using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {


    public GameObject[] items = new GameObject[inventorySize];

    public const int inventorySize = 20;


    public void addItem(GameObject item)
    {
        for(int i = 0; i < items.Length; i++)
        {
            if(items[i] == null)
            {
                items[i] = item;
                return;
            }
        }
        Debug.Log(item.name + " added");
    }

    public void removeItem(GameObject item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == item)
            {
                items[i] = null;
                return;
            }
        }
        //Debug.Log(item.name + " removed");
    }

    void OnGUI()
    {
        if (!isEmpty())
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] != null)
                {
                    GUI.contentColor = Color.red;
                    GUI.skin.label.fontSize = 25;
                    GUI.Label(new Rect(10, i * 20, 200, 50), items[i].name);
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
}
