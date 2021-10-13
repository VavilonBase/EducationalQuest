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
        public float[] playerPosition;        
        public PlayerInfo playerInfo;
    }

    public class SaveSerial: MonoBehaviour
    {
        float[] playerPosition;
        PlayerInfo playerInfo;
        public void SaveGame()
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath
              + "/MySaveData.dat");
            SaveData data = new SaveData();

            data.playerPosition = playerPosition;
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

                data.playerPosition = playerPosition;
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

        public Vector3 GetPlayerPosition()
        {            
            return new Vector3(playerPosition[0], playerPosition[1], playerPosition[2]);
        }
        public PlayerInfo GetPlayerInfo()
        {
            return playerInfo;
        }
        public SaveSerial(Vector3 pPos, PlayerInfo pInf)
        {
            playerPosition = new float[3];
            playerPosition[0] = pPos.x;
            playerPosition[1] = pPos.y;
            playerPosition[2] = pPos.z;
            playerInfo = pInf;
        }
    }

    CsGlobals gl;
    SaveSerial saveSerial;

    void Start()
    {
        gl = FindObjectOfType(typeof(CsGlobals)) as CsGlobals;
        saveSerial = new SaveSerial(gl.player.transform.position, gl.playerInfo);
    }

    public void onClickToSave()
    {        
        saveSerial = new SaveSerial(gl.player.transform.position, gl.playerInfo);        
        saveSerial.SaveGame();
    }

    public void onClickToLoad()
    {
        if (saveSerial.LoadGame())
        {
            gl.player.transform.position = saveSerial.GetPlayerPosition();
            gl.playerInfo = saveSerial.GetPlayerInfo();
        }
    }

    public void onClickToReset()
    {
        SaveSerial.ResetGame();
    }
}
