using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayInventory : MonoBehaviour
{
    private RectTransform[] catTransformArray;
    private int currentCategory = 1;
    private int currentItem = 0;

    public InventoryObject inventory;
    public GameObject templateConsole;
    public GameObject allCategories;
    public GameObject selectedCategory;
    public GameObject itemInfoPanel;
    public GameObject cursor;    

    private int numbOfCats = 12;

    public int X_START;
    public int Y_START;
    public int Y_END;
    public int Y_Cursor_Stop;
    public int Y_SPACE_BETWEEN_ITEMS;
    public int NUMBER_OF_ROWS;

    public int maxLines = 5;

    public float distanceToMove;

    public enum ItemType
    {
        Default,
        All,
        Favorite,
        IDdConsumable,
        UnkConsumable,
        OneHWeapons,
        TwoHWeapons,
        Ranged,
        Armor,
        Trinkets,
        Academy,
        Research
    }

    Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();
    public List<InventorySlot> sortedItems = new List<InventorySlot>();
    List<string> consoleOutput = new List<string>();
    List<GameObject> consoleText = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        //CreateDisplay();
        catTransformArray = allCategories.GetComponentsInChildren<RectTransform>();
        currentCategory = 1;
        distanceToMove = Mathf.Abs(catTransformArray[6].position.x - catTransformArray[5].position.x);
    }

    //Check Inventory runs when the Category is changed detected in Level Controller script
    public void CheckInventory()
    {
        Debug.Log("Check Inventory Ran||Current Cat: " + currentCategory);
        ClearDisplay();
        if (currentCategory == 1)
        {
            SortInventory();
        }
        else if(currentCategory == 2)
        {
            SortInventoryByFavorite();
        }
        else if(currentCategory == 4)
        {
            SortInventoryByUnkConsum();
        }
        else
        {
            SortInventoryByType();
        }
        
        UpdateDisplay();
        UpdateYEnd();
    }

    //Currently unused because I only run inventory checks when input is recieved
    //So CreateDisplay was running after and causing errors because I am not updating
    //every frame. Leaving in for reference if needed.
    /*public void CreateDisplay()
    {
        Debug.Log("Create Display Ran||Current Cat: " + currentCategory);
        //Displays All Items no ordering
        for (int i=0; i<inventory.Container.Count; i++)
        {
            var obj = Instantiate(inventory.Container[i].item.prefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.Container[i].amount.ToString("n0") + "x";
            obj.GetComponentsInChildren<TextMeshProUGUI>()[1].text  = inventory.Container[i].item.itemname;
            itemsDisplayed.Add(inventory.Container[i], obj);
        }
    }*/

    public void UpdateDisplay()
    {
        Debug.Log("Update Display Ran||Current Cat: " + currentCategory);
        for (int i = 0; i < sortedItems.Count; i++)
        {
            if (itemsDisplayed.ContainsKey(sortedItems[i]))
            {
                itemsDisplayed[sortedItems[i]].GetComponentInChildren<TextMeshProUGUI>().text = sortedItems[i].amount.ToString("n0") + "x";
            }
            else
            {
                var obj = Instantiate(sortedItems[i].item.prefab, Vector3.zero, Quaternion.identity, transform);
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                obj.GetComponentInChildren<TextMeshProUGUI>().text = sortedItems[i].amount.ToString("n0") + "x";
                obj.GetComponentsInChildren<TextMeshProUGUI>()[1].text = sortedItems[i].item.itemname;                
                itemsDisplayed.Add(sortedItems[i], obj);
            }
        }
    }

    public void ClearDisplay()
    {
        foreach(KeyValuePair<InventorySlot,GameObject> entry in itemsDisplayed)
        {
            Destroy(entry.Value);
        }
        itemsDisplayed.Clear();
    }

    public void UpdateCategory(int direction)
    {
        RectTransform selectedCatTransform = selectedCategory.GetComponent<RectTransform>();
        RectTransform cursorTransform = cursor.GetComponent<RectTransform>();

        if (direction < 0)
        {
            currentCategory++;
            //Debug.Log("Going Left: "+ ((-direction * distanceToMove) - 2));
        }
        else
        {
            //Debug.Log("Going Right: " + 2*(-direction * (distanceToMove - 2)));
        }

        if(currentCategory < 1)
        {
            currentCategory = 1;
        }
        if(currentCategory > numbOfCats)
        {
            currentCategory = numbOfCats;
        }

        //Debug.Log("Current Category: " + currentCategory);

        if (currentCategory > 1 && currentCategory < numbOfCats)
        {
            selectedCatTransform.localPosition = new Vector3(selectedCatTransform.localPosition.x + (direction * distanceToMove), 0.0f, 0.0f);
            selectedCategory.GetComponentInChildren<TextMeshProUGUI>().text = catTransformArray[currentCategory].name;
        }

        //Selector Going Left
        if (currentCategory < numbOfCats-1 && direction < 1)
        {
            catTransformArray[currentCategory].localPosition = new Vector3(selectedCatTransform.localPosition.x + (-direction * distanceToMove) - 2, 0.0f, 0.0f);
            ResetCursor();
        }
        
        //Selector Going right
        if(currentCategory > 1 && direction > 0)
        {   
            if(currentCategory == numbOfCats)
            {
                currentCategory = numbOfCats - 2;
                selectedCategory.GetComponentInChildren<TextMeshProUGUI>().text = catTransformArray[currentCategory].name;
                selectedCatTransform.localPosition = new Vector3(selectedCatTransform.localPosition.x + (direction * distanceToMove), 0.0f, 0.0f);
            }
            else
            {
                catTransformArray[currentCategory].localPosition = new Vector3(selectedCatTransform.localPosition.x + 2 * (-direction * (distanceToMove - 1)), 0.0f, 0.0f);
                currentCategory--;
                selectedCategory.GetComponentInChildren<TextMeshProUGUI>().text = catTransformArray[currentCategory].name;
            }
            ResetCursor();
        }
    }

    public void UpdateCursor(int direction)
    {
        RectTransform cursorTransform = cursor.GetComponent<RectTransform>();
        float deltaY = cursorTransform.localPosition.y + (direction * Y_SPACE_BETWEEN_ITEMS);
        if(deltaY <= Y_START && deltaY >= Y_Cursor_Stop)
        {
            cursorTransform.localPosition = new Vector3(0.0f, deltaY, 0.0f);
            currentItem += -direction;
            UpdateItemInfo();
        }
    }

    public void ResetCursor()
    {
        RectTransform cursorTransform = cursor.GetComponent<RectTransform>();
        cursorTransform.localPosition = new Vector3(0.0f, Y_START, 0.0f);
        currentItem = 0;
    }

    public void UpdateYEnd()
    {
        Debug.Log("ItemDisplayed Count: "+ itemsDisplayed.Count);
        Y_Cursor_Stop = Y_START + (-Y_SPACE_BETWEEN_ITEMS * (itemsDisplayed.Count-1));
        Debug.Log("Cursor Stop Y: " + Y_Cursor_Stop);
        Y_Cursor_Stop = Mathf.Max(Y_Cursor_Stop, Y_END);
    }

    public void UpdateItemInfo()
    {
        if(sortedItems.Count != 0)
        {
            itemInfoPanel.SetActive(true);
            itemInfoPanel.GetComponentsInChildren<TextMeshProUGUI>()[0].text = sortedItems[currentItem].item.itemname;
            itemInfoPanel.GetComponentsInChildren<TextMeshProUGUI>()[1].text = sortedItems[currentItem].item.description;
            itemInfoPanel.GetComponentsInChildren<TextMeshProUGUI>()[2].text = sortedItems[currentItem].item.value.ToString();
        }
        else
        {
            itemInfoPanel.SetActive(false);
        }
        
    }

    public void ToggleNewItems()
    {
        for(int i=0; i<inventory.Container.Count; i++)
        {
            inventory.Container[i].item.newToInventory = false;
        }
    }

    public void SortInventoryByType()
    {
        Debug.Log("Sort Inventory By Type Ran||Current Cat: " + currentCategory);
        bool inserted = false;
        sortedItems.Clear();
        for (int i = 0; i < inventory.Container.Count; i++)
        {
            inserted = false;
            if (inventory.Container[i].item.specificType == currentCategory)
            {
                if (sortedItems.Count == 0)
                {
                    sortedItems.Add(new InventorySlot(inventory.Container[i].item, inventory.Container[i].amount));
                }
                else
                {
                    for (int j = 0; j < sortedItems.Count; j++)
                    {
                        if (sortedItems[j].item.rarity <= inventory.Container[i].item.rarity && !inserted)
                        {
                            sortedItems.Insert(j, new InventorySlot(inventory.Container[i].item, inventory.Container[i].amount));
                            inserted = true;
                            j++;
                        }
                    }
                    if (!inserted)
                    {
                        sortedItems.Add(new InventorySlot(inventory.Container[i].item, inventory.Container[i].amount));
                    }
                }
            }
        }
    }

    public void SortInventory()
    {
        Debug.Log("Sort Inventory Ran||Current Cat: "+ currentCategory);
        bool inserted = false;
        sortedItems.Clear();
        for (int i = 0; i < inventory.Container.Count; i++)
        {
            inserted = false;
            if (sortedItems.Count == 0)
            {
                sortedItems.Add(new InventorySlot(inventory.Container[i].item, inventory.Container[i].amount));
            }
            else
            {
                for(int j=0; j<sortedItems.Count; j++)
                {
                    if(sortedItems[j].item.rarity <= inventory.Container[i].item.rarity && !inserted)
                    {
                        sortedItems.Insert(j, new InventorySlot(inventory.Container[i].item, inventory.Container[i].amount));
                        inserted = true;
                        j++;
                    }
                }
                if(!inserted)
                {
                    sortedItems.Add(new InventorySlot(inventory.Container[i].item, inventory.Container[i].amount));
                }
            }
        }
    }

    public void SortInventoryByFavorite()
    {
        sortedItems.Clear();
    }

    public void SortInventoryByIDConsum()
    {
        sortedItems.Clear();
    }
    public void SortInventoryByUnkConsum()
    {
        sortedItems.Clear();
    }

    public void AddPickUp(string output)
    {
        if(consoleOutput.Count == 0)
        {
            consoleOutput.Add(output);
        }
        else
        {
            consoleOutput.Insert(0, output);
        }
        if(consoleOutput.Count >= maxLines)
        {
            consoleOutput.RemoveAt(consoleOutput.Count - 1);
        }
    }

    public void printToConsole()
    {
        for(int i=0; i<consoleOutput.Count; i++)
        {
            RectTransform consoleTranform = consoleText[i].GetComponent<RectTransform>();
            consoleText.Add(Instantiate(templateConsole));
            consoleTranform.localPosition = new Vector3(consoleTranform.localPosition.x, consoleTranform.localPosition.y + (i*30), consoleTranform.localPosition.z);
        }
    }

    public Vector3 GetPosition(int i)
    {
        return new Vector3(X_START, Y_START + (-Y_SPACE_BETWEEN_ITEMS * i), 0.0f);
    }
}
