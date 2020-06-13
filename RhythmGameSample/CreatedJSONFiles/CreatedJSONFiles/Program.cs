using System;
using System.IO;
using System.Runtime.Serialization.Json;

namespace CreatedJSONFiles
{
    public class MusicalScore
    {
        public string title { get; set; }
        public string keyboard { get; set; }
        public string beat { get; set; }
        public string bpm { get; set; }
        public Notes[] notes;
    }

    public class Notes
    {
        public string timing { get; set; }
        public string type { get; set; }
        public string lane { get; set; }
    }

    public class CreateJson
    {
        private const double mm = 60;
        private const double ms = 1000;

        private const int LENGTH = 5000;

        private const string Apart = "Apart";
        private const string Bpart = "Bpart";
        private const string Cpart = "Cpart";

        private const int TypeLength = 3;
        private const int LaneLength = 9;

        private static void Main(string[] args)
        {
            CreateJson create = new CreateJson();
            create.OutJSONFile();
        }


        public void OutJSONFile()
        {
            double BPM = 135;
            double endTime = (2 * mm * ms) + (36 * ms);
            double beatTime = mm * ms / BPM;
            double timer = 5000;

            Random typeRand = new Random();
            Random laneRand = new Random();

            MusicalScore item = new MusicalScore();

            item.title = "Sample";
            item.keyboard = "32";
            item.beat = "4";
            item.bpm = BPM.ToString();

            item.notes = new Notes[LENGTH];
            for (int i = 0; i < LENGTH; i++)
            {
                item.notes[i] = new Notes();

                timer += beatTime;
                item.notes[i].timing = timer.ToString();

                switch (typeRand.Next(TypeLength))
                {
                    case 0: item.notes[i].type = Apart; break;
                    case 1: item.notes[i].type = Bpart; break;
                    case 2: item.notes[i].type = Cpart; break;
                }

                item.notes[i].lane = laneRand.Next(LaneLength).ToString();
            }

            using (var stream = new MemoryStream())
            {
                var serializer = new DataContractJsonSerializer(typeof(MusicalScore));
                serializer.WriteObject(stream, item);

                stream.Position = 0;
                StreamReader reader = new StreamReader(stream);
                Console.WriteLine(reader.ReadToEnd());
            }
        }
    }
}
