using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDes;
    [SerializeField] private Image itemIcon;
    [SerializeField] private Button craftButton;

    [SerializeField] private Image[] materialImage;

    public void SetupCraftWindow(ItemData_Equipment _data)
    {
        craftButton.onClick.RemoveAllListeners();

        for (int i = 0; i < materialImage.Length; i++)
        {
            materialImage[i].color = Color.clear;
            materialImage[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.clear; 
        }

        for (int i = 0; i < _data.craftMaterials.Count; i++)
        { 
            if(_data.craftMaterials.Count > materialImage.Length)
            {
                Debug.Log("more mats than slots");
            }

            materialImage[i].sprite = _data.craftMaterials[i].data.icon;
            materialImage[i].color = Color.white;

            materialImage[i].GetComponentInChildren<TextMeshProUGUI>().text = _data.craftMaterials[i].stackSize.ToString();
            materialImage[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.white;

        }
        itemIcon.sprite = _data.icon;
        itemName.text = _data.name;
        itemDes.text = _data.GetDescription();
        craftButton.onClick.AddListener(() => Inventory.instance.CanCraft(_data,_data.craftMaterials));
    }


}
