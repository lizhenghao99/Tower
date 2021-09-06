using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutAndDestroy : MonoBehaviour
{
    public float fadeOutTimer;
    SpriteRenderer renderer;

    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Color fade = renderer.color;
        fade.a = Mathf.MoveTowards(fade.a, 0, 1/fadeOutTimer * Time.deltaTime);
        renderer.color = fade;


        if (fade.a <= 0)
            gameObject.SetActive(false);
    }
}
