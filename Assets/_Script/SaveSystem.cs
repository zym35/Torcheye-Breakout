using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem : MonoBehaviour
{
    // Makes it a singleton / single instance
    public static SaveSystem Instance;
    private string _filePath;
    private List<PlayerData> tempData;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        _filePath = Application.persistentDataPath + "/high_score.ted";
    }

    public void Save(PlayerData saveData)
    {
        Debug.Log("Saving data...");
        var dataStream = new FileStream(_filePath, FileMode.Create);
        var converter = new BinaryFormatter();

        List<PlayerData> data;
        if (tempData != null)
        {
            data = tempData;
            for (int i = 0; i < tempData.Count; i++)
            {
                if (tempData[i].ExactEqual(saveData))
                {
                    tempData[i] = saveData;
                    converter.Serialize(dataStream, data);
                    dataStream.Close();
                    return;
                }
            }
        }
        else
        {
            data = new List<PlayerData>();
        }

        data.Add(saveData);
        data.Sort();
        if (data.Count == 4)
            data.RemoveAt(data.Count - 1);

        converter.Serialize(dataStream, data);
        dataStream.Close();
    }

    public List<PlayerData> Load()
    {
        Debug.Log("Loading data...");
        if (File.Exists(_filePath))
        {
            var dataStream = new FileStream(_filePath, FileMode.Open);
            var converter = new BinaryFormatter();

            if (dataStream.Length == 0)
            {
                dataStream.Close();
                return null;
            }

            var saveData = converter.Deserialize(dataStream) as List<PlayerData>;

            dataStream.Close();
            tempData = saveData;
            tempData.Sort();
            return saveData;
        }

        Debug.LogError("Save file not found in " + _filePath);
        return null;
    }
}