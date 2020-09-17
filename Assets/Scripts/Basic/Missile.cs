using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TowerUtils;
using DG.Tweening;

public class Missile : MonoBehaviour
{
    
    [SerializeField] public float rotationSpeed = 0.3f;
    [SerializeField] GameObject hitVfx;
    [HideInInspector] public float range;
    public event EventHandler<Vector3> hitEnemy;
    public event EventHandler outOfRange;
    public int type = 0;
    private Vector3 startingPosition;
    private bool launched = false;
    private Vector3 launchDirection;

    private void Start()
    {
        startingPosition = gameObject.transform.position;
    }

    private void Update()
    {
        var currDistance =
            Vector3.Distance(gameObject.transform.position, startingPosition);
        if (currDistance > range)
        {
            outOfRange?.Invoke(gameObject, EventArgs.Empty);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") || 
            other.gameObject.CompareTag("Prop"))
        {
            var fx = Instantiate(hitVfx);
            fx.transform.position = other.ClosestPointOnBounds(transform.position);
            fx.transform.rotation = Quaternion.LookRotation(launchDirection);
            hitEnemy?.Invoke(gameObject, gameObject.transform.position);
            Destroy(gameObject);
        }
    }

    public void Launch(Vector3 direction, float missileSpeed)
    {
        var audio = GetComponent<InstanceAudioManager>();
        if (audio != null)
        {
            audio.Play("Launch");
        }
        var floater = GetComponentInChildren<Floater>();
        if (floater != null)
        {
            floater.enabled = false;
        }
        launched = true;
        launchDirection = direction;

        var lookRotation = Quaternion.LookRotation(launchDirection);
        transform.rotation =
            Quaternion.Slerp(transform.rotation, lookRotation,
                Time.deltaTime * rotationSpeed);
        transform.DORotate(lookRotation.eulerAngles, rotationSpeed)
            .SetEase(Ease.InQuint);

        StartCoroutine(Utils.Timeout(() =>
        {
            GetComponent<Rigidbody>().AddForce(
                direction.normalized
                    * missileSpeed,
                ForceMode.VelocityChange);
        }, rotationSpeed));  
    }

    public void Cancel()
    {
        var floater = GetComponentInChildren<Floater>();
        if (floater != null)
        {
            floater.enabled = false;
        }
        GetComponent<Rigidbody>().useGravity = true;
        foreach (Material m in GetComponentInChildren<ParticleSystemRenderer>().materials)
        {
            if (m.HasProperty("_Opacity"))
            {
                DOTween.To(() => m.GetFloat("_Opacity"), 
                           (x) => m.SetFloat("_Opacity", x),
                            0f, 1f).SetEase(Ease.OutQuint)
                            .OnComplete(() => Destroy(gameObject));
            }
        }
    }
}
