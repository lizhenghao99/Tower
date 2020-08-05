using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using Werewolf.StatusIndicators.Components;

public class PlayerController : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] public SpriteRenderer spriteRenderer;
    [SerializeField] Waypoint waypoint;
    [SerializeField] float waypointXRotation;
    [SerializeField] float waypointHeight;
    [SerializeField] public Animator animator;
    [SerializeField] GameObject highlight;


    public EventHandler startCasting;
    public EventHandler finishCasting;
    public EventHandler startWalking;

    public bool isSelected = false;
    public bool isWalking { get; private set; } = false;
    public bool isCasting { get; private set; } = false;
    private int layerMask;

    private PlayerHealth health;

    private float autoStopWalkTimer = 3;

    private void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        layerMask = LayerMask.GetMask("Environment", "Player", "Enemy");
        health = GetComponent<PlayerHealth>();
    }


    // Update is called once per frame
    void Update()
    {
        // anmiation
        animator.SetFloat("Velocity", agent.velocity.magnitude);
        if (agent.velocity.magnitude > Mathf.Epsilon && !isCasting)
        {
            spriteRenderer.flipX = agent.velocity.x < -0.2;
        }
        if (health.isDead)
        {
            isSelected = false;
            agent.destination = gameObject.transform.position;
            agent.isStopped = true;
            return;
        }
        // mouse left
        if (Input.GetMouseButtonDown(0) 
            && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000, layerMask))
            {
                if (isSelected && hit.transform.tag == "Floor"
                    && !CardPlayer.Instance.isPlayingCard)
                {
                    Waypoint wp = FindObjectsOfType<Waypoint>()
                                    .Where(p => p.name == waypoint.name+"(Clone)")
                                    .FirstOrDefault();
                    if (wp != null)
                    {
                        wp.transform.position = hit.point 
                            + new Vector3(0,waypointHeight,0);
                    }
                    else
                    {
                        wp = Instantiate(
                            waypoint, 
                            hit.point + new Vector3(0,waypointHeight,0), 
                            Quaternion.Euler(waypointXRotation,0,0));
                        wp.destinationReached += OnDestinationReached;
                    }
                    agent.stoppingDistance = 0;
                    agent.SetDestination(hit.point);
                    isWalking = true;
                    isSelected = false;
                    EventSystem.current.SetSelectedGameObject(null);
                    GlobalAudioManager.Instance.Play("MovePlayer", Vector3.zero);
                    OnStartWalking();
                }
                else if (isSelected && hit.transform.tag == "Enemy"
                    && !CardPlayer.Instance.isPlayingCard)
                {
                    Waypoint wp = FindObjectsOfType<Waypoint>()
                                    .Where(p => p.name == waypoint.name + "(Clone)")
                                    .FirstOrDefault();
                    RaycastHit hitInfo;
                    var mask = LayerMask.GetMask("Enemy");
                    if (Physics.Linecast(gameObject.transform.position,
                                            hit.collider.transform.position,
                                            out hitInfo, mask))
                    {
                        var direction = Vector3.ProjectOnPlane(
                            gameObject.transform.position - hitInfo.point,
                            new Vector3(0, 1, 0)).normalized;
                        Vector3 destination;
                        if (Vector3.Distance(gameObject.transform.position, 
                            hitInfo.point) < GetComponent<AttackBase>().stopRange)
                        {
                            destination = gameObject.transform.position;
                        }
                        else
                        {
                            destination = hitInfo.point
                            + direction * GetComponent<AttackBase>().stopRange;
                        }
                        if (wp != null)
                        {
                            wp.transform.position = destination
                                + new Vector3(0, waypointHeight, 0);
                        }
                        else
                        {
                            wp = Instantiate(
                                waypoint,
                                destination + new Vector3(0, waypointHeight, 0),
                                Quaternion.Euler(waypointXRotation, 0, 0));
                            wp.destinationReached += OnDestinationReached;
                        }
                        agent.stoppingDistance = 0;
                        agent.SetDestination(destination);
                        isWalking = true;
                        isSelected = false;
                        EventSystem.current.SetSelectedGameObject(null);
                        GlobalAudioManager.Instance.Play("MovePlayer", Vector3.zero);
                        OnStartWalking();
                    }
                    else
                    {
                        print("line cast not hit");
                    }
                   
                }
                else 
                {
                    if (hit.transform.name == gameObject.name)
                    {
                        isSelected = true;
                        GlobalAudioManager.Instance.Play("SelectPlayer", Vector3.zero);
                    }
                } 
            }
        }

        if (Input.GetMouseButtonDown(1) && isSelected)
        {
            isSelected = false;
            EventSystem.current.SetSelectedGameObject(null);
        }

        // select effect
        if (isSelected)
        {
            highlight.SetActive(true);
            
        }
        else
        {
            highlight.SetActive(false);
        }   
        
        if (isCasting)
        {
            agent.isStopped = true;
        }
        else
        {
            agent.isStopped = false;
        }

        if (isWalking && autoStopWalkTimer < 0 && agent.velocity.magnitude < 0.1f)
        {
            agent.SetDestination(gameObject.transform.position);
            var wp = (FindObjectsOfType<Waypoint>()
                                    .Where(p => p.name == waypoint.name + "(Clone)")
                                    .FirstOrDefault());
            Destroy(wp.gameObject);
            isWalking = false;
            autoStopWalkTimer = 3;
        }
    }

    public void setIsCasting(bool flag)
    {
        isCasting = flag;
        if (flag)
        {
            OnStartCasting();
        }
        else
        {
            OnFinishCasting();
        }
    }

    public void OnDestinationReached(object sender, EventArgs e)
    {
        isWalking = false;
    }

    private void OnStartCasting()
    {
        startCasting?.Invoke(gameObject, EventArgs.Empty);
    }

    private void OnFinishCasting()
    {
        finishCasting?.Invoke(gameObject, EventArgs.Empty);
    }

    private void OnStartWalking()
    {
        startWalking?.Invoke(gameObject, EventArgs.Empty);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            autoStopWalkTimer -= Time.deltaTime;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            autoStopWalkTimer = 3;
        }
    }
}
