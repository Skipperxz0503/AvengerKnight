using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAnimTrigger : MonoBehaviour
{
    private Enemy_Skeleton enemy  => GetComponentInParent<Enemy_Skeleton>();
    private void AnimTrigger()
    {
        enemy.AnimFinishTrigger();
    }
    private void AtkTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.atkCheck.position, enemy.atkCheckRadius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                Player_Stats target = hit.GetComponent<Player_Stats>();
                enemy.stats.DoDamage(target);
            }
        }
    }
    private void OpenCounterWindow() => enemy.OpenCounterAtkWindow();
    private void CloseCounterWindow() => enemy.CloseCounterAtkWindow();

}
