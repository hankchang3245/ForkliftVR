using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using edu.tnu.dgd.debug;
using TMPro;
using edu.tnu.dgd.vr;

public class TestShowMenu : MonoBehaviour 
{
    private void Awake()
    {
        OVRManager.InputFocusLost += OnInputFocusLost;
        OVRManager.InputFocusAcquired += OnInputFocusAcquired;
        OVRManager.HMDAcquired += OnHMDAcquired;
        OVRManager.HMDLost += OnHMDLost;
        OVRManager.HMDMounted += OnHMDMounted;
        OVRManager.HMDUnmounted += OnHMDUnmounted;
        OVRManager.HSWDismissed += OnHSWDismissed;
        OVRManager.TrackingAcquired += OnTrackingAcquired;
        OVRManager.TrackingLost += OnTrackingLost;
        OVRManager.VrFocusAcquired += OnVrFocusAcquired;
        OVRManager.VrFocusLost += OnVrFocusLost;
    }
    void Start()
    {

    }

    private void Update()
    {
        
    }
    private void OnInputFocusLost()
    {
        ShowDebugLog.instance.Log("OVRManager.OnInputFocusLost ........", true);
    }

    private void OnInputFocusAcquired()
    {
        ShowDebugLog.instance.Log("OVRManager.OnInputFocusAcquired ........", true);
    }

    private void OnHMDAcquired()
    {
        ShowDebugLog.instance.Log("OVRManager.OnHMDAcquired ........", true);
    }

    private void OnHMDLost()
    {
        ShowDebugLog.instance.Log("OVRManager.OnHMDLost ........", true);
    }

    private void OnHMDMounted()
    {
        ShowDebugLog.instance.Log("OVRManager.OnHMDMounted ........", true);
    }

    private void OnHMDUnmounted()
    {
        ShowDebugLog.instance.Log("OVRManager.OnHMDUnmounted ........", true);
    }

    private void OnTrackingAcquired()
    {
        ShowDebugLog.instance.Log("OVRManager.OnTrackingAcquired ........", true);
    }

    private void OnHSWDismissed()
    {
        ShowDebugLog.instance.Log("OVRManager.OnHSWDismissed ........", true);
    }

    private void OnTrackingLost()
    {
        ShowDebugLog.instance.Log("OVRManager.OnTrackingLost ........", true);
    }


    private void OnVrFocusAcquired()
    {
        ShowDebugLog.instance.Log("OVRManager.OnVrFocusAcquired ........", true);
    }

    private void OnVrFocusLost()
    {
        ShowDebugLog.instance.Log("OVRManager.OnVrFocusLost ........", true);
    }

    private void OnXXXXXX()
    {
        ShowDebugLog.instance.Log("OVRManager.OnXXXXXX ........", true);
    }
}
