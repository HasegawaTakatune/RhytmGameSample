using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class NoteController : MonoBehaviour
{
    public GTimer.GameTimer GTime;

    /// <summary>
    /// ノーツタイプ
    /// </summary>
    public string Type { get; private set; }
    /// <summary>
    /// ノーツを発射するタイミング
    /// </summary>
    public float Timing { get; private set; }

    /// <summary>
    /// 初期位置からはじく位置までの距離
    /// </summary>
    private float distance;
    /// <summary>
    /// 初期位置からはじく位置までの時間
    /// </summary>
    private float during;

    /// <summary>
    /// 初期位置
    /// </summary>
    private Vector3 firstPos;
    /// <summary>
    /// ノーツが発射しているか判定
    /// </summary>
    private bool isGo;
    /// <summary>
    /// ノーツを発射させた時間
    /// </summary>
    private float goTime;

    private void OnEnable()
    {
        isGo = false;
        firstPos = transform.position;

        this.UpdateAsObservable().
            Where(_ => isGo).
            Subscribe(_ =>
            {
                //gameObject.transform.position = new Vector3(firstPos.x, firstPos.y + distance * (Time.time * 1000 - goTime) / during, firstPos.z);
                gameObject.transform.position = new Vector3(firstPos.x, firstPos.y + distance * (GTime.MSTime - goTime) / during, firstPos.z);
            });
    }

    public void SetParameter(string type, float timing)
    {
        Type = type;
        Timing = timing;
    }

    public void GoTo(float distance, float during)
    {
        this.distance = distance;
        this.during = during;
        goTime = GTime.MSTime;

        isGo = true;
    }
}