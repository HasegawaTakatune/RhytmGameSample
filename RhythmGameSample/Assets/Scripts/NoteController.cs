using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class NoteController : MonoBehaviour
{
    private const float Interval = 2.0f * 1000;

    public GTimer.GameTimer GTime;

    public Color color;

    [SerializeField] new SpriteRenderer renderer = default;

    [SerializeField] new BoxCollider2D collider2D = default;

    private bool doOnce = false;

    /// <summary>
    /// ノーツタイプ
    /// </summary>
    public string Type { get; private set; }
    /// <summary>
    /// ノーツを発射するタイミング
    /// </summary>
    public float Timing { get; private set; }

    /// <summary>
    /// レーン位置
    /// </summary>
    public int Lane { get; private set; }

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

    private void Start()
    {
        color = GetComponent<SpriteRenderer>().color;
    }

    private void OnBecameInvisible()
    {
        renderer.enabled = false;
        collider2D.enabled = false;
    }

    private void OnEnable()
    {
        isGo = false;
        firstPos = transform.position;

        this.UpdateAsObservable().
            Where(_ => isGo).
            Where(_ => GTime.MSTime <= (Timing + Interval)).
            Subscribe(_ =>
            {
                if (!doOnce)
                {
                    doOnce = true;
                    renderer.enabled = true;
                    collider2D.enabled = true;
                }
                //gameObject.transform.position = new Vector3(firstPos.x, firstPos.y + distance * (Time.time * 1000 - goTime) / during, firstPos.z);
                gameObject.transform.position = new Vector3(firstPos.x, firstPos.y + distance * (GTime.MSTime - goTime) / during, firstPos.z);
            });
    }

    public void SetParameter(string type, float timing, int lane)
    {
        Type = type;
        Timing = timing;
        Lane = lane;
    }

    public void GoTo(float distance, float during)
    {
        this.distance = distance;
        this.during = during;
        goTime = GTime.MSTime;

        isGo = true;
    }
}