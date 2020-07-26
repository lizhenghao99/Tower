using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Singleton<EnemySpawner>
{
    [SerializeField] WaveStage[] waveStages;
    private WaveStage currStage = null;

    // Start is called before the first frame update
    void Start()
    {
        LevelController.Instance.StartCombat += OnStartCombat;
    }

    // Update is called once per frame
    void Update()
    {
        if (currStage != null)
        {
            if (currStage.stageClear)
            {
                currStage = null;
                LevelController.Instance.ClearStage();
            }
            else
            {
                currStage.update();
            }
        }
    }

    public void OnStartCombat(object sender, EventArgs e)
    {
        currStage = waveStages[LevelController.Instance.currStage.index];
    }

    public void CheckAllEnemiesDead()
    {
        foreach (Wave w in currStage.waves)
        {
            foreach (Enemy e in w.enemies)
            {
                if (!e.GetComponent<Health>().isDead)
                {
                    return;
                }
            }
        }

        currStage.stageClear = true;
    }
}

[System.Serializable]
public class Wave
{
    [SerializeField] public Enemy[] enemies;
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

[System.Serializable]
public class WaveStage
{
    [SerializeField] public Wave[] waves;
    public bool stageClear = false;

    public void update()
    {
        foreach (Wave wave in waves)
        {
            wave.update();
        }
    }
}