using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using edu.tnu.dgd.login;
using edu.tnu.dgd.debug;
using edu.tnu.dgd.vrlearn;

public class OVRKeyboardController : MonoBehaviour
{
    private TouchScreenKeyboard overlayKeyboard;

    public InputField schoolInputField;
    public InputField userIdInputField;

    void Start()
    {
        //TouchScreenKeyboard.hideInput = false;
        //schoolInputField..OnPointerClick += OnSchoolInputFieldFocus;

        OVRManager.InputFocusLost += OnCheckKeyboardInput;

        if (ShowDebugLog.instance != null)
        {
            //ShowDebugLog.instance.Log("OVRKeyboardController.Start()");
        }

        Learner ln = Learner.Load(false);
        if (ln != null)
        {
            schoolInputField.text = ln.schoolId;
            userIdInputField.text = ln.id;
        }
        else
        {
            schoolInputField.text = "";
            userIdInputField.text = "";
        }
        
        
    }

    public void OnSchoolInputFieldFocus()
    {

    }

    public void OpenKeyboard()
    {
        overlayKeyboard = TouchScreenKeyboard.Open(schoolInputField.text, TouchScreenKeyboardType.Default);
        if (ShowDebugLog.instance != null)
        {
            //ShowDebugLog.instance.Log("OVRKeyboardController.OpenKeyboard()");
        }
    }

    void Update()
    {
        if (overlayKeyboard != null && overlayKeyboard.status == TouchScreenKeyboard.Status.Done)
        {
            //inputText = overlayKeyboard.text;
            /*
            if (ShowDebugLog.instance != null)
            {
                //ShowDebugLog.instance.Log("InputText:" + inputText);
                ShowDebugLog.instance.Log("OVRKeyboardController.Update()");
            }
            */
        }
    }

    void OnCheckKeyboardInput()
    {
        if (overlayKeyboard != null)
        {
            if (ShowDebugLog.instance != null)
            {
                //ShowDebugLog.instance.Log("OVRKeyboardController.OnCheckKeyboardInput()");
            }
            
            //inputText = overlayKeyboard.text;
            LoginController.instance.QuerySchoolNameBySchoolId();
        }
    }

}
