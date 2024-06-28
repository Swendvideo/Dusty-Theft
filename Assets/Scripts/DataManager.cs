using UnityEngine;

public class DataManager : MonoBehaviour
{

    public void SaveData(PlayerData pd)
    {
        string data = JsonUtility.ToJson(pd);
        Debug.Log(data);
        PlayerPrefs.SetString("PlayerData", data);
    }

    public PlayerData LoadData()
    {
        string data = PlayerPrefs.GetString("PlayerData", "");
        if(data != "")
        {
            return JsonUtility.FromJson<PlayerData>(data);
        }
        else
        {
            return new PlayerData();
        }
    }

    void Awake()
    {
        PlayerData data = new PlayerData();
        var data1 = data;
        data1.money = 1000;
        data = data1;
        SaveData(data);

        PlayerData datanew = LoadData();
        Debug.Log(datanew.money);
    }

    public struct PlayerData
    {
        public int money;
    }



}
