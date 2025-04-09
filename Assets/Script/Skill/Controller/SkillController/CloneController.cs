using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneController : MonoBehaviour
{
    private Player player;
    private SpriteRenderer sr;
    private Animator anim;
    [SerializeField] private float colorFadingSpeed;
    private float cloneTimer;

    [SerializeField] private Transform atkCheck;
    [SerializeField] private float atkCheckRadius = .8f;
    private Transform closestEnemy;
    private bool canDuplicateClone;
    private int facingDir = 1;

    private float chanceToDuplicate;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        cloneTimer -= Time.deltaTime;
        if (cloneTimer < 0) 
        {
            sr.color = new Color(1,1,1,sr.color.a - (Time.deltaTime * colorFadingSpeed));
            if(sr.color.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
    public void SetupClone(Transform _newTransform, float _cloneDuration, bool _canAtk,Vector3 _offset, Transform _closestEnemy, bool _canDuplicate, float _chanceToDuplicate, Player _player)
    {
        if (_canAtk) 
        {
            anim.SetInteger("AtkNumber", Random.Range(1, 3));
        }
        player = _player;
        transform.position = _newTransform.position + _offset;
        cloneTimer = _cloneDuration;


        closestEnemy = _closestEnemy;
        canDuplicateClone = _canDuplicate;
        chanceToDuplicate = _chanceToDuplicate;
        FaceClosestTarget();
    }
    private void AnimTrigger()
    {
        cloneTimer = -.1f;
    }
    private void AtkTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(atkCheck.position, atkCheckRadius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                player.stats.DoDamage(hit.GetComponent<CharacterStats>());


                
                if (canDuplicateClone)
                {
                    if(Random.Range(0,100) < chanceToDuplicate)
                    {
                        SkillManager.instance.clone.CreateClone(hit.transform, new Vector3(.5f * facingDir, 0));
                    }
                }
            }
        }
    }
    private void FaceClosestTarget()
    {
        
        if (closestEnemy != null) 
        {
            if (transform.position.x > closestEnemy.position.x) 
            {
                facingDir = -1;
                transform.Rotate(0, 180, 0);
            }
        }
    }
}
