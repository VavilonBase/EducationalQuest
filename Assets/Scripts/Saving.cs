using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class Saving : MonoBehaviour
{
    [Serializable]
    public class AccountSettingsData
    {
        public string jwt;
        public bool hideInfo;
    }

    public static void SaveAccountSettings(PlayerInfo playerInfo)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath
          + "/accountSettings.dat");
        AccountSettingsData data = new AccountSettingsData();

        data.jwt = playerInfo.responseUserData.jwt;
        data.hideInfo = playerInfo.needToHideInfo;

        bf.Serialize(file, data);
        file.Close();
        Debug.Log("Account settings saved!");
    }

    public static AccountSettingsData LoadAccountSettings()
    {
        if (File.Exists(Application.persistentDataPath
          + "/accountSettings.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file =
              File.Open(Application.persistentDataPath
              + "/accountSettings.dat", FileMode.Open);
            AccountSettingsData data = (AccountSettingsData)bf.Deserialize(file);
            file.Close();

            Debug.Log("Account settings loaded!");
            return data;
        }
        else
        {
            Debug.LogError("There is no save account settings data!");
            return null;
        }
    }

    public static void DeleteAccountSettings()
    {
        if (File.Exists(Application.persistentDataPath
          + "/accountSettings.dat"))
        {
            Debug.Log("Starting data reset...");
            File.Delete(Application.persistentDataPath
              + "/accountSettings.dat");
            Debug.Log("data reset complete!");
        }
        else
            Debug.LogError("No save data to delete.");
    }
}
