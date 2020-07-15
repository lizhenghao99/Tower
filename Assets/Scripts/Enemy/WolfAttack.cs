using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfAttack : EnemyAttack
{
    private float swipeTimer = 5f;
    protected override void SpeicalAttackUpdate()
    {
        swipeTimer -= Time.deltaTime;
        if (swipeTimer < 0)
        {
            swipe();
            swipeTimer = 5f;
        }
    }

    private void swipe()
    {
        var swipeMask = LayerMask.GetMask("Player", "Minion", "base");
        Collider[] colliders = Physics.OverlapSphere(
            gameObject.transform.position, attackRange + 3, swipeMask);
        foreach (Collider c in colliders)
        {
            if (c.gameObject.CompareTag("Minion") 
                && !c.gameObject.GetComponent<Minion>().taunt)
            {
                var direction =
                    Vector3.ProjectOnPlane(
                        c.transform.position - gameObject.transform.position,
                        new Vector3(0, 1, 0)).normalized;

                c.GetComponent<Rigidbody>().AddForce(
                    direction * 20, ForceMode.VelocityChange);
            }
            c.GetComponent<Health>().TakeDamage(attackDamage);
        }
    }
}
