using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using edu.tnu.dgd.game;

namespace edu.tnu.dgd.menu
{
    public class MainSettingController: MonoBehaviour
    {
        public GameObject mainMenuCanvas;

        private GameObject chkYes;
        private GameObject chkNo;
        public bool _isShowGuide = true;

        public bool isShowGuide
        {
            get
            {
                return _isShowGuide;
            }

            set
            {
                _isShowGuide = value;
                ShowToggleTexture();
            }
        }

        private void Awake()
        {
            chkYes = transform.Find("Menu/ToggleGuide/BackgroundYes").gameObject;
            chkNo = transform.Find("Menu/ToggleGuide/BackgroundNo").gameObject;

            ShowToggleTexture();
        }

        private void ShowToggleTexture()
        {
            chkYes.SetActive(_isShowGuide);
            chkNo.SetActive(!_isShowGuide);
        }
        public void ToggleShowGuidePanel()
        {
            isShowGuide = !isShowGuide;

            GuideController.instance.ShowHideGuidePanel(isShowGuide);
        }

        public void HideMyself()
        {
            mainMenuCanvas.SetActive(true);
            this.gameObject.SetActive(false);
        }

    }
}
