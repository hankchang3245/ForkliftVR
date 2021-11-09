using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace edu.tnu.dgd.outline
{
    public class BaseOutline : MonoBehaviour
    {
        private GameObject outlineObject;

        private float blinkStartTime;
        private float blinkDuration;

        [Tooltip("啟用長時間的閃爍。")]
        private bool enableLongDuration = false;

        [HideInInspector]
        public bool isBlinking = false;

        public float blinkInterval = 0.3f;

        public GameObject outline
        {
            get
            {
                return outlineObject;
            }

            set
            {
                outlineObject = value;
            }
        }

        public void HideOutline()
        {
            if (outlineObject != null)
            {
                outlineObject.SetActive(false);
            }
        }

        public void ShowOutline()
        {
            if (outlineObject != null)
            {
                outlineObject.SetActive(true);
            }
        }

        public void ShowHintBlink(float duration)
        {
            //ShowOutlineBlink(duration, Color.yellowGameController.instance.hintColor);
        }

        public void DisableCollider()
        {
            Collider col = transform.GetComponent(typeof(Collider)) as Collider;
            if (col != null)
            {
                col.enabled = false;
            }
        }

        private void ShowOutlineBlink(float duration, Material color)
        {
            stopPrevOutline();

            blinkDuration = duration;
            blinkStartTime = Time.time;
            ChangeColor(color);
            isBlinking = true;
            if (duration <= 0)  // long duration
            {
                enableLongDuration = true;
                this.InvokeRepeating("ToggleLongOutline", 0.1f, blinkInterval);
            }
            else
            {
                enableLongDuration = false;
                this.InvokeRepeating("ToggleOutline", 0.1f, blinkInterval);
            }
        }

        private void stopPrevOutline()
        {
            if (!isBlinking)
            {
                return;
            }

            blinkDuration = 0f;
            enableLongDuration = false;
            isBlinking = false;
            outlineObject.SetActive(false);
            this.CancelInvoke();
        }

        public void ShowSuccessBlink(float duration)
        {
            //ShowOutlineBlink(duration, GameController.instance.successColor);
        }

        public void HideHintBlink()
        {
            enableLongDuration = false;
        }

        public void ShowErrorBlink(float duration)
        {
            //ShowOutlineBlink(duration, GameController.instance.errorColor);
        }

        private void ChangeColor(Material color)
        {
            if (outlineObject == null)
            {
                return;
            }
            SkinnedMeshRenderer[] renders = outlineObject.GetComponentsInChildren<SkinnedMeshRenderer>();

            for (int i = 0; renders != null && i < renders.Length; i++)
            {
                Material[] mats = new Material[1] { color };
                renders[i].materials = mats;
            }
        }

        private void ToggleOutline()
        {
            if ((Time.time - blinkStartTime) >= blinkDuration)
            {
                isBlinking = false;
                outlineObject.SetActive(false);
                this.CancelInvoke();
                return;
            }
            if (outlineObject == null)
            {
                return;
            }

            if (outlineObject.activeSelf)
            {
                outlineObject.SetActive(false);
            }
            else
            {
                outlineObject.SetActive(true);
            }
        }

        private void ToggleLongOutline()
        {
            if (enableLongDuration == false)
            {
                isBlinking = false;
                outlineObject.SetActive(false);
                this.CancelInvoke();
                return;
            }
            if (outlineObject == null)
            {
                return;
            }

            if (outlineObject.activeSelf)
            {
                outlineObject.SetActive(false);
            }
            else
            {
                outlineObject.SetActive(true);
            }
        }
    }
}