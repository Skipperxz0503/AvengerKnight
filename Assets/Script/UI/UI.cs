using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject skillTreeUI;
    [SerializeField] private GameObject craftUI;
    [SerializeField] private GameObject optionUI;
    [SerializeField] private GameObject ingameUI;

    public UI_ItemTooltip itemTooltip;
    public UI_StatTooltip statsTooltip;
    public UI_CraftWindow craftWindow;  
    public UI_SkillTooltip skillTooltip;


    private void Awake()
    {
        SwitchTo(skillTreeUI);
    }
    void Start()
    {
        SwitchTo(ingameUI);

        itemTooltip.gameObject.SetActive(false);
        statsTooltip.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            SwitchWithKeyTo(characterUI);
        }
    }

    public void SwitchTo(GameObject _menu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        if(_menu != null) 
            _menu.SetActive(true);
    }
    public void SwitchWithKeyTo(GameObject _menu)
    {
        if(_menu != null && _menu.activeSelf)
        {
            _menu.SetActive(false);
            CheckIngameUI();
            return;
        }
        SwitchTo(_menu);
    }
    private void CheckIngameUI()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).gameObject.activeSelf)
                return;
        }
        SwitchTo(ingameUI);
    }
}
