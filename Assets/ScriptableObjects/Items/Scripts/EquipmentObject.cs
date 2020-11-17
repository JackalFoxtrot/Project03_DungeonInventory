using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Equipment Object", menuName = "Inventory Systems/Items/Equipment")]
public class EquipmentObject : ItemObject
{
    public float atkBonus;
    public float defBound;
    public void Awake()
    {
        type = ItemType.Equipment;
    }
}
