using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class KeyBorad : MonoBehaviour
{
    private const int LENGTH = 44;
    [SerializeField] SpriteRenderer[] keys = new SpriteRenderer[LENGTH];

    private void Reset()
    {
        for (int i = 0; i < LENGTH; i++)
        {
            keys[i] = GameObject.Find("beatPoint" + i)?.GetComponent<SpriteRenderer>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        NoteController note = collision?.GetComponent<NoteController>();
        if (note != null)
        {
            int lane = note.Lane;
            keys[lane].color = note.color;
            Observable.Timer(TimeSpan.FromMilliseconds(200))
                .Subscribe(_ => keys[lane].color = Color.white);
        }
    }
}
