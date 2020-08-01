using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    private PlayerController[] players;
    private DiscardButton[] discardButtons;

    // Start is called before the first frame update
    void Start()
    {
        players = FindObjectsOfType<PlayerController>();
        discardButtons = FindObjectsOfType<DiscardButton>();
    }

    // Update is called once per frame
    void Update()
    {
        RightClickUpdate();
    }

    private void RightClickUpdate()
    {
        // mouse right
        if (Input.GetMouseButtonDown(1))
        {
            foreach (PlayerController p in players)
            {
                if (p.isSelected && !CardPlayer.Instance.isPlayingCard)
                {
                    GlobalAudioManager.Instance.Play("Cancel", Vector3.zero);
                }   
            }

            foreach (DiscardButton b in discardButtons)
            {
                if (b.isDiscarding)
                {
                    GlobalAudioManager.Instance.Play("Cancel", Vector3.zero);
                }
            }
        }
    }

    public void LeftClick()
    {
        GlobalAudioManager.Instance.Play("Click", Vector3.zero);
    }
}
