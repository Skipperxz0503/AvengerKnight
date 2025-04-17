using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public Player player;

    public int money;
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public bool Purchasable(int _price)
    {
        if(_price > money)
            return false;
        money -= _price;
        return true; 
    }

    public int CurrentSoulsAmmount() => money;
}
