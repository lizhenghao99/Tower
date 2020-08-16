using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FireballImpact : MonoBehaviour
{
    [SerializeField] float range = 5f;
    [SerializeField] float burnDuration = 0.5f;
    [SerializeField] float burnAmount = 100f;
    [SerializeField] float lifetime = 10f;

    private EffectManager effectManager;
    private PlayerController player;

    private void Start()
    {
        effectManager = FindObjectOfType<EffectManager>();
        player = FindObjectsOfType<PlayerController>()
            .Where(p => p.gameObject.name == "Daoshi")
            .FirstOrDefault();
        StartCoroutine(ApplyBurn());
    }

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime < 0)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator ApplyBurn()
    {
        foreach (Collider c in Physics.OverlapSphere(
            gameObject.transform.position, range, 
            LayerMask.GetMask("Enemy")))
        {
            effectManager.Register(player.gameObject, c.gameObject,
                Effect.Type.Burn, burnDuration, burnAmount);
        }
        yield return new WaitForSeconds(1f);
    }
}
