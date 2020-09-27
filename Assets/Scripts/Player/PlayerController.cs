using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using Werewolf.StatusIndicators.Components;

namespace ProjectTower
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] public NavMeshAgent agent;
        [SerializeField] public SpriteRenderer spriteRenderer;
        [SerializeField] Waypoint waypoint;
        [SerializeField] float waypointXRotation;
        [SerializeField] float waypointHeight;
        [SerializeField] public Animator animator;
        [SerializeField] public GameObject highlight;


        public event EventHandler startCasting;
        public event EventHandler finishCasting;
        public event EventHandler startWalking;

        public bool isSelected = false;
        public bool isWalking { get; protected set; } = false;
        public bool isCasting { get; protected set; } = false;
        private int layerMask;

        private PlayerHealth health;
        public Waypoint myWaypoint = null;

        private float autoStopWalkTimer = 3;

        public StateMachine stateMachine { get; private set; }
        public PlayerIdleState idleState { get; private set; }
        public PlayerWalkState walkState { get; private set; }
        public PlayerCastState castState { get; private set; }
        public PlayerDeathState deathState { get; private set; }

        public PlayerChaseState chaseState { get; private set; }
        public PlayerAttackState attackState { get; private set; }
        public PlayerStunState stunState { get; private set; }

        private void Start()
        {
            agent.updateRotation = false;
            agent.updateUpAxis = false;
            layerMask = LayerMask.GetMask("Environment", "Player", "Enemy");
            health = GetComponent<PlayerHealth>();
            health.death += OnPlayerDeath;
            health.revive += OnPlayerRevive;

            stateMachine = new StateMachine();
            idleState = new PlayerIdleState(gameObject, stateMachine);
            walkState = new PlayerWalkState(gameObject, stateMachine);
            castState = new PlayerCastState(gameObject, stateMachine);
            deathState = new PlayerDeathState(gameObject, stateMachine);
            chaseState = new PlayerChaseState(gameObject, stateMachine);
            attackState = new PlayerAttackState(gameObject, stateMachine);
            stunState = new PlayerStunState(gameObject, stateMachine);
            stateMachine.Init(idleState);
        }


        // Update is called once per frame
        private void Update()
        {
            stateMachine.CurrentState.LogicUpdate();
        }

        public void SetIsWalking(bool flag)
        {
            isWalking = flag;
            if (flag)
            {
                stateMachine.ChangeState(walkState);
            }
            else
            {
                stateMachine.ChangeState(idleState);
            }
        }

        public void SetIsCasting(bool flag)
        {
            isCasting = flag;
            if (flag)
            {
                stateMachine.ChangeState(castState);
            }
            else
            {
                stateMachine.ChangeState(idleState);
            }
        }

        public void UpdateWalkAnimation()
        {
            animator.SetFloat("Velocity", agent.velocity.magnitude); 
        }

        public void UpdateFlip()
        {
            if (agent.velocity.magnitude > 0.1f)
            {
                spriteRenderer.flipX = agent.velocity.x < -0.1;
            }
        }

        public void FreezeWalkAnimation()
        {
            animator.SetFloat("Velocity", 0);
        }

        public void HandleLeftClick()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000, layerMask))
            {
                if (isSelected && hit.transform.tag == "Floor"
                    && !CardPlayer.Instance.isPlayingCard)
                {
                    SetPlayerDestination(hit.point);
                    myWaypoint.Display();
                    GlobalAudioManager.Instance.Play("MovePlayer", Vector3.zero);
                }
                else if (isSelected && hit.transform.tag == "Enemy"
                    && !CardPlayer.Instance.isPlayingCard)
                {
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
                        SetPlayerDestination(destination);
                        myWaypoint.Display();
                        GlobalAudioManager.Instance.Play("MovePlayer", Vector3.zero);
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

        public void HandleRightClick()
        {
            if (isSelected)
            {
                isSelected = false;
            }
        }

        public void SetPlayerDestination(Vector3 dest)
        {
            if (myWaypoint != null)
            {
                Destroy(myWaypoint.gameObject);
            }

            myWaypoint = Instantiate(
                waypoint,
                dest + new Vector3(0, waypointHeight, 0),
                Quaternion.Euler(waypointXRotation, 0, 0));
            myWaypoint.destinationReached += OnDestinationReached;
            SetIsWalking(true);
        }

        public void Stun()
        {
            stateMachine.ChangeState(stunState);
        }

        public void UnStun()
        {
            stateMachine.ChangeState(idleState);
        }

        public void WalkAutoStop()
        {
            if (autoStopWalkTimer < 0 && agent.velocity.magnitude < 0.1f)
            {
                agent.SetDestination(gameObject.transform.position);
                var wp = (FindObjectsOfType<Waypoint>()
                                        .Where(p => p.name == waypoint.name + "(Clone)")
                                        .FirstOrDefault());
                Destroy(wp.gameObject);
                SetIsWalking(false);
                autoStopWalkTimer = 3;
            }
        }

        public void OnDestinationReached(object sender, EventArgs e)
        {
            SetIsWalking(false);
            myWaypoint = null;
        }

        public void OnStartCasting()
        {
            startCasting?.Invoke(gameObject, EventArgs.Empty);
        }

        public void OnFinishCasting()
        {
            finishCasting?.Invoke(gameObject, EventArgs.Empty);
        }

        public void OnStartWalking()
        {
            startWalking?.Invoke(gameObject, EventArgs.Empty);
        }

        public void OnCollisionStay(Collision collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                autoStopWalkTimer -= Time.deltaTime;
            }
        }

        public void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                autoStopWalkTimer = 3;
            }
        }

        public void OnPlayerDeath(object sender, EventArgs e)
        {
            if (myWaypoint != null)
            {
                Destroy(myWaypoint.gameObject);
                myWaypoint = null;
            }
            
            stateMachine.ChangeState(deathState);
        }

        public void OnPlayerRevive(object sender, EventArgs e)
        {
            stateMachine.ChangeState(idleState);
        }
    }
}