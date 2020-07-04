using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*  README
 *  The way this works is that it will fadeout all the SpriteRenderers that are childs of the gameobject with this script.
 *  After the fadeout, the gameobject is destroyed;
 */


[RequireComponent(typeof(TimedObjectDestructor))]
public class SecretWall : MonoBehaviour
{
    [Tooltip("This is public just so you can test it out in the inspector, idealy some game system will change this once.")]
    public bool triggerFadeOut = false;
    [Range(0, 1), Tooltip("Time it takes to fadeout.")]
    public float fadeTime = 0;

    TimedObjectDestructor destructor;
    List<SpriteRenderer> cachedRenderers = new List<SpriteRenderer>(); //This one is for fading out Sprites
    List<MeshRenderer> cachedMeshRenderers = new List<MeshRenderer>(); //This one is for fading out the Mesh Renderers used by the Ferr2D terrain tool

    void Awake()
    {
        CacheRenderers();
        destructor = GetComponent<TimedObjectDestructor>();

    }
    void CacheRenderers()
    {
        SpriteRenderer[] sr = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer s in sr)
        {
            cachedRenderers.Add(s);
        }

        MeshRenderer[] mr = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer m in mr)
        {
            cachedMeshRenderers.Add(m);
        }
    }
    void Update()
    {
        if (triggerFadeOut == true)
        {
            destructor.Destroy(fadeTime);
            TriggerFadeOut();
        }

    }

    float alpha = 1;
    void TriggerFadeOut()
    {
        alpha = Mathf.MoveTowards(alpha, 0, 1 / fadeTime * Time.deltaTime);
        if (cachedRenderers.Count > 0)
        {
            foreach (SpriteRenderer r in cachedRenderers)
            {
                Color color = r.color;
                color.a = alpha;

                r.color = color;
            }
        }

        if (cachedMeshRenderers.Count > 0)
        {
            foreach (MeshRenderer mr in cachedMeshRenderers)
            {
                foreach (Material m in mr.materials)
                {
                    Color color = m.GetColor("_Color");
                    color.a = alpha;
                    m.SetColor("_Color", color);

                }
            }
        }

    }

    //Trigger the object's action when the player enters the collision
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player" && !triggerFadeOut)
        {
            triggerFadeOut = true;
        }
    }



}
