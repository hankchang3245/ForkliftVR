using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace edu.tnu.dgd.effect
{
    public class BlinkingEffector : MonoBehaviour
    {
        public GameObject _blinkingTarget;
        public GameObject _originalMesh;

        public float blinkStartTime = 0f;       // 何時開始
        public float blinkDuration = 5f;        // 持續多久；-1為持續直到呼叫Stop為止
        public float blinkInterval = 0.5f;      // 隔多久閃一次

        [Tooltip("啟用長時間的閃爍。")]
        public bool enableLongDuration = false;

        [HideInInspector]
        public bool isBlinking = false;

        private float blinkEnableTime = 0f;

        public GameObject target
        {
            get
            {
                return _blinkingTarget;
            }

            set
            {
                _blinkingTarget = value;
            }
        }

        private void Awake()
        {
            if (_blinkingTarget == null)
            {
                _blinkingTarget = this.gameObject;
            }
        }

        public void StartBlinking(float duration)
        {
            this.blinkDuration = duration;
            StartBlinking();
        }

        public void StartBlinking()
        {
            StopBlinking();

            blinkEnableTime = Time.time;
            isBlinking = true;
            if (blinkDuration <= 0)  // long duration
            {
                enableLongDuration = true;
                this.InvokeRepeating("StartLongBlinking", blinkStartTime, blinkInterval);
            }
            else
            {
                enableLongDuration = false;
                this.InvokeRepeating("StartShortBlinking", blinkStartTime, blinkInterval);
            }
        }

        public void StopBlinking()
        {
            blinkDuration = 0f;
            enableLongDuration = false;
            isBlinking = false;
            _blinkingTarget.SetActive(false);
            if (_originalMesh != null)
            {
                _originalMesh.SetActive(true);
            }

            this.CancelInvoke();
        }

        private void StartShortBlinking()
        {
            if ((Time.time - blinkEnableTime) >= blinkDuration)
            {
                isBlinking = false;
                _blinkingTarget.SetActive(false);
                if (_originalMesh != null)
                {
                    _originalMesh.SetActive(true);
                }
                this.CancelInvoke();

                return;
            }

            _blinkingTarget.SetActive(!_blinkingTarget.activeSelf);
            if (_originalMesh != null)
            {
                _originalMesh.SetActive(!_originalMesh.activeSelf);
            }
        }

        private void StartLongBlinking()
        {
            if (enableLongDuration == false)
            {
                isBlinking = false;
                _blinkingTarget.SetActive(false);
                if (_originalMesh != null)
                {
                    _originalMesh.SetActive(true);
                }
                this.CancelInvoke();
                return;
            }

            _blinkingTarget.SetActive(!_blinkingTarget.activeSelf);
            if (_originalMesh != null)
            {
                _originalMesh.SetActive(!_originalMesh.activeSelf);
            }
        }

        private void OnDisable()
        {
            StopBlinking();
        }
    }
}