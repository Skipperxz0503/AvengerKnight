using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clone : Skill
{
    [Header("Clone info")]
    [SerializeField] private float atkMultiple;
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;

    [Header("Clone Atk")]
    [SerializeField] private UI_SkillTreeSlot cloneAtkUnlockedButton;
    [SerializeField] private float cloneClone;
    [SerializeField] private bool canAtk;

    [SerializeField] private UI_SkillTreeSlot mirrageFurryUnlockedButton;
    [SerializeField] private float mirrageFurries;
    public bool applyOnHit { get; private set; }




    [Header("Clone can Duplicate")]
    [SerializeField] private UI_SkillTreeSlot multipleUnlockedButton;
    [SerializeField] private float kagebushin;
    [SerializeField] private float chanceToDuplicate;
    [SerializeField] private bool canDuplicateClone;


    [Header("Crystal instead of Clone")]
    [SerializeField] private UI_SkillTreeSlot crystalUnlockedButton;
    public bool crystalInsteadOfclone;

    protected override void Start()
    {
        base.Start();
        cloneAtkUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockCloneAtk);
        mirrageFurryUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockMirrageFurry);
        multipleUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockKagebushin);
        crystalUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockTimeBomb);
    }
    private void UnlockCloneAtk()
    {
        if (cloneAtkUnlockedButton.unlocked)
        {
            canAtk =  true;
            atkMultiple = cloneClone;
        }
    }
    private void UnlockMirrageFurry()
    {
        if (mirrageFurryUnlockedButton.unlocked)
        {
            applyOnHit = true;
            atkMultiple = mirrageFurries;

        }
    }
    private void UnlockKagebushin()
    {
        if (multipleUnlockedButton.unlocked)
        {
            canDuplicateClone = true;
            atkMultiple = kagebushin;

        }
    }
    private void UnlockTimeBomb()
    {
        if (crystalUnlockedButton.unlocked)
        {
            crystalInsteadOfclone = true;
        }
    }




    public void CreateClone(Transform _clonePosition, Vector3 _offset)
    {
        if (crystalInsteadOfclone) 
        {
            SkillManager.instance.crystal.CreateCrystal();         
            return;
        }


        GameObject newClone = Instantiate(clonePrefab);
        newClone.GetComponent<CloneController>().
            SetupClone(_clonePosition, cloneDuration,canAtk, _offset, FindClosestEnemy(newClone.transform),canDuplicateClone, chanceToDuplicate,player,atkMultiple);
    }


    public void CreateCloneOnCounterAtk(Transform _enemyTransform)
    {
            StartCoroutine(CreateCloneWithDelay(_enemyTransform, new Vector3(2 * player.facingDir, 0)));
    }

    private IEnumerator CreateCloneWithDelay(Transform _transform, Vector3 _offset)
    {
        yield return new WaitForSeconds(.4f);
        CreateClone(_transform, _offset);
    }

}
