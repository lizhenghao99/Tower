using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [SerializeField]
    float propultionForce;

    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*
         * Here i'm just using the same method that makes the Character Jump, with an aditional modifier to make it jump higher, since
         * the character controller on the demo uses raycasts to move the character around.
         * For a character controller that uses the rigidbody velocities and forces to move around, this could be changed to
         * simply add a force to the character's rigidbody.
         */
        collision.gameObject.SendMessage("OnJumpInputDown", propultionForce);
        animator.SetTrigger("boing");
    }
}
