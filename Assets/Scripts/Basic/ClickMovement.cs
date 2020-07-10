using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClickMovement : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;

    private int layer_mask;

    // Start is called before the first frame update
    void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        layer_mask = LayerMask.GetMask("Ground");
    }

    // Update is called once per frame
    void Update()
    {

        animator.SetFloat("horizontal", agent.velocity.x);
        spriteRenderer.flipX = agent.velocity.x < 0;
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000, layer_mask))
            {
                agent.stoppingDistance = 0;
                agent.SetDestination(hit.point);
            }
        }
    }
}
