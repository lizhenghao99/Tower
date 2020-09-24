using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectTower
{
    public abstract class PlayerResource : MonoBehaviour
    {
        [SerializeField] int startingPrimaryResource;
        [SerializeField] int startingSecondaryResource;
        [SerializeField] public int maxPrimaryResource;
        [SerializeField] public int maxSecondaryResource;

        public int primaryResource { get; protected set; }
        public int secondaryResource { get; protected set; }

        public event EventHandler resourceChanged;

        // Start is called before the first frame update
        protected virtual void Start()
        {
            primaryResource = startingPrimaryResource;
            secondaryResource = startingSecondaryResource;
        }

        protected void InvokeResourceChanged(object sender)
        {
            resourceChanged?.Invoke(sender, EventArgs.Empty);
        }

        public abstract bool IsResourceEnough(int primaryAmount, int secondaryAmount);
        public abstract bool ChangeResource(int primaryAmount, int secondaryAmount);
    }
}