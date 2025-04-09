using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimTrigger : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();
    private void AnimTrigger()
    {
        player.AnimTrigger();
    }
    private void AtkTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.atkCheck.position, player.atkCheckRadius);
        foreach(var hit in colliders)
        {
            if(hit.GetComponent<Enemy>() != null)
            {
                Enemy_Stats _target = hit.GetComponent<Enemy_Stats>();
                if(_target != null) 
                    player.stats.DoDamage(_target);

                ItemData_Equipment weaponData = Inventory.instance.GetEquipment(EquipmentType.Weapon);

                if (weaponData != null) 
                {
                    weaponData.Effect(_target.transform);
                }

            }
        }
    }

    private void WeaponEffect()
    {
        
    }
    private void ThrowSword()
    {
        SkillManager.instance.sword.CreateSword();
    }
}
