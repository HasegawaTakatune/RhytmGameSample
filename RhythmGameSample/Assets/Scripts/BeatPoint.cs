using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class BeatPoint : MonoBehaviour
{
    [SerializeField] private new SpriteRenderer renderer;

    private void Reset()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        renderer.color = Color.blue;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Hit");

        renderer.color = Color.yellow;
        Observable.Timer(TimeSpan.FromMilliseconds(200))
            .Subscribe(_ => renderer.color = Color.blue);
    }
}
