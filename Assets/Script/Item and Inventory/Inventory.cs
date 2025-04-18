using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Inventory : MonoBehaviour,SaveManagerInterface
{
    public static Inventory instance;

    public List<ItemData> startingEquipment;

    public List<InventoryItem> equipment;
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary;

    public List<InventoryItem> inventory;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;

    public List<InventoryItem> stash;
    public Dictionary<ItemData, InventoryItem> stashDictionary;


    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform staskSlotParent;
    [SerializeField] private Transform equipmentSlotParent;
    [SerializeField] private Transform statSlotParent;

    private UI_ItemSlot[] inventoryItemSlot;
    private UI_ItemSlot[] stashItemSlot;
    private UI_EquipmentSlot[] equipmentSlot;
    private UI_StatSlot[] statSlot;


    [Header("Item cooldown")]
    private float lastTimeUseFlask;
    private float lastTimeUseArmor;

    public float flaskCooldown {  get; private set; }
    private float armorCooldown;

    [Header("Database")]
    public List<InventoryItem> loadedItem;
    public List<ItemData_Equipment> loadedEquipment;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        inventory = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();

        stash = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();

        equipment = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemData_Equipment, InventoryItem>();

        inventoryItemSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        stashItemSlot = staskSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        equipmentSlot = equipmentSlotParent.GetComponentsInChildren<UI_EquipmentSlot>();
        statSlot = statSlotParent.GetComponentsInChildren<UI_StatSlot>();
        AddStartingItem();

    }

    private void AddStartingItem()
    {
        foreach(ItemData_Equipment item in loadedEquipment)
        {
            EquipItem(item);
        }


        if(loadedItem.Count > 0)
        {
            foreach (InventoryItem item in loadedItem)
            {
                for(int i = 0; i < item.stackSize; i++)
                {
                    AddItem(item.data);
                }
            }
            return;
        }


        for (int i = 0; i < startingEquipment.Count; i++)
        {
            if(startingEquipment[i] != null)
                AddItem(startingEquipment[i]);
        }
    }

    public void EquipItem(ItemData _item)
    {

        ItemData_Equipment newEquipment = _item as ItemData_Equipment;
        InventoryItem newItem = new InventoryItem(newEquipment);

        ItemData_Equipment oldEquipment = null;

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == newEquipment.equipmentType)
            {
                oldEquipment = item.Key;
            }
        }

        if(oldEquipment != null)
        {
            UnequipItem(oldEquipment);
            AddItem(oldEquipment);
        }

        equipment.Add(newItem);
        equipmentDictionary.Add(newEquipment, newItem);
        newEquipment.AddModifiers();


        RemoveItem(_item);
        UpdateUISlot();
    }

    public void UnequipItem(ItemData_Equipment itemToRemove)
    {
        if (equipmentDictionary.TryGetValue(itemToRemove, out InventoryItem value))
        {
            equipment.Remove(value);
            equipmentDictionary.Remove(itemToRemove);
            itemToRemove.RemoveModifiers();
        }
    }

    private void UpdateUISlot()
    {
        for (int i = 0; i < equipmentSlot.Length; i++)
        {
            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
            {
                if (item.Key.equipmentType == equipmentSlot[i].slotType)
                {
                    equipmentSlot[i].UpdateSlot(item.Value);
                }
            }
        }



        for (int i = 0; i < inventoryItemSlot.Length; i++)
        {
            inventoryItemSlot[i].CleanUpSlot();
        }
        for (int i = 0; i < stashItemSlot.Length; i++)
        {
            stashItemSlot[i].CleanUpSlot();
        }



        for (int i = 0; i < inventory.Count; i++)
        {
            inventoryItemSlot[i].UpdateSlot(inventory[i]);
        }
        for (int i = 0; i < stash.Count; i++)
        {
            stashItemSlot[i].UpdateSlot(stash[i]);
        }

        UpdateStatUI();

    }

    public void UpdateStatUI()
    {
        for (int i = 0; i < statSlot.Length; i++)
        {
            statSlot[i].UpdateStatValueUI();
        }
    }

    public void AddItem(ItemData _item)
    {
        if (_item.itemType == ItemType.Equipment && CanAddItem())
        {
            AddToInventory(_item);
        }
        else if(_item.itemType == ItemType.Material)
        {
            AddToStash(_item);
        }

        UpdateUISlot();
    }

    private void AddToStash(ItemData _item)
    {
        if (stashDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            stash.Add(newItem);
            stashDictionary.Add(_item, newItem);
        }
    }

    private void AddToInventory(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            inventory.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }
    }

    public void RemoveItem(ItemData _item) 
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                inventory.Remove(value);
                inventoryDictionary.Remove(_item);
            }
            else
                value.RemoveStack();
        }
        if (stashDictionary.TryGetValue(_item, out InventoryItem stashValue))
        {
            if (stashValue.stackSize <= 1)
            {
                stash.Remove(stashValue);
                stashDictionary.Remove(_item);
            }
            else
                stashValue.RemoveStack();
        }
        UpdateUISlot();
    }
    
    public bool CanAddItem()
    {
        if(inventory.Count >= inventoryItemSlot.Length)
            return false;
        return true;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            ItemData newItem = inventory[inventory.Count - 1].data;
            RemoveItem(newItem);
        }
    }

    public bool CanCraft(ItemData_Equipment _itemToCraft, List<InventoryItem> _requiredMaterials)
    {
        List<InventoryItem> materialToRemove = new List<InventoryItem>();
        for (int i = 0; i < _requiredMaterials.Count; i++) 
        {
            if (stashDictionary.TryGetValue(_requiredMaterials[i].data, out InventoryItem stashValue))
            {
                if (stashValue.stackSize < _requiredMaterials[i].stackSize)
                {
                    return false;
                }
                else
                {
                    materialToRemove.Add(stashValue);
                }
            }
            else
            {
                return false;
            }
        }

        for(int i = 0; i < materialToRemove.Count; i++)
        {
            RemoveItem(materialToRemove[i].data);
        }
        AddItem(_itemToCraft);
        return true;
    }

    public List<InventoryItem> GetEquipmentList() => equipment;
    public List<InventoryItem> GetStashList() => stash;
    public ItemData_Equipment GetEquipment(EquipmentType _type)
    {
        ItemData_Equipment equipedItem = null;

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == _type)
            {
                equipedItem = item.Key;
            }
        }
        return equipedItem; 
    }

    public void UseFlask()
    {
        ItemData_Equipment currentFlask = GetEquipment(EquipmentType.Flask);

        if (currentFlask == null)
            return;

        bool canUseFlask = Time.time > lastTimeUseFlask + flaskCooldown;
        if (canUseFlask)
        {
            flaskCooldown = currentFlask.itemCooldown;
            currentFlask.Effect(null);
            lastTimeUseFlask = Time.time;
        }
        else 
        {
            Debug.Log("Flask is on cooldown");
        }
    }
    public bool CanUseArmor()
    {
        ItemData_Equipment currentArmor = GetEquipment(EquipmentType.Armor);
        if(Time.time > lastTimeUseArmor  + armorCooldown)
        {
            armorCooldown = currentArmor.itemCooldown;
            lastTimeUseArmor = Time.time;
            return true;
        }
        Debug.Log("Item is on cooldown");
        return false;
    }

    public void LoadData(GameData _data)
    {
        foreach(KeyValuePair<string, int> pair in _data.inventory)
        {
            foreach(var item in GetItemDatabase())
            {
                if(item != null && item.itemId == pair.Key)
                {
                    InventoryItem loadItem = new InventoryItem(item);
                    loadItem.stackSize = pair.Value;

                    loadedItem.Add(loadItem);
                }
            }
        }
        foreach (string loadedItemId in _data.equipmentID)
        {
            foreach (var item in GetItemDatabase())
            {
                if (item != null && loadedItemId == item.itemId)
                {
                    loadedEquipment.Add(item as ItemData_Equipment);
                }
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.inventory.Clear();
        _data.equipmentID.Clear();

        foreach(KeyValuePair<ItemData, InventoryItem> pair in inventoryDictionary)
        {
            _data.inventory.Add(pair.Key.itemId, pair.Value.stackSize);
        }
        foreach (KeyValuePair<ItemData, InventoryItem> pair in stashDictionary)
        {
            _data.inventory.Add(pair.Key.itemId, pair.Value.stackSize);
        }
        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> pair in equipmentDictionary)
        {
            _data.equipmentID.Add(pair.Key.itemId);
        }
    }

    private List<ItemData> GetItemDatabase()
    {
        List<ItemData> itemDataBase = new List<ItemData>();
        string[] assetName = AssetDatabase.FindAssets("", new[] { "Assets/Script/Data/Items" });
        foreach (string SOname in assetName)
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(SOname);
            var itemData = AssetDatabase.LoadAssetAtPath<ItemData>(SOpath);
            itemDataBase.Add(itemData);
        }
        return itemDataBase;
    }
}
