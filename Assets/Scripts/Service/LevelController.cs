using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;
using UnityEngine.AI;
using ProjectTower;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ProjectTower
{
    public class LevelController : MonoBehaviour
    {
        [Header("Sequence")]
        [SerializeField] Stage[] stages;
        [SerializeField] Collider[] bounds;
        [SerializeField] Transform introTarget;
        [SerializeField] float introZoom;
        [SerializeField] Transform[] cinemaTargets;
        [Header("Players")]
        [SerializeField] Health baseTower;
        [SerializeField] PlayerController[] players;
        [Header("Materials")]
        [SerializeField] Material[] cardMaterials;
        [Header("Sun")]
        [SerializeField] Light sunLight;
        [Header("Ending")]
        [SerializeField] LevelEnding levelEnding;
        [SerializeField] GameObject earlyEnding;
        [Header("UI")]
        [SerializeField] CanvasGroup[] groupsToHide;

        public event EventHandler StartCombat;
        public event EventHandler EndCombat;
        public event EventHandler StageClear;

        private ProCamera2DNumericBoundaries numericBoundaries;
        private ProCamera2DCinematics cinematics;

        private List<float> playerOffsets;

        private int alivePlayerCount;

        public Stage currStage { get; private set; } = null;

        private void Awake()
        {
            Time.timeScale = 1f;

            foreach (Material m in cardMaterials)
            {
                m.SetFloat("_Saturation", 1f);
                m.SetFloat("_OutlineStrength", 0f);
            }

            numericBoundaries = Camera.main.GetComponent<ProCamera2DNumericBoundaries>();
            cinematics = Camera.main.GetComponent<ProCamera2DCinematics>();
            var globalAudioManager = GlobalAudioManager.Instance;
        }

        // Start is called before the first frame update
        void Start()
        {
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Minion"),
                                         LayerMask.NameToLayer("Minion"),
                                         true);
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Minion"),
                                         LayerMask.NameToLayer("Player"),
                                         true);
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("BaseTower"),
                                         LayerMask.NameToLayer("Player"),
                                         true);

            foreach (CanvasGroup g in groupsToHide)
            {
                g.alpha = 0f;
            }

            playerOffsets = new List<float>();
            foreach (PlayerController p in players)
            {
                var distance = p.GetComponent<NavMeshAgent>().speed * 5f;
                playerOffsets.Add(distance);
            }

            baseTower.Death += OnTowerDeath;
            foreach (PlayerController p in players)
            {
                p.GetComponent<PlayerHealth>().Death += OnPlayerDeath;
                p.GetComponent<PlayerHealth>().Revive += OnPlayerRevive;
            }
            alivePlayerCount = players.Length;
            StartNextStage();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnStartCombat()
        {
            foreach (CanvasGroup g in groupsToHide)
            {
                g.DOFade(1f, 0.3f).SetEase(Ease.OutQuint);
            }

            Minion[] minions = FindObjectsOfType<Minion>();
            foreach (Minion m in minions)
            {
                m.gameObject.SetActive(false);
            }
            StartCombat?.Invoke(gameObject, EventArgs.Empty);
        }

        public void OnEndCombat()
        {
            foreach (CanvasGroup g in groupsToHide)
            {
                g.DOFade(0f, 0.2f).SetEase(Ease.OutQuint);
            }

            EndCombat?.Invoke(gameObject, EventArgs.Empty);
        }

        public void OnStageClear()
        {
            StageClear?.Invoke(gameObject, EventArgs.Empty);
        }

        public void ClearStage()
        {
            StartCoroutine(ProjectTower.Utils.Timeout(() => StartNextStage()
                , 5f));
            OnStageClear();
        }

        public void StartNextStage()
        {
            Stage nextStage;
            if (currStage)
            {
                bounds[currStage.index].gameObject.SetActive(false);
                cinematics.RemoveCinematicTarget(cinemaTargets[currStage.index]);

                OnEndCombat();

                if (currStage.index + 1 >= stages.Length)
                {
                    Win();
                    return;
                }
                else
                {
                    nextStage = stages[currStage.index + 1];
                }
            }
            else
            {
                nextStage = stages[0];
            }
            currStage = nextStage;

            sunLight.DOIntensity(currStage.sunLightIntensity, 0.3f)
                .SetEase(Ease.InQuad);


            // cinematics
            if (currStage.index == 1)
            {
                cinematics.RemoveCinematicTarget(introTarget);
            }
            cinematics.AddCinematicTarget(
                cinemaTargets[currStage.index],
                3f, 1.5f, 1f, EaseType.EaseInOut);
            if (currStage.index == 0)
            {
                cinematics.AddCinematicTarget(
                    introTarget,
                    1f, 3f, introZoom, EaseType.EaseInOut);
            }
            cinematics.Play();

            // camera bounds
            numericBoundaries.TopBoundary = currStage.top;
            numericBoundaries.BottomBoundary = currStage.bottom;
            numericBoundaries.LeftBoundary = currStage.left;
            numericBoundaries.RightBoundary = currStage.right;
        }

        public void OnCinematicTargetReached(int index)
        {
            if (index != 1)
            {
                // player positions
                baseTower.transform.position = currStage.basePosition;
                for (int i = 0; i < players.Length; i++)
                {
                    players[i].transform.position = currStage.charPostions[i] -
                        new Vector3(playerOffsets[i], 0, 0);
                    players[i].SetPlayerDestination(currStage.charPostions[i]);
                }
            }
        }

        public void OnCinematicFinished()
        {
            OnStartCombat();
        }

        public void OnTowerDeath(object sender, EventArgs e)
        {
            Invoke("Lose", 5f);
        }

        public void Win()
        {
            levelEnding.Win();
        }

        public void Lose()
        {
            levelEnding.Lose();
        }

        public void OnPlayerDeath(object sender, EventArgs e)
        {
            alivePlayerCount -= 1;
            if (alivePlayerCount <= 0)
            {
                Invoke("EarlyLose", 5f);
            }
        }

        public void OnPlayerRevive(object sender, EventArgs e)
        {
            alivePlayerCount += 1;
        }

        public void EarlyLose()
        {
            if (alivePlayerCount <= 0)
            {
                earlyEnding.SetActive(true);
                earlyEnding.GetComponent<CanvasGroup>().DOFade(1f, 0.3f)
                    .SetEase(Ease.OutQuint);
            }
        }

        public void CancelEarlyLose()
        {
            earlyEnding.GetComponent<CanvasGroup>().DOFade(0f, 0.3f)
                    .SetEase(Ease.OutQuint)
                    .OnComplete(() => earlyEnding.SetActive(false));
        }
    }
}