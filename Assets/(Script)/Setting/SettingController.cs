using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace edu.tnu.dgd.setting
{
    public class SettingController : MonoBehaviour
    {
        public float heightOffset;
        private GameObject playerCamera;

        void Start()
        {
            playerCamera = GameObject.Find("VRCamera");
        }


        void Update()
        {
            float newh = Mathf.Lerp(this.transform.position.y, playerCamera.transform.position.y - heightOffset, 0.01f);
            this.transform.position = new Vector3(this.transform.position.x, newh, this.transform.position.z);
        }
    }
}
