using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using edu.tnu.dgd.value;
using UnityEngine.Assertions;
using edu.tnu.dgd.game;

namespace edu.tnu.dgd.project.forklift
{
    public class DrivingPoint : MonoBehaviour
    {
        public GuideDataType guideType = GuideDataType.Basic;
        public string checkingTag;

        public bool enableCheck = true;
        private Collider[] colliders;

        [HideInInspector]
        public bool hasPassed = false;

        void Awake()
        {
            colliders = gameObject.GetComponents<Collider>();
        }

        private void OnTriggerExit(Collider other)
        {
            if (enableCheck && other.gameObject.CompareTag(checkingTag))
            {
                hasPassed = true;
            }
        }
    }
}

