using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using edu.tnu.dgd.game;

namespace edu.tnu.dgd.login
{
    public class LoginChecking : MonoBehaviour
    {

        [HideInInspector]
        public bool hasLogined = false;



        private static LoginChecking _instance;

        public static LoginChecking instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<LoginChecking>();
                }

                return _instance;
            }
        }

        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        private void Start()
        {
            InitializedSetting();
        }

        public void InitializedSetting()
        {
            if (PlayerPrefs.HasKey(StringConstants.Setting_ForkliftSFX) == false)
            {
                PlayerPrefs.SetInt(StringConstants.Setting_ForkliftSFX, 1);
            }

            if (PlayerPrefs.HasKey(StringConstants.Setting_ForkliftSFXVolume) == false)
            {
                PlayerPrefs.SetInt(StringConstants.Setting_ForkliftSFXVolume, 3);
            }

            if (PlayerPrefs.HasKey(StringConstants.Setting_GuideDisplay) == false)
            {
                PlayerPrefs.SetInt(StringConstants.Setting_GuideDisplay, 0);
            }

            if (PlayerPrefs.HasKey(StringConstants.Setting_GuideSpeech) == false)
            {
                PlayerPrefs.SetInt(StringConstants.Setting_GuideSpeech, 1);
            }

            if (PlayerPrefs.HasKey(StringConstants.Setting_GuideSpeechVolume) == false)
            {
                PlayerPrefs.SetInt(StringConstants.Setting_GuideSpeechVolume, 5);
            }

            if (PlayerPrefs.HasKey(StringConstants.Setting_NoGasMaxSpeed) == false)
            {
                PlayerPrefs.SetInt(StringConstants.Setting_NoGasMaxSpeed, 2);
            }

            if (PlayerPrefs.HasKey(StringConstants.Setting_SpeechType) == false)
            {
                PlayerPrefs.SetInt(StringConstants.Setting_SpeechType, 0);
            }
        }

    }
}