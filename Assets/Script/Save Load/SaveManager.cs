using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SaveManager : MonoBehaviour
{
    [SerializeField] private string fileName;
    [SerializeField] GameObject skillTreeUI;

    private GameData gameData;
    private List<SaveManagerInterface> saveManagers;
    private FileData dataHandler;

    [ContextMenu("Delete save file")]
    private void DeleteSaveData()
    {
        dataHandler = new FileData(Application.persistentDataPath, fileName);
        dataHandler.DeleteData();
    }


    public static SaveManager instance;
    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else 
            instance = this;
    }

    private void Start()
    {
        dataHandler = new FileData(Application.persistentDataPath,fileName);
        saveManagers = FindSaveManagers();
        LoadGame();
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        gameData = dataHandler.Load();


        if(this.gameData == null)
            NewGame();
        foreach (SaveManagerInterface saveManager in saveManagers)
        {
            saveManager.LoadData(gameData);
        }
    
    }
    public void SaveGame() 
    { 
        foreach(SaveManagerInterface saveManager in saveManagers)
        {
            saveManager.SaveData(ref gameData);
        }
        dataHandler.Save(gameData);
    }
    private void OnApplicationQuit()
    {
        SaveGame();
    }
    private List<SaveManagerInterface> FindSaveManagers()
    {
        skillTreeUI.SetActive(true);
        IEnumerable<SaveManagerInterface> saveManagers = FindObjectsOfType<MonoBehaviour>().OfType<SaveManagerInterface>();
        skillTreeUI.SetActive(false);
        return new List<SaveManagerInterface>(saveManagers);
    }
}
