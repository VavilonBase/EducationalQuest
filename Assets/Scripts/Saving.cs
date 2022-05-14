using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class Saving : MonoBehaviour
{
    [Serializable]
    public class AccountSettingsData
    {
        public string jwt;
    }

    [Serializable]
    class SaveData
    {  
        //Test comment
        public PlayerInfo playerInfo;
    }
       

    public class SaveSerial: MonoBehaviour
    {        
        PlayerInfo playerInfo;
        public PlayerInfo PlayerInfo { get { return playerInfo; } }

        float[] keyPosition;
        public float[] KeyPosition { get { return keyPosition; } }

        public static void SaveAccountSettings(ResponseUserData rud)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath
              + "/accountSettings.dat");
            AccountSettingsData data = new AccountSettingsData();

            data.jwt = rud.jwt;

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

        public void SaveGame()
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath
              + "/MySaveData.dat");
            SaveData data = new SaveData();
            
            data.playerInfo = playerInfo;
            
            bf.Serialize(file, data);
            file.Close();
            Debug.Log("Game data saved!");
        }

        public bool LoadGame()
        {            
            if (File.Exists(Application.persistentDataPath
              + "/MySaveData.dat"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file =
                  File.Open(Application.persistentDataPath
                  + "/MySaveData.dat", FileMode.Open);
                SaveData data = (SaveData)bf.Deserialize(file);
                file.Close();

                playerInfo = data.playerInfo;
                
                Debug.Log("Game data loaded!");
                return true;
            }
            else
            {
                Debug.LogError("There is no save data!");
                return false;
            }       
        }

        public static void ResetGame()
        {
            if (File.Exists(Application.persistentDataPath
              + "/MySaveData.dat"))
            {
                Debug.Log("Starting data reset...");
                File.Delete(Application.persistentDataPath
                  + "/MySaveData.dat");                
                Debug.Log("data reset complete!");
            }
            else
                Debug.LogError("No save data to delete.");
        }

        //конструктор
        public SaveSerial(PlayerInfo pInf, Vector3 keyPos)
        {            
            playerInfo = pInf;            
            keyPosition = new float[3];
            keyPosition[0] = keyPos.x;            
            keyPosition[1] = keyPos.y;
            keyPosition[2] = keyPos.z;
        }
    }

    CsGlobals gl;
    SaveSerial saveSerial;

    void Start()
    {
        gl = FindObjectOfType(typeof(CsGlobals)) as CsGlobals;        
    }

    public void Update()
    {
        
    }

    public void onClickToSave()
    {
        saveSerial = new SaveSerial(gl.playerInfo, gl.key.transform.position);
        saveSerial.SaveGame();
    }

    public void onClickToLoad()
    {
        if (saveSerial.LoadGame())
        {
            gl.playerInfo = saveSerial.PlayerInfo;
            //gl.boardsInfo = saveSerial.BoardsInfo;
            Vector3 newKeyPos = new Vector3(saveSerial.KeyPosition[0], saveSerial.KeyPosition[1], saveSerial.KeyPosition[2]);            
            gl.key.transform.position = newKeyPos;
            gl.RELOAD = true;            
        }
    }

    public void onClickToReset()
    {
        SaveSerial.ResetGame();
    }
}
