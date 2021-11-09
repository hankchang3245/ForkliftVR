using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;
using ArduinoBluetoothAPI;
using edu.tnu.dgd.debug;
using edu.tnu.dgd.vr.calibration;
using edu.tnu.dgd.game;

namespace edu.tnu.dgd.bluetooth
{
    public class BluetoothWithKeyboardInputAdapter : MonoBehaviour
    {
        // Forklift
        public bool isEngineOn = true;
        [HideInInspector]
        public float forksUp = 0f;
        [HideInInspector]
        public float forksDown = 0f;
        [HideInInspector]
        public float mastTiltBackwards = 0f;
        [HideInInspector]
        public float mastTiltForwards = 0f;
        [HideInInspector]
        public float forksRight = 0f;
        [HideInInspector]
        public float forksLeft = 0f;

        // Vehicle
        [HideInInspector]
        public int accelerationInt = 0;

        //[HideInInspector]
        //public float accelerationOffset = 0f;

        [HideInInspector]
        public float turnRight = 0f;
        [HideInInspector]
        public float turnLeft = 0f;

        [HideInInspector]
        public float brakeForPedal = 0f;

        [HideInInspector]
        public float brakeOffset = 0f;

        [HideInInspector]
        public float handbrake = 0f;
        [HideInInspector]
        public float clutch = 0f;
        [HideInInspector]
        public float horn = 0f;
        [HideInInspector]
        public float headlights = 0f;
        [HideInInspector]
        public float interiorLights = 0f;
        [HideInInspector]
        public float leftSignalLights = 0f;
        [HideInInspector]
        public float rightSignalLights = 0f;
        [HideInInspector]
        public byte gearPosition = BT_GearStick_Neutral;


        public const int ByteIndex_GearAndCalibation = 0; // 定位、前進、後退、空檔
        public const int ByteIndex_Brake = 1;           // 煞車
        public const int ByteIndex_Gas = 2;             // 油門
        public const int ByteIndex_ForkUpDown = 3;      // 貨叉升降推桿
        public const int ByteIndex_ForkTilt = 4;        // 貨叉傾斜推桿
        public const int ByteIndex_HandBrake = 5;       // 手煞車
        public const int ByteIndex_SteeringWheel = 6;   // 方向盤

        public const int TestDataQradient_Index = ByteIndex_ForkUpDown;

        // Bluetooth Byte Index: 0 (前進/後退)
        public const byte BT_GearStick_Drive = 1;
        public const byte BT_GearStick_Neutral = 2;
        public const byte BT_GearStick_Reverse = 4;
        public const byte BT_Calibration = 8;
        public const byte BT_FeetOn_Ground = 16;
        public const byte BT_FeetOn_Brake = 32;
        public const byte BT_FeetOn_Gas = 64;

        public GameObject bt_connected;
        public GameObject bt_disconnected;
        public UnityEvent[] customEventTriggers;
        public string deviceName = "DGDBT"; // 密碼

        private BluetoothHelper _bluetoothHelper;
        //public ShowDebugLog _log;
        //private int _retryConnectionCount = 0;
        //private int[] _dataGradient = new int[20];
        private byte[] _receivedBytes;

        // 判斷加速或減速
        //private int _prevAccelerationInt;

        

        void Start()
        {
            /*
            if (DontDestroyOnLoadController.instance.bluetoothConnected)
            {
                BT_Connected();
            }
            else
            {
                BT_DisConnected();
            }
            */
            TryConnection();
        }

        private void TryConnection()
        {
            try
            {
                _bluetoothHelper = BluetoothHelper.GetInstance(deviceName);
                _bluetoothHelper.OnConnected += OnConnected;
                _bluetoothHelper.OnConnectionFailed += OnConnectionFailed;
                _bluetoothHelper.OnDataReceived += OnMessageReceived;

                _bluetoothHelper.setLengthBasedStream();
                _bluetoothHelper.Connect();
            }
            catch (Exception ex)
            {
                if (_bluetoothHelper != null && _bluetoothHelper.isConnected())
                {
                    BT_Connected();
                }
                else
                {
                    BT_DisConnected();
                }
                
                //if (_log != null) _log.Log("Exception：" + ex.Message);
            }
        }

        public void BT_Connected()
        {
            bt_connected.SetActive(true);
            bt_disconnected.SetActive(false);
        }

        public void BT_DisConnected()
        {
            bt_connected.SetActive(false);
            bt_disconnected.SetActive(true);
        }

        IEnumerator blinkSphere()
        {
            yield return new WaitForSeconds(0.5f);
        }

        void Update()
        {
            if (_bluetoothHelper == null)
            {
                TryConnection();
            }
        }

        void OnMessageReceived(BluetoothHelper helper)
        {
            if (helper == null || !helper.Available)
            {
                return;
            }

            _receivedBytes = _bluetoothHelper.ReadBytes();
            if (_receivedBytes == null || _receivedBytes.Length < 7)
            {
                //if (_log != null) _log.Log("Received data is null!");
                return;
            }
            //if (_log != null) _log.Log(" Data", BitConverter.ToString(_receivedBytes));

            //_dataGradient[_receivedBytes[TestDataQradient_Index]]++;
            //string gradient = String.Join(",", Array.ConvertAll(_dataGradient, ele => ele.ToString()));
            //_log.Log(TestDataQradient_Index + "-Sum", gradient);

            /**************************************************************
             * 第 0 位元組：前進(0x1)、空檔(0x2)、後退(0x4)、校正(0x8)
             **************************************************************/
            if ((_receivedBytes[ByteIndex_GearAndCalibation] & BT_Calibration) > 0)
            {
                CalibrationController.instance.AutoCalibrate();
                GuideController.instance.StoreAllTrafficPolePosition();
            }

            if ((_receivedBytes[ByteIndex_GearAndCalibation] & BT_GearStick_Neutral) > 0)
            {
                gearPosition = BT_GearStick_Neutral;
            }
            else if ((_receivedBytes[ByteIndex_GearAndCalibation] & BT_GearStick_Drive) > 0)
            {
                gearPosition = BT_GearStick_Drive;
            }
            else if ((_receivedBytes[ByteIndex_GearAndCalibation] & BT_GearStick_Reverse) > 0)
            {
                gearPosition = BT_GearStick_Reverse;
            }
            /*****************************************************************
             * 第 1 位元組：煞車(0 ~ 9)
             *****************************************************************/
            int ff = _receivedBytes[ByteIndex_Brake];
            brakeForPedal = ff / 9f;

            if (ff > 0)
            {
                brakeOffset += (ff/30f);
            }
            else
            {
                brakeOffset = 0f;
            }

            /*****************************************************************
             * 第 2 位元組：油門(0 ~ 9)
             *****************************************************************/
            this.accelerationInt = (int)_receivedBytes[ByteIndex_Gas];
            //this.acceleration = accInt / 9f;
            //this.accelerationOffset = (accInt - this._prevAccelerationInt) / 9f;
            //this._prevAccelerationInt = accInt;

            /*
            if (this.brakeForUI > 0.1f)
            {
                this.accelerationOffset = 0f;
            }
            */

            /*****************************************************************
             * 第 3 位元組：升降推桿(0 ~ 10) ==> 0~4：往上，5：不動、6~10：往下 
             *****************************************************************/
            int forkUpDown = (int)_receivedBytes[ByteIndex_ForkUpDown];
            this.forksUp = 0f;
            this.forksDown = 0f;
            if (forkUpDown <= 4)
            {
                this.forksDown = (5f - forkUpDown) / 5f;
            } 
            else if (forkUpDown >= 6)
            {
                this.forksUp = (forkUpDown - 5f) / 5f;
            }

            /*****************************************************************
             * 第 4 位元組：傾斜推桿(0 ~ 10) ==> 0~4：往後，5：不動、6~10：往前 
             *****************************************************************/
            int forkTilt = (int)_receivedBytes[ByteIndex_ForkTilt];
            this.mastTiltBackwards = 0f;
            this.mastTiltForwards = 0f;
            if (forkTilt <= 4)
            {
                this.mastTiltBackwards = (5f - forkTilt) / 5f;
            }
            else if (forkTilt >= 6)
            {
                this.mastTiltForwards = (forkTilt - 5f) / 5f;
            }
            /*****************************************************************
             * 第 5 位元組：手剎車(0 ~ 10)
             *****************************************************************/
            int hbrake = (int)_receivedBytes[ByteIndex_HandBrake];
            this.handbrake = hbrake / 10f;

            /*****************************************************************
             * 第 6,7 位元組：方向盤(0 - 320))
             * 中間數是160、一邊是 0-159、另一邊是 161-320
             *****************************************************************/
            int steeringWheelInt = (int)_receivedBytes[ByteIndex_SteeringWheel];
            steeringWheelInt = steeringWheelInt*256 + (int)_receivedBytes[ByteIndex_SteeringWheel+1];


            const float midValue = 160f;
            //if (159 <= steeringWheelInt && steeringWheelInt <= 161)
            if (steeringWheelInt == midValue)
            {
                this.turnLeft = 0f;
                this.turnRight = 0f;
            } 
            else if (steeringWheelInt > (midValue))
            {
                this.turnLeft = (steeringWheelInt - midValue) / midValue;
                this.turnRight = 0f;
            } 
            else
            {
                this.turnLeft = 0f;
                this.turnRight = (midValue - steeringWheelInt) / midValue;
            }
        }

        void OnConnected(BluetoothHelper helper)
        {
            try
            {
                helper.StartListening();
                BT_Connected();
                //DontDestroyOnLoadController.instance.bluetoothConnected = true;
                //if (_log != null) _log.Log("Bluetooth start listening....");
            }
            catch (Exception ex)
            {
                BT_DisConnected();
                //DontDestroyOnLoadController.instance.bluetoothConnected = false;
                //if (_log != null) _log.Log("Exception:" + ex.Message);
            }
        }

        void OnConnectionFailed(BluetoothHelper helper)
        {
            if (helper != null)
            {
                helper = null;
            }

            //_retryConnectionCount++;
            BT_DisConnected();
            //if (_log != null) _log.Log("Connection Failed... Try Connection ....Retry Count:" + _retryConnectionCount);
            TryConnection();
        }

        void OnDestroy()
        {
            //if (_log != null) _log.Log("Bluetooth connection is CLOSED!");

            BT_DisConnected();
            if (_bluetoothHelper != null)
                _bluetoothHelper.Disconnect();
        }

    }
}
