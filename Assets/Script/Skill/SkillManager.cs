using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;
    public Dash dash {  get; private set; }
    public Clone clone { get; private set; }
    public Sword sword { get; private set; }
    public Annihilation annihilation { get; private set; }
    public Crystal crystal { get; private set; }
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    private void Start()
    {
        dash = GetComponent<Dash>(); 
        clone = GetComponent<Clone>();
        sword = GetComponent<Sword>();
        annihilation = GetComponent<Annihilation>();
        crystal = GetComponent<Crystal>();
    }
}
