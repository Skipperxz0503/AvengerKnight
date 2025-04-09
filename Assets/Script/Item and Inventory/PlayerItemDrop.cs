using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemDrop : ItemDrop
{
    [Header("Player's drop")]
    [SerializeField] private float chanceToLooseItems;
    [SerializeField] private float chanceToLooseMaterials;
    public override void GenerateDrop()
    {
        Inventory inventory = Inventory.instance;
        List<InventoryItem> currentStash = inventory.GetStashList();
        List<InventoryItem> currentEquipment = inventory.GetEquipmentList();
        List<InventoryItem> itemToUnequip = new List<InventoryItem>();
        List<InventoryItem> materialToUnequip = new List<InventoryItem>();

        foreach (InventoryItem item in currentEquipment) 
        {
            if (Random.Range(0, 100) <= chanceToLooseItems) 
            {
                DropItem(item.data);
                itemToUnequip.Add(item);
            }
        }
        for (int i = 0; i < itemToUnequip.Count; i++)
        {
            inventory.UnequipItem(itemToUnequip[i].data as ItemData_Equipment);
        }


        foreach (InventoryItem item in currentStash)
        {
            if (Random.Range(0, 100) <= chanceToLooseMaterials)
            {
                DropItem(item.data);
                materialToUnequip.Add(item);
            }
        }
        for (int i = 0; i < materialToUnequip.Count; i++)
        {
            inventory.RemoveItem(materialToUnequip[i].data);
        }
    }
}
