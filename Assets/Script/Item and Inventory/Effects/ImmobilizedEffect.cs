using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Immobilized", menuName = "Data/Item effect/Immobilized")]
public class ImmobilizedEffect : ItemEffect
{
    [SerializeField] private float duration;

    public override void ExecuteEffect(Transform _transform)
    {
        Player_Stats playerStats = PlayerManager.instance.player.GetComponent<Player_Stats>();
        if(playerStats.currentHealth > playerStats.GetMaxHealthValue() * 0.1f)
            return;

        if(!Inventory.instance.CanUseArmor())
            return; 

        Collider2D[] colliders = Physics2D.OverlapCircleAll(_transform.position, 2);

        foreach (var hit in colliders)
        {
            hit.GetComponent<Enemy>()?.ImmobilizedFor(duration);

        }
    }
}
