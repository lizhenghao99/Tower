using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Follow : MonoBehaviour
{
    [SerializeField] Transform body;
    [SerializeField] Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = body.position + offset;
    }
}