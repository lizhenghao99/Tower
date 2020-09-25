using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ProjectTower
{
    public class PlayerState : State
    {
        protected PlayerController playerController;
        protected PlayerAutoAttack playerAutoAttack;

        public PlayerState(GameObject owner, StateMachine stateMachine)
            : base(owner, stateMachine)
        {
            playerController = owner.GetComponent<PlayerController>();
            playerAutoAttack = owner.GetComponent<PlayerAutoAttack>();
        }
        
        public override void Enter()
        {
            // do nothing
        }

        public override void Exit()
        {
            // do nothing
        }

        public override void LogicUpdate()
        {
            // select effect
            if (playerController.isSelected)
            {
                playerController.highlight.SetActive(true);

            }
            else
            {
                playerController.highlight.SetActive(false);
            }
            // Handle Input
            if (Input.GetMouseButtonDown(0)
                && !EventSystem.current.IsPointerOverGameObject())
            {
                playerController.HandleLeftClick();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                playerController.HandleRightClick();
            }
        }
    }
}