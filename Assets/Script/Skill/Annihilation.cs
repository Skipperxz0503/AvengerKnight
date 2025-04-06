using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Annihilation : Skill
{
    [SerializeField] private int atksAmount;
    [SerializeField] private float cloneCooldown;
    [SerializeField] private float anniDuration;
    [Space]
    [SerializeField] private GameObject annihilationPrefab;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;


    AnnihilationCotroller currentAnnihilation;
    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();
        GameObject newAnni = Instantiate(annihilationPrefab,player.transform.position, Quaternion.identity);

        currentAnnihilation = newAnni.GetComponent<AnnihilationCotroller>();
        currentAnnihilation.SetupAnnihilation(maxSize, growSpeed,shrinkSpeed,atksAmount,cloneCooldown, anniDuration);
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }
    public bool SkillCompleted()
    {
        if (!currentAnnihilation) 
        {
            return false;
        }



        if (currentAnnihilation.playerCanExitState)
        {
            currentAnnihilation = null;
            return true;
        }
        return false;
    }
}
