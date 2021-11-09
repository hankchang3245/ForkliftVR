using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using edu.tnu.dgd.value;
using edu.tnu.dgd.project.forklift;
using UnityEngine.Assertions;
using TMPro;
using edu.tnu.dgd.vrlearn;
using edu.tnu.dgd.forklift;

namespace edu.tnu.dgd.game
{
    public class GuideController : MonoBehaviour
    {
        public bool enableGuidePoint = true;
        public StationProperty stationProperty;
        public GameObject forkliftCarObject;

        [HideInInspector]
        public bool playSFX = true;

        [HideInInspector]
        public float sfxVolume = 0.5f;

        private GameObject guideStepsObject;
        //private int prevChildIndex = -1;
        private int _currentChildIndex = -1;
        private GuidePoint[] guidePointList;

        private TMP_Text guideRemarkText;
        private bool showGuidePanel = false;

        private static GuideController _instance;
        public SafetyBeltController safetyBeltController;

        private AudioSource audioSource;
        private bool hasFinished = false;

        public GameController _gameController;

        public static GuideController instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<GuideController>();
                }

                return _instance;
            }
        }

        private void Awake()
        {

            bool playSfx = PlayerPrefs.GetInt(StringConstants.Setting_GuideSpeech, 1) > 0;
            float volume = PlayerPrefs.GetInt(StringConstants.Setting_GuideSpeechVolume, 5) / 10f;

            this.playSFX = playSfx && !_gameController.isTrainingTest; // 如果是測驗模擬就沒有引導說明語音及文字
            this.sfxVolume = volume;

            string selectedActivity = PlayerPrefs.GetString("CurrActivity");

            Assert.IsNotNull(stationProperty);
            Assert.IsNotNull(forkliftCarObject);

            guideStepsObject = forkliftCarObject.transform.Find("UI/Display/GuideSteps").gameObject;
            guideRemarkText = guideStepsObject.transform.Find("Canvas/ContentText").GetComponent<TMP_Text>();
            Init();
        }
        private void Start()
        {
            ShowHideGuidePanel(PlayerPrefs.GetInt(StringConstants.Setting_GuideDisplay, 0) > 0);
        }

        public void ResetStation()
        {
            stationProperty.ResetTrafficPoleList();
        }

        public void StoreAllTrafficPolePosition()
        {
            stationProperty.StoreAllTrafficPolePosition();
        }

        public void Init()
        {
            //Debug.Log(">>>>>>>>>>>>>>>>>>>>safetyBeltController.hasFastenSafetyBelt=" + safetyBeltController.hasFastenSafetyBelt);
            if(safetyBeltController.hasFastenSafetyBelt)
            {
                SetCurrentChildIndex(0);
            }
            else
            {
                SetCurrentChildIndex(-1);
            }

            guidePointList = stationProperty.PrepareGuidePoints();

            GameController.instance.Init();
            EnableNextGuidePointCollider();
        }

        public void SetCurrentChildIndex(int idx)
        {
            _currentChildIndex = idx;
            ForkliftRelocationController.instance.CurrentChildIndexChanged(_currentChildIndex);
        }

        public void EnableNextGuidePointCollider()
        {
            DisableCurrentPointCollider();

            if (_currentChildIndex < 0)
            {
                GameController.instance.progress = 0;
            }

            if (_currentChildIndex < (guidePointList.Length-1))
            {
                SetCurrentChildIndex(_currentChildIndex+1);

                ShowStatus();
                guidePointList[_currentChildIndex].gameObject.SetActive(enableGuidePoint); 

                if (_currentChildIndex == 0) // 當第0個GuidePoint執行完成後，自動跳到第1個GuidePoint
                {
                    InvokeRepeating("EnableFirstGuidePoint", 5f, 5f);
                }
            }
            else
            {
                hasFinished = true; // 已完成
            }
        }

        private void ShowStatus()
        {
            int idx = _currentChildIndex >= 1 ? (_currentChildIndex - 1) : 0;
            
            
            if (hasFinished)
            {
                GameController.instance.mission = "--";
                GameController.instance.progress = 100f;
            }
            else
            {
                GameController.instance.mission = "" + (guidePointList[_currentChildIndex].childIndex);
                GameController.instance.progress = (idx) * 100f / (guidePointList.Length-1);
            }
        }

        private void EnableFirstGuidePoint()
        {
            if (_currentChildIndex == 0 && safetyBeltController.hasFastenSafetyBelt) // 當第0個GuidePoint執行完成後，自動跳到第1個GuidePoint
            {
                EnableNextGuidePointCollider();
                CancelInvoke("EnableFirstGuidePoint");
            }
        }

        private void DisableCurrentPointCollider()
        {
            if (_currentChildIndex >= 0 && _currentChildIndex < guidePointList.Length)
            {
                guidePointList[_currentChildIndex].gameObject.SetActive(false);
            }
        }

        public void GoToFireGuidePoint(int childIndex)
        {
            if (childIndex < 0 || childIndex >= guidePointList.Length)
            {
                return;
            }

            // stop current guide point
            guidePointList[_currentChildIndex].gameObject.SetActive(false);
            guidePointList[childIndex].gameObject.SetActive(true);

            FireGuidePoint(guidePointList[childIndex]);
            ShowStatus();
        }


        public void FireGuidePoint(GuidePoint gp)
        {
            SetCurrentChildIndex(gp.childIndex);
            if (enabled && gp.fired == false)
            {
                gp.fired = true;
                if (playSFX && !GameController.instance.isTrainingTest) // GameController.instance.isTrainingTest: 如果是測驗模擬就不撥放引導說明
                {
                    SoundType st = (PlayerPrefs.GetInt(StringConstants.Setting_SpeechType) > 0) ? 
                                                        SoundType.Male : SoundType.Female;

                    edu.tnu.dgd.audio.AudioController.instance.SoundVolume = sfxVolume;
                    AudioClip clip = edu.tnu.dgd.audio.AudioController.instance.LoadClip(gp.GetAudioClipPath(st));

                    if (audioSource != null && audioSource.isPlaying)
                    {
                        audioSource.Stop();
                    }
                    audioSource = edu.tnu.dgd.audio.AudioController.instance.PlayOneShot(clip);
                }
                Transform tr = ForkliftController.instance.gameObject.transform;

                LearnController.instance.WriteExperience(Experience.VERB.FORKLIFT_REACH_TASK, gp.gameObject.name, typeof(GuidePoint).ToString(), tr, GameController.instance.elapsedTime);
                
                ShowHideGuidePanel(showGuidePanel);
                GuideData gd = GuideDataStore.instance.FindDataById(gp.guideType, gp.guideId);
                guideRemarkText.text = gd.GetFullRemarkString();

            }
        }
        
        public void FireStopGuidePoint(GuidePoint gp)
        {
            //prevChildIndex = gp.childIndex;
            if (enabled)
            {
                //GuideData gd = GuideDataStore.instance.FindDataById(gp.guideType, gp.guideId);
                guideRemarkText.text = "";
                guideStepsObject.SetActive(false);
            }
            EnableNextGuidePointCollider();
        }
        
        public void ShowHideGuidePanel(bool val)
        {
            showGuidePanel = val;
            guideStepsObject.SetActive(showGuidePanel && !GameController.instance.isTrainingTest);
        }
       
    }
}

