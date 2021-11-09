using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using edu.tnu.dgd.debug;

namespace edu.tnu.dgd.game
{
    public class SafetyBeltController : MonoBehaviour
    {
        public Animator animator;
        private Dictionary<string, float> fingerNameDict;
        private float handEnterTime = -1;
        private float handExitTime = -1;
        public GameObject highlightObject;

        [HideInInspector]
        private bool _hasFastenSafetyBelt = false;


        private void Start()
        {
            fingerNameDict = new Dictionary<string, float>();
        }

        public bool hasFastenSafetyBelt
        {
            get
            {
                return _hasFastenSafetyBelt;
            }

            set
            {
                _hasFastenSafetyBelt = value;
                //ShowDebugLog.instance.Log("========================================> hasFastenSafetyBelt");
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_hasFastenSafetyBelt)
            {
                return;
            }
            try
            {
                if (other.gameObject.name.IndexOf("_Capsule") > 0)
                {
                    ToggleHighlight(true);
                    fingerNameDict.Add(other.gameObject.name, Time.time);
                    if (handEnterTime < 0)
                    {
                        handEnterTime = Time.time;
                        Invoke("CheckTriggerStay", 0.4f);
                    }
                }
            }
            catch (ArgumentException ex)
            {

            }

        }

        private void ToggleHighlight(bool enable)
        {
            highlightObject.SetActive(enable);
        }


        public void FastenSafetyBelt()
        {
            hasFastenSafetyBelt = true;

            //ShowDebugLog.instance.Log("Set FastenSafetyBelt = true");
        }

        private void OnTriggerStay(Collider other)
        {
            if (_hasFastenSafetyBelt)
            {
                return;
            }
            try
            {
                if (other.gameObject.name.IndexOf("_Capsule") > 0)
                {
                    handExitTime = Time.time;
                }

            }
            catch (ArgumentNullException ex)
            {

            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (_hasFastenSafetyBelt)
            {
                return;
            }
            try
            {
                if (other.gameObject.name.IndexOf("_Capsule") > 0)
                {
                    ToggleHighlight(false);
                    fingerNameDict.Remove(other.gameObject.name);
                    if (fingerNameDict.Count == 0)
                    {
                        handEnterTime = -1;
                        handExitTime = -1;
                    }
                }
            }
            catch (ArgumentNullException ex)
            {
                ToggleHighlight(false);
            }
        }

        private void CheckTriggerStay()
        {
            if ((handExitTime - handEnterTime) >= 0.3f)
            {
                animator.SetTrigger("FastenSafeBelt");
            }
        }
    }
}
