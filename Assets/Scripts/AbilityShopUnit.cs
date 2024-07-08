using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class AbilityShopUnit : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    [SerializeField] RectangleGraphic icon;
    [SerializeField] TMP_Text priceText;
    bool isPurchasable;
    PlayerAbility playerAbility;
    public event Action<PlayerAbility> OnPurchase;
    public event Action ReloadShop;
    public event Action<string> ShowDescription;

    public void Init(PlayerAbility playerAbility, bool isPurchasable)
    {
        this.icon.Texture = playerAbility.sprite.texture;
        this.isPurchasable = isPurchasable;
        this.playerAbility = playerAbility;
        if(isPurchasable)
        {
            this.priceText.text = playerAbility.priceInShop.ToString();
        }
        else
        {
            if(playerAbility == GameManager.Instance.DataManager.selectedAbility)
            {
                priceText.text = "Экипировано";
            }
            else
            {
                priceText.text = "Куплено";
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(!isPurchasable)
        {
            GameManager.Instance.DataManager.selectedAbility = playerAbility;
            ReloadShop.Invoke();
        }
        if(isPurchasable && GameManager.Instance.DataManager.Money >= playerAbility.priceInShop)
        {
            OnPurchase.Invoke(playerAbility);
            priceText.text = "Куплено";
            isPurchasable = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowDescription.Invoke(playerAbility.abilityDescription);
    }
}
