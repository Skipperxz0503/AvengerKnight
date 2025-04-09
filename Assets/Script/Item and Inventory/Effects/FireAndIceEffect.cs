using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Fire and Ice effect", menuName = "Data/Item effect/FireAndIce Effect")]
public class FireAndIceEffect : ItemEffect
{
    [SerializeField] private GameObject fireAndIcePrefab;
    [SerializeField] private float xVelocity;

    public override void ExecuteEffect(Transform _respawnPosition)
    {
        Player player = PlayerManager.instance.player;

        bool thirdAtk = player.playerAtk.comboCounter == 2;

        if (thirdAtk)
        {

            GameObject newFireAndIce = Instantiate(fireAndIcePrefab, _respawnPosition.position, player.transform.rotation);

            newFireAndIce.GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity * player.facingDir, 0);

            Destroy(newFireAndIce, 3);
        }

    }
}
