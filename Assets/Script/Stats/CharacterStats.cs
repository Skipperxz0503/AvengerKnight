using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.AI;
public enum StatType
{
    strength,
    agility,
    intelligent,
    vitality,
    damage,
    critChance,
    critPower,
    health,
    armor,
    evasion,
    magicResist,
    fireDamage,
    iceDamage,
    lightningDamage
}

public class CharacterStats : MonoBehaviour
{
    private EntityFX fx;



    [Header("Major stats")]
    public Stat strength; // 1 point increase 1 dame and 1% crit damage
    public Stat agility; // 1 point increase 1% evasion and 1% crit chance
    public Stat intelligence; // 1 point increase 1 AP and 3 MS
    public Stat vitality; // 1 point increase 3-5 health 

    [Header("Defensive stats")]
    public Stat maxHealth;
    public Stat armor;
    public Stat evasion;
    public Stat magicResist;

    [Header("Offensive stats")]
    public Stat damage;
    public Stat critChance;
    public Stat critPower;

    [Header("Magic stats")]
    public Stat fireDame;
    public Stat iceDame;
    public Stat lightningDame;

    public bool isIgnited; //dealt damage over time
    public bool isChilled; //reduce 20% of armor
    public bool isShocked; //reduce 20% of accuracy


    [SerializeField] private float ailmentsDuration = 4;
    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;




    private float ignitedDamageCooldown = .3f;
    private float ignitedDamageTimer;
    private int igniteDamage;
    private int shockDamage;
    [SerializeField] private GameObject shockStrikePrefab;



    public int currentHealth;

    public System.Action onHealthChanged;
    public bool isDead {  get; private set; }   


    protected virtual void Start()
    {
        critPower.SetDefaultValue(150);
        currentHealth = GetMaxHealthValue();

        fx = GetComponent<EntityFX>();

    }

    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;


        ignitedDamageTimer -= Time.deltaTime;

        if (ignitedTimer < 0)
            isIgnited = false;

        if (chilledTimer < 0)
            isChilled = false;

        if (shockedTimer < 0)
            isShocked = false;
        if(isIgnited)
            ApplyIgniteDamage();
    }

    public virtual void IncreaseStatBy(int _modifier, float _duration, Stat _statModify)
    {
        StartCoroutine(StatModCoroutine(_modifier, _duration, _statModify));
    }

    private IEnumerator StatModCoroutine(int _modifier, float _duration, Stat _statModify)
    {
        _statModify.Addmodifier(_modifier);
        yield return new WaitForSeconds(_duration);
        _statModify.Removemodifier(_modifier);
    }

    public virtual void DoDamage(CharacterStats _targetStats)
    {
        if (TargetCanAvoid(_targetStats))
        {
            return;
        }

        int totalDamage = damage.GetValue() + strength.GetValue();
        if (CanCrit())
        {
            totalDamage = CalculateCritDamage(totalDamage);
        }
            

        totalDamage = CheckTargerArmor(_targetStats, totalDamage);

        _targetStats.TakeDamage(totalDamage);
        DoMagicDamage(_targetStats);
    }
    #region Magical damage and ailments
    private void ApplyIgniteDamage()
    {
        if (ignitedDamageTimer < 0 )
        {
            DecreaseHealthBy(igniteDamage);

            if (currentHealth < 0 && !isDead)
                Die();

            ignitedDamageTimer = ignitedDamageCooldown;
        }
    }

    public virtual void DoMagicDamage(CharacterStats _targetStats)
    {
        int _fireDamage = fireDame.GetValue();
        int _iceDamage = iceDame.GetValue();
        int _lightningDame = lightningDame.GetValue();


        int totalMagicDame = _fireDamage + _iceDamage + _lightningDame + intelligence.GetValue();
        totalMagicDame = CheckTargetResistance(_targetStats, totalMagicDame);

        _targetStats.TakeDamage(totalMagicDame);

        if (Mathf.Max(_fireDamage, _iceDamage, _lightningDame) <= 0)
        {
            return;
        }

        AttempToApplyAilment(_targetStats, _fireDamage, _iceDamage, _lightningDame);

    }

    private static void AttempToApplyAilment(CharacterStats _targetStats, int _fireDamage, int _iceDamage, int _lightningDame)
    {
        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightningDame;
        bool canApplyChilled = _iceDamage > _fireDamage && _iceDamage > _lightningDame;
        bool canApplyShocked = _lightningDame > _iceDamage && _lightningDame > _fireDamage;

        while (!canApplyIgnite && !canApplyChilled && !canApplyShocked)
        {
            if (Random.value < .33f && _fireDamage > 0)
            {
                canApplyIgnite = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChilled, canApplyShocked);
                return;
            }

            if (Random.value < .33f && _iceDamage > 0)
            {
                canApplyChilled = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChilled, canApplyShocked);
                return;
            }

            if (Random.value < .33f && _lightningDame > 0)
            {
                canApplyShocked = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChilled, canApplyShocked);
                return;
            }

        }

        if (canApplyIgnite)
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f));

        if (canApplyShocked)
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_lightningDame * .1f));
        _targetStats.ApplyAilments(canApplyIgnite, canApplyChilled, canApplyShocked);
    }

    public void ApplyAilments(bool _ignite, bool _chill, bool _shock)
    {
        bool canApplyIgnite = !isIgnited && !isChilled && !isShocked;
        bool canApplyChill = !isIgnited && !isChilled && !isShocked;
        bool canApplyShock = !isIgnited && !isChilled;


        if (_ignite && canApplyIgnite)
        {
            isIgnited = _ignite;
            ignitedTimer = ailmentsDuration;

            fx.IgniteFxFor(ailmentsDuration);
        }

        if (_chill && canApplyChill)
        {
            isChilled = _chill;
            chilledTimer = ailmentsDuration;
            float slowPercentage = .2f;
            GetComponent<Entity>().SlowEntityBy(slowPercentage, ailmentsDuration);
            fx.ChillFxFor(ailmentsDuration);
        }
        
        if (_shock && canApplyShock)
        {
            if (!isShocked)
            {
                ApplyShock(_shock);

            }
            else
            {
                if (GetComponent<Player>() != null)
                    return;
                HitNearestTargetWithShock();
            }


        }
        
    }

    public void ApplyShock(bool _shock)
    {
        if(isShocked)
            return;

        isShocked = _shock;
        shockedTimer = ailmentsDuration;
        fx.ShockFxFor(ailmentsDuration);
    }

    private void HitNearestTargetWithShock()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null && Vector2.Distance(transform.position, hit.transform.position) > 1)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
            if (closestEnemy == null)
                closestEnemy = transform;
        }

        if (closestEnemy != null)
        {
            GameObject newShockStrike = Instantiate(shockStrikePrefab, transform.position, Quaternion.identity);
            newShockStrike.GetComponent<ShockStrikeController>().Setup(shockDamage, closestEnemy.GetComponent<CharacterStats>());
        }
    }

    public void SetupIgniteDamage(int _damage) => igniteDamage = _damage;

    public void SetupShockStrikeDamage(int _damage) => shockDamage = _damage;

    #endregion

    public virtual void TakeDamage(int _damage)
    {
        DecreaseHealthBy(_damage);
        GetComponent<Entity>().DamageImpact();
        fx.StartCoroutine("FlashFX");
        if (currentHealth < 0 && !isDead) 
        {
            Die();
        }
        
    }


    public virtual void IncreaseHealthBy(int _amount)
    {
        currentHealth += _amount;
        if(currentHealth > maxHealth.GetValue())
        {
            currentHealth = maxHealth.GetValue();
        }
        if(onHealthChanged != null) 
            onHealthChanged();
    }

    protected virtual void DecreaseHealthBy(int _damage)
    {
        currentHealth -= _damage;
        if (onHealthChanged != null)
            onHealthChanged();
    }


    protected virtual void Die()
    {
        isDead = true;
    }

    #region Stat calculate
    private int CheckTargerArmor(CharacterStats _targetStats, int totalDamage)
    {
        if (_targetStats.isChilled) 
        {
            totalDamage -= Mathf.RoundToInt((_targetStats.armor.GetValue()));
        }
        else
        {
            totalDamage -= _targetStats.armor.GetValue();
        }


        totalDamage -= _targetStats.armor.GetValue();
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }
    private int CheckTargetResistance(CharacterStats _targetStats, int totalMagicDame)
    {
        totalMagicDame -= _targetStats.magicResist.GetValue() + (_targetStats.intelligence.GetValue() * 3);
        totalMagicDame = Mathf.Clamp(totalMagicDame, 0, int.MaxValue);
        return totalMagicDame;
    }
    private bool TargetCanAvoid(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();
        if (isShocked)
        {
            totalEvasion += 20;
        }
        if (Random.Range(0, 100) < totalEvasion)
        {
            return true;
        }
        return false;
    }

    private bool CanCrit()
    {
        int totalCritChance = critChance.GetValue() + agility.GetValue();

        if(Random.Range(0, 100) <= totalCritChance)
            return true;
        return false;
    }
    private int CalculateCritDamage(int _damage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * .01f;

        float critDamage = _damage * totalCritPower;

        return Mathf.RoundToInt(critDamage);
    }

    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue() + vitality.GetValue() * 5;
    }

    #endregion
    public Stat GetStat(StatType _statType)
    {
        if (_statType == StatType.strength) return strength;
        if (_statType == StatType.agility) return agility;
        if (_statType == StatType.intelligent) return intelligence;
        if (_statType == StatType.vitality) return vitality;
        if (_statType == StatType.damage) return damage;
        if (_statType == StatType.critChance) return critChance;
        if (_statType == StatType.critPower) return critPower;
        if (_statType == StatType.health) return maxHealth;
        if (_statType == StatType.armor) return armor;
        if (_statType == StatType.evasion) return evasion;
        if (_statType == StatType.magicResist) return magicResist;
        if (_statType == StatType.fireDamage) return fireDame;
        if (_statType == StatType.iceDamage) return iceDame;
        if (_statType == StatType.lightningDamage) return lightningDame;

        return null;
    }

}
