using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayInventory : MonoBehaviour
{
    private RectTransform[] catTransformArray;
    private int currentCategory;

    public InventoryObject inventory;
    public GameObject allCategories;
    public GameObject selectedCategory;

    private int numbOfCats = 12;

    public int X_START;
    public int Y_START;
    public int Y_SPACE_BETWEEN_ITEMS;
    public int NUMBER_OF_ROWS;

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

    // Start is called before the first frame update
    void Start()
    {
        CreateDisplay();
        catTransformArray = allCategories.GetComponentsInChildren<RectTransform>();
        currentCategory = 1;
        distanceToMove = Mathf.Abs(catTransformArray[6].position.x - catTransformArray[5].position.x);
    }

    //Check Inventory runs when the Category is changed detected in Level Controller script
    public void CheckInventory()
    {
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
    }
    public void CreateDisplay()
    {
        //Displays All Items no ordering
        for(int i=0; i<inventory.Container.Count; i++)
        {
            var obj = Instantiate(inventory.Container[i].item.prefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.Container[i].amount.ToString("n0") + "x";
            obj.GetComponentsInChildren<TextMeshProUGUI>()[1].text  = inventory.Container[i].item.itemname;
            itemsDisplayed.Add(inventory.Container[i], obj);
        }
    }

    public void UpdateDisplay()
    {
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
        
        if(direction < 0)
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
        }
    }

    public void SortInventoryByType()
    {
        sortedItems.Clear();
        for(int i=0; i<inventory.Container.Count; i++)
        { 
            if(inventory.Container[i].item.specificType == currentCategory)
            {
                sortedItems.Add(new InventorySlot(inventory.Container[i].item, inventory.Container[i].amount));
            }
        }
    }

    public void SortInventory()
    {
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

    public Vector3 GetPosition(int i)
    {
        return new Vector3(X_START, Y_START + (-Y_SPACE_BETWEEN_ITEMS * i), 0.0f);
    }
}
