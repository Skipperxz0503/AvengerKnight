using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Stats : CharacterStats
{
    private Enemy enemy;
    private ItemDrop myDropSystem;

    [Header("Level details")]
    [SerializeField] private int level = 1;

    [Range(0f,1f)]
    [SerializeField] private float percentageModifier = .5f;


    protected override void Start()
    {
        ApplyLevelModifier();
        base.Start();
        enemy = GetComponent<Enemy>();
        myDropSystem = GetComponent<ItemDrop>();
    }

    private void ApplyLevelModifier()
    {
        Modify(strength);
        Modify(agility);
        Modify(intelligence);
        Modify(vitality);

        Modify(maxHealth);
        Modify(armor);
        Modify(evasion);
        Modify(magicResist);

        Modify(damage);
        Modify(critChance);
        Modify(critPower);

        Modify(fireDame);
        Modify(iceDame);
        Modify(lightningDame);
    }

    private void Modify(Stat _stat)
    {
        for (int i = 1; i < level; i++)
        { 
            float modifier = _stat.GetValue() *percentageModifier;

            _stat.Addmodifier(Mathf.RoundToInt(modifier));

        }
    }


    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
    }
    protected override void Die()
    {
        base.Die();
        enemy.Die();
        myDropSystem.GenerateDrop();
    }
}
