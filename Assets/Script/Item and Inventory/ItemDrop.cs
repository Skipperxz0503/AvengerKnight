using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private int amountOfItems;
    [SerializeField] private ItemData[] possibleDrop;
    private List<ItemData> dropList = new List<ItemData>();


    [SerializeField] private GameObject dropPrefab;


    public virtual void GenerateDrop()
    {
        for (int i = 0; i < possibleDrop.Length; i++) 
        {
            if(Random.Range(0,100) <= possibleDrop[i].dropChance)
                dropList.Add(possibleDrop[i]);
        }
        for (int i = 0; i < amountOfItems; i++)
        {
            if (dropList.Count == 0)
            {
                return;
            }
            ItemData randomItem = dropList[Random.Range(0, dropList.Count - 1)];
            dropList.Remove(randomItem);
            DropItem(randomItem);
        }
    }



    protected void DropItem(ItemData _itemData)
    {
        GameObject newDrop = Instantiate(dropPrefab, transform.position, Quaternion.identity);

        Vector2 randomVelocity = new Vector2(Random.Range(-3,3), Random.Range(10,15));

        newDrop.GetComponent<ItemObject>().SetupItem(_itemData, randomVelocity);
    }
}
 