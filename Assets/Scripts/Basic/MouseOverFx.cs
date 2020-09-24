using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectTower
{
    public class MouseOverFx : MonoBehaviour
    {
        [SerializeField] GameObject fx;
        // Start is called before the first frame update


        private void OnMouseOver()
        {
            fx.SetActive(true);
        }

        private void OnMouseExit()
        {
            fx.SetActive(false);
        }
    }
}