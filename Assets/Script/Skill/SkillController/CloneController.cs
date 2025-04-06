using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneController : MonoBehaviour
{
    private SpriteRenderer sr;
    private Animator anim;
    [SerializeField] private float colorFadingSpeed;
    private float cloneTimer;

    [SerializeField] private Transform atkCheck;
    [SerializeField] private float atkCheckRadius = .8f;
    private Transform closestEnemy;
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
    public void SetupClone(Transform _newTransform, float _cloneDuration, bool _canAtk,Vector3 _offset)
    {
        if (_canAtk) 
        {
            anim.SetInteger("AtkNumber", Random.Range(1, 3));
        }
        transform.position = _newTransform.position + _offset;
        cloneTimer = _cloneDuration;
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
                hit.GetComponent<Enemy>().Damage();
            }
        }
    }
    private void FaceClosestTarget()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);
        float closestDistance = Mathf.Infinity;
        foreach (var hit in colliders) 
        {
            if (hit.GetComponent<Enemy>() != null) 
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);
                if(distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
        }
        if (closestEnemy != null) 
        {
            if (transform.position.x > closestEnemy.position.x) 
            {
                transform.Rotate(0, 180, 0);
            }
        }
    }
}
