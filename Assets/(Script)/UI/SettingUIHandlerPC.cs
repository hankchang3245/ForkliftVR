using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

namespace edu.tnu.dgd.ui
{
    public class SettingUIHandlerPC : MonoBehaviour
    {

        [SerializeField]
        private GraphicRaycaster graphicRaycaster;

        [SerializeField]
        private OVRRaycaster ovrRaycaster;

        [SerializeField]
        private Canvas settingCanvas = null;

        public void Awake()
        {
            Assert.IsNotNull(graphicRaycaster);
            Assert.IsNotNull(ovrRaycaster);
            Assert.IsNotNull(settingCanvas);

            ovrRaycaster.enabled = false;
            graphicRaycaster.enabled = true;

            settingCanvas.renderMode = RenderMode.ScreenSpaceOverlay;



        }

        /*
        public void ToggleLaserPointer(bool isOn)
        {
            if (lp)
            {
                if (isOn)
                {
                    lp.enabled = true;
                }
                else
                {
                    lp.enabled = false;
                }
            }
        }
        */
    }

}