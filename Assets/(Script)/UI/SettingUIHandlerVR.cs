using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using UnityEngine.Events;

namespace edu.tnu.dgd.ui
{
    public class SettingUIHandlerVR : MonoBehaviour
    {
        [SerializeField]
        private bool useVR = true;

        [SerializeField]
        private Canvas settingCanvas = null;

        [SerializeField]
        private GraphicRaycaster graphicRaycaster;

        [SerializeField]
        private OVRRaycaster ovrRaycaster;

        private GameObject exitButtonVR;
        private GameObject exitButtonPC;


        [Header("若為VR版本，以下欄位不得為空!")]

        [SerializeField]
        private GameObject uiHelpersToInstantiate = null;

        public LaserPointer.LaserBeamBehavior laserBeamBehavior = LaserPointer.LaserBeamBehavior.On;

        public void Awake()
        {
            Assert.IsNotNull(settingCanvas);
            Assert.IsNotNull(graphicRaycaster);
            Assert.IsNotNull(ovrRaycaster);

            exitButtonVR = settingCanvas.gameObject.transform.Find("VRExitButton").gameObject;
            exitButtonPC = settingCanvas.gameObject.transform.Find("ExitButton").gameObject;


            exitButtonVR.SetActive(false);
            exitButtonPC.SetActive(false);

            if (this.useVR)
            {
                graphicRaycaster.enabled = false;
                ovrRaycaster.enabled = true;

                //gameObject.SetActive(false);
                exitButtonVR.SetActive(true);

                Instantiate(uiHelpersToInstantiate);

                LaserPointer lp = FindObjectOfType<LaserPointer>();
                if (!lp)
                {
                    Debug.LogError("Debug UI requires use of a LaserPointer and will not function without it. Add one to your scene, or assign the UIHelpers prefab to the DebugUIBuilder in the inspector.");
                    return;
                }
                lp.laserBeamBehavior = laserBeamBehavior;
                ovrRaycaster.pointer = lp.gameObject;
            } 
            else // Use PC
            {
                ovrRaycaster.enabled = false;
                graphicRaycaster.enabled = true;
                exitButtonPC.SetActive(true);

                settingCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            }
            ToggleExitButton(true);




        }

        private void Start()
        {
            SettingConfigurationController settingConf = settingCanvas.gameObject.GetComponent<SettingConfigurationController>();
            if (settingConf != null)
            {
                settingConf.useVR = this.useVR;
            }
            
        }

        private void ToggleExitButton(bool show)
        {
            if (useVR)
            {
                exitButtonVR.SetActive(show);
                exitButtonPC.SetActive(false);
            }
            else
            {
                exitButtonVR.SetActive(false);
                exitButtonPC.SetActive(show);
            }
        }


    }

}