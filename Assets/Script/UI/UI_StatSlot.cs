using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_StatSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private UI ui;

    [SerializeField] private string statName;

    [SerializeField] private StatType statType;
    [SerializeField] private TextMeshProUGUI statValueText;
    [SerializeField] private TextMeshProUGUI statNameText;

    [SerializeField] private string statDescription;

    private void OnValidate()
    {
        gameObject.name = "Stat - " + statName;
        if(statNameText  != null )
            statNameText.text = statName;
    }
    void Start()
    {
        UpdateStatValueUI();
        ui = GetComponentInParent<UI>();
    }

    
    public void UpdateStatValueUI()
    {
        Player_Stats playerStats = PlayerManager.instance.player.GetComponent<Player_Stats>();
        if (playerStats != null) 
        {
            statValueText.text = playerStats.GetStat(statType).GetValue().ToString();


            if(statType == StatType.health)
                statValueText.text = playerStats.GetMaxHealthValue().ToString();
            if(statType == StatType.damage)
                statValueText.text = (playerStats.damage.GetValue() + playerStats.strength.GetValue()).ToString();
            if (statType == StatType.critPower)
                statValueText.text = (playerStats.critPower.GetValue() + playerStats.strength.GetValue()).ToString();
            if (statType == StatType.critChance)
                statValueText.text = (playerStats.critChance.GetValue() + playerStats.agility.GetValue()).ToString();
            if (statType == StatType.evasion)
                statValueText.text = (playerStats.evasion.GetValue() + playerStats.agility.GetValue()).ToString();
            if (statType == StatType.magicResist)
                statValueText.text = (playerStats.magicResist.GetValue() + playerStats.intelligence.GetValue() * 3).ToString();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.statsTooltip.ShowStatTooltip(statDescription);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.statsTooltip.HideStatTooltip();
    }
}
