using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillTreeSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private int skillCost;
    [SerializeField] private string skillName;
    [SerializeField] private string skillDes;
    [SerializeField] private Color lockedSkillColor;

    public bool unlocked;
    private UI ui;
    private Image skillImg;

    [SerializeField] private UI_SkillTreeSlot[] shouldUnlocked;
    [SerializeField] private UI_SkillTreeSlot[] shouldLocked;



    private void OnValidate()
    {
        gameObject.name = skillName;
    }

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => UnlockSkillSlot());
    }
    private void Start()
    {
        skillImg = GetComponent<Image>();
        skillImg.color = lockedSkillColor;
        ui = GetComponentInParent<UI>();
    }

    public void UnlockSkillSlot()
    {
        if (!PlayerManager.instance.Purchasable(skillCost))
            return;
        for (int i = 0; i < shouldUnlocked.Length; i++)
        {
            if(shouldUnlocked[i].unlocked == false)
                return;
        }
        for (int i = 0; i < shouldLocked.Length; i++)
        {
            if(shouldLocked[i].unlocked == true)
                return;
        }
        unlocked = true;
        skillImg.color = Color.white;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillTooltip.ShowToolTip(skillDes, skillName);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillTooltip.HideToolTip();
    }
}
