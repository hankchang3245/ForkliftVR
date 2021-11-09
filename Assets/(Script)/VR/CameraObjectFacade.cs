using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace edu.tnu.dgd.vr
{
    public class CameraObjectFacade : MonoBehaviour
    {
        private Camera mainCamera;
        private OVRCameraRig cameraRig;

        private static CameraObjectFacade _instance;
        public static CameraObjectFacade instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<CameraObjectFacade>();
                }
                return _instance;
            }
        }

        private void Start()
        {
            cameraRig = FindObjectOfType<OVRCameraRig>();
            if (cameraRig == null)
            {
                Camera[] cameras = FindObjectsOfType<Camera>();
                foreach (Camera cam in cameras)
                {
                    if (cam.CompareTag("MainCamera"))
                    {
                        mainCamera = cam;
                        break;
                    }
                }
            }
        }

        public Vector3 cameraEyeCenterPosition
        {
            get
            {
                if (cameraRig != null)
                {
                    return cameraRig.centerEyeAnchor.position;
                }
                else
                {
                    if (mainCamera != null)
                    {
                        return mainCamera.gameObject.transform.position;
                    }
                    
                    return Vector3.zero;
                }
            }
        }

        private void OnDestroy()
        {
            _instance = null;
        }
    }
}
