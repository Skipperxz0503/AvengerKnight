using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : Skill
{
    [SerializeField] private float crystalDuration;
    [SerializeField] private GameObject crystalPrefab;
    private GameObject currentCrystal;

    [Header("Crystal mirage")]
    [SerializeField] private bool cloneInsteadOfCrystal;



    [Header("Moving crystal")]
    [SerializeField] private bool canExplode;


    [Header("Moving crystal")]
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float moverSpeed;

    [Header("Crystal stack")]
    [SerializeField] private bool canUseStack; 
    [SerializeField] private int stackAmount;
    [SerializeField] private float multiStackCooldown;
    [SerializeField] private float useTimeWindow;
    [SerializeField] private List<GameObject> crystalLeft = new List<GameObject>();




    public override void UseSkill()
    {
        base.UseSkill();
        if (CanUseMultiCrystal())
        {
            return;
        }

        if(currentCrystal == null)
        {
            CreateCrystal();
        }
        else
        {
            if (canMoveToEnemy) 
            {
                return;
            }

            Vector2 playerPosition = player.transform.position;
            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPosition;

            if (cloneInsteadOfCrystal) 
            {
                SkillManager.instance.clone.CreateClone(currentCrystal.transform, Vector3.zero);
                Destroy(currentCrystal);
            }
            else
            {
                currentCrystal.GetComponent<CrystalController>()?.FinishCrystal();
            }
        }
    }

    public void CreateCrystal()
    {
        currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
        CrystalController currentCrystalScript = currentCrystal.GetComponent<CrystalController>();

        currentCrystalScript.SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moverSpeed, FindClosestEnemy(currentCrystal.transform),player);
    }
    public void CurrenCrystalRandomTarget() => currentCrystal.GetComponent<CrystalController>().RandomEnemy();


    private bool CanUseMultiCrystal()
    {
        if (canUseStack)
        {
            if (crystalLeft.Count > 0) 
            {
                if (crystalLeft.Count == stackAmount)
                    Invoke("ResetAbility", useTimeWindow);

                cooldown = 0;
                GameObject crystalToSpawn = crystalLeft[crystalLeft.Count - 1];
                GameObject newCrystal = Instantiate(crystalToSpawn, player.transform.position, Quaternion.identity);
                crystalLeft.Remove(crystalToSpawn);
                newCrystal.GetComponent<CrystalController>().SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moverSpeed, FindClosestEnemy(newCrystal.transform), player);

                if (crystalLeft.Count <= 0) 
                {
                    cooldown = multiStackCooldown;
                    RefilCrystal();
                }

            return true;
            }

        }
        return false;
    }

    private void RefilCrystal()
    {
        int addAmount = stackAmount - crystalLeft.Count;
        for (int i = 0; i < addAmount; i++) 
        {
            crystalLeft.Add(crystalPrefab);
        }
    }

    private void ResetAbility()
    {
        if (cooldownTimer > 0)
            return;

        cooldownTimer = multiStackCooldown;
        RefilCrystal();
    }
}
