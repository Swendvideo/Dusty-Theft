using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] Transform content;
    [SerializeField] AbilityShopUnit abilityShopUnitPrefab;
    [SerializeField] TMP_Text abilityDescription;
    List<AbilityShopUnit> abilityShopUnits = new List<AbilityShopUnit>();

    public void UpdateShop()
    {
        
        foreach(AbilityShopUnit abilityShopUnit in abilityShopUnits)
        {
            Destroy(abilityShopUnit.gameObject);
        }
        abilityShopUnits.Clear();
        foreach(PlayerAbility playerAbility in GameManager.Instance.DataManager.playerAbilities)
        {
            var abilityUnit = Instantiate(abilityShopUnitPrefab,content);
            abilityUnit.Init(playerAbility,!GameManager.Instance.DataManager.PurchasedPlayerAbilities.Contains(playerAbility));
            abilityUnit.OnPurchase += BuyPlayerAbility;
            abilityUnit.ReloadShop += UpdateShop;
            abilityUnit.ShowDescription += ShowDescription;
            abilityShopUnits.Add(abilityUnit);
        }
        gameObject.SetActive(true);
    }

    public void BuyPlayerAbility(PlayerAbility playerAbility)
    {
        GameManager.Instance.DataManager.PurchasedPlayerAbilities.Add(playerAbility);
        GameManager.Instance.DataManager.AddMoney(-playerAbility.priceInShop);
    }

    public void ShowDescription(string description)
    {
        abilityDescription.text = description;
    }

    public void OnBackButton()
    {
        gameObject.SetActive(false);
    }
}
