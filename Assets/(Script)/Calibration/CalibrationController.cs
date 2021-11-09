using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using edu.tnu.dgd.debug;
using edu.tnu.dgd.project.forklift;
using UnityEngine.Assertions;


namespace edu.tnu.dgd.vr.calibration
{
    public class CalibrationController : MonoBehaviour
    {
        private static CalibrationController _instance;
        public static CalibrationController instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<CalibrationController>();
                }
                return _instance;
            }
        }

        OVRHand[] hands;
        private ShowDebugLog log;

        public Transform leftCalibrationPosition;
        public Transform rightCalibrationPosition;

        public Transform fixedLeftCalibrationPosition;
        public Transform fixedRightCalibrationPosition;

        public GameObject root;

        public GameObject calibrationNoneIcon;
        public GameObject calibrationOKIcon;
        public Text calibrationDistanceText;

        public float offset = 0.0005f;

        public Transform forklift;

        public OVRCameraRig cameraRig;

        public StationProperty stationProperty;


        void Start()
        {
            Assert.IsNotNull(stationProperty);

            log = ShowDebugLog.instance;

            RootCalibratedOrientation rootCalibratedOrientation = RootCalibratedOrientation.instance;

            if (rootCalibratedOrientation != null)
            {
                Transform parent = cameraRig.gameObject.transform.parent;
                cameraRig.gameObject.transform.SetParent(null);

                this.root.transform.position = rootCalibratedOrientation.transform.position;
                this.root.transform.rotation = rootCalibratedOrientation.transform.rotation;
                this.rightCalibrationPosition.position = rootCalibratedOrientation.rightHandCalibrationPosition;
                cameraRig.gameObject.transform.SetParent(parent);
            }

            ComputeCalibrationDistance();
        }

        public void ComputeCalibrationDistance()
        {
            float dist = Vector3.Distance(rightCalibrationPosition.position, fixedRightCalibrationPosition.position) * 100;
            calibrationDistanceText.text = dist.ToString("F1") + " cm";
            if (dist < 1f)
            {
                calibrationNoneIcon.SetActive(false);
                calibrationOKIcon.SetActive(true);
            }
            else
            {
                calibrationNoneIcon.SetActive(true);
                calibrationOKIcon.SetActive(false);
            }
        }

        private void Update()
        {
            if (hands == null || hands.Length == 0)
            {
                hands = FindObjectsOfType<OVRHand>();
                return;
            }
        }

        public void CloseCalibration()
        {
            leftCalibrationPosition.gameObject.SetActive(false);
            rightCalibrationPosition.gameObject.SetActive(false);

            fixedLeftCalibrationPosition.gameObject.SetActive(false);
            fixedRightCalibrationPosition.gameObject.SetActive(false);

            //calibrationControlPanel.SetActive(false);
        }

        public void AutoCalibrate()
        {
            ShowBall();
            cameraRig.gameObject.transform.SetParent(null);
            SetLeftPosition(VRConstant.LeftHand);
            SetRightPosition(VRConstant.RightHand);
            DoCalibrateLeft();
            ComputeCalibrationDistance();
            cameraRig.gameObject.transform.SetParent(forklift);
            Invoke("HideBall", 5f);
            RecordRootTransform();
        }

        private void ShowBall()
        {
            fixedRightCalibrationPosition.gameObject.GetComponent<MeshRenderer>().enabled = true;
        }

        private void HideBall()
        {
            fixedRightCalibrationPosition.gameObject.GetComponent<MeshRenderer>().enabled = false;

        }

        private void RecordRootTransform()
        {
            RootCalibratedOrientation rootCalibratedOrientation = null;
            if (RootCalibratedOrientation.instance == null)
            {
                // 將目前校正過的Root方位記錄在RootOrientation物件中
                GameObject rootCali = new GameObject("RootOrientation");
                rootCali.transform.SetParent(null);
                rootCalibratedOrientation = rootCali.AddComponent<RootCalibratedOrientation>();
            } 
            else
            {
                rootCalibratedOrientation = RootCalibratedOrientation.instance;
            }

            if (rootCalibratedOrientation != null)
            {
                rootCalibratedOrientation.gameObject.transform.position = this.root.transform.position;
                rootCalibratedOrientation.gameObject.transform.rotation = this.root.transform.rotation;
                rootCalibratedOrientation.rightHandCalibrationPosition = this.rightCalibrationPosition.position;
            }

        }

        public void DoCalibrateRight()
        {
            Vector3 fixedVec = fixedLeftCalibrationPosition.position - fixedRightCalibrationPosition.position;
            Vector3 currentVec = leftCalibrationPosition.position - rightCalibrationPosition.position;
            Vector3 vec1 = new Vector3(fixedVec.x, 0, fixedVec.z);
            Vector3 vec2 = new Vector3(currentVec.x, 0, currentVec.z);
            float angle = Vector3.SignedAngle(vec1.normalized, vec2.normalized, Vector3.up);

            root.transform.position = root.transform.position + (leftCalibrationPosition.position - fixedLeftCalibrationPosition.position);
            root.transform.Rotate(0, angle, 0);
            ComputeCalibrationDistance();

        }

        public void DoCalibrateLeft()
        {
            Vector3 fixedVec = fixedRightCalibrationPosition.position - fixedLeftCalibrationPosition.position;
            Vector3 currentVec = rightCalibrationPosition.position - leftCalibrationPosition.position;
            Vector3 vec1 = new Vector3(fixedVec.x, 0, fixedVec.z);
            Vector3 vec2 = new Vector3(currentVec.x, 0, currentVec.z);
            float angle = Vector3.SignedAngle(vec1.normalized, vec2.normalized, Vector3.up);

            //PrintPosition("DoCalibrateLeft", angle);
            root.transform.position = root.transform.position + (leftCalibrationPosition.position - fixedLeftCalibrationPosition.position);
            root.transform.Rotate(0, angle, 0);
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
            if (handIndex == VRConstant.RightHand)
            {
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
            rightCalibrationPosition.SetParent(null);
            //PrintPosition("SetRightPosition", newpos);
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

        /*
        private void PrintPosition(string subject, float val)
        {
            log.Log(subject, val);
        }

        private void PrintPosition(string subject, Vector3 pos)
        {
            log.Log(subject, pos);
        }
        */
    }

}