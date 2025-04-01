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
}
