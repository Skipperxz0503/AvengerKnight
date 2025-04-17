using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Stats : CharacterStats
{
    private Player  player;


    protected override void Start()
    {
        base.Start();
        player = GetComponent<Player>();   
    }
    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);

    }
    protected override void Die()
    {
        base.Die();
        player.Die();
        GetComponent<PlayerItemDrop>()?.GenerateDrop();
    }

    protected override void DecreaseHealthBy(int _damage)
    {
        base.DecreaseHealthBy(_damage);

        ItemData_Equipment currentArmor = Inventory.instance.GetEquipment(EquipmentType.Armor);
        if (currentArmor != null) 
            currentArmor.Effect(player.transform);
    }

    public override void OnAtkEvade()
    {
        player.skill.dodge.MirrageOnDodge();
    }
    public void CloneDamage(CharacterStats _targetStats, float _multipler)
    {
        if (TargetCanAvoid(_targetStats))
        {
            return;
        }

        int totalDamage = damage.GetValue() + strength.GetValue();

        if (_multipler > 0)
        {
            totalDamage = Mathf.RoundToInt(totalDamage * _multipler);
        }

        if (CanCrit())
        {
            totalDamage = CalculateCritDamage(totalDamage);
        }


        totalDamage = CheckTargerArmor(_targetStats, totalDamage);

        _targetStats.TakeDamage(totalDamage);
        DoMagicDamage(_targetStats);
    }
}
