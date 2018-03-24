using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class FileData : MonoBehaviour {

    public void SaveBestScore(BestScore bestScoreData)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/bestScore.dat");
        bf.Serialize(file, bestScoreData);
        file.Close();
    }

    public BestScore LoadBestScore()
    {
        if (File.Exists(Application.persistentDataPath + "/bestScore.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/bestScore.dat", FileMode.Open);
            BestScore data = (BestScore)bf.Deserialize(file);
            file.Close();
            return data;
        }
        return null;
    }

    public void SaveSetting(Setting settingData)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/setting.dat");
        bf.Serialize(file, settingData);
        file.Close();
    }

    public Setting LoadSetting()
    {
        if (File.Exists(Application.persistentDataPath + "/setting.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/setting.dat", FileMode.Open);
            Setting data = (Setting)bf.Deserialize(file);
            file.Close();
            return data;
        }
        else
        {
            Setting data = new Setting();
            data.soundOn = true;
            data.adsOn = true;
            return data;
        }
        return null;
    }
}
