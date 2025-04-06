using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnnihilationCotroller : MonoBehaviour
{
    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> keyCodeList;


    private float maxSize;
    private float growSpeed;
    private bool canGrow = true;
    private bool canShrink;
    private float shrinkSpeed;

    private bool canCreateHotKey = true;
    private bool canCloneAtk;
    private int atksAmount = 4;
    private float cloneAtkCooldown = .3f;
    private float cloneAtkTimer;
    private float anniTimer;


    public List<Transform> target = new List<Transform>();
    private List<GameObject> createdHotKey = new List<GameObject>();


    public bool playerCanExitState {  get; private set; }


    public void SetupAnnihilation(float _maxSize, float _growSpeed, float _shrinkSpeed, int _atkAmount, float _cloneAtkCooldown, float _anniTimer)
    {
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        atksAmount = _atkAmount;
        cloneAtkCooldown = _cloneAtkCooldown;
        anniTimer = _anniTimer;
    }



    private void Update()
    {


        cloneAtkTimer -= Time.deltaTime;
        anniTimer -= Time.deltaTime;

        if (anniTimer < 0) 
        {
            anniTimer = Mathf.Infinity;
            if (target.Count > 0) 
            { 
                ReleaseCloneAtk();
            }
            else 
            { 
                FinishAnnihilation();
            }
        }


        if (Input.GetKeyDown(KeyCode.R))
        {
            ReleaseCloneAtk();
        }

        CloneAtkLogic();

        if (canGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }
        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);
            if (transform.localScale.x < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void ReleaseCloneAtk()
    {
        DestroyHotKey();
        if (target.Count <= 0)
        {
            return;
        }
        canCloneAtk = true;
        canCreateHotKey = false;
        PlayerManager.instance.player.MakeTransparent(true);
    }

    private void CloneAtkLogic()
    {
        if (cloneAtkTimer < 0 && canCloneAtk && atksAmount >0)
        {
            cloneAtkTimer = cloneAtkCooldown;

            int randomIndex = Random.Range(0, target.Count);
            float xOffset;
            if (Random.Range(0, 100) > 50)
            {
                xOffset = 2;
            }
            else
            {
                xOffset = -2;
            }

            SkillManager.instance.clone.CreateClone(target[randomIndex], new Vector3(xOffset, 0));
            atksAmount--;
            if (atksAmount <= 0)
            {
                Invoke("FinishAnnihilation", .5f);
            }
        }
    }

    private void FinishAnnihilation()
    {
        DestroyHotKey();
        playerCanExitState = true;
        canShrink = true;
        canCloneAtk = false;
    }

    private void DestroyHotKey()
    {
        if (createdHotKey.Count <= 0)
        {
            return;
        }
        for(int i = 0; i < createdHotKey.Count; i++)
        {
            Destroy(createdHotKey[i]);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(true);
            CreateHotKey(collision);

        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(false);
        }
    }



    private void CreateHotKey(Collider2D collision)
    {
        if(keyCodeList.Count <= 0)
        {
            return;
        }
        if (!canCreateHotKey)
        {
            return;
        }

        GameObject newHotKey = Instantiate(hotKeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);
        createdHotKey.Add(newHotKey);

        KeyCode chosenKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
        keyCodeList.Remove(chosenKey);

        AnnihilationHotKeyController newHotKeyScript = newHotKey.GetComponent<AnnihilationHotKeyController>();
        newHotKeyScript.SetupHotKey(chosenKey, collision.transform, this);
    }

    public void AddEnemyToList(Transform _enemyTransform) => target.Add(_enemyTransform);
}
