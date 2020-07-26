using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using System.IO;
using UnityMidi;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GTimer.GameTimer GTime;

    [SerializeField] private MidiPlayer midiPlayer;

    /// <summary>
    /// 譜面ファイルパス
    /// </summary>
    public string JsonFilePath;
    /// <summary>
    /// 曲ファイルパス
    /// </summary>
    public string MusicFilePath;

    /// <summary>
    /// 開始ボタン
    /// </summary>
    [SerializeField] Button play;
    /// <summary>
    /// 一時停止ボタン
    /// </summary>
    [SerializeField] Button stop;
    /// <summary>
    /// 楽譜読込ボタン
    /// </summary>
    [SerializeField] Button setChart;

    /// <summary>
    /// ノーツA
    /// </summary>
    [SerializeField] GameObject Apart;
    /// <summary>
    /// ノーツB
    /// </summary>
    [SerializeField] GameObject Bpart;
    /// <summary>
    /// ノーツC
    /// </summary>
    [SerializeField] GameObject Cpart;

    /// <summary>
    /// ノーツ配置レーン
    /// </summary>
    [SerializeField] Transform[] lanes = new Transform[43];
    /// <summary>
    /// ノーツをはじく位置
    /// </summary>
    [SerializeField] Transform[] beatPoints = new Transform[43];

    /// <summary>
    /// AudioSource
    /// </summary>
    AudioSource Music;

    /// <summary>
    /// ゲーム開始時間
    /// </summary>
    float playTime;
    /// <summary>
    /// ノーツの初期位置からはじく位置までの距離
    /// </summary>
    float distance;
    /// <summary>
    /// ノーツの初期位置からはじく位置までにかける時間
    /// </summary>
    float during;
    /// <summary>
    /// ゲーム中判定
    /// </summary>
    bool isPlaying;
    /// <summary>
    /// 発射対象のノーツのインデックス
    /// </summary>
    int goIndex;

    /// <summary>
    /// ノーツタッチタイミングの成功／失敗の判定領域
    /// </summary>
    float checkRange;
    /// <summary>
    /// 成功する領域
    /// </summary>
    float beatRange;
    /// <summary>
    /// ノーツのタイミングのみ入った配列
    /// </summary>
    List<float> NoteTimings;

    /// <summary>
    /// 曲タイトル
    /// </summary>
    string title;
    /// <summary>
    /// 鍵盤の種類
    /// </summary>
    string keyboard;
    /// <summary>
    /// 拍数
    /// </summary>
    int beat;
    /// <summary>
    /// 曲のBPM
    /// </summary>
    int BPM;
    /// <summary>
    /// 曲の全ノーツ
    /// </summary>
    List<NoteController> notes;

    /// <summary>
    /// イベントを通知するサブジェクトを追加
    /// </summary>
    Subject<string> SoundEffectSubject = new Subject<string>();

    /// <summary>
    /// イベントを検知するオブサーバーを追加
    /// </summary>
    public IObservable<string> OnSoundEffect { get { return SoundEffectSubject; } }

    /// <summary>
    /// イベントを通知するサブジェクトを追加
    /// </summary>
    Subject<string> MessageEffectSubject = new Subject<string>();

    /// <summary>
    /// イベントを検知するオブサーバーを追加
    /// </summary>
    public IObservable<string> OnMessageEffect { get { return MessageEffectSubject; } }

    private void Reset()
    {
        for (int i = 0; i < lanes.Length; i++)
        {
            lanes[i] = GameObject.Find("lane" + i).transform;
            beatPoints[i] = GameObject.Find("beatPoint" + i).transform;
        }
    }

    private void OnEnable()
    {

        Music = GetComponent<AudioSource>();

        distance = Math.Abs(beatPoints[0].position.y - lanes[0].position.y);
        during = 2 * 1000;
        isPlaying = false;
        goIndex = 0;

        checkRange = 120;
        beatRange = 80;

        Debug.Log(distance);

        play.onClick.AsObservable().Subscribe(_ => Play());

        stop.onClick.AsObservable().Subscribe(_ => Stop());

        setChart.onClick.AsObservable().Subscribe(_ => LoadChart());

        this.UpdateAsObservable()
            .Where(_ => isPlaying)
            .Where(_ => notes.Count > goIndex)
            //.Where(_ => notes[goIndex].GetComponent<NoteController>().Timing <= ((Time.time * 1000 - playTime) + during))
            .Where(_ => notes[goIndex].Timing <= ((GTime.MSTime - playTime) + during))
            .Subscribe(_ =>
            {
                Debug.Log(goIndex + " Timing :" + notes[goIndex].Timing);
                notes[goIndex].GoTo(distance, during);
                goIndex++;
            });

        this.UpdateAsObservable()
            .Where(_ => isPlaying)
            .Where(_ => Input.GetKeyDown(KeyCode.A))
            .Subscribe(_ =>
            {
                //Beat("Apart", Time.time * 1000 - playTime);
                Beat("Apart", GTime.MSTime - playTime);
                SoundEffectSubject.OnNext("don");
            });

        this.UpdateAsObservable()
            .Where(_ => isPlaying)
            .Where(_ => Input.GetKeyDown(KeyCode.W))
            .Subscribe(_ =>
            {
                //Beat("Bpart", Time.time * 1000 - playTime);
                Beat("Bpart", GTime.MSTime - playTime);
                SoundEffectSubject.OnNext("ka");
            });

        this.UpdateAsObservable()
            .Where(_ => isPlaying)
            .Where(_ => Input.GetKeyDown(KeyCode.D))
            .Subscribe(_ =>
            {
                //Beat("Cpart", Time.time * 1000 - playTime);
                Beat("Cpart", GTime.MSTime - playTime);
                SoundEffectSubject.OnNext("don");
            });
    }

    private void Update()
    {
        //Debug.Log("GTime.MainTime : " + GTime.MainTime);
        //Debug.Log("GTime.MSTime : " + GTime.MSTime);
    }

    private void LoadChart()
    {
        notes = new List<NoteController>();
        NoteTimings = new List<float>();

        //string bundle = Path.Combine(Application.streamingAssetsPath, JsonFilePath);
        //Debug.Log("Json file :" + bundle);
        //AssetBundle assetBundle = AssetBundle.LoadFromFile(bundle);

        string jsonText = File.ReadAllText(Application.streamingAssetsPath + "/" + JsonFilePath); //Resources.Load<TextAsset>(JsonFilePath).ToString();

        //bundle = Path.Combine(Application.streamingAssetsPath, MusicFilePath);
        //assetBundle = AssetBundle.LoadFromFile(bundle);

        //Music.clip = Resources.Load<AudioClip>(MusicFilePath);       

        JsonNode json = JsonNode.Parse(jsonText);
        title = json["title"].Get<string>();
        keyboard = json["keyboard"].Get<string>();
        beat = int.Parse(json["beat"].Get<string>());
        BPM = int.Parse(json["bpm"].Get<string>());

        JsonRhythm jsonRhythm = JsonUtility.FromJson<JsonRhythm>(jsonText);
        List<note> rhythmNotes = new List<note>();
        rhythmNotes.AddRange(jsonRhythm.notes);

        rhythmNotes.Sort((a, b) => a.timing - b.timing);

        foreach (var note in json["notes"])
        {
            string type = note["type"].Get<string>();
            int lane = int.Parse(note["lane"].Get<string>());
            float timing = float.Parse(note["timing"].Get<string>());
            float height = float.Parse(note["length"].Get<string>());
            Vector2 size = new Vector2(1, height);

            GameObject Note = null;

            switch (type)
            {
                case "Apart":
                    Note = Instantiate(Apart, lanes[lane].position, Quaternion.identity);
                    break;

                case "Bpart":
                    Note = Instantiate(Bpart, lanes[lane].position, Quaternion.identity);
                    break;

                case "Cpart":
                    Note = Instantiate(Cpart, lanes[lane].position, Quaternion.identity);
                    break;

                default:
                    Debug.LogError("Not found type.");
                    break;
            }

            SpriteRenderer renderer = Note.GetComponent<SpriteRenderer>();
            BoxCollider2D collider2D = Note.GetComponent<BoxCollider2D>();
            float baseHeight = renderer.bounds.size.y;

            renderer.size = size;
            collider2D.size = size;


            Vector2 basePos = lanes[lane].position;
            Vector2 initPos = new Vector2(basePos.x, basePos.y - ((baseHeight * size.y) - baseHeight));

            Note.transform.position = initPos;

            NoteController noteController = Note?.GetComponent<NoteController>();
            if (noteController != null)
            {
                noteController.SetParameter(type, timing, lane);
                noteController.GTime = GTime;

                notes.Add(noteController);
                NoteTimings.Add(timing);
            }

            //if (Note != null)
            //{
            //    notes.Add(Note);
            //    NoteTimings.Add(timing);
            //}
        }
        //Debug.Log("Created notes :" + notes.Count);
    }

    private void Play()
    {
        if (isPlaying)
        {
            //Music.UnPause();
            midiPlayer.UnPause();
        }
        else
        {
            //Music.Stop();
            //Music.Play();
            //midiPlayer.Play();
            playTime = GTime.MSTime;
            isPlaying = true;
            //Debug.Log("Game Start!");
            MusicPlay();
        }

        //Music.Stop();
        //Music.Play();
        //playTime = Time.time * 1000;
        //playTime = GTime.MSTime;
        //isPlaying = true;
        //Debug.Log("Game Start!");
    }

    async void MusicPlay()
    {
        const float WAIT = 1.5f * 1000f;
        await Task.Run(() => { while (GTime.MSTime < WAIT) { } });
        midiPlayer.Play();
    }

    private void Stop()
    {
        if (isPlaying)
        {
            midiPlayer.Pause();
            //Music.Pause();
        }
    }

    private void Beat(string type, float timing)
    {
        float minDiff = -1;
        int minDiffIndex = -1;

        for (int i = 0; i < NoteTimings.Count; i++)
        {
            if (NoteTimings[i] > 0)
            {
                float diff = Math.Abs(NoteTimings[i] - timing);
                if (minDiff == -1 || minDiff > diff)
                {
                    minDiff = diff;
                    minDiffIndex = i;
                }
            }
        }

        if (minDiff != -1 & minDiff < checkRange)
        {
            if (minDiff < beatRange & notes[minDiffIndex].GetComponent<NoteController>().Type == type)
            {
                NoteTimings[minDiffIndex] = -1;
                notes[minDiffIndex].SetActive(false);

                MessageEffectSubject.OnNext("good");
                Debug.Log("beat " + type + " success.");
            }
            else
            {
                NoteTimings[minDiffIndex] = -1;
                notes[minDiffIndex].SetActive(false);

                MessageEffectSubject.OnNext("failure");
                Debug.Log("beat " + type + " failure.");
            }
        }
        else
        {
            Debug.Log("through");
        }
    }
}
