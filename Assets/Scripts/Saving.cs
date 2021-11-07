using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saving : MonoBehaviour
{
    [Serializable]
    class SaveData
    {        
        public PlayerInfo playerInfo;
        public TaskBoardInformation taskInfo;
    }

    public class SaveSerial: MonoBehaviour
    {        
        PlayerInfo playerInfo;
        TaskBoardInformation taskInfo;

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

                taskInfo = data.taskInfo;
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
                Debug.Log("Data reset complete!");
            }
            else
                Debug.LogError("No save data to delete.");
        }

        
        public PlayerInfo GetPlayerInfo()
        {
            return playerInfo;
        }
        public SaveSerial(PlayerInfo pInf, TaskBoardInformation tbInf)
        {            
            playerInfo = pInf;
            taskInfo = tbInf;
        }
    }

    CsGlobals gl;
    SaveSerial saveSerial;

    void Start()
    {
        gl = FindObjectOfType(typeof(CsGlobals)) as CsGlobals;
        saveSerial = new SaveSerial(gl.playerInfo, gl.boardsInfo[0]);
    }

    public void onClickToSave()
    {        
        saveSerial = new SaveSerial(gl.playerInfo, gl.boardsInfo[0]);        
        saveSerial.SaveGame();
    }

    public void onClickToLoad()
    {
        if (saveSerial.LoadGame())
        {            
            gl.playerInfo = saveSerial.GetPlayerInfo();
        }
    }

    public void onClickToReset()
    {
        SaveSerial.ResetGame();
    }
}
