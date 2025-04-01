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
}
