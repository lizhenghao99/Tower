using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ProjectTower
{
    public class WorldClickableObject : MonoBehaviour
    {
        [System.Serializable]
        public class ClickEvent : UnityEvent { };
        [SerializeField] ClickEvent onClick;

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 1000))
                {
                    if (hit.collider.gameObject == gameObject)
                    {
                        onClick.Invoke();
                    }
                }
            }
        }
    }
}