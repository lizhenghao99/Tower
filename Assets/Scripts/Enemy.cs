using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    private PlayerController[] players;
    private PlayerController target;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        players = FindObjectsOfType<PlayerController>();
        SetTarget();
    }

    private void SetTarget()
    {
        PlayerController closestPlayer = players[0];
        float min_dist = Mathf.Infinity;
        foreach(PlayerController player in players)
        {
            float dist = Vector3.Distance(gameObject.transform.position,
                                            player.transform.position);
            if (dist < min_dist)
            {
                closestPlayer = player;
                min_dist = dist;
            }
        }
        target = closestPlayer;
    }

    public void Spawn()
    {
        gameObject.SetActive(true);
    }


    // Update is called once per frame
    void Update()
    {
        SetTarget();
        agent.SetDestination(target.transform.position);
    }
}
