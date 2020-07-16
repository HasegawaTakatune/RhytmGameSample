using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SelectedMusic : MonoBehaviour
{
    private const string MIDI = ".mid";
    private const string JSON = ".json";

    [SerializeField] private Dropdown musicDropdown;
    [SerializeField] private GameManager manager;

    private void Reset()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        musicDropdown = GetComponent<Dropdown>();
    }

    private void Start()
    {
        musicDropdown.ClearOptions();

        string[] files = Directory.GetFiles(Application.streamingAssetsPath, "*" + MIDI, SearchOption.AllDirectories);
        List<string> options = new List<string>();

        for (int i = 0; i < files.Length; i++)
        {
            string fileName = Path.GetFileNameWithoutExtension(files[i]);

            if (File.Exists(Application.streamingAssetsPath + "/" + fileName + JSON))
                options.Add(fileName);
        }

        musicDropdown.AddOptions(options);
    }

    public void OnValueChanged(Dropdown input)
    {
        string fileName = input.options[input.value].text;
        manager.JsonFilePath = fileName + MIDI;
        manager.MusicFilePath = fileName + JSON;
    }

}
