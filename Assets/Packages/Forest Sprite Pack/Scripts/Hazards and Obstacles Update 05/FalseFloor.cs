using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FalseFloor : MonoBehaviour {

    [Tooltip("This is public just so you can test it out in the inspector, idealy some game system will change this once.")]
    public bool triggerFloor = false;

    public GameObject particles;

    private void Update()
    {
        if (triggerFloor == true)
        {
            TriggerFloor();
            triggerFloor = false;
        }
    }

    void TriggerFloor()
    {
        GameObject instantiatedParticle = Instantiate(particles, null);
        instantiatedParticle.transform.position = transform.position;
        Destroy(this.gameObject);
    }

    //Trigger the object's action when the player enters the trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player" && !triggerFloor)
        {
            triggerFloor = true;
        }
    }


}
