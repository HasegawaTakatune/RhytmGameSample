using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class SoundEffectManager : MonoBehaviour
{
    /// <summary>
    /// ゲームマネージャ
    /// </summary>
    [SerializeField] GameManager GameManager;
    /// <summary>
    /// 効果音「ドン」
    /// </summary>
    [SerializeField] AudioSource DonPlayer;
    /// <summary>
    /// 効果音「カッ」
    /// </summary>
    [SerializeField] AudioSource KaPlayer;

    private void Reset()
    {
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        DonPlayer = transform.Find("DonPlayer").GetComponent<AudioSource>();
        KaPlayer = transform.Find("KaPlayer").GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        GameManager
            .OnSoundEffect
            .Where(type => type == "don")
            .Subscribe(type => DonPlay());

        GameManager
            .OnSoundEffect
            .Where(type => type == "ka")
            .Subscribe(type => KaPlay());
    }

    private void DonPlay()
    {
        DonPlayer.Stop();
        DonPlayer.Play();
    }

    private void KaPlay()
    {
        KaPlayer.Stop();
        KaPlayer.Play();
    }
}
