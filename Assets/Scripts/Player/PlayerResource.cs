using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerResource : MonoBehaviour
{
    [SerializeField] int startingPrimaryResource;
    [SerializeField] int startingSecondaryResource;

    public int primaryResource { get; protected set; }
    public int secondaryResource { get; protected set; }
    // Start is called before the first frame update
    void Start()
    {
        primaryResource = startingPrimaryResource;
        secondaryResource = startingSecondaryResource;
    }

    public abstract bool changeResource(int primaryAmount, int secondaryAmount);
}
