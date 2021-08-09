using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectTower
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] WaveStage[] waveStages;
        private WaveStage currStage = null;
        private LevelController levelController;

        // Start is called before the first frame update
        void Start()
        {
            levelController = FindObjectOfType<LevelController>();
            levelController.StartCombat += OnStartCombat;
        }

        // Update is called once per frame
        void Update()
        {
            if (currStage != null)
            {
                if (currStage.stageClear)
                {
                    currStage = null;
                    levelController.ClearStage();
                }
                else
                {
                    currStage.LogicUpdate();
                }
            }
        }

        public void OnStartCombat(object sender, EventArgs e)
        {
            currStage = waveStages[levelController.currStage.index];
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

        public void LogicUpdate()
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
            foreach (Enemy enemy in enemies)
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

        public void LogicUpdate()
        {
            foreach (Wave wave in waves)
            {
                wave.LogicUpdate();
            }
        }
    }
}