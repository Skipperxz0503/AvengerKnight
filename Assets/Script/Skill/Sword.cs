using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public enum SwordType
{
    Regular,
    Bounce,
    Pierce,
    Spin
}

public class Sword : Skill
{
    public SwordType swordType = SwordType.Regular;
    [Header("Bounce Info")]
    [SerializeField] private UI_SkillTreeSlot bounceUnlockedButton;
    //public bool bounceUnlocked { get; private set; }
    [SerializeField] private int bounceAmount;
    [SerializeField] private float bounceGravity;
    [SerializeField] private float bounceSpeed;

    [Header("Pierce Info")]
    [SerializeField] private UI_SkillTreeSlot pierceUnlockedButton;
    //public bool pierceUnlocked { get; private set; }
    [SerializeField] private int pierceAmount;
    [SerializeField] private float pierceGravity;

    [Header("Spin Info")]
    [SerializeField] private UI_SkillTreeSlot sawUnlockedButton;
    //public bool sawUnlocked { get; private set; }
    [SerializeField] private float dameCooldown;
    [SerializeField] private float maxDistance ;
    [SerializeField] private float spinDuration ;
    [SerializeField] private float spinGravity ;

    [Header("Skill info")]
    [SerializeField] private UI_SkillTreeSlot swordUnlockedButton;
    public bool swordUnlocked { get; private set; }
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchForce;
    [SerializeField] private float swordGravity;
    [SerializeField] private float freezeTimeDuration;
    [SerializeField] private float returnSpeed;

    [SerializeField] private UI_SkillTreeSlot immobilizedUnlockButton;
    public bool immobilizedUnlocked {  get; private set; }
    [SerializeField] private UI_SkillTreeSlot exhaustedUnlockButton;
    public bool exhausedUnlocked {  get; private set; }

    private Vector2 finalDir;

    [Header("Aim dot")]
    [SerializeField] private int dotAmount;
    [SerializeField] private float spaceBeetwen;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotsParent;

    private GameObject[] dots;

    protected override void Start()
    {
        base.Start();
        GenDot();
        SetupGravity();
        swordUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockSword);
        bounceUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockBounce);
        pierceUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockPierce);
        sawUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockSaw);
        immobilizedUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockImmobilized);
        exhaustedUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockExhausted);

    }

    private void SetupGravity()
    {
        if (swordType == SwordType.Bounce)
        {
            swordGravity = bounceGravity;
        }
        else if (swordType == SwordType.Pierce)
        {
            swordGravity = pierceGravity;
        }
        else if (swordType == SwordType.Spin) 
        {
            swordGravity = spinGravity;
        }
    }

    protected override void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse1)) 
        {
            finalDir = new Vector2(AimDir().normalized.x * launchForce.x, AimDir().normalized.y * launchForce.y);
        }
        if (Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < dots.Length; i++) 
            {
                dots[i].transform.position = DotPosition(i * spaceBeetwen);
            }
        }
    }
    public void CreateSword()
    {
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        SwordController newSwordScript = newSword.GetComponent<SwordController>();

        if (swordType == SwordType.Bounce)
        {
            newSwordScript.SetupBounce(true, bounceAmount, bounceSpeed);
        }
        else if (swordType == SwordType.Pierce) 
        { 
            newSwordScript.SetupPierce(pierceAmount);
        }
        else if (swordType == SwordType.Spin)
        {
            newSwordScript.SetupSpin(true, maxDistance, spinDuration, dameCooldown);
        }


        newSwordScript.SetupSword(finalDir, swordGravity, player, freezeTimeDuration, returnSpeed);

        player.AssignNewSword(newSword);

        DotsActive(false);
    }

    private void UnlockImmobilized()
    {
        if(immobilizedUnlockButton.unlocked)
            immobilizedUnlocked = true;
    }
    private void UnlockExhausted()
    {
        if (exhaustedUnlockButton.unlocked)
            exhausedUnlocked = true;
    }
    private void UnlockSword()
    {
        if (swordUnlockedButton.unlocked)
        {
            swordType = SwordType.Regular;
            swordUnlocked = true;
        }
    }

    private void UnlockBounce()
    {
        if(bounceUnlockedButton.unlocked)
            swordType = SwordType.Bounce;
    }
    private void UnlockPierce()
    {
        if (pierceUnlockedButton.unlocked)
            swordType = SwordType.Pierce;
    }
    private void UnlockSaw()
    {
        if (sawUnlockedButton.unlocked)
            swordType = SwordType.Spin;
    }



    #region Aim setup
    public Vector2 AimDir()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - playerPosition;
        return direction; 
    }

    public void DotsActive(bool _isActive)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(_isActive);
        }
    }


    private void GenDot()
    {
        dots = new GameObject[dotAmount];
        for (int i = 0; i < dotAmount; i++)
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParent);
            dots[i].SetActive(false);
        }
    }

    private Vector2 DotPosition(float t)
    {
        Vector2 position = (Vector2)player.transform.position + new Vector2(
            AimDir().normalized.x * launchForce.x,
            AimDir().normalized.y * launchForce.y) * t + .5f * (Physics2D.gravity * swordGravity) * (t *t);
        return position;
    }
    #endregion
}

