using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using edu.tnu.dgd.game;
using edu.tnu.dgd.login;

namespace edu.tnu.dgd.menu
{
    
    public class MainMenuController: MonoBehaviour
    {
        public bool useVR = false;
        
        public bool showMainMenuOnAwake = false;
        //public bool showToggleButton = true;

        private GameObject mainMenu;


        private MainMenuItem _selectedItem;

        public MainMenuItem selectedItem
        {
            set
            {
                if (_selectedItem != value)
                {
                    if (_selectedItem != null)
                        _selectedItem.DisableSelected();

                    _selectedItem = value;
                    if (_selectedItem != null)
                    {
                        _selectedItem.EnableSelected();
                    }
                }
            }

            get
            {
                return _selectedItem;
            }
        }

        public GameObject loginObject;

        private static MainMenuController _instance;
        public static MainMenuController instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<MainMenuController>();
                }
                return _instance;
            }
        }

        private void Awake()
        {
            mainMenu = this.transform.Find("MainMenu").gameObject;
        }

        private void Start()
        {
            
            Assert.IsNotNull(mainMenu);
            Assert.IsNotNull(loginObject);
            
            mainMenu.SetActive(LoginChecking.instance.hasLogined);
            loginObject.SetActive(!LoginChecking.instance.hasLogined);
             
        }

/*
        public void ToggleMainMenu(InteractableStateArgs obj)
        {
            if (obj.NewInteractableState == InteractableState.ActionState)
            {
                mainMenu.SetActive(!mainMenu.activeSelf);
            }
        }
*/
/*
        public void LaunchComponentBrief(InteractableStateArgs obj)
        {
            if (obj.NewInteractableState == InteractableState.ActionState)
            {
                LaunchComponentBrief();
            }
        }
*/
        public void LaunchComponentBrief()
        {
            PlayerPrefs.SetString("PrevActivity", PlayerPrefs.GetString("CurrActivity", StringConstants.Scene_Default));

            PlayerPrefs.SetString("CurrActivity", StringConstants.Activity_ComponentBrief);
            SceneManager.LoadSceneAsync(StringConstants.Scene_ComponentBrief);
        }

/*
        public void LaunchBasicTraining(InteractableStateArgs obj)
        {
            if (obj.NewInteractableState == InteractableState.ActionState)
            {
                LaunchBasicTraining();
            }
        }
*/

        public void LaunchBasicTraining()
        {
            PlayerPrefs.SetString("PrevActivity", PlayerPrefs.GetString("CurrActivity", StringConstants.Scene_Default));
            PlayerPrefs.SetString("CurrActivity", StringConstants.Activity_BasicTraining);

            SceneManager.LoadSceneAsync(StringConstants.Scene_BasicGame);
        }

/*
        public void LaunchAdvTraining(InteractableStateArgs obj)
        {
            if (obj.NewInteractableState == InteractableState.ActionState)
            {
                LaunchAdvTraining();
            }
        }
*/

        public void LaunchAdvTraining()
        {
            PlayerPrefs.SetString("PrevActivity", PlayerPrefs.GetString("CurrActivity", StringConstants.Scene_Default));
            PlayerPrefs.SetString("CurrActivity", StringConstants.Activity_AdvanceTraining);

            SceneManager.LoadSceneAsync(StringConstants.Scene_AdvanceGame);
        }

/*
        public void LaunchDrivingTest(InteractableStateArgs obj)
        {
            if (obj.NewInteractableState == InteractableState.ActionState)
            {
                LaunchDrivingTest();
            }
        }
*/

        public void LaunchDrivingTest()
        {
            PlayerPrefs.SetString("PrevActivity", PlayerPrefs.GetString("CurrActivity", StringConstants.Scene_Default));
            PlayerPrefs.SetString("CurrActivity", StringConstants.Activity_DrivingTest);

            SceneManager.LoadSceneAsync(StringConstants.Scene_DrivingTest);
        }

/*
        public void LaunchSetting(InteractableStateArgs obj)
        {
            if (obj.NewInteractableState == InteractableState.ActionState)
            {
                LaunchSetting();
            }            
        }
*/

        public void LaunchSetting()
        {
            PlayerPrefs.SetString("PrevActivity", PlayerPrefs.GetString("CurrActivity", StringConstants.Scene_Default));
            PlayerPrefs.SetString("CurrActivity", StringConstants.Activity_Setting);

            SceneManager.LoadSceneAsync(StringConstants.Scene_Setting);
        }

/*
        public void QuitApplication(InteractableStateArgs obj)
        {
            QuitApplication();
        }
*/
        public void QuitApplication()
        {
            Application.Quit();
        }

        public void CloseMainMenu()
        {
            mainMenu.SetActive(false);
        }

/*
        public void CleanObstacle(InteractableStateArgs obj)
        {

        }

        public void RestartLevel(InteractableStateArgs obj)
        {
            if (obj.NewInteractableState == InteractableState.ActionState)
            {
                SceneManager.LoadSceneAsync(PlayerPrefs.GetString("CurrActivity", StringConstants.Scene_Default));
            }

        }
        public void GoToMainMenu(InteractableStateArgs obj)
        {
            if (obj.NewInteractableState == InteractableState.ActionState)
            {
                SceneManager.LoadSceneAsync(StringConstants.Scene_MainMenu);
            }
        }

        public void GoToLastPosition(InteractableStateArgs obj)
        {

        }

        public void GoTo2ndLastPosition(InteractableStateArgs obj)
        {

        }
        public void GoTo3rdLastPosition(InteractableStateArgs obj)
        {

        }
*/
        public void RestoreMenu()
        {
            mainMenu.SetActive(true);
        }

        private void OnDestroy()
        {
            _instance = null;
        }

    }
}
