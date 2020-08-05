using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TowerUtils;

public class EnemyDrop : MonoBehaviour
{
    [Header("Money")]
    [SerializeField] int minMoneyDrop;
    [SerializeField] int maxMoneyDrop;

    private InventoryManager inventoryManager;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Health>().death += OnEnemyDeath;
        inventoryManager = FindObjectOfType<InventoryManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnemyDeath(object sender, EventArgs e)
    {
        StartCoroutine(Utils.Timeout(() =>
        {
            var gain = UnityEngine.Random.Range(minMoneyDrop, maxMoneyDrop);
            inventoryManager.ChangeMoney(gain);
            GlobalAudioManager.Instance.Play("MoneyDrop", Vector3.zero);
        }, 1f));    
    }
}
