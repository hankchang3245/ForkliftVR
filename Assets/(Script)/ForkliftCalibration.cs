using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using edu.tnu.dgd.debug;
using UnityEngine.UI;

public class ForkliftCalibration : MonoBehaviour
{
    OVRHand[] hands;
    private ShowDebugLog log;
    //public Text textLog;
    //public Transform workspace;

    private bool isIndexFingerPinching;
    private float ringFingerPinchStrength;

    public Transform centerPointOfSteeringWheel;
    public Transform endPointOfRightControlBar;
    private Transform r_index_finger_tip_marker;

    private Vector3 centerPointOfSteeringWheelPosition = Vector3.zero;
    private Vector3 endPointOfRightControlBarPosition = Vector3.zero;
    private int centerPointOfSteeringWheelCount = 0;
    private int endPointOfRightControlBarCount = 0;
    public GameObject root;


    void Start()
    {
        log = ShowDebugLog.instance;
        //textLog.text = textLog.text + "Start....\n";
        //log.Log("Start....");
        hands = FindObjectsOfType<OVRHand>();
        //textLog.text = textLog.text + hands + "....\n";
        //log.Log(""+hands);

    }

    void Update()
    {
        if (hands == null || hands.Length == 0)
        {
            hands = FindObjectsOfType<OVRHand>();
            //textLog.text =  "hands.Length = 0\n" + textLog.text;
            return;
        }

        bool fingIndex = hands[0].GetFingerIsPinching(OVRHand.HandFinger.Ring);
        float fingRing = hands[0].GetFingerPinchStrength(OVRHand.HandFinger.Ring);

        if (hands[0].GetFingerIsPinching(OVRHand.HandFinger.Ring))
        {
            r_index_finger_tip_marker = hands[1].gameObject.transform.Find("OculusHand_R/b_r_wrist/b_r_index1/b_r_index2/b_r_index3/r_index_finger_tip_marker");
            centerPointOfSteeringWheel.position = r_index_finger_tip_marker.position;
            //centerPointOfSteeringWheel.rotation = r_index_finger_tip_marker.rotation;

            //centerPointOfSteeringWheelPosition += centerPointOfSteeringWheel.localPosition;
            //centerPointOfSteeringWheelCount++;

            //PrintPosition("Cenetr of Steering Wheel", centerPointOfSteeringWheelPosition/centerPointOfSteeringWheelCount);
            //PrintPosition("Cenetr of Steering Wheel(net)", centerPointOfSteeringWheel.localPosition);
            PrintPosition("Relative", (centerPointOfSteeringWheel.localPosition - endPointOfRightControlBar.localPosition));
        }
        else if (hands[0].GetFingerIsPinching(OVRHand.HandFinger.Pinky))
        {
            r_index_finger_tip_marker = hands[1].gameObject.transform.Find("OculusHand_R/b_r_wrist/b_r_index1/b_r_index2/b_r_index3/r_index_finger_tip_marker");
            endPointOfRightControlBar.position = r_index_finger_tip_marker.position;
            //endPointOfRightControlBar.rotation = r_index_finger_tip_marker.rotation;

            //endPointOfRightControlBarPosition += endPointOfRightControlBar.localPosition;
            //endPointOfRightControlBarCount++;

            //PrintPosition("Right Control Bar", endPointOfRightControlBarPosition / endPointOfRightControlBarCount);
            //PrintPosition("Right Control Bar(net)", endPointOfRightControlBar.localPosition);
            PrintPosition("Relative", (centerPointOfSteeringWheel.localPosition - endPointOfRightControlBar.localPosition));
        }
        /*
        if (hands[0].GetFingerIsPinching(OVRHand.HandFinger.Thumb) &&
            hands[0].GetFingerIsPinching(OVRHand.HandFinger.Index) &&
            hands[1].GetFingerIsPinching(OVRHand.HandFinger.Thumb) &&
            hands[1].GetFingerIsPinching(OVRHand.HandFinger.Index))
        {
            workspace.position = workspace.position + (centerPointOfSteeringWheel.localPosition - endPointOfRightControlBar.localPosition);
            PrintPosition("Relative", (centerPointOfSteeringWheel.localPosition - endPointOfRightControlBar.localPosition));
        }
        */

    }

    private void PrintPosition(string subject, float val)
    {
        //textLog.text = subject + ": " + pos.ToString("F5") + "\n" + textLog.text;
        //log.Log(subject, val);
    }

    private void PrintPosition(string subject, Vector3 pos)
    {
        //log.Log(subject, pos);
        //textLog.text = subject + ": (" + pos.x.ToString("F5") + ", " + pos.y.ToString("F5") + ", " + pos.z.ToString("F5") + ")\n" + textLog.text;
    }
}
