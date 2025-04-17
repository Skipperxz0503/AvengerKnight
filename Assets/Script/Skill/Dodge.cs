using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dodge : Skill
{
    [SerializeField] private UI_SkillTreeSlot unlockDodgeButton;
    public bool dodgeUnlocked;

    [SerializeField] private UI_SkillTreeSlot unlockMirrageDodgeButton;
    public bool mirrageDodgeUnlocked;

    protected override void Start()
    {
        base.Start();
        unlockDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockDodge);
        unlockMirrageDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockMirrageDodge);
    }

    private void UnlockDodge()
    {
        if (unlockDodgeButton.unlocked)
        {
            player.stats.evasion.Addmodifier(15);
            Inventory.instance.UpdateStatUI();
            dodgeUnlocked = true;
        }
    }
    private void UnlockMirrageDodge()
    {
        if (unlockMirrageDodgeButton.unlocked)
            mirrageDodgeUnlocked = true;
    }
    public void MirrageOnDodge()
    {
        if (mirrageDodgeUnlocked)
            SkillManager.instance.clone.CreateClone(player.transform, Vector3.zero);
    }
}
