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
                hit.GetComponent<Enemy>().Damage();
            }
        }
    }
    private void ThrowSword()
    {
        SkillManager.instance.sword.CreateSword();
    }
}
