using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System;
using edu.tnu.dgd.menu;
using edu.tnu.dgd.forklift;
using edu.tnu.dgd.vehicle;
using UnityEngine.Events;
using UnityStandardAssets.Vehicles.Car;
using UnityStandardAssets.CrossPlatformInput;
using edu.tnu.dgd.game;

namespace edu.tnu.dgd.bluetooth
{
    [RequireComponent(typeof(BluetoothInputAdapter))]
    public class BluetoothPlayerInput : MonoBehaviour
    {
        public BluetoothInputAdapter _inputAdapter;
        public ForkliftController _forkliftController;
        public VehicleController _vehicleController;

        private float _mastTilt = 0;
        private float _forksVertical = 0;

        //private int _accelerationInt = 0;
        //private float _acceleration = 0f;
        private float _steering = 0f;

        private VehicleGearPosition _gearPosition = VehicleGearPosition.Neutral;

        public GameController _gameController;
        public TrainngMenuController _trainngMenuController;

        void Start()
        {
            _vehicleController.useVRInput = true;

            Assert.IsNotNull(_forkliftController);
            Assert.IsNotNull(_vehicleController);
            Assert.IsNotNull(_inputAdapter);
            Assert.IsNotNull(_gameController);
            Assert.IsNotNull(_trainngMenuController);
        }

        void Update()
        {

            /******************************
             * 貨叉控制
             ******************************/
            _forkliftController.IsEngineOn = _inputAdapter.isEngineOn;

            // 貨叉傾斜
            if (_inputAdapter.mastTiltBackwards > 0)
            {
                _mastTilt = -1 * _inputAdapter.mastTiltBackwards;
            }
            else if (_inputAdapter.mastTiltForwards > 0)
            {
                _mastTilt = _inputAdapter.mastTiltForwards;
            }
            else
            {
                _mastTilt = 0f;
            }
            _forkliftController.RotateMast(_mastTilt);

            // 貨叉升降
            if (_inputAdapter.forksUp > 0)
            {
                _forksVertical = _inputAdapter.forksUp;
            }
            else if (_inputAdapter.forksDown > 0)
            {
                _forksVertical = -1 * _inputAdapter.forksDown;
            }
            else
            {
                _forksVertical = 0f;
            }
            _forkliftController.MoveForksVertically(_forksVertical);

            // 貨叉動畫更新
            _forkliftController.UpdateLevers(_forksVertical, _mastTilt);


            /******************************
             * 車輛控制
             ******************************/
            _vehicleController.IsEngineOn = _inputAdapter.isEngineOn;

            if (_inputAdapter.gearPosition == BluetoothInputAdapter.BT_GearStick_Neutral) // 空檔
            {
                _gearPosition = VehicleGearPosition.Neutral;
                _trainngMenuController.ShowToggleButton(true);
            }
            else if (_inputAdapter.gearPosition == BluetoothInputAdapter.BT_GearStick_Drive) // 前進
            {
                _gearPosition = VehicleGearPosition.Drive;
                _trainngMenuController.ShowToggleButton(false);
            }
            else if (_inputAdapter.gearPosition == BluetoothInputAdapter.BT_GearStick_Reverse) // 後退
            {
                _gearPosition = VehicleGearPosition.Reverse;
                _trainngMenuController.ShowToggleButton(false);
            }
            _vehicleController.GearPosition = _gearPosition;

            // 油門
            _vehicleController.AccelerationGradientInput = _inputAdapter.accelerationInt/9f;
            _vehicleController.AccelerationInt = _inputAdapter.accelerationInt;

            // 方向盤
            // turn left  (反時針) ==> > 0
            // turn right (順時針) ==> < 0
            _steering = 0f;
            _steering = (_inputAdapter.turnRight > 0f) ? -1 * _inputAdapter.turnRight : 0;
            _steering = (_inputAdapter.turnLeft > 0f) ? _inputAdapter.turnLeft : _steering;
            _vehicleController.SteeringInput = _steering;

            // 煞車
            _vehicleController.BrakeForPedal = _inputAdapter.brakeForPedal;
            //_vehicleController.BrakeOffsetInput = _inputAdapter.brakeOffset;
            _vehicleController.BrakeInt = _inputAdapter.brakeInt;

            if (_inputAdapter.brakeForPedal > 0f)
            {
                _vehicleController.AccelerationGradientInput = 0f;
                _vehicleController.AccelerationInt = 0;
            }

            // 手煞車
            _vehicleController.HandBrakeInput = _inputAdapter.handbrake;

            if (_gameController.startElapsedTimeTimer == false)
            {
                _gameController.startElapsedTimeTimer = _vehicleController.CanStartMoving();
            }

            // 離合器
            //_vehicleController.ClutchInput = (_inputAdapter.clutch > 0) ? 1f : 0f;

            // ------------------
            /* 用不到
            if (_inputAdapter.horn > 0)
                _vehicleController.Horn();

            if (_inputAdapter.headlights > 0)
                _vehicleController.HeadlightsOn = !_vehicleController.HeadlightsOn;

            if (_inputAdapter.interiorLights > 0)
                _vehicleController.InteriorLightsOn = !_vehicleController.InteriorLightsOn;

            if (_inputAdapter.leftSignalLights > 0)
                _vehicleController.LeftSinalLightsOn = !_vehicleController.LeftSinalLightsOn;

            if (_inputAdapter.rightSignalLights > 0)
                _vehicleController.RightSinalLightsOn = !_vehicleController.RightSinalLightsOn;

            for (int i = 0; i < _inputAdapter.customEventTriggers.Length; i++)
            {
                _inputAdapter.customEventTriggers[i]?.Invoke();
            }
            */
        }
    }
}