using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using edu.tnu.dgd.game;
using edu.tnu.dgd.vrlearn;
using edu.tnu.dgd.login;

namespace edu.tnu.dgd.ui
{
    public class SettingConfigurationController : MonoBehaviour
    {
        public Slider forkliftSfx;
        public Slider forkliftSfxVolume;
        public Slider noGasMaxSpeedSilder;
        public Slider guideDisplay;
        public Slider guideSpeech;
        public Slider guideSpeechVolume;

        public GameObject speechTypeGameObject;
        public Slider speechType;
        public GameObject networkStatusRoot;
        public TMP_Text userIdText;

        [HideInInspector]
        public bool useVR = true;

        private TMP_Text forkliftSfxVolumeText;
        private TMP_Text guideSpeechVolumeText;
        private TMP_Text noGasMaxSpeedText;

        private GameObject connectedText;
        private GameObject disconnectText;

        private GameObject menuPanel;
        private GameObject exitButton;
        private GameObject vrExitButton;

        private GameObject changeUserButton;

        // Vehicle Parameter Setting
        public Slider maxTorqueSlider;
        public Slider maxSpeedSlider;
        public Slider torqueMultiplicationSlider;
        public Slider torqueLimitationSlider;
        public Slider brakeTorqueSlider;

        private GameObject forkliftParameterPanel;
        private GameObject changeForkliftParameterButton;

        private void Awake()
        {
            menuPanel = this.transform.Find("MenuPanel").gameObject;
            exitButton = this.transform.Find("ExitButton").gameObject;
            vrExitButton = this.transform.Find("VRExitButton").gameObject;
            changeUserButton = this.transform.Find("ChangeUserButton").gameObject;
            // Forklift Parameter
            forkliftParameterPanel = this.transform.Find("ForkliftParameterPanel").gameObject;
            changeForkliftParameterButton = this.transform.Find("ChangeForkliftParameterButton").gameObject;
         }

        public void OpenLoginPanel()
        {
            LoginPanelHelper.instance.OpenLoginPanel();
        }

        private void ToggleExitButton(bool show)
        {
            if (useVR)
            {
                vrExitButton.SetActive(show);
                exitButton.SetActive(false);
            }
            else
            {
                vrExitButton.SetActive(false);
                exitButton.SetActive(show);
            }
        }

        void Start()
        {


            ToggleExitButton(true);

            forkliftSfxVolumeText = forkliftSfxVolume.gameObject.transform.Find("Volume").gameObject.GetComponent<TMP_Text>();
            guideSpeechVolumeText = guideSpeechVolume.gameObject.transform.Find("Volume").gameObject.GetComponent<TMP_Text>();
            noGasMaxSpeedText = noGasMaxSpeedSilder.gameObject.transform.Find("Speed").gameObject.GetComponent<TMP_Text>();

            connectedText = networkStatusRoot.transform.Find("Connected").gameObject;
            disconnectText = networkStatusRoot.transform.Find("Disconnected").gameObject;

            UpdateUI();

            InvokeRepeating("UpdateNetworkStatusUI", 1f, 5f);

            UpdateVehicleParameterUI();

            //Debug.Log(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>" + DontDestroyOnLoadController.instance.bluetoothConnected);
        }

        public void UpdateUI()
        {
            forkliftSfx.value = PlayerPrefs.GetInt(StringConstants.Setting_ForkliftSFX);

            forkliftSfxVolume.value = PlayerPrefs.GetInt(StringConstants.Setting_ForkliftSFXVolume);
            forkliftSfxVolumeText.text = "" + forkliftSfxVolume.value;
            forkliftSfxVolume.gameObject.SetActive((forkliftSfx.value > 0));

            noGasMaxSpeedSilder.value = PlayerPrefs.GetInt(StringConstants.Setting_NoGasMaxSpeed);
            noGasMaxSpeedText.text = "" + noGasMaxSpeedSilder.value;


            guideDisplay.value = PlayerPrefs.GetInt(StringConstants.Setting_GuideDisplay);

            guideSpeech.value = PlayerPrefs.GetInt(StringConstants.Setting_GuideSpeech);

            guideSpeechVolume.value = PlayerPrefs.GetInt(StringConstants.Setting_GuideSpeechVolume);
            guideSpeechVolumeText.text = "" + guideSpeechVolume.value;
            guideSpeechVolume.gameObject.SetActive((guideSpeech.value > 0));

            speechType.value = PlayerPrefs.GetInt(StringConstants.Setting_SpeechType);

            //ShowDebugLog.instance.Log("..................1");
            Learner learner = Learner.Load();
            if (learner != null && userIdText != null)
            {
                userIdText.text = learner.id;
            }
            
            speechTypeGameObject.SetActive((guideSpeech.value > 0));
        }



        private void UpdateNetworkStatusUI()
        {
            connectedText.gameObject.SetActive(false);
            disconnectText.gameObject.SetActive(false);

            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                disconnectText.gameObject.SetActive(true);
            }
            else
            {
                connectedText.gameObject.SetActive(true);
            }
        }

        public void NoGasMaxSpeedChanged(Slider slider)
        {
            PlayerPrefs.SetInt(StringConstants.Setting_NoGasMaxSpeed, Mathf.FloorToInt(slider.value));
            UpdateUI();
        }

        public void HidePanel()
        {
            ToggleExitButton(false);
            menuPanel.SetActive(false);
            changeUserButton.SetActive(false);
        }

        public void RestorePanel()
        {
            menuPanel.SetActive(true);
            ToggleExitButton(true);
            changeUserButton.SetActive(true);

            Learner ln = Learner.Load(false);
            if (ln != null)
            {
                userIdText.text = ln.id;
            }
           
        }

        public void ForkliftSFXChanged(Slider slider)
        {
            PlayerPrefs.SetInt(StringConstants.Setting_ForkliftSFX, Mathf.FloorToInt(slider.value));
            UpdateUI();
        }

        public void ForkliftSFXVolumeChanged(Slider slider)
        {
            PlayerPrefs.SetInt(StringConstants.Setting_ForkliftSFXVolume, Mathf.FloorToInt(slider.value));
            forkliftSfxVolumeText.text = "" + Mathf.FloorToInt(slider.value);

            UpdateUI();
        }

        public void GuideDisplayChanged(Slider slider)
        {
            PlayerPrefs.SetInt(StringConstants.Setting_GuideDisplay, Mathf.FloorToInt(slider.value));

            UpdateUI();
        }

        public void GuideSpeechChanged(Slider slider)
        {
            PlayerPrefs.SetInt(StringConstants.Setting_GuideSpeech, Mathf.FloorToInt(slider.value));

            UpdateUI();
        }

        public void GuideSpeechVolumeChanged(Slider slider)
        {
            PlayerPrefs.SetInt(StringConstants.Setting_GuideSpeechVolume, Mathf.FloorToInt(slider.value));
            guideSpeechVolumeText.text = ""+Mathf.FloorToInt(slider.value);

            UpdateUI();
        }


        public void GuideSpeechTypeChanged(Slider slider)
        {
            PlayerPrefs.SetInt(StringConstants.Setting_SpeechType, Mathf.FloorToInt(slider.value));

            UpdateUI();
        }

        private void OnDisable()
        {
            CancelInvoke("UpdateNetworkStatusUI");
        }
        
        //Vehicle Parameter Setting

        public void OpenVehicleParameterSetting()
        {
            forkliftParameterPanel.SetActive(true);
            menuPanel.SetActive(false);
            changeUserButton.SetActive(false);
            changeForkliftParameterButton.SetActive(false);
            vrExitButton.SetActive(false);
        }

        public void CloseVehicleParameterSetting()
        {
            forkliftParameterPanel.SetActive(false);
            menuPanel.SetActive(true);
            changeUserButton.SetActive(true);
            changeForkliftParameterButton.SetActive(true);
            vrExitButton.SetActive(true);
        }

        public void MaxTorqueChanged(Slider slider)
        {
            PlayerPrefs.SetInt(StringConstants.Vehicle_MaxTorque, Mathf.FloorToInt(slider.value));
            UpdateVehicleParameterUI();
        }

        public void MaxSpeedChanged(Slider slider)
        {
            PlayerPrefs.SetInt(StringConstants.Vehicle_MaxSpeed, Mathf.FloorToInt(slider.value));
            UpdateVehicleParameterUI();
        }

        public void TorqueMultiplicationChanged(Slider slider)
        {
            PlayerPrefs.SetFloat(StringConstants.Vehicle_TorqueMultiplication, Mathf.RoundToInt(slider.value * 10f)/10f);
            UpdateVehicleParameterUI();
        }

        public void TorqueLimitationChanged(Slider slider)
        {
            PlayerPrefs.SetFloat(StringConstants.Vehicle_TorqueLimitation, slider.value);
            UpdateVehicleParameterUI();
        }

        public void BrakeTorqueChanged(Slider slider)
        {
            PlayerPrefs.SetFloat(StringConstants.Vehicle_BrakeTorque, slider.value);
            UpdateVehicleParameterUI();
        }

        public void UpdateVehicleParameterUI()
        {
            maxTorqueSlider.value = PlayerPrefs.GetInt(StringConstants.Vehicle_MaxTorque, 100);
            TMP_Text maxTorqueText = maxTorqueSlider.gameObject.transform.Find("ValueText").gameObject.GetComponent<TMP_Text>();
            maxTorqueText.text = "" + maxTorqueSlider.value;

            maxSpeedSlider.value = PlayerPrefs.GetInt(StringConstants.Vehicle_MaxSpeed, 10);
            TMP_Text maxSpeedText = maxSpeedSlider.gameObject.transform.Find("ValueText").gameObject.GetComponent<TMP_Text>();
            maxSpeedText.text = "" + maxSpeedSlider.value;

            torqueMultiplicationSlider.value = PlayerPrefs.GetFloat(StringConstants.Vehicle_TorqueMultiplication, 1.5f);
            TMP_Text torqueMultiplicationText = torqueMultiplicationSlider.gameObject.transform.Find("ValueText").gameObject.GetComponent<TMP_Text>();
            torqueMultiplicationText.text = "" + Mathf.RoundToInt(torqueMultiplicationSlider.value*10f)/10f;

            torqueLimitationSlider.value = PlayerPrefs.GetFloat(StringConstants.Vehicle_TorqueLimitation, 250f);
            TMP_Text torqueLimitationText = torqueLimitationSlider.gameObject.transform.Find("ValueText").gameObject.GetComponent<TMP_Text>();
            torqueLimitationText.text = "" + torqueLimitationSlider.value;

            brakeTorqueSlider.value = PlayerPrefs.GetFloat(StringConstants.Vehicle_BrakeTorque, 500f);
            TMP_Text brakeForceText = brakeTorqueSlider.gameObject.transform.Find("ValueText").gameObject.GetComponent<TMP_Text>();
            brakeForceText.text = "" + brakeTorqueSlider.value;
            
        }
    }
}

