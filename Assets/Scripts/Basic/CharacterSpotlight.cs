using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class CharacterSpotlight : MonoBehaviour
{
    [SerializeField] float heightOffset;
    [SerializeField] float spotlightDistance;
    private Transform target;
    private Transform parent;

    // Start is called before the first frame update
    void Start()
    {
        target = Camera.main.GetComponentInChildren<AudioListener>().transform;
        parent = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        var direction = (parent.position - target.position).normalized;
        Vector3 newPos;

        if (parent.position.z - target.position.z < 0)
        {
            var reflectedDirection =
                new Vector3(direction.x, direction.y, -direction.z);
            newPos = parent.position - reflectedDirection * spotlightDistance
            + new Vector3(0, heightOffset, 0);
        }
        else
        {
            newPos = parent.position - direction * spotlightDistance
            + new Vector3(0, heightOffset, 0);
        }

        gameObject.transform.position = newPos;
        gameObject.transform.LookAt(
            parent.position + new Vector3(0, heightOffset, 0));
    }
}
