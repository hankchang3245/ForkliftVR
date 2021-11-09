using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using edu.tnu.dgd.util;


namespace edu.tnu.dgd.value
{

    public class AssembleProcessStore
    {
        private Dictionary<string, AssembleProcess> dataStore;
        private static AssembleProcessStore _instance;

        public static AssembleProcessStore instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AssembleProcessStore();
                }
                return _instance;
            }
        }

        public AssembleProcessStore()
        {
            dataStore = new Dictionary<string, AssembleProcess>();
            AssembleProcess[] parts = ReadFromAsset();
            for (int i = 0; i < parts.Length; i++)
            {
                dataStore.Add(parts[i].nameCh, parts[i]);
            }
        }

        public List<string> GetAllAudioClipNames()
        {
            List<string> result = new List<string>();

            foreach (AssembleProcess ap in dataStore.Values)
            {
                foreach (string f in ap.step0_voices)
                {
                    result.Add(f);
                }

                foreach (string f in ap.step_voices)
                {
                    result.Add(f);
                }
            }

            return result;
        }

        public AssembleProcess FindDataByName(string name)
        {
            AssembleProcess ret;
            if (dataStore.TryGetValue(name, out ret))
            {
                return ret;
            }

            return null;
        }

        public static AssembleProcess[] ReadFromAsset()
        {
            TextAsset ta = Resources.Load<TextAsset>("Process/process");
            string jsonStr = ta.text;

            //string path = Path.Combine(Application.dataPath, "Resources", "Process", "process.json");
            //string jsonStr = File.ReadAllText(path);
            AssembleProcess[] parts = JsonHelper.fromJson<AssembleProcess[]>(jsonStr);
            Debug.Log(parts.Length);

            return parts;
        }
    }
}
