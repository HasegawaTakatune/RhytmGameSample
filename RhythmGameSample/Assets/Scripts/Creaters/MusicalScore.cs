using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    [System.Serializable]
    public class MusicalScore
    {
        public string title;
        public int keyboard;
        public int beat;
        public int bpm;
        [SerializeField] public List<Notes> notes;
    }

    [System.Serializable]
    public class Notes
    {
        public const string Apart = "Apart";
        public const string Bpart = "Bpart";
        public const string Cpart = "Cpart";

        public string timing;
        public string type;
        public int lane;
        public string length;
    }
}
