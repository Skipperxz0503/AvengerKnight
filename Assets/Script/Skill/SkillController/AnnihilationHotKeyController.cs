using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AnnihilationHotKeyController : MonoBehaviour
{
    private SpriteRenderer sr;
    private KeyCode myHotKey;
    private TextMeshProUGUI myText;

    private Transform myEnemy;
    private AnnihilationCotroller annihilation;
    


    public void SetupHotKey(KeyCode _myHotKey, Transform _myEnemy, AnnihilationCotroller _myAnnihilation)
    {
        sr = GetComponent<SpriteRenderer>();
        myText = GetComponentInChildren<TextMeshProUGUI>();
        myEnemy = _myEnemy;
        annihilation = _myAnnihilation;

        myHotKey = _myHotKey;
        myText.text = myHotKey.ToString();
        
    }
    private void Update()
    {
        if (Input.GetKeyDown(myHotKey))
        {
            annihilation.AddEnemyToList(myEnemy);
            myText.color = Color.clear;
            sr.color = Color.clear;
        }
    }

}
