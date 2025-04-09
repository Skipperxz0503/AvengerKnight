using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone : Skill
{
    [Header("Clone info")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [SerializeField] private bool canAtk;
    [SerializeField] private bool createCloneOnDashStart;
    [SerializeField] private bool createCloneOnDashOver;
    [SerializeField] private bool canCreateCloneOnCounterAtk;

    [Header("Clone can Duplicate")]
    [SerializeField] private float chanceToDuplicate;
    [SerializeField] private bool canDuplicateClone;


    [Header("Crystal instead of Clone")]
    public bool crystalInsteadOfclone;

    public void CreateClone(Transform _clonePosition, Vector3 _offset)
    {
        if (crystalInsteadOfclone) 
        {
            SkillManager.instance.crystal.CreateCrystal();         
            return;
        }


        GameObject newClone = Instantiate(clonePrefab);
        newClone.GetComponent<CloneController>().
            SetupClone(_clonePosition, cloneDuration,canAtk, _offset, FindClosestEnemy(newClone.transform),canDuplicateClone, chanceToDuplicate,player);
    }

    public void CreateCloneOnDashStart()
    {
        if(createCloneOnDashStart) 
            CreateClone(player.transform, Vector3.zero);
    }

    public void CreateCloneOnDashOver()
    {
        if (createCloneOnDashOver)
            CreateClone(player.transform, Vector3.zero);
    }
    public void CreateCloneOnCounterAtk(Transform _enemyTransform)
    {
        if (canCreateCloneOnCounterAtk)
            StartCoroutine(CreateCloneWithDelay(_enemyTransform, new Vector3(2 * player.facingDir, 0)));
    }

    private IEnumerator CreateCloneWithDelay(Transform _transform, Vector3 _offset)
    {
        yield return new WaitForSeconds(.4f);
        CreateClone(_transform, _offset);
    }

}
