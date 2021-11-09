using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using edu.tnu.dgd.debug;
using TMPro;
using edu.tnu.dgd.vr;

public class TestCalibration : MonoBehaviour 
{
    OVRHand[] hands;
    private ShowDebugLog log;
    //public Text textLog;
    //public Transform workspace;


    public Transform leftCalibrationPosition;
    public Transform rightCalibrationPosition;

    public Transform fixedLeftCalibrationPosition;
    public Transform fixedRightCalibrationPosition;

    private Vector3 centerPointOfSteeringWheelPosition = Vector3.zero;
    private Vector3 endPointOfRightControlBarPosition = Vector3.zero;

    private int centerPointOfSteeringWheelCount = 0;
    private int endPointOfRightControlBarCount = 0;
    public GameObject root;

    //    public GameObject calibrationNone;
    //    public GameObject calibrationOK;
    public TMP_Text calibrationDistanceText;

    //public bool enableCalibration = false;

    public float offset = 0.0005f;

    public TMP_Text actionText;

    public Transform forklift;

    public GameObject calibrationControlPanel;

    private static TestCalibration _instance;
    public static TestCalibration instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<TestCalibration>();
            }
            return _instance;
        }
    }


    public OVRCameraRig cameraRig;




    void Start()
    {
        log = ShowDebugLog.instance;
        //textLog.text = textLog.text + "Start....\n";
        log.Log("Start....");
        //hands = FindObjectsOfType<OVRHand>();
        //textLog.text = textLog.text + hands + "....\n";
        //log.Log(""+hands);
        ComputeCalibrationDistance();
    }

    public void ComputeCalibrationDistance()
    {
        float dist = Vector3.Distance(rightCalibrationPosition.position, fixedRightCalibrationPosition.position)*100;
        calibrationDistanceText.text = dist.ToString("F1") + " cm";
        if (dist < 2f)
        {
            cameraRig.gameObject.transform.SetParent(forklift);
        }
    }

    private void Update()
    {
        if (hands == null || hands.Length == 0)
        {
            hands = FindObjectsOfType<OVRHand>();
            //textLog.text =  "hands.Length = 0\n" + textLog.text;
            return;
        }
    }

    public void CloseCalibration()
    {
        leftCalibrationPosition.gameObject.SetActive(false);
        rightCalibrationPosition.gameObject.SetActive(false);

        fixedLeftCalibrationPosition.gameObject.SetActive(false);
        fixedRightCalibrationPosition.gameObject.SetActive(false);

        calibrationControlPanel.SetActive(false);
    }

    public void AutoCalibrate()
    {
        SetLeftPosition(VRConstant.LeftHand);
        SetRightPosition(VRConstant.RightHand);
        DoCalibrateLeft();
        ComputeCalibrationDistance();
    }

    public void DoCalibrateRight()
    {
        /*
        Vector3 fixedVec = fixedRightCalibrationPosition.position - fixedRightCalibrationPosition.position;
        Vector3 currentVec = leftCalibrationPosition.position - rightCalibrationPosition.position;
        float angle = Vector3.Angle(new Vector3(fixedVec.x, 0, fixedVec.z), new Vector3(currentVec.x, 0, currentVec.z));

        root.transform.position = root.transform.position + (leftCalibrationPosition.position - fixedLeftCalibrationPosition.position);
        root.transform.rotation = Quaternion.Euler(0, angle, 0);
        */
        /*
        Vector3 leftVec = fixedLeftCalibrationPosition.position - leftCalibrationPosition.position;
        Vector3 rightVec = fixedRightCalibrationPosition.position - rightCalibrationPosition.position;
        //float angle = Vector3.Angle(new Vector3(leftVec.x, 0, leftVec.z), new Vector3(rightVec.x, 0, rightVec.z));

        root.transform.position = root.transform.position + rightVec;
        */

        Vector3 fixedVec = fixedLeftCalibrationPosition.position - fixedRightCalibrationPosition.position;
        Vector3 currentVec = leftCalibrationPosition.position - rightCalibrationPosition.position;
        float angle = Vector3.Angle(new Vector3(fixedVec.x, 0, fixedVec.z), new Vector3(currentVec.x, 0, currentVec.z));

        root.transform.position = root.transform.position + (leftCalibrationPosition.position - fixedLeftCalibrationPosition.position);
        if (angle > 0)
        {
            angle = angle * -1;
        }
        root.transform.Rotate(0, angle, 0);
        log.Log("DoCalibrateRight.............angle=" + angle);
        ComputeCalibrationDistance();

    }

    public void DoCalibrateLeft()
    {
        /*
        Vector3 leftVec = fixedLeftCalibrationPosition.position - leftCalibrationPosition.position;
        Vector3 rightVec = fixedRightCalibrationPosition.position - rightCalibrationPosition.position;
        //float angle = Vector3.Angle(new Vector3(leftVec.x, 0, leftVec.z), new Vector3(rightVec.x, 0, rightVec.z));
        */

        Vector3 fixedVec = fixedRightCalibrationPosition.position - fixedLeftCalibrationPosition.position;
        Vector3 currentVec = rightCalibrationPosition.position - leftCalibrationPosition.position;
        float angle = Vector3.Angle(new Vector3(fixedVec.x, 0, fixedVec.z), new Vector3(currentVec.x, 0, currentVec.z));

        root.transform.position = root.transform.position + (leftCalibrationPosition.position - fixedLeftCalibrationPosition.position);
        if (angle < 0)
        {
            angle = angle * -1;
        }
        root.transform.Rotate(0, angle, 0);
        log.Log("DoCalibrateLeft.............angle=" + angle);
        ComputeCalibrationDistance();
    }

    public void SetLeftPosition()
    {
        SetLeftPosition(VRConstant.RightHand);
    }
    public void SetLeftPosition(int handIndex)
    {
        leftCalibrationPosition.SetParent(null);
        Transform index_finger_tip_marker = null;
        if (handIndex == VRConstant.RightHand) {
            index_finger_tip_marker = hands[handIndex].gameObject.transform.Find("OculusHand_R/b_r_wrist/b_r_index1/b_r_index2/b_r_index3/r_index_finger_tip_marker");
        } 
        else
        {
            index_finger_tip_marker = hands[handIndex].gameObject.transform.Find("OculusHand_L/b_l_wrist/b_l_index1/b_l_index2/b_l_index3/l_index_finger_tip_marker");
        }
        leftCalibrationPosition.position = index_finger_tip_marker.position;
        leftCalibrationPosition.SetParent(forklift);

        Vector3 newpos = leftCalibrationPosition.localPosition;
        //PrintPosition("SetLeftPosition", newpos);
        leftCalibrationPosition.SetParent(null);
    }
    public void SetRightPosition()
    {
        SetRightPosition(VRConstant.RightHand);
    }

    public void SetRightPosition(int handIndex)
    {
        rightCalibrationPosition.SetParent(null);
        //r_index_finger_tip_marker = hands[handIndex].gameObject.transform.Find("OculusHand_R/b_r_wrist/b_r_index1/b_r_index2/b_r_index3/r_index_finger_tip_marker");
        Transform index_finger_tip_marker = null;
        if (handIndex == VRConstant.RightHand)
        {
            index_finger_tip_marker = hands[handIndex].gameObject.transform.Find("OculusHand_R/b_r_wrist/b_r_index1/b_r_index2/b_r_index3/r_index_finger_tip_marker");
        }
        else
        {
            index_finger_tip_marker = hands[handIndex].gameObject.transform.Find("OculusHand_L/b_l_wrist/b_l_index1/b_l_index2/b_l_index3/l_index_finger_tip_marker");
        }
        rightCalibrationPosition.position = index_finger_tip_marker.position;
        rightCalibrationPosition.SetParent(forklift);

        Vector3 newpos = rightCalibrationPosition.localPosition;
        //PrintPosition("SetRightPosition", newpos);
        rightCalibrationPosition.SetParent(null);
    }

    public void MoveForward()
    {
        Vector3 newPos = new Vector3(root.transform.position.x, root.transform.position.y, root.transform.position.z);
        newPos.z += offset;
        root.transform.position = newPos;
        //PrintPosition("MoveForward", root.transform.position);
    }

    public void MoveBackward()
    {
        Vector3 newPos = new Vector3(root.transform.position.x, root.transform.position.y, root.transform.position.z);
        newPos.z -= offset;
        root.transform.position = newPos;
       // PrintPosition("MoveBackward", root.transform.position);
    }

    public void MoveRight()
    {
        Vector3 newPos = new Vector3(root.transform.position.x, root.transform.position.y, root.transform.position.z);
        newPos.x += offset;
        root.transform.position = newPos;
        //PrintPosition("MoveRight", root.transform.position);
    }

    public void MoveLeft()
    {
        Vector3 newPos = new Vector3(root.transform.position.x, root.transform.position.y, root.transform.position.z);
        newPos.x -= offset;
        root.transform.position = newPos;
        //PrintPosition("MoveLeft", root.transform.position);
    }

    public void MoveUp()
    {
        Vector3 newPos = new Vector3(root.transform.position.x, root.transform.position.y, root.transform.position.z);
        newPos.y += offset;
        root.transform.position = newPos;
        //PrintPosition("MoveUp", root.transform.position);
    }

    public void MoveDown()
    {

        Vector3 newPos = new Vector3(root.transform.position.x, root.transform.position.y, root.transform.position.z);
        newPos.y -= offset;
        root.transform.position = newPos;
        //PrintPosition("MoveDown", root.transform.position);
    }

    private void PrintPosition(string subject, float val)
    {
        //textLog.text = subject + ": " + pos.ToString("F5") + "\n" + textLog.text;
        log.Log(subject, val);
    }

    private void PrintPosition(string subject, Vector3 pos)
    {
        log.Log(subject, pos);
        //textLog.text = subject + ": (" + pos.x.ToString("F5") + ", " + pos.y.ToString("F5") + ", " + pos.z.ToString("F5") + ")\n" + textLog.text;
    }
}
