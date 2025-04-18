using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using Unity.VisualScripting;

public class FileData 
{
    private string dataDirPath = "";
    private string dataFileName = "";

    public FileData(string _dataDirPath, string _dataFileName)
    {
        dataDirPath = _dataDirPath;
        dataFileName = _dataFileName;
    }

    public void Save(GameData _data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            string storeData = JsonUtility.ToJson(_data, true);
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine(storeData);
                }
            }
        }
        catch (Exception e) 
        {
            Debug.LogError(fullPath + e);
        }
    }

    public GameData Load() 
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadData = null;
        if (File.Exists(fullPath)) 
        {
            try
            {
                string loadedData = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        loadedData = reader.ReadToEnd();
                    }
                }
                loadData = JsonUtility.FromJson<GameData>(loadedData);
            }
            catch (Exception e)
            {
                Debug.LogError(fullPath + e);
            }
        }
        return loadData;
    }
    public void DeleteData()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        if(File.Exists(fullPath)) 
            File.Delete(fullPath);
    }
}
