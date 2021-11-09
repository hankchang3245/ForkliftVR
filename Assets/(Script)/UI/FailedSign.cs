using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using edu.tnu.dgd.vr;
using edu.tnu.dgd.vrlearn;

namespace edu.tnu.dgd.ui
{
    public class FailedSign : MonoBehaviour
    {
        public GameObject crossSign;


        void Update()
        {
            crossSign.transform.LookAt(CameraObjectFacade.instance.cameraEyeCenterPosition);
        }


    }
}

