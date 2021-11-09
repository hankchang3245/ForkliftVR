using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Assertions;
using edu.tnu.dgd.game;
using UnityStandardAssets.Vehicles.Car;
using UnityStandardAssets.CrossPlatformInput;

namespace edu.tnu.dgd.vehicle
{ 
    public class VehiclePlayerInputPC : MonoBehaviour
    {
        public bool enablePlayerInput = true;
        public VehicleInputSettingsPC inputSettings;
        public UnityEvent[] customEvents;

        public VehicleController _vehicleController;

        //private int _accelerationInt = 0;
        private float _steering = 0f;
        private float _brake = 0f;

        private const float STEERING_TOTAL = 320f;
        //private const float GAS_TOTAL = 10f;

        public float steeringSpeed = 0.75f;
        public float accelSpeed = 0.02f;
        public float brakeSpeed = 0.015f;
        private float accumulatedAccel = 0f;
        private float accumulatedBrake = 0f;

        public GameController _gameController;

        /// <summary>
        /// Initializing references
        /// </summary>
        void Start()
        {
            Assert.IsNotNull(inputSettings);
            Assert.IsNotNull(_vehicleController);

            _vehicleController.HandBrakeInput = 1f; // 一開始手剎車是啟用的
        }
        
        /// <summary>
        /// Handling player input
        /// </summary>
        void Update()
        {
            if (enablePlayerInput)
            {
                if (inputSettings == null) return;

                #region Vehicle Controls

                /*
                if (Input.GetKeyDown(inputSettings.resetVehiclePosition))
                {
                    GameController.instance.ResetVehiclePosition();
                    //_vehicleController.ResetPosition();
                    //GuideController.instance.ResetStation();
                    //FailedSignObjectCollector.instance.RemoveAll();
                }
                */
                /*
                if (Input.GetKeyDown(KeyCode.Alpha9))
                {
                    GameController.instance.RestartLevel();
                }
                */
                if (Input.GetKeyDown(inputSettings.toggleEngine))
                {
                    _vehicleController.IsEngineOn = !_vehicleController.IsEngineOn;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha0))
                {
                    _gameController.GoToPosition(0);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    _gameController.GoToPosition(1);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    _gameController.GoToPosition(2);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    _gameController.GoToPosition(3);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    _gameController.GoToPosition(4);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha5))
                {
                    _gameController.GoToPosition(5);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha6))
                {
                    _gameController.GoToPosition(6);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha7))
                {
                    _gameController.GoToPosition(7);
                }
                else if (Input.GetKeyDown(inputSettings.cleanObstacle))
                {
                    _gameController.CleanObstacle();
                }


                if (Input.GetKeyDown(inputSettings.gearPositionNeutral))
                {
                    _vehicleController.GearPosition = VehicleGearPosition.Neutral;
                }
                else if (Input.GetKeyDown(inputSettings.gearPositionDrive))
                {
                    _vehicleController.GearPosition = VehicleGearPosition.Drive;
                }
                else if (Input.GetKeyDown(inputSettings.gearPositionReverse))
                {
                    _vehicleController.GearPosition = VehicleGearPosition.Reverse;
                }

                // 方向盤 steeringSpeed加負號是必須的
                _steering += (-steeringSpeed * Input.GetAxis("Horizontal"));
                _steering = Mathf.Clamp(_steering, -STEERING_TOTAL / 2, STEERING_TOTAL / 2);
                if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow))
                {
                    _steering = 0f;
                }
                _vehicleController.SteeringInput = _steering * 2f / STEERING_TOTAL;

                // 加油
                accumulatedAccel += (accelSpeed * Input.GetAxis("Vertical"));
                accumulatedAccel = Mathf.Clamp(accumulatedAccel, 0f, 9f);
                int accInt = Mathf.RoundToInt(accumulatedAccel);

                _vehicleController.AccelerationInt = accInt;
                _vehicleController.AccelerationGradientInput = accInt / 9f;

                if (_gameController.startElapsedTimeTimer == false)
                {
                    _gameController.startElapsedTimeTimer = _vehicleController.CanStartMoving();
                }

                // 煞車
                if (Input.GetKey(inputSettings.brakes))
                {
                    accumulatedBrake += brakeSpeed;
                } 
                else
                {
                    accumulatedBrake = 0f;
                }
                accumulatedBrake = Mathf.Clamp(accumulatedBrake, 0f, 9f);
                int brakeInt = Mathf.RoundToInt(accumulatedBrake);

                //_vehicleController.BrakeOffsetInput = _brake;
                _vehicleController.BrakeInt = brakeInt;
                _vehicleController.BrakeForPedal = brakeInt / 9f;


                if (brakeInt > 0)
                {
                    accumulatedAccel = 0f;
                    _vehicleController.AccelerationGradientInput = 0f;
                    _vehicleController.AccelerationInt = 0;
                }

                //Debug.LogFormat(">>>>>>>>>>>>>>>>>>>>brakeInt={0}  accInt={1}", brakeInt, accInt);

                /* Light
                 * 
                if (Input.GetKeyDown(inputSettings.horn))
                    _vehicleController.Horn();

                if (Input.GetKeyDown(inputSettings.headlights))
                    _vehicleController.HeadlightsOn = !_vehicleController.HeadlightsOn;

                if (Input.GetKeyDown(inputSettings.interiorLights))
                    _vehicleController.InteriorLightsOn = !_vehicleController.InteriorLightsOn;

                if (Input.GetKeyDown(inputSettings.leftSignalLights))
                    _vehicleController.LeftSinalLightsOn = !_vehicleController.LeftSinalLightsOn;

                if (Input.GetKeyDown(inputSettings.rightSignalLights))
                    _vehicleController.RightSinalLightsOn = !_vehicleController.RightSinalLightsOn;

                if (Input.GetKeyDown(inputSettings.toggleCamera))
                    _vehicleController.CameraToggleRequested = true;

                */

                if (Input.GetKey(inputSettings.handbrakeDisable))
                    _vehicleController.HandBrakeInput = 0f;
                else if (Input.GetKey(inputSettings.handbrakeEnable))
                    _vehicleController.HandBrakeInput = 1f;
                else if (Input.GetKey(inputSettings.cameraLookForward))
                    _vehicleController.CamLookDirection = VehicleCameraLookDirection.Forward;
                else if (Input.GetKey(inputSettings.cameraLookCenter))
                    _vehicleController.CamLookDirection = VehicleCameraLookDirection.Center;
                else if (Input.GetKey(inputSettings.cameraLookBackward))
                    _vehicleController.CamLookDirection = VehicleCameraLookDirection.Backward;
                else if (Input.GetKey(inputSettings.cameraLookLeftTop))
                    _vehicleController.CamLookDirection = VehicleCameraLookDirection.LeftTop;
                else if (Input.GetKey(inputSettings.cameraLookLeftBottom))
                    _vehicleController.CamLookDirection = VehicleCameraLookDirection.LeftBottom;
                else if (Input.GetKey(inputSettings.cameraLookLeftBackward))
                    _vehicleController.CamLookDirection = VehicleCameraLookDirection.LeftBackward;
                else if (Input.GetKey(inputSettings.cameraLookRightTop))
                    _vehicleController.CamLookDirection = VehicleCameraLookDirection.RightTop;
                else if (Input.GetKey(inputSettings.cameraLookRightBottom))
                    _vehicleController.CamLookDirection = VehicleCameraLookDirection.RightBottom;
                else if (Input.GetKey(inputSettings.cameraLookRightBackward))
                    _vehicleController.CamLookDirection = VehicleCameraLookDirection.RightBackward;
                else if (Input.GetKey(inputSettings.cameraLookUp))
                    _vehicleController.CamLookDirection = VehicleCameraLookDirection.Up;
                else if (Input.GetKey(inputSettings.cameraLookDown))
                    _vehicleController.CamLookDirection = VehicleCameraLookDirection.Down;

                #endregion

                #region Player Custom Events

                for (int i = 0; i < inputSettings.customEventTriggers.Length; i++)
                {
                    if (Input.GetKeyDown(inputSettings.customEventTriggers[i]))
                    {
                        if (customEvents.Length > i)
                            customEvents[i].Invoke();
                    }
                }

                #endregion 
            }
        }
        
    } 
}
