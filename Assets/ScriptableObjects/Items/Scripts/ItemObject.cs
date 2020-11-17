using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Consumable,
    Equipment,
    Key,
    Default
}
public abstract class ItemObject : ScriptableObject
{
    public GameObject prefab;
    public ItemType type;
    [TextArea(5, 10)]
    public string itemname;
    [TextArea(5, 10)]
    public string specificType;
    [TextArea(15,20)]
    public string description;

    public bool identified;
    public bool favorited;

    public int value;
    public int rarity;
}
