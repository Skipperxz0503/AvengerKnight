using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;
    private bool canRotate = true;
    private bool isReturning;

    private float freezeTimeDuration;
    private float returnSpeed = 13;


    [Header("Pierce info")]
    private float pierceAmount;

    [Header("Bounce info")]
    private float bounceSpeed;
    private bool isBouncing;
    private int bounceAmount;
    private List<Transform> enemyTarget;
    private int targetIndex;

    [Header("Spin info")]
    private float maxDistance;
    private float spinDuration;
    private float spinTimer;
    private bool wasStopped;
    private bool isSpinning;

    private float dameTimer;
    private float dameCooldown;

    private float spinDir;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
    }

    private void DestroyMe()
    {
        Destroy(gameObject);
    }

    public void SetupSword(Vector2 _dir, float _gravityScale, Player _player, float _freezeTimeDuration, float _returnSpeed)
    {
        player = _player;
        freezeTimeDuration = _freezeTimeDuration;
        returnSpeed = _returnSpeed;
        rb.velocity = _dir;
        rb.gravityScale = _gravityScale;
        if (pierceAmount <= 0)
        {
            anim.SetBool("Rotation", true);
        }

        spinDir = Mathf.Clamp(rb.velocity.x, -1, 1);

        Invoke("DestroyMe", 7);

    }

    public void SetupBounce(bool _isBouncing, int _bounceAmount, float _bounceSpeed)
    {
        bounceSpeed = _bounceSpeed;
        isBouncing = _isBouncing;
        bounceAmount = _bounceAmount;

        enemyTarget = new List<Transform>();
    }

    public void SetupPierce(int _pierceAmount)
    {
        pierceAmount = _pierceAmount;
    }

    public void SetupSpin(bool _isSpinning, float _maxDistance, float _spinDuration, float _dameCooldown)
    {
        isSpinning = _isSpinning;
        maxDistance = _maxDistance;
        spinDuration = _spinDuration;
        dameCooldown = _dameCooldown;

    }


    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        //rb.isKinematic = false;
        transform.parent = null;
        isReturning = true;
    }
    private void Update()
    {
        if (canRotate)
        {
            transform.right = rb.velocity;
        }
        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, player.transform.position) < 1)
            {
                player.CatchSword();
            }
        }
        BounceLogic();
        SpinLogic();

    }

    private void SpinLogic()
    {
        if (isSpinning)
        {
            if (Vector2.Distance(player.transform.position, transform.position) > maxDistance && !wasStopped)
            {
                StopWhenSpinning();
            }
            if (wasStopped)
            {
                spinTimer -= Time.deltaTime;

                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDir, transform.position.y), 1.5f * Time.deltaTime);

                if (spinTimer < 0)
                {
                    isReturning = true;
                    isSpinning = false;
                }

                dameTimer -= Time.deltaTime;
                if (dameTimer < 0)
                {
                    dameTimer = dameCooldown;
                    Collider2D[] colider = Physics2D.OverlapCircleAll(transform.position, 1);
                    foreach (var hit in colider)
                    {
                        if (hit.GetComponent<Enemy>() != null)
                        {
                            SwordSkillDame(hit.GetComponent<Enemy>());
                        }
                    }
                }

            }
        }
    }

    private void StopWhenSpinning()
    {
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
    }

    private void BounceLogic()
    {
        if (isBouncing && enemyTarget.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position, bounceSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < .1f)
            {
                SwordSkillDame(enemyTarget[targetIndex].GetComponent<Enemy>());

                targetIndex++;
                bounceAmount--;
                if (bounceAmount <= 0)
                {
                    isBouncing = false;
                    isReturning = true;
                }
                if (targetIndex >= enemyTarget.Count)
                {
                    targetIndex = 0;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (isReturning)
        {
            return;
        }

        if (collision.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            SwordSkillDame(enemy);
        }
        SetupBounceTarget(collision);

        StuckInto(collision);
    }

    private void SwordSkillDame(Enemy enemy)
    {
        player.stats.DoDamage(enemy.GetComponent<CharacterStats>());

        if(player.skill.sword.immobilizedUnlocked)
            enemy.ImmobilizedFor(freezeTimeDuration);

        if(player.skill.sword.exhausedUnlocked)
            enemy.GetComponent<Enemy_Stats>().BeExhaustedFor(2);
        //enemy.StartCoroutine("FreezeTimeFor", freezeTimeDuration);
        ItemData_Equipment amuletEquiped = Inventory.instance.GetEquipment(EquipmentType.Amulet);

        if (amuletEquiped != null)
        {
            amuletEquiped.Effect(enemy.transform);
        }
    }

    private void SetupBounceTarget(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            if (isBouncing && enemyTarget.Count <= 0)
            {
                Collider2D[] colider = Physics2D.OverlapCircleAll(transform.position, 10);
                foreach (var hit in colider)
                {
                    if (hit.GetComponent<Enemy>() != null)
                    {
                        enemyTarget.Add(hit.transform);
                    }
                }
            }
        }
    }

    private void StuckInto(Collider2D collision)
    {
        if (pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
        {
            pierceAmount--;
            return;
        }
        if (isSpinning)
        {
            if (!wasStopped)
            {
                StopWhenSpinning();
            }
            return;
        }

        canRotate = false;
        cd.enabled = false;

        rb.bodyType = RigidbodyType2D.Kinematic;

        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (isBouncing && enemyTarget.Count > 0)
        {
            return;
        }


        anim.SetBool("Rotation", false);
        transform.parent = collision.transform;
    }
}
