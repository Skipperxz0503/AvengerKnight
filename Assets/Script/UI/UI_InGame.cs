using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    [SerializeField] private Player_Stats playerStats;
    [SerializeField] private Slider slider;
    [SerializeField] private Image dashImg;
    [SerializeField] private Image parryImg;
    [SerializeField] private Image crystalImg;
    //[SerializeField] private Image swordImg;
    [SerializeField] private Image anniImg;
    [SerializeField] private Image potionImg;
    [SerializeField] private TextMeshProUGUI currentSouls;

    private SkillManager skill;
    void Start()
    {
        if (playerStats != null)
            playerStats.onHealthChanged += UpdateHealthUI;

        skill = SkillManager.instance;
    }


    void Update()
    {
        currentSouls.text = "Souls: " + PlayerManager.instance.CurrentSoulsAmmount().ToString("#,#");


        if (Input.GetKeyDown(KeyCode.LeftShift) && skill.dash.dashUnlocked)
            SetImgCooldown(dashImg);
        if(Input.GetKeyDown(KeyCode.F) && skill.parry.parryUnlocked)
            SetImgCooldown(parryImg);
        if(Input.GetKeyDown(KeyCode.C) && skill.crystal.crystalUnlocked)
            SetImgCooldown(crystalImg);
        //if(Input.GetKeyDown(KeyCode.Mouse1))
        //    SetImgCooldown(swordImg);
        if (Input.GetKeyDown(KeyCode.R) && skill.annihilation.anniUnlocked)
            SetImgCooldown(anniImg);
        if(Input.GetKeyDown (KeyCode.Alpha1) && Inventory.instance.GetEquipment(EquipmentType.Flask) != null)
            SetImgCooldown(potionImg);


        CheckCooldown(dashImg,skill.dash.cooldown);
        CheckCooldown(parryImg,skill.parry.cooldown);
        CheckCooldown(crystalImg,skill.crystal.cooldown);
        //CheckCooldown(swordImg,skill.sword.cooldown);
        CheckCooldown(anniImg, skill.annihilation.cooldown);
        CheckCooldown(potionImg, Inventory.instance.flaskCooldown);
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = playerStats.GetMaxHealthValue();
        slider.value = playerStats.currentHealth;
    }
    private void SetImgCooldown(Image _image)
    {
        if(_image.fillAmount <= 0)
            _image.fillAmount = 1;
    }

    private void CheckCooldown(Image _image, float _cooldown)
    {
        if(_image.fillAmount > 0)
            _image.fillAmount -= 1 / _cooldown * Time.deltaTime;
    }
}
