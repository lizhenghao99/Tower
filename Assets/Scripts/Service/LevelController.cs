using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;
using UnityEngine.AI;
using TowerUtils;
using DG.Tweening;

public class LevelController : Singleton<LevelController>
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

    public EventHandler StartCombat;
    public EventHandler EndCombat;
    public EventHandler StageClear;

    private ProCamera2DNumericBoundaries numericBoundaries;
    private ProCamera2DCinematics cinematics;

    public Stage currStage { get; private set; } = null;

    private void Awake()
    {
        foreach (Material m in cardMaterials)
        {
            m.SetFloat("_HsvSaturation", 1f);
        }

        if (Screen.width/Screen.height > 1.8f)
        {
            Camera.main.rect = new Rect(0.128f, 0f, 0.744f, 1f);
        }

        numericBoundaries = Camera.main.GetComponent<ProCamera2DNumericBoundaries>();
        cinematics = Camera.main.GetComponent<ProCamera2DCinematics>();
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
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Base"),
                                     LayerMask.NameToLayer("Player"),
                                     true);

        StartNextStage();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnStartCombat()
    {
        Minion[] minions = FindObjectsOfType<Minion>();
        foreach (Minion m in minions)
        {
            m.gameObject.SetActive(false);
        }
        StartCombat?.Invoke(gameObject, EventArgs.Empty);
    }

    public void OnEndCombat()
    {
        EndCombat?.Invoke(gameObject, EventArgs.Empty);
    }

    public void OnStageClear()
    {
        StageClear?.Invoke(gameObject, EventArgs.Empty);
    }

    public void ClearStage()
    {
        StartCoroutine(TowerUtils.Utils.Timeout(() => StartNextStage()
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
                // you win
                print("YOU WIN!");
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
                players[i].transform.position =
                    currStage.charPostions[i] - new Vector3(5f, 0, 0);
                players[i].GetComponent<NavMeshAgent>()
                    .SetDestination(currStage.charPostions[i]);
                players[i].OnDestinationReached(gameObject, EventArgs.Empty);
            }
        }
    }

    public void OnCinematicFinished()
    {
        OnStartCombat();
    }
}