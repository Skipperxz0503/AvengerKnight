using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Amulet,
    Flask
}

[CreateAssetMenu(fileName = "new item data", menuName = "Data/Equipment")]
public class ItemData_Equipment : ItemData  
{
    public EquipmentType equipmentType;



    public float itemCooldown;
    public ItemEffect[] itemEffects;

    [Header("Major stats")]
    public int strength; // 1 point increase 1 dame and 1% crit damage
    public int agility; // 1 point increase 1% evasion and 1% crit chance
    public int intelligence; // 1 point increase 1 AP and 3 MS
    public int vitality; // 1 point increase 3-5 health 

    [Header("Defensive stats")]
    public int maxHealth;
    public int armor;
    public int evasion;
    public int magicResist;

    [Header("Offensive stats")]
    public int damage;
    public int critChance;
    public int critPower;

    [Header("Magic stats")]
    public int fireDame;
    public int iceDame;
    public int lightningDame;

    [Header("Craft requirements")]
    public List<InventoryItem> craftMaterials;




    public void Effect(Transform _enemyPosition)
    {
        foreach( var item in itemEffects)
        {
            item.ExecuteEffect(_enemyPosition);
        }
    }
    public void AddModifiers()
    {
        Player_Stats playerStats = PlayerManager.instance.player.GetComponent<Player_Stats>();

        playerStats.strength.Addmodifier(strength);
        playerStats.agility.Addmodifier(agility);
        playerStats.intelligence.Addmodifier(intelligence);
        playerStats.vitality.Addmodifier(vitality);

        playerStats.maxHealth.Addmodifier(maxHealth);
        playerStats.armor.Addmodifier(armor);
        playerStats.evasion.Addmodifier(evasion);
        playerStats.magicResist.Addmodifier(magicResist);

        playerStats.damage.Addmodifier(damage);
        playerStats.critChance.Addmodifier(critChance);
        playerStats.critPower.Addmodifier(critPower);

        playerStats.fireDame.Addmodifier(fireDame);
        playerStats.iceDame.Addmodifier(iceDame);
        playerStats.lightningDame.Addmodifier(lightningDame);
    }

    public void RemoveModifiers() 
    {
        Player_Stats playerStats = PlayerManager.instance.player.GetComponent<Player_Stats>();

        playerStats.strength.Removemodifier(strength);
        playerStats.agility.Removemodifier(agility);
        playerStats.intelligence.Removemodifier(intelligence);
        playerStats.vitality.Removemodifier(vitality);

        playerStats.maxHealth.Removemodifier(maxHealth);
        playerStats.armor.Removemodifier(armor);
        playerStats.evasion.Removemodifier(evasion);
        playerStats.magicResist.Removemodifier(magicResist);

        playerStats.damage.Removemodifier(damage);
        playerStats.critChance.Removemodifier(critChance);
        playerStats.critPower.Removemodifier(critPower);

        playerStats.fireDame.Removemodifier(fireDame);
        playerStats.iceDame.Removemodifier(iceDame);
        playerStats.lightningDame.Removemodifier(lightningDame);
    }    
}
