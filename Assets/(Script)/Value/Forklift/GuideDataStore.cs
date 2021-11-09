using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using edu.tnu.dgd.util;


namespace edu.tnu.dgd.value
{
    public class GuideDataStore
    {
        private Dictionary<string, GuideData> dataStore;
        private static GuideDataStore _instance;
        public static GuideDataStore instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GuideDataStore();
                }
                return _instance;
            }
        }

        public GuideDataStore()
        {
            dataStore = new Dictionary<string, GuideData>();

            GuideData[] guides = ReadFromAsset("forklift_basic_guide");
            for (int i = 0; i < guides.Length; i++)
            {
                guides[i].guideDataType = GuideDataType.Basic;
                dataStore.Add(MakeKey(GuideDataType.Basic, guides[i].id), guides[i]);
            }

            guides = ReadFromAsset("forklift_advanced_guide");
            for (int i = 0; i < guides.Length; i++)
            {
                guides[i].guideDataType = GuideDataType.Advanced;
                dataStore.Add(MakeKey(GuideDataType.Advanced, guides[i].id), guides[i]);
            }
        }

        private string MakeKey(GuideDataType guideType, int guideId)
        {
            string guideName = "bas";
            if (guideType == GuideDataType.Advanced)
            {
                guideName = "adv";
            }
            return guideName + ":" + guideId;
        }

        public GuideData FindDataById(GuideDataType guideType, int guideId)
        {
            string key = MakeKey(guideType, guideId);

            GuideData ret;
            if (dataStore.TryGetValue(key, out ret))
            {
                return ret;
            }

            return null;
        }

        private GuideData[] ReadFromAsset(string guideFileName)
        {
            TextAsset ta = Resources.Load<TextAsset>("Data/" + guideFileName);
            string jsonStr = ta.text;

            //string path = Path.Combine(Application.dataPath, "Resources", "Process", "process.json");
            //string jsonStr = File.ReadAllText(path);
            GuideData[] parts = JsonHelper.fromJson<GuideData[]>(jsonStr);

            return parts;
        }
    }
}
