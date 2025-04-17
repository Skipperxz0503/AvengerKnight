using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Parry : Skill
{
    [SerializeField] private UI_SkillTreeSlot parryUnlockedButton;
    public bool parryUnlocked {  get; private set; }

    [SerializeField] private UI_SkillTreeSlot healingParryUnlockedButton;
    public bool healingParryUnlocked { get; private set;}

    [SerializeField] private UI_SkillTreeSlot mirrageParryUnlockedButton;
    public bool mirrageParryUnlocked { get; private set; }

    public override void UseSkill()
    {
        base.UseSkill();
        if (healingParryUnlocked)
            player.stats.IncreaseHealthBy(5);
    }
    protected override void Start()
    {
        base.Start();
        parryUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockParry);
        healingParryUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockHealingParry);
        mirrageParryUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockMirrageParry);

    }
    private void UnlockParry()
    {
        if (parryUnlockedButton.unlocked)
            parryUnlocked = true;
    }
    private void UnlockHealingParry()
    {
        if (healingParryUnlockedButton.unlocked)
            healingParryUnlocked = true;
    }
    private void UnlockMirrageParry()
    {
        if (mirrageParryUnlockedButton.unlocked)
            mirrageParryUnlocked = true;
    }
    public void MirrageOnParry(Transform _spwanTransform)
    {
        if (mirrageParryUnlocked)
            SkillManager.instance.clone.CreateCloneOnCounterAtk(_spwanTransform);
    }
}
