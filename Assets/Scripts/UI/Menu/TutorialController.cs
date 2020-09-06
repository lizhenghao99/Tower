using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;
using TowerUtils;

public class TutorialController : MonoBehaviour
{
    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] BaseTowerHealth tower;
    [SerializeField] GameObject healVfx;
    [Space]
    [SerializeField] EnemyAttack tutorialEnemy;
    [SerializeField] EnemyAttack tutorialBoss;
    [Space]
    [SerializeField] GameObject inspectMenu;
    [Space]
    [SerializeField] DiscardButton discardButton;
    [Space]
    [SerializeField] GameObject control0;
    [Space]
    [SerializeField] GameObject movement0;
    [SerializeField] GameObject movement1;
    [SerializeField] GameObject movement2;
    [SerializeField] GameObject movement3;
    [Space]
    [SerializeField] GameObject combat0;
    [SerializeField] GameObject combat1;
    [SerializeField] GameObject combat2;
    [SerializeField] GameObject combat3;
    [SerializeField] GameObject combat4;
    [SerializeField] GameObject combat5;
    [SerializeField] GameObject combat6;
    [Space]
    [SerializeField] GameObject playerDeathHint;
    [SerializeField] GameObject towerDeathHint;
    [Space]
    [SerializeField] GameObject PointerBlock;



    private PlayerController player;
    private bool inTutorialScreen = false;
    private int tutorialIndex = -2;
    private bool playerStartedWalking = false;

    private bool introTutorialFinished = false;

    private GameObject currentTutorial;
    private Health bossHealth;
    private EnemySpawner enemySpawner;
    private LevelController levelController;

    // Start is called before the first frame update
    void Start()
    {
        enemySpawner = FindObjectOfType<EnemySpawner>();
        enemySpawner.enabled = false;
        player = FindObjectOfType<PlayerController>();
        tutorialEnemy.gameObject.SetActive(true);
        tutorialEnemy.enabled = false;
        levelController = FindObjectOfType<LevelController>();
        levelController.StartCombat += OnStartCombat;
        levelController.EndCombat += OnEndCombat;
        playerHealth.death += OnPlayerDeath;
        tower.death += OnTowerDeath;
        bossHealth = tutorialBoss.GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        if (tutorialIndex == 0)
        {
            ExitTutorial(control0);
            EnterTutorial(movement0);
        }
        if (tutorialIndex == 1 && player.isSelected)
        {
            ExitTutorial(movement0);
            EnterTutorial(movement1);
        }
        if (tutorialIndex == 2)
        {
            if (playerStartedWalking)
            {
                if (player.GetComponent<NavMeshAgent>().velocity.magnitude
                    < Mathf.Epsilon)
                {
                    playerStartedWalking = false;
                    ExitTutorial(movement1);
                    EnterTutorial(movement2);
                }
            }
            else
            {
                if (!player.isSelected)
                {
                    player.isSelected = true;
                }
                if (player.GetComponent<NavMeshAgent>().velocity.magnitude
                    > 0.1)
                {
                    playerStartedWalking = true;
                    player.isSelected = false;
                }
            }
        }
        if (tutorialIndex == 3 && player.isSelected)
        {
            ExitTutorial(movement2);
            EnterTutorial(movement3);
        }
        if (tutorialIndex == 4)
        {
            if (playerStartedWalking)
            {
                if (player.GetComponent<NavMeshAgent>().velocity.magnitude
                    < Mathf.Epsilon)
                {
                    playerStartedWalking = false;
                    ExitTutorial(movement3);
                    tutorialEnemy.enabled = true;
                    tutorialIndex++;
                    StartCoroutine(Utils.Timeout(() =>
                    {
                        EnterTutorial(combat0);
                        Time.timeScale = 0f;
                    }, 5f));
                }
            }
            else
            {
                if (!player.isSelected)
                {
                    player.isSelected = true;
                }
                if (player.GetComponent<NavMeshAgent>().velocity.magnitude
                    > 0.1)
                {
                    playerStartedWalking = true;
                    player.isSelected = false;
                }
            }
        }
        if (tutorialIndex == 7)
        {
            ExitTutorial(combat0);
            tutorialIndex++;
            StartCoroutine(Utils.Timeout(() =>
            {
                EnterTutorial(combat1);
                Time.timeScale = 0f;
            }, 3f));
        }
        if (tutorialIndex == 10)
        {
            ExitTutorial(combat1);
            EnterTutorial(combat2);
            Time.timeScale = 0f;
        }
        if (tutorialIndex == 12)
        {
            ExitTutorial(combat2);
            EnterTutorial(combat3);
            tutorialEnemy.enabled = false;
            tutorialEnemy.GetComponent<NavMeshAgent>().isStopped = true;
            tutorialEnemy.GetComponentInChildren<Animator>()
                .SetFloat("Velocity", 0f);
        }
        if (tutorialIndex == 13 && inspectMenu.activeInHierarchy)
        {
            ExitTutorial(combat3);
            EnterTutorial(combat4);
        }
        if (tutorialIndex == 15)
        {
            ExitTutorial(combat4);
            tutorialIndex++;
        }
        if (tutorialIndex == 16 && !inspectMenu.activeInHierarchy)
        {
            EnterTutorial(combat5);
            tutorialEnemy.enabled = true;
            tutorialEnemy.GetComponent<NavMeshAgent>().isStopped = false;
        }
        if (tutorialIndex == 17 && player.isCasting)
        {
            ExitTutorial(combat5);
            tutorialIndex++;
            StartCoroutine(Utils.Timeout(() =>
            {
                EnterTutorial(combat6);
            }, 3f));
        }
        if (tutorialIndex == 19 && discardButton.isDiscarding)
        {
            tutorialIndex++;
        }
        if (tutorialIndex == 20 && player.isCasting)
        {
            ExitTutorial(combat6);
            tutorialIndex++;
            StartCoroutine(Utils.Timeout(() =>
            {
                StartFirstStage();
            }, 3f));
        }
    }

    public void StartFirstStage()
    {
        enemySpawner.enabled = true;
        enemySpawner.OnStartCombat(gameObject, EventArgs.Empty);
    }

    public void EnterTutorial(GameObject tutorialScreen)
    {
        currentTutorial = tutorialScreen;
        inTutorialScreen = true;
        tutorialScreen.SetActive(true);
        tutorialScreen.GetComponent<CanvasGroup>()
            .DOFade(1f, 0.3f).SetEase(Ease.OutQuint).SetUpdate(true);
        GlobalAudioManager.Instance.Play("Pause", Vector3.zero);
        tutorialIndex++;
    }

    public void ExitTutorial(GameObject tutorialScreen)
    {
        inTutorialScreen = false;
        tutorialScreen.GetComponent<CanvasGroup>()
            .DOFade(0f, 0.2f).SetEase(Ease.OutQuint).SetUpdate(true)
            .OnComplete(() => tutorialScreen.SetActive(false));
    }

    public void ContinueTutorial()
    {
        Time.timeScale = 1f;
        tutorialIndex++;
    }

    private void OnStartCombat(object sender, EventArgs e)
    {
        PointerBlock.SetActive(false);
        if (levelController.currStage.index == 0)
        {
            EnterTutorial(control0);
        }
    }

    private void OnEndCombat(object sender, EventArgs e)
    {
        if (tutorialEnemy.gameObject.activeInHierarchy)
        {
            tutorialEnemy.GetComponent<Health>().TakeDamagePercent(1f);
        }
    }

    private void OnPlayerDeath(object sender, EventArgs e)
    {
        if (bossHealth.currHealth <= bossHealth.maxHealth * 0.05) return;
        playerHealth.Revive(0.7f);
        Instantiate(healVfx, playerHealth.transform);
        GlobalAudioManager.Instance.Play("Revive", playerHealth.transform.position);
        currentTutorial = playerDeathHint;
        StartCurrentTutorial();
        Time.timeScale = 0f;
    }

    private void OnTowerDeath(object sender, EventArgs e)
    {
        if (bossHealth.currHealth <= bossHealth.maxHealth * 0.2) return;
        tower.Revive(0.5f);
        Instantiate(healVfx, tower.transform);
        GlobalAudioManager.Instance.Play("Revive", tower.transform.position);
        currentTutorial = towerDeathHint;
        StartCurrentTutorial();
        Time.timeScale = 0f;
    }

    public void StartCurrentTutorial()
    {
        inTutorialScreen = true;
        currentTutorial.SetActive(true);
        currentTutorial.GetComponent<CanvasGroup>()
            .DOFade(1f, 0.3f).SetEase(Ease.OutQuint).SetUpdate(true);
        GlobalAudioManager.Instance.Play("Pause", Vector3.zero);
    }

    public void ExitCurrentTutorial()
    {
        Time.timeScale = 1f;
        ExitTutorial(currentTutorial);
    }

    public void SkipTutorial()
    {
        enemySpawner.enabled = true;
        tutorialEnemy.enabled = true;
        GlobalAudioManager.Instance.Play("Pause", Vector3.zero);

        levelController.StartCombat -= OnStartCombat;
        levelController.EndCombat -= OnEndCombat;
        playerHealth.death -= OnPlayerDeath;
        tower.death -= OnTowerDeath;

        Destroy(gameObject);
    }
}
