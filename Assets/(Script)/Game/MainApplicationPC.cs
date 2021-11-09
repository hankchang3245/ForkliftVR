using UnityEngine;
using UnityEngine.UI;
using edu.tnu.dgd.forklift;
using edu.tnu.dgd.vehicle;
using edu.tnu.dgd.value;
using UnityEngine.Assertions;

namespace edu.tnu.dgd.game
{
    [RequireComponent(typeof(ForkliftPlayerInputPC), typeof(VehiclePlayerInputPC))]
    public class MainApplicationPC : MonoBehaviour
    {
        public Text gearText;
        public Text speedText;
        public Text wheelAngleText;
        public Text txtControls;

        private ForkliftPlayerInputPC _forkliftInput;
        private VehiclePlayerInputPC _vehicleInput;
        public VehicleController _vehicleController;

        private bool _showControlsText = false;
        private const string _defaultText = "顯示/隱藏 鍵盤輸入設定： <color=yellow>Esc</color>";
        private string _controlsText = string.Empty;
        private GameObject panel;

        
        void Start()
        {
            _forkliftInput = GetComponent<ForkliftPlayerInputPC>();
            _vehicleInput = GetComponent<VehiclePlayerInputPC>();
            //_vehicleController = VehicleController.instance;

            panel = txtControls.gameObject.transform.parent.gameObject;

            Assert.IsNotNull(_forkliftInput);
            Assert.IsNotNull(_vehicleInput);
            Assert.IsNotNull(_vehicleController);

            FormatControlsText();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (panel.activeSelf == false)
                {
                    panel.SetActive(true);
                }
                else
                {
                    panel.SetActive(false);
                }
            }
            speedText.text = _vehicleController.SpeedString;
            gearText.text = _vehicleController.GearString;
            wheelAngleText.text = _vehicleController.SteeringWheelAngleString;
        }

        private void FormatControlsText()
        {
            _controlsText = string.Format("{0}{1}", _defaultText, System.Environment.NewLine);

            //Forklift
            _controlsText += string.Format("{0}<color=orange>貨叉控制</color>{0}", System.Environment.NewLine);
            //_controlsText += string.Format("貨叉引擎 開啟/關閉： {0}{1}", _forkliftInput.inputSettings.toggleEngine, System.Environment.NewLine);
            _controlsText += string.Format("貨叉 向上/向下：  <color=yellow>{0}/{1}</color>{2}", _forkliftInput.inputSettings.forksUp, _forkliftInput.inputSettings.forksDown, System.Environment.NewLine);
            //_controlsText += string.Format("Forks left/right: {0}/{1}{2}", _forkliftInput.inputSettings.forksLeft, _forkliftInput.inputSettings.forksRight, System.Environment.NewLine);
            _controlsText += string.Format("貨叉傾斜 往後/往前： <color=yellow>{0}/{1}</color>{2}", _forkliftInput.inputSettings.mastTiltBackwards, _forkliftInput.inputSettings.mastTiltForwards, System.Environment.NewLine);
            //Vehicle
            _controlsText += string.Format("{0}<color=orange>堆高機控制</color>{0}", System.Environment.NewLine);
            //_controlsText += string.Format("堆高機引擎 開啟/關閉： {0}{1}", _vehicleInput.inputSettings.toggleEngine, System.Environment.NewLine);
            _controlsText += string.Format("前進/後退/空檔： <color=yellow>{0}/{1}/{2}</color>{3}", _vehicleInput.inputSettings.gearPositionDrive, _vehicleInput.inputSettings.gearPositionReverse, _vehicleInput.inputSettings.gearPositionNeutral, System.Environment.NewLine);
            _controlsText += string.Format("加速/減速： <color=yellow>{0}/{1}</color>{2}", _vehicleInput.inputSettings.acceleration, _vehicleInput.inputSettings.reverse, System.Environment.NewLine);
            _controlsText += string.Format("方向盤 向左/向右： <color=yellow>{0}/{1}</color>{2}", _vehicleInput.inputSettings.turnLeft, _vehicleInput.inputSettings.turnRight, System.Environment.NewLine);
            _controlsText += string.Format("煞車： <color=yellow>{0}</color>{1}", _vehicleInput.inputSettings.brakes, System.Environment.NewLine);
            _controlsText += string.Format("手煞車 拉上/放開： <color=yellow>{0}/{1}</color>{2}", _vehicleInput.inputSettings.handbrakeEnable, _vehicleInput.inputSettings.handbrakeDisable, System.Environment.NewLine);
            //_controlsText += string.Format("寸動踏板： {0}{1}", _vehicleInput.inputSettings.clutch, System.Environment.NewLine);
            //_controlsText += string.Format("喇叭： {0}{1}", _vehicleInput.inputSettings.horn, System.Environment.NewLine);
            //_controlsText += string.Format("頭燈： {0}{1}", _vehicleInput.inputSettings.headlights, System.Environment.NewLine);
            //_controlsText += string.Format("重置堆高機位置： <color=yellow>{0}</color>{1}", _vehicleInput.inputSettings.resetVehiclePosition, System.Environment.NewLine);
            _controlsText += string.Format("回復交通桿： <color=yellow>{0}</color>{1}", _vehicleInput.inputSettings.cleanObstacle, System.Environment.NewLine);
            if (GameController.instance.stationType == GuideDataType.Basic)
            {
                _controlsText += string.Format("重置位置 開始前進/開始後退： <color=yellow>0,1</color>{0}", System.Environment.NewLine);
            }
            else // GuideDataType.Advanced
            {
                _controlsText += string.Format("重置位置： <color=yellow>0,1,2,3,4,5,6,7</color>{0}", System.Environment.NewLine);
            }

            _controlsText += string.Format("{0}<color=orange>攝影機鏡頭</color>{0}", System.Environment.NewLine);
            _controlsText += string.Format("左上方/左下方/左後方： <color=yellow>{0}/{1}/{2}</color>{3}", 
                        _vehicleInput.inputSettings.cameraLookLeftTop,
                        _vehicleInput.inputSettings.cameraLookLeftBottom,
                        _vehicleInput.inputSettings.cameraLookLeftBackward,
                        System.Environment.NewLine);
            _controlsText += string.Format("前方/中間/後方： <color=yellow>{0}/{1}/{2}</color>{3}",
                        _vehicleInput.inputSettings.cameraLookForward,
                        _vehicleInput.inputSettings.cameraLookCenter,
                        _vehicleInput.inputSettings.cameraLookBackward,
                        System.Environment.NewLine);
            _controlsText += string.Format("右上方/右下方/右後方： <color=yellow>{0}/{1}/{2}</color>{3}",
                        _vehicleInput.inputSettings.cameraLookRightTop,
                        _vehicleInput.inputSettings.cameraLookRightBottom,
                        _vehicleInput.inputSettings.cameraLookRightBackward,
                        System.Environment.NewLine);
            _controlsText += string.Format("上方/車底： <color=yellow>{0}/{1}</color>{2}", _vehicleInput.inputSettings.cameraLookUp, _vehicleInput.inputSettings.cameraLookDown, System.Environment.NewLine);

            txtControls.text = _controlsText;
        }
    }
}
