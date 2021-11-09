using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using edu.tnu.dgd.scene;
using edu.tnu.dgd;
using edu.tnu.dgd.debug;
using UnityEngine.Events;

namespace edu.tnu.dgd.menu
{
    /*[RequireComponent(typeof(Interactable))]*/
    public class MainMenuItem : MonoBehaviour
    {
        public string itemName;
        public GameObject hightlight;
        public InvokeCallbackTiming invokeCallbackTiming = InvokeCallbackTiming.Enter;
        public UnityEvent callbackAction;
        private Stack<string> triggerStack;

        private void Start()
        {
            triggerStack = new Stack<string>();
        }

        public void EnableSelected()
        {
            if (hightlight != null)
            {
                hightlight.SetActive(true);
            }
        }

        public void DisableSelected()
        {
            if (hightlight != null)
            {
                hightlight.SetActive(false);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (ShowDebugLog.instance != null)
            {
                //ShowDebugLog.instance.Log("OnTriggerEnter", other.gameObject.name);
            }

            try
            {
                if (other.gameObject.name.IndexOf("_Capsule") > 0)
                {
                    MainMenuController.instance.selectedItem = this;
                    triggerStack.Push(other.gameObject.name);

                    if (invokeCallbackTiming == InvokeCallbackTiming.Enter)
                    {
                        callbackAction?.Invoke();
                    }
                }
            }
            catch (ArgumentException ex)
            {

            }
        }


        private void OnTriggerExit(Collider other)
        {
            if (ShowDebugLog.instance != null)
            {
                //ShowDebugLog.instance.Log("OnTriggerExit", other.gameObject.name);
            }

            try
            {

                if (other.gameObject.name.IndexOf("_Capsule") > 0)
                {
                    triggerStack.Pop();
                }
            }
            catch (ArgumentNullException ex)
            {

            }

            if (triggerStack.Count == 0 && invokeCallbackTiming == InvokeCallbackTiming.Exit)
            {
                callbackAction?.Invoke();
                MainMenuController.instance.selectedItem = null;
            } 
        }
    }

    public enum InvokeCallbackTiming
    {
        Enter,
        Stay,
        Exit
    }
}
