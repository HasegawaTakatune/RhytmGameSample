using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class NotesKeyBoard : MonoBehaviour
{
    [SerializeField] private GTimer.GameTimer gtimer;

    public Common.MusicalScore music = new Common.MusicalScore();
    private KeyPressTimer[] pressTimers = new KeyPressTimer[10];

    private bool isPlaying = true;

    private void OnEnable()
    {
        pressTimers[0] = new KeyPressTimer();
        music.title = "TestMusic";
        music.keyboard = 32;
        music.beat = 3;
        music.bpm = 135;
        music.notes = new List<Common.Notes>();

        this.UpdateAsObservable()
            .Where(_ => isPlaying)
            .Where(_ => Input.GetKeyDown(KeyCode.Alpha1))
            .Subscribe(_ =>
            {
                float ff = gtimer.MainTime;
                pressTimers[0].keyDownTime = ff;
            });

        this.UpdateAsObservable()
            .Where(_ => isPlaying)
            .Where(_ => Input.GetKeyUp(KeyCode.Alpha1))
            .Subscribe(_ =>
            {
                float ff = gtimer.MainTime;
                pressTimers[0].keyUpTime = ff;
                pressTimers[0].GetTiming(out float timing, out float length);
                Common.Notes notes = new Common.Notes
                {
                    timing = timing.ToString(),
                    length = length.ToString(),
                    type = Common.Notes.Apart,
                    lane = 0
                };

                music.notes.Add(notes);
            });
    }
}

public class KeyPressTimer
{
    private const float PUSH_TIME = 1.0f;
    private const float MIN_TIME = 1.0f;

    public float keyDownTime = 0;
    public float keyUpTime = 0;

    public void GetTiming(out float timing, out float length)
    {
        timing = keyDownTime;
        length = keyUpTime - keyDownTime;

        if (length < PUSH_TIME)
            length = MIN_TIME;
    }

    public void Reset()
    {
        keyDownTime = 0;
        keyUpTime = 0;
    }
}