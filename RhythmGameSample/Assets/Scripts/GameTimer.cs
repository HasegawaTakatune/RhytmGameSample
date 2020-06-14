using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace GTimer
{
    public class GameTimer : MonoBehaviour
    {
        [SerializeField] private Button PlayButton;
        [SerializeField] private Button StopButton;
        [SerializeField] private Button FrontSkipButton;
        [SerializeField] private Button BackSkipButton;

        [SerializeField] private AudioSource Music;

        public float MainTime;
        public float MSTime;

        private bool isPlaying = false;

        private void Reset()
        {
            PlayButton = GameObject.Find("PlayButton").GetComponent<Button>();
            StopButton = GameObject.Find("StopButton").GetComponent<Button>();
            FrontSkipButton = GameObject.Find("FrontSkipButton").GetComponent<Button>();
            BackSkipButton = GameObject.Find("BackSkipButton").GetComponent<Button>();

            Music = GameObject.Find("GameManager").GetComponent<AudioSource>();
        }

        private void Start()
        {
            ResetTime();
        }

        private void OnEnable()
        {
            PlayButton.onClick.AsObservable().Subscribe(_ => Play());
            StopButton.onClick.AsObservable().Subscribe(_ => Stop());
            FrontSkipButton.onClick.AsObservable().Subscribe(_ => FrontSkip30());
            BackSkipButton.onClick.AsObservable().Subscribe(_ => BackSkip30());
        }

        public void ResetTime()
        {
            MainTime = 0;
            MSTime = 0;
        }

        public void Play()
        {
            isPlaying = true;
            StartCoroutine(TimeStart());
        }

        private IEnumerator TimeStart()
        {
            while (isPlaying)
            {
                yield return null;
                MainTime += Time.deltaTime;
                MSTime = MainTime * 1000;
            }
        }

        public void Stop()
        {
            isPlaying = false;
        }

        public void FrontSkip30()
        {
            TimeSkip(30);
        }

        public void BackSkip30()
        {
            TimeSkip(-30);
        }

        private void TimeSkip(float value)
        {
            MainTime += value * Time.deltaTime;
            MSTime = MainTime * 1000;
            Music.time = MainTime;
        }
    }
}