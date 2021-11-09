using System;
using edu.tnu.dgd.util;
using UnityEngine;

namespace edu.tnu.dgd.scene
{
    [Serializable]
    public class PreviousScene
    {
        [SerializeField]
        private string _prevScene;

        public string prevScene
        {
            get
            {
                return _prevScene;
            }
        }


        public PreviousScene(string scene)
        {
            _prevScene = scene;
        }

        public static string Load()
        {
            string sc = PlayerPrefs.GetString("PrevScene");
            if (sc != null)
            {
                return sc;
            }

            return "";

        }


        public void Save()
        {
            PlayerPrefs.SetString("PrevScene", prevScene);
            PlayerPrefs.Save();
        }
    }
}
