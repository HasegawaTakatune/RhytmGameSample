using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class MessageEffectManager : MonoBehaviour
{

    [SerializeField] GameManager GameManager;
    [SerializeField] GameObject Good;
    [SerializeField] GameObject Failure;

    private void Reset()
    {
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        Good = transform.Find("Good").gameObject;
        Failure = transform.Find("Failure").gameObject;
    }

    private void OnEnable()
    {
        GameManager
            .OnMessageEffect
            .Where(result => result == "good")
            .Subscribe(result => GoodShow());

        GameManager
            .OnMessageEffect
            .Where(result => result == "failure")
            .Subscribe(result => FailureShow());
    }

    private void GoodShow()
    {
        Good.SetActive(false);
        Good.SetActive(true);

        Observable.Timer(TimeSpan.FromMilliseconds(200))
            .Subscribe(_ => Good.SetActive(false));
    }

    private void FailureShow()
    {
        Failure.SetActive(false);
        Failure.SetActive(true);

        Observable.Timer(TimeSpan.FromMilliseconds(200))
            .Subscribe(_ => Failure.SetActive(false));
    }
}
