using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TMPro;
using edu.tnu.dgd.debug;


namespace edu.tnu.dgd.game
{
    public class DeviceDependenceDataLocator : MonoBehaviour
    {
        public GameObject cameraObject;
        public GameObject forkliftCarObject;

        public Transform cameraPosition;
        public GameObject humanObject;
        public TMP_Text guideInstructionText;

        public GameObject guidePanelObject;
        public Transform guidePanelPosition;

        public SafetyBeltController safetyBeltController;

        public bool useVR = true;

        [Header("若為VR版本，以下欄位不得為空!")]

        public GameObject vrMenuGameObject;


        private Transform vrMenuPosition;

        private static DeviceDependenceDataLocator _instance;


        public static DeviceDependenceDataLocator instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<DeviceDependenceDataLocator>();
                }

                return _instance;
            }
        }

        void Start()
        {
            Assert.IsNotNull(cameraObject);
            Assert.IsNotNull(forkliftCarObject);
            Assert.IsNotNull(cameraPosition);
            Assert.IsNotNull(safetyBeltController);

            cameraObject.transform.position = cameraPosition.position;
            cameraObject.transform.rotation = cameraPosition.rotation;
            cameraObject.transform.SetParent(cameraPosition);

            Assert.IsNotNull(humanObject);

            if (useVR)
            {
                Assert.IsNotNull(vrMenuGameObject);
                vrMenuPosition = forkliftCarObject.transform.Find("UI/VRMenuPosition");
                vrMenuGameObject.transform.SetParent(vrMenuPosition);
                vrMenuGameObject.transform.localPosition = Vector3.zero;
                vrMenuGameObject.transform.localRotation = Quaternion.identity;

                safetyBeltController.hasFastenSafetyBelt = false;
                //ShowDebugLog.instance.Log("1......... hasFastenSafetyBelt = " + safetyBeltController.hasFastenSafetyBelt);

                humanObject.SetActive(true);
            }
            else // use PC
            {
                safetyBeltController.hasFastenSafetyBelt = true;
                //ShowDebugLog.instance.Log("2......... hasFastenSafetyBelt = " + safetyBeltController.hasFastenSafetyBelt);
                humanObject.SetActive(false);
            }

            // VR版與PC版會有不同的位置
            guidePanelObject.transform.localPosition = guidePanelPosition.localPosition;
            guidePanelObject.transform.localRotation = guidePanelPosition.localRotation;
            guidePanelObject.SetActive(true);

            /*
            forkliftCarObject.transform.SetParent(stationProp.gameObject.transform);

            Transform tr = stationProp.CarOriginLocalTransform;
            if (tr != null)
            {
                forkliftCarObject.transform.localPosition = tr.localPosition;
                forkliftCarObject.transform.localRotation = tr.localRotation;
            }
            */
        }

    }
}
