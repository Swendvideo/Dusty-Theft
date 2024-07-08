using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public int Money
    {
        get;
        private set;
    }

    public int Difficulty
    {
        get;
        private set;
    }
    public int TreasureCostSumPerDifficulty;
    public List<EnemyType> EnemyTypes;
    public List<PlayerAbility> playerAbilities;
    public List<Treasure> treasures;
    public PlayerAbility selectedAbility;
    public List<PlayerAbility> PurchasedPlayerAbilities;

    public void AddMoney(int money)
    {
        Money += money;
        GameManager.Instance.UIManager.UpdateMoneyIndicator();
        SaveData();
    }

    public void SaveData()
    {
        string data = JsonUtility.ToJson(new PlayerData(PurchasedPlayerAbilities,Money));
        Debug.Log(data);
        PlayerPrefs.SetString("Data", data);
    }

    public PlayerData LoadData()
    {
        string data = PlayerPrefs.GetString("Data", "");
        if(data != "")
        {
            return JsonUtility.FromJson<PlayerData>(data);
        }
        else
        {
            return new PlayerData();
        }
    }
    private void Start()
    {
        PlayerData playerData = LoadData();
        if(playerData.PurchasedPlayerAbilities == null)
        {
            PurchasedPlayerAbilities = new List<PlayerAbility>();
        }
        else
        {
            PurchasedPlayerAbilities = playerData.PurchasedPlayerAbilities;
        }
        Money = playerData.money;
        GameManager.Instance.UIManager.UpdateMoneyIndicator();
    }
    public void ChangeDifficulty(int difficulty)
    {
        Difficulty = difficulty;
    }

    public int GetEnemyCoinBasedOnDifficulty()
    {
        int enemyCoin = (int)math.round(0.5f*Difficulty +0.1f);
        return enemyCoin;
    }

    public List<Treasure> GetTreasuresBasedOnDifficulty(float costMultiplier)
    {
        float maxCostSum = Difficulty * TreasureCostSumPerDifficulty *costMultiplier;
        List<Treasure> suitableTreasures = new List<Treasure>();
        while(maxCostSum > 0)
        {
            var randomCost = UnityEngine.Random.Range(1, maxCostSum);
            var treasure = treasures.Where(t => t.cost <= randomCost).OrderByDescending(t => t.cost).First();
            maxCostSum -= treasure.cost;
            suitableTreasures.Add(treasure);
        }
        return suitableTreasures;
    }

    [Serializable]
    public struct PlayerData
    {
        public int money;
        public List<PlayerAbility> PurchasedPlayerAbilities;
        public PlayerData(List<PlayerAbility> ppa,int m)
        {
            PurchasedPlayerAbilities = ppa;
            money = m;
        }
    }
}
