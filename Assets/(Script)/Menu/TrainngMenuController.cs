using UnityEngine;
using UnityEngine.SceneManagement;
using edu.tnu.dgd.game;
using edu.tnu.dgd.debug;
using Oculus;

namespace edu.tnu.dgd.menu
{
    public class TrainngMenuController: MonoBehaviour
    {
        public bool useVR = true;
        public GameObject toggleButton;
        
        public bool showMenuOnAwake = false;
        public bool showToggleButton = true;

        private GameObject menuPanel;
        private GameObject reloadPanel;

        private static TrainngMenuController _instance;

        public static TrainngMenuController instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<TrainngMenuController>();
                }

                return _instance;
            }
        }

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

        private void Awake()
        {
            
            menuPanel = this.transform.Find("MenuPanel").gameObject;
            reloadPanel = this.transform.Find("ReloadPanel").gameObject;
        }

        private void Start()
        {
            if (toggleButton != null)
            {
                toggleButton.SetActive(showToggleButton);
            }

            menuPanel.SetActive(showMenuOnAwake);
            reloadPanel.SetActive(false);
            if (GuideController.instance != null)
            {
                GuideController.instance.ShowHideGuidePanel(PlayerPrefs.GetInt(StringConstants.Setting_GuideDisplay, 0) > 0);
            }
        }

        public void ShowToggleButton(bool show)
        {
            toggleButton.SetActive(show);
        }

        public void ToggleMenuPanel(InteractableStateArgs obj)
        {
            if (obj.NewInteractableState == InteractableState.ActionState)
            {
                menuPanel.SetActive(!menuPanel.activeSelf);
            }
        }

        public void CloseMenu(InteractableStateArgs obj)
        {
            if (obj.NewInteractableState == InteractableState.ActionState)
            {
                CloseMenu();
            }
        }

        public void CleanObstacle()
        {
            //ShowDebugLog.instance.Log(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>CleanObstacle()...");
            GameController.instance.CleanObstacle();
        }
        /*
        public void CleanObstacle(InteractableStateArgs obj)
        {
            //ShowDebugLog.instance.Log(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>CleanObstacle()" + obj.ToString());
            
            if (obj.NewInteractableState == InteractableState.ActionState)
            {
                CleanObstacle();
            }
            
        }
        */

        public void RestartLevel()
        {
            //ShowDebugLog.instance.Log(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>RestartLevel()...");

            menuPanel.SetActive(false);
            reloadPanel.SetActive(true);

            GameController.instance.RestartLevel();
        }

        /*
        public void RestartLevel(InteractableStateArgs obj)
        {
            //ShowDebugLog.instance.Log(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>RestartLevel()" + obj.ToString());
            
            if (obj.NewInteractableState == InteractableState.ActionState)
            {
                RestartLevel();
            }
        }
        */

        public void GoToMainMenu()
        {
            SceneManager.LoadSceneAsync(StringConstants.Scene_MainMenu);
        }

        public void GoToMainMenu(InteractableStateArgs obj)
        {
            if (obj.NewInteractableState == InteractableState.ActionState)
            {
                GoToMainMenu();
            }
        }

        public void GoToPosition(int i)
        {
            //ShowDebugLog.instance.Log(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>GoToPosition()..." + i);
            GameController.instance.GoToPosition(i);
            Invoke("CloseMenu", 0.5f);
        }

        public void CloseMenu()
        {
            menuPanel.SetActive(false);
        }

        /*
        public void GoToLastPosition(InteractableStateArgs obj)
        {
            GameController.instance.GoToLastPosition();
        }

        public void GoTo2ndLastPosition(InteractableStateArgs obj)
        {
            GameController.instance.GoTo2ndLastPosition();
        }

        public void GoTo3rdLastPosition(InteractableStateArgs obj)
        {
            GameController.instance.GoTo3rdLastPosition();
        }
        */
    }
}
