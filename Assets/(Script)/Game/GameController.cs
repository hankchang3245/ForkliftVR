using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using edu.tnu.dgd.scene;
using UnityEngine.SceneManagement;
using TMPro;
using edu.tnu.dgd.forklift;
using edu.tnu.dgd.vehicle;
using edu.tnu.dgd.vrlearn;
using edu.tnu.dgd.value;
using edu.tnu.dgd.project.forklift;

namespace edu.tnu.dgd.game
{
    public class GameController : MonoBehaviour
    {

        public bool isTrainingTest = false;
        public TMP_Text learnerId;
        public TMP_Text headTextLine1;
        public TMP_Text headTextLine2;

        public GuideDataType stationType = GuideDataType.Basic;

        [HideInInspector]
        public bool isLogout = false;

        private bool _startElapsedTimeTimer = false;

        public bool startElapsedTimeTimer
        {
            get
            {
                return _startElapsedTimeTimer;
            }

            set
            {
                _startElapsedTimeTimer = value;
            }
        }

        private string _mission;
        private float _progress;
        private float _elapsedTime = 0f; // sec
        private int _failedCount;
        private bool _enableGuide = true;

        public ForkliftController forkliftController;
        public VehicleController vehicleController;
        public GuideController guideController;
        public SafetyBeltController safetyBeltController;

        [Header("若為第三站，以下欄位不得為空!")]
        public PalletAndOilDrum origOilDrum;
        public PalletAndOilDrum forkOilDrum;

        private static GameController _instance;
        public static GameController instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<GameController>();
                }
                return _instance;
            }
        }

        public string mission
        {
            get
            {
                return _mission;
            }

            set
            {
                _mission = value;
                if (headTextLine1 != null)
                {
                    if ("0".Equals(_mission))
                    {
                        headTextLine1.text = string.Format("準備開始操作...");
                    }
                    else
                    {
                        headTextLine1.text = string.Format("任務：{0}    完成：{1}%", _mission, Mathf.FloorToInt(_progress));
                    }
                }
                
            }
        }

        public float progress
        {
            get
            {
                return _progress;
            }

            set
            {
                _progress = value;
                if (_progress < 0)
                {
                    _progress = 0;
                }
                if (headTextLine1 != null)
                {
                    if ("0".Equals(_mission))
                    {
                        headTextLine1.text = string.Format("準備開始操作...");
                    }
                    else
                    {
                        headTextLine1.text = string.Format("任務：{0}    完成：{1}%", _mission, Mathf.FloorToInt(_progress));
                    }
                }
            }
        }

        public int elapsedTime
        {
            get
            {
                return Mathf.FloorToInt(_elapsedTime);
            }
        }

        public int failedCount
        {
            get
            {
                return _failedCount;
            }

            set
            {
                _failedCount = value;
                if (headTextLine1 != null)
                {
                    headTextLine2.text = string.Format("時間：{0}    失誤：{1}", TimeToString(Mathf.FloorToInt(_elapsedTime)), _failedCount);
                }
            }
        }

        public bool enableGuide
        {
            get
            {
                return _enableGuide;
            }

            set
            {
                _enableGuide = value;
            }
        }

        /*
        public float noGasMaxSpeed
        {
            get
            {
                return PlayerPrefs.GetInt(StringConstants.Setting_NoGasMaxSpeed, 2);
            }
        }

        public float forkliftSFXVolume
        {
            get
            {
                return PlayerPrefs.GetInt(StringConstants.Setting_ForkliftSFXVolume, 3) / 10f;
            }
        }
        
        public bool enableForkliftSFX
        {
            get
            {
                return PlayerPrefs.GetInt(StringConstants.Setting_ForkliftSFX, 0) > 0;
            }
        }
        */
        

        private void Start()
        {
            /*
            bool playSfx = PlayerPrefs.GetInt(StringConstants.Setting_ForkliftSFX, 0) > 0;
            float volume = PlayerPrefs.GetInt(StringConstants.Setting_ForkliftSFXVolume, 3) / 10f;

            forkliftController.playSFX = playSfx;
            forkliftController.sfxVolume = volume;

            vehicleController.playSFX = playSfx;
            vehicleController.sfxVolume = volume;

            vehicleController.NoGasMaxSpeed = PlayerPrefs.GetInt(StringConstants.Setting_NoGasMaxSpeed, 2);


            bool playSfx = PlayerPrefs.GetInt(StringConstants.Setting_GuideSpeech, 1) > 0;
            float volume = PlayerPrefs.GetInt(StringConstants.Setting_GuideSpeechVolume, 5) / 10f;

            guideController.playSFX = playSfx && !isTrainingTest; // 如果是測驗模擬就沒有引導說明語音及文字
            guideController.sfxVolume = volume;
            */

            Learner ln = Learner.Load();
            if (ln != null)
            {
                learnerId.text = ln.id;
            }


            // 油桶
            if (origOilDrum != null) origOilDrum.gameObject.SetActive(true);
            if (forkOilDrum != null) forkOilDrum.gameObject.SetActive(false);


            InvokeRepeating("UpdateTimer", 10f, 30f);
        }

        void UpdateTimer()
        {
            headTextLine2.text = string.Format("時間：{0}    失誤：{1}", TimeToString(Mathf.FloorToInt(_elapsedTime)), _failedCount);
        }

        private string TimeToString(int time)
        {
            int min = Mathf.FloorToInt(time / 60);
            int sec = Mathf.FloorToInt(time - min * 60);
            return "" + min + ":" + sec.ToString().PadLeft(2, '0');
        }

        public void Init()
        {
            mission = "";
            progress = 0f;
            _elapsedTime = 0f;
            failedCount = 0;
        }

        private float timerAcculator = 0f; // 為了節省顯示"時間"的UI，當有增加一秒時才顯示
        private void Update()
        {
            if (_startElapsedTimeTimer)
            {
                _elapsedTime += Time.deltaTime;
            }
        }

        public void LaunchBasicTraining()
        {
            PlayerPrefs.SetString("PrevActivity", PlayerPrefs.GetString("CurrActivity", StringConstants.Scene_Default));
            PlayerPrefs.SetString("CurrActivity", StringConstants.Activity_BasicTraining);
            SceneManager.LoadSceneAsync(StringConstants.Scene_BasicGame);
        }

        public void LaunchAdvTraining()
        {
            PlayerPrefs.SetString("PrevActivity", PlayerPrefs.GetString("CurrActivity", StringConstants.Scene_Default));
            PlayerPrefs.SetString("CurrActivity", StringConstants.Activity_AdvanceTraining);
            SceneManager.LoadSceneAsync(StringConstants.Scene_AdvanceGame);
        }

        public void LaunchMainMenu()
        {
            PlayerPrefs.SetString("PrevActivity", PlayerPrefs.GetString("CurrActivity", StringConstants.Scene_Default));
            PlayerPrefs.SetString("CurrActivity", StringConstants.Activity_MainMenu);

            SceneManager.LoadSceneAsync(StringConstants.Scene_MainMenu);
        }

        public void CleanObstacle()
        {
            
            guideController.ResetStation();
        }

        public void ResetVehiclePosition()
        {
            vehicleController.ResetPosition();
        }

        public void RestartLevel()
        {
            /*
            Init();
            ResetVehiclePosition();
            CleanObstacle();
            */
            DontDestroyOnLoadController.instance.RestartLevel();
        }

        private void OnDestroy()
        {
            _instance = null;
        }

        public void GoToPosition(int i)
        {
            if (stationType == GuideDataType.Basic && i < 2)
            {
                ChangeForkliftLocation(ForkliftRelocationController.instance.GetLocation(i));
            } 
            else if (stationType == GuideDataType.Advanced && i < 8)
            {
                if (i < 2) // 油桶要放在原來地上
                {
                    if (origOilDrum != null && forkOilDrum != null)
                    {
                        origOilDrum.gameObject.SetActive(true);
                        origOilDrum.ResetTransform();
                        forkOilDrum.gameObject.SetActive(false);
                    }
                    ChangeForkliftLocation(ForkliftRelocationController.instance.GetLocation(i));
                }
                else // i >= 2 // 油桶要放在貨叉上
                {
                    if (origOilDrum != null && forkOilDrum != null)
                    {
                        origOilDrum.gameObject.SetActive(false);
                        forkOilDrum.gameObject.SetActive(true);
                        forkOilDrum.ResetTransform();
                    }
                    ChangeForkliftLocation(ForkliftRelocationController.instance.GetLocation(i));
                }

            }
        }

        private void RemoveAllChildren(Transform parent)
        {
            if (parent.childCount == 0)
            {
                return;
            }

            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                Destroy(parent.GetChild(i).gameObject);
            }
        }


        private void ChangeForkliftLocation(GameObject obj)
        {
            vehicleController.gameObject.transform.position = obj.transform.position;
            vehicleController.gameObject.transform.rotation = obj.transform.rotation;
            RelatedGuidePoint rgp = obj.GetComponent<RelatedGuidePoint>();
            GuideController.instance.GoToFireGuidePoint(rgp.relatedGuidePoint.childIndex);
        }

    }
}