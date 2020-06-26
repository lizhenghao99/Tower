using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Wave[] waves;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Wave wave in waves)
        {
            wave.update();
        }
    }
}

[System.Serializable]
public class Wave
{
    [SerializeField] Enemy[] enemies;
    [SerializeField] float timer;

    public void update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                SpawnEnemies();
            }
        }
    }

    private void SpawnEnemies()
    {
        foreach(Enemy enemy in enemies)
        {
            enemy.Spawn();
        }
    }
}