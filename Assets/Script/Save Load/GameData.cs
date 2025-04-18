using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int soul;
    public SerializeableDictionary<string, int> inventory;
    public SerializeableDictionary<string, bool> skillTree;
    public List<string> equipmentID;
    public GameData()
    {
        this.soul = 0;
        inventory = new SerializeableDictionary<string, int>();
        skillTree = new SerializeableDictionary<string, bool>();
        equipmentID = new List<string>();
    }
}
