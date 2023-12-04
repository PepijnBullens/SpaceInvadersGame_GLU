using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimation : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private float fps;

    private SpriteRenderer spriteRenderer;
    private float timer;
    private float interval;
    private int index;

    [SerializeField]
    private bool playOnceAndDestroy;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); 
        interval = 1f/fps;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if(timer >= interval)
        {
            spriteRenderer.sprite = sprites[index];

            index++;
            if (index == sprites.Length)
            {
                if(playOnceAndDestroy)
                {
                    Destroy(gameObject);
                }
                index = 0;
            } 

            timer = 0;
        }
    }
}
