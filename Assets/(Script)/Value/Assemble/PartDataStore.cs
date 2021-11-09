using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using edu.tnu.dgd.util;
using UnityEngine.UI;


namespace edu.tnu.dgd.value
{

    public class PartDataStore
    {
        private Dictionary<string, PartData> dataStore;
        private static PartDataStore _instance;


        public static PartDataStore instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PartDataStore();
                }
                return _instance;
            }
        }

        public PartDataStore()
        {
            dataStore = new Dictionary<string, PartData>();
            PartData[] parts = ReadFromAsset();
            for (int i = 0; i < parts.Length; i++)
            {
                dataStore.Add(parts[i].nameCh, parts[i]);
            }
        }

        public PartData FindDataByNameCh(string name)
        {
            PartData ret;
            if (dataStore.TryGetValue(name, out ret))
            {
                return ret;
            }

            return null;
        }

        public static PartData[] ReadFromAsset()
        {
            TextAsset ta = Resources.Load<TextAsset>("PartData/part_data");
            string jsonStr = ta.text;

            //string path = Path.Combine(Application.dataPath, "Resources", "PartData", "part_data.json");

            //string jsonStr = File.ReadAllText(path);
            PartData[] parts = JsonHelper.fromJson<PartData[]>(jsonStr);
            Debug.Log(parts.Length);

            return parts;
        }
    }
}
