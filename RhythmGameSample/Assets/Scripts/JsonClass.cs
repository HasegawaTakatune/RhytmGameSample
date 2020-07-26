using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class JsonRhythm
{
    public string title;
    public int beat;
    public int bpm;
    public int keyboard;
    public note[] notes;
}

[Serializable]
public class note
{
    public float length;
    public string type;
    public int lane;
    public int timing;
}
