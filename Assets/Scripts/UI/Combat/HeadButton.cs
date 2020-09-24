using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

namespace ProjectTower
{
    public class HeadButton : Selectable
    {
        public Card.Owner owner;

        private PlayerController player;
        public bool isDiscarding = false;
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            player = FindObjectsOfType<PlayerController>()
               .Where(p => p.gameObject.name == owner.ToString())
               .FirstOrDefault();
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            player.isSelected = true;
            GlobalAudioManager.Instance.Play("SelectPlayer", Vector3.zero);
        }
    }
}