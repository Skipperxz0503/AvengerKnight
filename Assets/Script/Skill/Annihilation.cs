using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Annihilation : Skill
{
    [SerializeField] private UI_SkillTreeSlot anniUnlockedButton;
    public bool anniUnlocked {  get; private set; }
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

    private void UnlockAnnihilation()
    {
        if (anniUnlockedButton.unlocked)
            anniUnlocked = true;
    }
    protected override void Start()
    {
        base.Start();
        anniUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockAnnihilation);
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
    public float GetAnniRadius()
    {
        return maxSize / 2;
    }

}
