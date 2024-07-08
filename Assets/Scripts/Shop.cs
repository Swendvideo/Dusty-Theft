using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] Transform content;
    [SerializeField] AbilityShopUnit abilityShopUnitPrefab;
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
            abilityShopUnits.Add(abilityUnit);
        }
        gameObject.SetActive(true);
    }

    public void BuyPlayerAbility(PlayerAbility playerAbility)
    {
        GameManager.Instance.DataManager.PurchasedPlayerAbilities.Add(playerAbility);
        GameManager.Instance.DataManager.AddMoney(-playerAbility.priceInShop);
    }

    public void OnBackButton()
    {
        gameObject.SetActive(false);
    }
}
