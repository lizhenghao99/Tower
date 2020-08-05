using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] int loseMoneyPunishment;

    private int currMoney;

    // Start is called before the first frame update
    void Start()
    {
        var data = SaveSystem.Load();
        currMoney = data.money;
    }

    // Update is called once per frame
    void Update()
    {
        moneyText.text = currMoney.ToString();
    }

    public bool ChangeMoney(int value)
    {
        var newValue = currMoney + value;
        if (newValue < 0)
        {
            moneyText.GetComponent<RectTransform>().DOShakeAnchorPos(0.3f)
                .SetEase(Ease.OutQuint).SetUpdate(true);
            return false;
        }
        else
        {
            currMoney = newValue;
            return true;
        }
    }

    public void SaveInventory()
    {
        SaveSystem.SaveMoney(currMoney);
    }

    public void LoseMoneyPunish()
    {
        currMoney =
            Mathf.Clamp(currMoney - loseMoneyPunishment, 0, 99999999);
    }

    public void Win()
    {
        SaveInventory();
    }

    public void Lose()
    {
        LoseMoneyPunish();
        SaveInventory();
    }
}
