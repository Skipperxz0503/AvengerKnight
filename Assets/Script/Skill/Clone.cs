using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone : Skill
{
    [Header("Clone info")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [SerializeField] private bool canAtk;

    public void CreateClone(Transform _clonePosition, Vector3 _offset)
    {
        GameObject newClone = Instantiate(clonePrefab);
        newClone.GetComponent<CloneController>().SetupClone(_clonePosition, cloneDuration,canAtk, _offset);
    }
}
