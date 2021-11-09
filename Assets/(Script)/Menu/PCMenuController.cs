using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using edu.tnu.dgd.login;
using TMPro;
using edu.tnu.dgd.game;

namespace edu.tnu.dgd.menu
{
    
    public class PCMenuController: MonoBehaviour
    {
        public GameObject loginObject;
        public GameObject menuPanel;
        public GameObject loadingPanel;
        private TMP_Text loadingText;

        private const string settingString = "畫面載入中";
        private const string loadingString = "系統載入中";
        private const string quitAppString = "系統登出中";

        private static PCMenuController _instance;

        public static PCMenuController instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<PCMenuController>();
                }
                return _instance;
            }
        }

        private void Awake()
        {
            Assert.IsNotNull(loginObject);
            Assert.IsNotNull(menuPanel);
            Assert.IsNotNull(loadingPanel);

            loadingText = loadingPanel.transform.Find("Text").GetComponent<TMP_Text>();
            loadingText.text = loadingString;

        }

        private void Start()
        {

            if (LoginChecking.instance.hasLogined == false)
            {
                ShowLoginPanel(true);
                LoginController login = loginObject.GetComponent<LoginController>();
                login.useVR = false;
                login.onClose.AddListener(RestoreMenu);
            }
            else
            {
                ShowLoginPanel(false);
            }

        }

        private void ShowLoginPanel(bool show)
        {
            loginObject.SetActive(show);
            menuPanel.SetActive(!show);
        }

        public void RestoreMenu()
        {
            menuPanel.SetActive(true);
        }

        private void ShowLoading(string text)
        {
            loadingText.text = text;
            menuPanel.SetActive(false);
            loadingPanel.SetActive(true);

        }


        public void LaunchMainMenu()
        {
            PlayerPrefs.SetString("PrevActivity", PlayerPrefs.GetString("CurrActivity", StringConstants.Scene_Default));
            PlayerPrefs.SetString("CurrActivity", StringConstants.Activity_MainMenu);

            ShowLoading(settingString);

            SceneManager.LoadSceneAsync(StringConstants.Scene_MainMenu);
        }

        public void LaunchComponentBrief()
        {
            PlayerPrefs.SetString("PrevActivity", PlayerPrefs.GetString("CurrActivity", StringConstants.Scene_Default));
            PlayerPrefs.SetString("CurrActivity", StringConstants.Activity_ComponentBrief);

            ShowLoading(loadingString);
            
            SceneManager.LoadSceneAsync(StringConstants.Scene_ComponentBrief);
        }

        public void LaunchBasicTraining()
        {
            PlayerPrefs.SetString("PrevActivity", PlayerPrefs.GetString("CurrActivity", StringConstants.Scene_Default));
            PlayerPrefs.SetString("CurrActivity", StringConstants.Activity_BasicTraining);

            ShowLoading(loadingString);
            SceneManager.LoadSceneAsync(StringConstants.Scene_BasicGame);
        }

        public void LaunchAdvTraining()
        {
            PlayerPrefs.SetString("PrevActivity", PlayerPrefs.GetString("CurrActivity", StringConstants.Scene_Default));
            PlayerPrefs.SetString("CurrActivity", StringConstants.Activity_AdvanceTraining);

            ShowLoading(loadingString);
            SceneManager.LoadSceneAsync(StringConstants.Scene_AdvanceGame);
        }

        public void LaunchDrivingTest()
        {
            PlayerPrefs.SetString("PrevActivity", PlayerPrefs.GetString("CurrActivity", StringConstants.Scene_Default));
            PlayerPrefs.SetString("CurrActivity", StringConstants.Activity_DrivingTest);

            ShowLoading(loadingString);
            SceneManager.LoadSceneAsync(StringConstants.Scene_DrivingTest);
        }

        public void LaunchSetting()
        {
            PlayerPrefs.SetString("PrevActivity", PlayerPrefs.GetString("CurrActivity", StringConstants.Scene_Default));
            PlayerPrefs.SetString("CurrActivity", StringConstants.Activity_Setting);

            ShowLoading(settingString);
            SceneManager.LoadSceneAsync(StringConstants.Scene_Setting);
        }

        public void QuitApplication()
        {
            ShowLoading(quitAppString);
            Application.Quit();
        }

    }
}
