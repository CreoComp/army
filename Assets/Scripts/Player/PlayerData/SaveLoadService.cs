using System;
using UnityEngine;

public class SaveLoadService
{
    private static SaveLoadService instance;
    public static SaveLoadService Instance
    {
        get
        {
            if (instance == null)
                instance = new SaveLoadService();

            return instance;
        }
    }


    public PlayerData PlayerData;
    public Action ChangeCharacteristic;

    public void Save()
    {
        string characters = JsonUtility.ToJson(PlayerData);
        PlayerPrefs.SetString("charactersCharacteristics", characters);

        ChangeCharacteristic?.Invoke();
    }

    public void Load()
    {
        if(!PlayerPrefs.HasKey("charactersCharacteristics"))
        {
            PlayerData = new PlayerData();
            return;
        }

        string characters = PlayerPrefs.GetString("charactersCharacteristics");
        PlayerData = JsonUtility.FromJson<PlayerData>(characters);
    }
}
