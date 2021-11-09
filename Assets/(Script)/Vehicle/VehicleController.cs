using System.Collections.Generic;
using UnityEngine;
using TMPro;
using edu.tnu.dgd.game;
using UnityEngine.Events;

namespace edu.tnu.dgd.vehicle
{
    [RequireComponent(typeof(Rigidbody))]
    public class VehicleController : MonoBehaviour
    {
        private static VehicleController _instance;
        public static VehicleController instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<VehicleController>();
                }

                return _instance;
            }
        }

        #region VARIABLES AND PROPERTIES

        private const float _mphConversion = 2.23694f;
        private const float _kphConversion = 3.6f;

        public VehicleDrivetrainType drivetrainType;
        public VehicleSteeringMode steeringMode;
        public VehicleSpeedUnit speedUnit;
        private VehicleTransmissionType _transmissionType = VehicleTransmissionType.Automatic;
        private VehicleCameraLookDirection _camLookDirection;

        public bool useVRInput = false;

        [SerializeField] public WheelCollider[] frontWheelsColliders;
        [SerializeField] public WheelCollider[] rearWheelsColliders;
        [SerializeField] public WheelCollider[] extraWheelsColliders;
        [SerializeField] public GameObject[] frontWheelsMeshes;
        [SerializeField] public GameObject[] rearWheelsMeshes;
        [SerializeField] public GameObject[] extraWheelsMeshes;
        [SerializeField] public Transform steeringWheel;
        [SerializeField] public Transform gasPedal;
        [SerializeField] public Transform brakesPedal;
        [SerializeField] public Transform clutchPedal;
        [SerializeField] public Transform gearStick; // 前進後退
        [SerializeField] public Transform handbrakeStick; // 手剎車

        [SerializeField] public Transform centerOfMass;
        [SerializeField] public AudioSource engineStartSFX;
        [SerializeField] public AudioSource engineSFX;
        [SerializeField] public AudioSource hornSFX;
        [SerializeField] public AudioSource wheelsSkiddingSFX;
        [SerializeField] public AudioSource backUpBeeperSFX;
        [SerializeField] public AudioSource signalLightsSFX;
        [SerializeField] public AudioSource openDoorSFX;
        [SerializeField] public AudioSource closeDoorSFX;
        [SerializeField] public TMP_Text speedText;
        [SerializeField] public TMP_Text gearText;
        [SerializeField] public TMP_Text wheelAngleText;
        private GameObject forShowRotationAngleWheelsMeshes;
        [SerializeField] public bool playSFX = true;
        [SerializeField] public float sfxVolume = 0.5f;

        [SerializeField] public Light[] headlights;
        [SerializeField] public Light[] rearLights;
        [SerializeField] public Light[] brakeLights;
        [SerializeField] public Light[] interiorLights;
        [SerializeField] public Light[] signalLightsLeft;
        [SerializeField] public Light[] signalLightsRight;
        [SerializeField] public Light[] reverseAlarmLights;
        [SerializeField] public Transform alarmLightsPivot;
        [SerializeField] public VehicleDoor driverDoor;
        [SerializeField] public VehicleDoor[] passengersDoors;

        public float TRUST_TORQUE = 0f;


        // Local only
        private Rigidbody _rigidbody;
        private WheelCollider[] _steeringWheelsColliders;
        private int _steeringWheelsCollidersCount = 0;
        private List<WheelCollider> _torqueWheelsColliders;
        private List<WheelCollider> _allWheelsColliders;
        private float _torqueWheelsCount = 0f;
        private float _individualWheelTorque = 0f;
        private float _individualWheelReverseTorque = 0f;
        private float _thrustTorque = 0f;
        private float _currentTorque = 0f;
        private float _currentSteerAngle = 0f;
        private float _steerHelperLastRotation = 0f;
        private float _currentGasPedalAngle = 0f;
        private float _currentBrakesPedalAngle = 0f;
        private float _currentClutchPedalAngle = 0f;
        private float _currentRevs = 0f; // Calculated only when the Revs propertie is requested, if you need this, call Revs instead
        private bool _leftSinalLightsOn = false;
        private bool _rightSinalLightsOn = false;
        private bool _movingBackwards = false;
        private bool _cameraToggleRequested = false;
        private float _defaultSteeringSpeed = 100f;
        private float _articulatedSteeringSpeed = 25f;

        private float _gearStickForwardAngle = 0f;
        private float _gearStickBackwardAngle = 0f;
        private float _handbrakeStickAngle = 0f;

        private Vector3 _wheelPosition;
        private Quaternion _wheelRotation;

        // Input
        private float _steering = 0f;
        //private float _brakeOffset = 0f;
        private float _brakeForPedal = 0f;
        private float _handbrake = 0f;
        private float _clutch = 0f;

        private float _accelerationGradient = 0f;
        private float _handbrakeGradient = 1f;

        // Settings
        [SerializeField] private bool _isEngineOn = true;
        [Range(0f, 180f)]
        [SerializeField] private float _maxSteeringAngle = 80f;
        [SerializeField] private float _highSpeedSteeringAngle = 10f;
        [SerializeField] private float _maxTorque = 1000f;
        [SerializeField] private float _reverseTorque = 500f;
        [SerializeField] private float _brakesTorque = 5000f;
        [SerializeField] private float _handbrakeTorque = 10000f;
        [SerializeField] private float _idleEngineRevs = 800f; // Engine RPM when vehicle is idling
        [SerializeField] private float _maxEngineRevs = 3000f; // Maximum engine RPM
        [SerializeField] private float _maxSpeed = 10f;
        [SerializeField] private float _noGasMaxSpeed = 2f;
        [SerializeField] private float _maxReverseSpeed = 6f;
        [SerializeField] private bool _softSteering = true;
        [SerializeField] private float _steeringWheelAngleMultiplierClockwise = 10.85f;
        [SerializeField] private float _steeringWheelAngleMultiplierCounterClockwise = 10.125f;
        [SerializeField] private float _downforce = 100f;
        [SerializeField] private int _numberOfGears = 1;
        [Range(0f, 1f)]
        [SerializeField] private float _steerHelper = 1;
        [Range(0f, 1f)]
        [SerializeField] private float _tractionControl = 1f;
        [SerializeField] private float _tractionSlipLimit = 0.6f;
        [SerializeField] private Vector3 _centerOfMassOffset = Vector3.zero;
        [SerializeField] private float _minEnginePitch = 1f;
        [SerializeField] private float _maxEnginePitch = 2f;
        [SerializeField] private float _sfxForwardSlipLimit = 0.9f;
        [SerializeField] private float _sfxSidewaysSlipLimit = 0.6f;
        [SerializeField] private bool _headlightsOn = false;
        [SerializeField] private bool _interiorLightsOn = false;
        [SerializeField] private bool _rotateAlarmLights = false;
        [SerializeField] private float _alarmLightsRotationSpeed = 10f;


        private float _steeringWheelAngle = 0f; // 方向盤旋轉角度

        private bool _nonMovingArticulatedSteering = false;
        private int _currentGear = 0;
        private VehicleGearPosition _gearPosition = VehicleGearPosition.Neutral;
        private float _currentSpeed = 0f;

        private float[] _gearsSpeedLimits;
        private float _speedUnitConversion = 0f;

        [HideInInspector]
        public UnityEvent cameraDirectionChanged;

        private Vector3 _originLocalPosition;
        private Quaternion _originLocalRotation;

        private Vector3 _originReverseLocalPosition;
        private Quaternion _originReverseLocalRotation;

        //private float noGasAcceleration = 0f;

        private float _torqueMultiplication = 1.5f;
        private float _torqueLimitation = 250f;

        public float TorqueMultiplication
        {
            get
            {
                return _torqueMultiplication;
            }

            set
            {
                this._torqueMultiplication = value;
            }
        }

        public float TorqueLimitation
        {
            get
            {
                return _torqueLimitation;
            }

            set
            {
                this._torqueLimitation = value;
            }
        }

        public int AccelerationInt
        {
            get
            {
                return _accelerationInt;
            }

            set
            {
                _accelerationInt = value;
            }
        }

        // 本屬性是用來檢查車輛是否已開始啟動可移動
        public bool CanStartMoving()
        {

            if ((_gearPosition == VehicleGearPosition.Drive || _gearPosition == VehicleGearPosition.Reverse) && _currentSpeed > 0.1f)
            {
                return true;
            }

            return false;
        }
        public float SteeringWheelAngle { get; }
        public Vector3 OriginLocalPosition { get; set; }
        public Quaternion OriginLocalRotation { get; set; }
        public Vector3 OriginReverseLocalPosition { get; set; }
        public Quaternion OriginReverseLocalRotation { get; set; }

        // Inputs
        public float SteeringInput { get { return _steering; } set { _steering = Mathf.Clamp(value, -1f, 1f); } }
        
        public float AccelerationGradientInput
        {
            get
            {
                return _accelerationGradient;
            }

            set
            {
                _accelerationGradient = Mathf.Clamp(value, 0f, 1f);
            }
        }


        public float NoGasMaxSpeed
        {
            get
            {
                return _noGasMaxSpeed;
            }

            set
            {
                _noGasMaxSpeed = value;
            }
        }

        /*
        public float BrakeOffsetInput
        {
            get
            {
                return _brakeOffset;
            }

            set
            {
                _brakeOffset = Mathf.Clamp01(value);
            }
        }
        */

        public int BrakeInt
        {
            get
            {
                return _brakeInt;
            }

            set
            {
                _brakeInt = value;
            }
        }

        public float BrakeForPedal
        {
            get
            {
                return _brakeForPedal;
            }

            set
            {
                _brakeForPedal = Mathf.Clamp01(value);
            }
        }

        
        public float HandBrakeInput
        {
            get
            {
                return _handbrake;
            }

            set
            {
                _handbrakeGradient = Mathf.Clamp01(value);
                _handbrake = (_handbrakeGradient > 0) ? 1f : 0f;
            }
        }

        public float ClutchInput { get { return _clutch; } set { _clutch = Mathf.Clamp01(value); } }
        public float MaxTorque { get { return _maxTorque; } set { _maxTorque = Mathf.Abs(value); } }
        public float ReverseTorque { get { return _reverseTorque; } set { _reverseTorque = Mathf.Abs(value); } }
        public float BrakesTorque { get { return _brakesTorque; } set { _brakesTorque = Mathf.Abs(value); } }
        public float HandbrakesTorque { get { return _handbrakeTorque; } set { _handbrakeTorque = Mathf.Abs(value); } }
        public float MaxSpeed { get { return _maxSpeed; } set { _maxSpeed = Mathf.Abs(value); } }
        public float MaxReverseSpeed { get { return _maxReverseSpeed; } set { _maxReverseSpeed = Mathf.Abs(value); } }
        public float Downforce { get { return _downforce; } set { _downforce = Mathf.Abs(value); } }
        public float MaxSteeringAngle { get { return _maxSteeringAngle; } set { _maxSteeringAngle = Mathf.Clamp(value, 0f, 360f); } }
        public float HighSpeedSteeringAngle { get { return _highSpeedSteeringAngle; } set { _highSpeedSteeringAngle = Mathf.Clamp(value, 0f, 360f); } }
        public bool SoftSteering { get { return _softSteering; } set { _softSteering = value; } }
        public int NumberOfGears { get { return _numberOfGears; } set { _numberOfGears = Mathf.Abs(value); } }
        public float SteerHelper { get { return _steerHelper; } set { _steerHelper = Mathf.Clamp01(value); } }
        public float TractionControl { get { return _tractionControl; } set { _tractionControl = Mathf.Clamp01(value); } }
        public float TractionSlipLimit { get { return _tractionSlipLimit; } set { _tractionSlipLimit = Mathf.Abs(value); } }
        public float SfxForwardSlipLimit { get { return _sfxForwardSlipLimit; } set { _sfxForwardSlipLimit = Mathf.Abs(value); } }
        public float SfxSidewaysSlipLimit { get { return _sfxSidewaysSlipLimit; } set { _sfxSidewaysSlipLimit = Mathf.Abs(value); } }
        public bool RotateAlarmLights { get { return _rotateAlarmLights; } set { _rotateAlarmLights = value; } }
        public float AlarmLightsRotationSpeed { get { return _alarmLightsRotationSpeed; } set { _alarmLightsRotationSpeed = value; } }
        //public bool NonMovingArticulatedSteering { get { return _nonMovingArticulatedSteering; } set { _nonMovingArticulatedSteering = value; } }

        public VehicleCameraLookDirection CamLookDirection
        {
            get
            {
                return _camLookDirection;
            }
            set
            {
                if (_camLookDirection != value)
                {
                    _camLookDirection = value;
                    if (cameraDirectionChanged != null)
                    {
                        cameraDirectionChanged.Invoke();
                    }
                }
            }
        }

        public VehicleGearPosition GearPosition { get { return _gearPosition; } set { _gearPosition = value; } }
        public float CurrentSpeed { get { return _currentSpeed; } }
        public int CurrentGear { get { return _currentGear; } }
        public Vector3 Velocity { get { return _rigidbody.velocity; } }

        public bool CameraToggleRequested
        {
            get
            {
                bool lastValue = _cameraToggleRequested;
                _cameraToggleRequested = false;
                return lastValue;
            }

            set
            {
                _cameraToggleRequested = value;
            }
        }

        public float Revs
        {
            get
            {
                if (_isEngineOn)
                {
                    _currentGear = (_currentGear <= 0) ? 1 : _currentGear;

                    float speedGearRatio = _currentSpeed / _gearsSpeedLimits[_currentGear - 1];
                    _currentRevs = Mathf.Lerp(_idleEngineRevs, _maxEngineRevs, speedGearRatio);
                    _currentRevs = Mathf.Round(_currentRevs);
                }
                else
                    _currentRevs = 0f;

                return _currentRevs;
            }
        }

        public bool IsEngineOn
        {
            get
            {
                return _isEngineOn;
            }

            set
            {
                if (!_isEngineOn && value)
                    StartEngine();
                else if (_isEngineOn && !value)
                    StopEngine();
            }
        }

        public float IdleEngineRevs
        {
            get
            {
                return _idleEngineRevs;
            }

            set
            {
                _idleEngineRevs = Mathf.Abs(value);
                _maxEngineRevs = _maxEngineRevs < _idleEngineRevs ? _idleEngineRevs : _maxEngineRevs;
            }
        }
        public float MaxEngineRevs
        {
            get
            {
                return _maxEngineRevs;
            }

            set
            {
                _maxEngineRevs = value < _idleEngineRevs ? _idleEngineRevs : Mathf.Abs(value);
            }
        }

        public float SteeringWheelAngleMultiplierClockwise
        {
            get
            {
                return _steeringWheelAngleMultiplierClockwise;
            }

            set
            {
                _steeringWheelAngleMultiplierClockwise = Mathf.Abs(value);
                _steeringWheelAngleMultiplierClockwise = _steeringWheelAngleMultiplierClockwise < 1f ? 1f : _steeringWheelAngleMultiplierClockwise;
            }
        }
        public float SteeringWheelAngleMultiplierCounterClockwise
        {
            get
            {
                return _steeringWheelAngleMultiplierCounterClockwise;
            }

            set
            {
                _steeringWheelAngleMultiplierCounterClockwise = Mathf.Abs(value);
                _steeringWheelAngleMultiplierCounterClockwise = _steeringWheelAngleMultiplierCounterClockwise < 1f ? 1f : _steeringWheelAngleMultiplierCounterClockwise;
            }
        }

        public Vector3 CenterOfMassOffset
        {
            get { return _centerOfMassOffset; }
            set
            {
                _centerOfMassOffset = value;

                if (centerOfMass != null) centerOfMass.localPosition = _centerOfMassOffset;
                if (_rigidbody != null) _rigidbody.centerOfMass = _centerOfMassOffset;
            }
        }

        public float MinEnginePitch
        {
            get { return _minEnginePitch; }
            set
            {
                _minEnginePitch = value;
                _maxEnginePitch = _maxEnginePitch < _minEnginePitch ? _minEnginePitch : _maxEnginePitch;
            }
        }

        public float MaxEnginePitch
        {
            get { return _maxEnginePitch; }
            set
            {
                _maxEnginePitch = value < _minEnginePitch ? _minEnginePitch : value;
            }
        }

        public bool HeadlightsOn
        {
            get { return _headlightsOn; }
            set
            {
                _headlightsOn = value;

                if (headlights != null)
                {
                    foreach (var headlight in headlights)
                        headlight.enabled = _headlightsOn;
                }

                if (rearLights != null)
                {
                    foreach (var rearLight in rearLights)
                        rearLight.enabled = _headlightsOn;
                }
            }
        }

        public bool InteriorLightsOn
        {
            get { return _interiorLightsOn; }
            set
            {
                _interiorLightsOn = value;

                if (interiorLights != null)
                {
                    foreach (var interiorLight in interiorLights)
                        interiorLight.enabled = _interiorLightsOn;
                }
            }
        }

        public bool LeftSinalLightsOn
        {
            get { return _leftSinalLightsOn; }
            set
            {
                _leftSinalLightsOn = value;

                if (_leftSinalLightsOn)
                {
                    InvokeRepeating("LeftSignalLights", 0.2f, 0.2f);
                    RightSinalLightsOn = false;
                }
                else
                {
                    CancelInvoke("LeftSignalLights");
                    ToggleLights(signalLightsLeft, false);
                }
            }
        }

        public bool RightSinalLightsOn
        {
            get { return _rightSinalLightsOn; }
            set
            {
                _rightSinalLightsOn = value;

                if (_rightSinalLightsOn)
                {
                    InvokeRepeating("RightSignalLights", 0.2f, 0.2f);
                    LeftSinalLightsOn = false;
                }
                else
                {
                    CancelInvoke("RightSignalLights");
                    ToggleLights(signalLightsRight, false);
                }
            }
        }

        public bool MovingBackwards
        {
            get { return _movingBackwards; }
        }

        #endregion

        private int _brakeInt = 0;

        private int noGasAccelerationDefaultInt = 0;
        private int _accelerationInt = 0;
        private int noGasAccelerationSignedInt = 0;
        private float[] _torqueMapping = new float[]{ // 10
            0f , 4f, 10f, 20f, 30f, 40f, 60f, 100f, 200f, 300f
        };

        private float[] _brakeMapping = new float[]{ // 10
            0f , 40f, 90f, 190f, 250f, 800f, 1200f, 1800f, 2300f, 2800f
        };

        public float MapTorque()
        {
            return MapTorque(_accelerationInt);
        }

        public float MapBrake()
        {
            return MapBrake(_brakeInt);
        }

        public float MapBrake(int brke)
        {
            if (brke > 9 || brke < 0)
            {
                return 0f;
            }

            return _brakeMapping[brke];
        }

        public float MapTorque(int gas)
        {
            if (gas > 9 || gas < 0)
            {
                return 0f;
            }

            float ff = 0f;
            if (_gearPosition == VehicleGearPosition.Drive)
            {
                ff = 1f;
            }
            else if (_gearPosition == VehicleGearPosition.Reverse)
            {
                ff = -1f;
            }
            ////Debug.LogFormat("=======MapTorque====ff= {0} ===noGasAccelerationInt={1} => gas={2}", ff, noGasAccelerationSignedInt,  gas);

            if (noGasAccelerationSignedInt != 0)  //無加油時
            {
                return _torqueMapping[Mathf.Abs(noGasAccelerationSignedInt)] * Mathf.Sign(noGasAccelerationSignedInt) * ff;
            }

            return _torqueMapping[gas] * ff;

        }

        private void Awake()
        {
            this.playSFX = PlayerPrefs.GetInt(StringConstants.Setting_ForkliftSFX, 0) > 0;
            this.sfxVolume = PlayerPrefs.GetInt(StringConstants.Setting_ForkliftSFXVolume, 3) / 10f;

            _maxTorque = PlayerPrefs.GetInt(StringConstants.Vehicle_MaxTorque, 100);
            _maxSpeed = PlayerPrefs.GetInt(StringConstants.Vehicle_MaxSpeed, 8);
            _torqueMultiplication = PlayerPrefs.GetFloat(StringConstants.Vehicle_TorqueMultiplication, 2f);
            _torqueLimitation = PlayerPrefs.GetFloat(StringConstants.Vehicle_TorqueLimitation, 300f);
            _brakesTorque = PlayerPrefs.GetFloat(StringConstants.Vehicle_BrakeTorque, 500f);


            //Debug.Log("=============_brakesTorque:" + _brakesTorque);

            GenerateTorqueMapping();
            /*
            for (int i = 0; i < 10; i++)
            {
                Debug.LogFormat("_torqueMapping[{0}]={1}", i, _torqueMapping[i]);
            }

            Debug.LogFormat("noGasAccelerationDefaultInt={0}", noGasAccelerationDefaultInt);
            */
        }

        #region UNITY DEFAULT EVENTS
        /// <summary>
        /// Use this for initialization
        /// </summary>
        void Start()
        {
            InitializeVehicle();
            if (_isEngineOn && engineSFX != null && !engineSFX.isPlaying && playSFX)
            {
                engineSFX.Play();
            }
        }

        /*
         * f(x) = 300/9^2 * x^2 ==> 300:TorqueLimitation
         */
        private void GenerateTorqueMapping()
        {
            bool hasFound = false;
            noGasAccelerationDefaultInt = 1;
            _torqueMapping = new float[10];
            for(int i = 0; i < 10; i++)
            {
                _torqueMapping[i] = Mathf.Pow(i, TorqueMultiplication) * (TorqueLimitation / Mathf.Pow(9f, TorqueMultiplication));
                
                if (hasFound == false && _torqueMapping[i] > 20f && _torqueMapping[i] <= 40f) //無加油時自動加入Torque之大小介於20與40
                {
                    hasFound = true;
                    noGasAccelerationDefaultInt = i;
                }
            }

        }

        /// <summary>
        /// Update physics
        /// </summary>
        void FixedUpdate()
        {
            if (!_isEngineOn)
                return;

            if (useVRInput)
            {
                VRSteering();
            }
            else
            {
                Steering();
            }
            ApplySteerHelper();
            Brakes();

            Gearbox();

            // Added by Hank 2020-12-17
            //AutomaticReachIdleSpeed();

            Engine();

            ApplyDownForce();
            ApplyTractionControl();
        }

        /// <summary>
        /// Update visuals
        /// </summary>
        private void Update()
        {
            UpdateWheelMeshesRotation(frontWheelsColliders, frontWheelsMeshes);
            UpdateWheelMeshesRotation(rearWheelsColliders, rearWheelsMeshes);
            //UpdateWheelMeshesRotation(extraWheelsColliders, extraWheelsMeshes);

            UpdatePedals();
            UpdateGearStick();
            UpdateHandbrakeStick();


            UpdateUI();
            VehicleSFX();
            //VehicleLights();
        }

        #endregion

        #region PRIVATE VEHICLE METHODS

        /// <summary>
        /// Initialize vehicle
        /// </summary>
        private void InitializeVehicle()
        {
            _rigidbody = GetComponent<Rigidbody>();

            if (centerOfMass != null) centerOfMass.localPosition = _centerOfMassOffset;
            if (_rigidbody != null) _rigidbody.centerOfMass = _centerOfMassOffset;

            /*
            _maxSpeedFactorMPH = _maxSpeed / _mphConversion;
            _maxSpeedFactorKPH = _maxSpeed / _kphConversion;
            */

            _speedUnitConversion = (speedUnit == VehicleSpeedUnit.KPH) ? _kphConversion : _mphConversion;

            // Set steering wheels
            switch (steeringMode)
            {
                case VehicleSteeringMode.FrontWheelsSteering:
                    _steeringWheelsColliders = frontWheelsColliders;

                    // Added by Hank 2020-12-05 Begin
                    if (rearWheelsMeshes.Length > 0)
                    {
                        forShowRotationAngleWheelsMeshes = frontWheelsMeshes[0];
                    }
                    // End

                    break;
                case VehicleSteeringMode.RearWheelsSteering:
                    _steeringWheelsColliders = rearWheelsColliders;

                    // Added by Hank 2020-12-05 Begin
                    if (rearWheelsMeshes.Length > 0)
                    {
                        forShowRotationAngleWheelsMeshes = rearWheelsMeshes[0];
                    }
                    // End

                    break;
            }


            _steeringWheelsCollidersCount = _steeringWheelsColliders.Length;

            // End
            _allWheelsColliders = new List<WheelCollider>();
            _allWheelsColliders.AddRange(frontWheelsColliders);
            _allWheelsColliders.AddRange(rearWheelsColliders);
            _allWheelsColliders.AddRange(extraWheelsColliders);

            // Connected wheels to the drivetrain
            _torqueWheelsColliders = new List<WheelCollider>();
            switch (drivetrainType)
            {
                case VehicleDrivetrainType.FWD:
                    _torqueWheelsColliders.AddRange(frontWheelsColliders);
                    break;
                case VehicleDrivetrainType.RWD:
                    _torqueWheelsColliders.AddRange(rearWheelsColliders);
                    break;
                case VehicleDrivetrainType.AWD:
                    _torqueWheelsColliders.AddRange(frontWheelsColliders);
                    _torqueWheelsColliders.AddRange(rearWheelsColliders);
                    _torqueWheelsColliders.AddRange(extraWheelsColliders);
                    break;
            }

            // Calculate individual wheels torque
            _torqueWheelsCount = _torqueWheelsColliders.Count;
            if (_torqueWheelsCount > 0)
            {
                _individualWheelTorque = (_maxTorque / _torqueWheelsCount);
                _individualWheelReverseTorque = (_reverseTorque / _torqueWheelsCount);
            }

            // Calculate gears speed limit array
            float speedGearFactor = (_maxSpeed / _numberOfGears);
            _gearsSpeedLimits = new float[_numberOfGears];
            for (int i = 0; i < _gearsSpeedLimits.Length; i++)
            {
                _gearsSpeedLimits[i] = Mathf.RoundToInt(speedGearFactor * (i + 1));
            }

            _currentTorque = _maxTorque - (_tractionControl * _maxTorque);

            if (driverDoor != null)
            {
                driverDoor.OpenSFX = openDoorSFX;
                driverDoor.CloseSFX = closeDoorSFX;
            }

            if (passengersDoors != null)
            {
                for (int i = 0; i < passengersDoors.Length; i++)
                {
                    passengersDoors[i].OpenSFX = openDoorSFX;
                    passengersDoors[i].CloseSFX = closeDoorSFX;
                }
            }

            if (engineStartSFX != null)
            {
                engineStartSFX.volume = sfxVolume;
            }

            if (engineSFX != null)
            {
                engineSFX.volume = sfxVolume;
            }

            if (hornSFX != null)
            {
                hornSFX.volume = sfxVolume;
            }

            if (wheelsSkiddingSFX != null)
            {
                wheelsSkiddingSFX.volume = sfxVolume;
            }

            if (backUpBeeperSFX != null)
            {
                backUpBeeperSFX.volume = sfxVolume;
            }

            if (backUpBeeperSFX != null)
            {
                backUpBeeperSFX.volume = sfxVolume;
            }

            if (openDoorSFX != null)
            {
                openDoorSFX.volume = sfxVolume;
            }

            if (closeDoorSFX != null)
            {
                closeDoorSFX.volume = sfxVolume;
            }

        }


        public void ResetPosition()
        {
            if (this.GearPosition == VehicleGearPosition.Reverse)
            {
                this.transform.localPosition = OriginReverseLocalPosition;
                this.transform.localRotation = OriginReverseLocalRotation;
            }
            else
            {
                this.transform.localPosition = OriginLocalPosition;
                this.transform.localRotation = OriginLocalRotation;
            }
        }

        /// <summary>
        /// Handles wheels steering
        /// </summary>
        private void Steering()
        {
            //bool isMoving = (Mathf.RoundToInt(_currentSpeed) > 0);
            bool isMoving = _currentSpeed > 0; // Updated by Hank 2020-11-30
            bool articulated = false; //(steeringMode == WSMVehicleSteeringMode.ArticulatedSteering);

            if (!articulated || (articulated && (isMoving || _nonMovingArticulatedSteering)))
            {
                float steeringSpeed = articulated ? _articulatedSteeringSpeed * 1f - (_currentSpeed / _maxSpeed) : _defaultSteeringSpeed;
                float wheelsSteerAngle = Mathf.LerpAngle(_highSpeedSteeringAngle, _maxSteeringAngle, 1f - (_currentSpeed / _maxSpeed));

                if (_softSteering)
                {
                    _currentSteerAngle = Mathf.MoveTowards(_currentSteerAngle, _steering * wheelsSteerAngle, steeringSpeed * Time.deltaTime);
                }
                else
                {
                    _currentSteerAngle = _steering * wheelsSteerAngle;
                }

                // Steering Wheel
                if (steeringWheel != null)
                {
                    //steeringWheel.localEulerAngles = new Vector3(0f, _currentSteerAngle * _steeringWheelAngleMultiplier, 0f); // Hank Updated 2020-11-29
                    if (_steering >= 0)
                    {
                        steeringWheel.localEulerAngles = new Vector3(0f, -1f * _currentSteerAngle * _steeringWheelAngleMultiplierClockwise, 0f);
                    }
                    else
                    {
                        steeringWheel.localEulerAngles = new Vector3(0f, -1f * _currentSteerAngle * _steeringWheelAngleMultiplierCounterClockwise, 0f);
                    }
                }


                // Wheels
                if (_steeringWheelsColliders != null)
                {
                    for (int i = 0; i < _steeringWheelsCollidersCount; i++)
                        _steeringWheelsColliders[i].steerAngle = _currentSteerAngle;
                }
            }

            // Articulated vehicles - EXPERIMENTAL
            //if (articulated && (isMoving || _nonMovingArticulatedSteering))
            //{
            //    if (articulatedVehiclePivot != null)
            //        articulatedVehiclePivot.localEulerAngles = new Vector3(0f, _currentSteerAngle, 0f);
            //}
        }

        /// <summary>
        /// Handles wheels steering
        /// </summary>
        private void VRSteering()
        {
            //bool isMoving = (Mathf.RoundToInt(_currentSpeed) > 0);
            bool isMoving = _currentSpeed > 0; // Updated by Hank 2020-11-30
            bool articulated = false; //(steeringMode == WSMVehicleSteeringMode.ArticulatedSteering);

            if (!articulated || (articulated && (isMoving || _nonMovingArticulatedSteering)))
            {
                if (_softSteering)
                {
                    _currentSteerAngle = Mathf.MoveTowards(_currentSteerAngle, _steering * _maxSteeringAngle, _defaultSteeringSpeed * Time.deltaTime);
                }
                else
                {
                    _currentSteerAngle = _steering * _maxSteeringAngle;
                }

                // Steering Wheel
                if (steeringWheel != null)
                {
                    //steeringWheel.localEulerAngles = new Vector3(0f, _currentSteerAngle * _steeringWheelAngleMultiplier, 0f); // Hank Updated 2020-11-29
                    if (_steering >= 0)
                    {
                        steeringWheel.localEulerAngles = new Vector3(0f, -1f * _currentSteerAngle * _steeringWheelAngleMultiplierClockwise, 0f);
                    }
                    else
                    {
                        steeringWheel.localEulerAngles = new Vector3(0f, -1f * _currentSteerAngle * _steeringWheelAngleMultiplierCounterClockwise, 0f);
                    }
                }


                // Wheels
                if (_steeringWheelsColliders != null)
                {
                    for (int i = 0; i < _steeringWheelsCollidersCount; i++)
                        _steeringWheelsColliders[i].steerAngle = _currentSteerAngle;
                }
            }
        }

        /// <summary>
        /// Apply steering helper
        /// </summary>
        /// 

        private float brakeDuration = 0f;
        private void ApplySteerHelper()
        {
            WheelHit wheelhit;
            foreach (WheelCollider wheelColliders in _torqueWheelsColliders)
            {
                wheelColliders.GetGroundHit(out wheelhit);
                if (wheelhit.normal == Vector3.zero)
                    return; // wheels arent on the ground so dont realign the rigidbody velocity
            }

            // this if is needed to avoid gimbal lock problems that will make the car suddenly shift direction
            if (Mathf.Abs(_steerHelperLastRotation - transform.eulerAngles.y) < 10f)
            {
                var turnadjust = (transform.eulerAngles.y - _steerHelperLastRotation) * _steerHelper;
                Quaternion velRotation = Quaternion.AngleAxis(turnadjust, Vector3.up);
                _rigidbody.velocity = velRotation * _rigidbody.velocity;

           }
            _steerHelperLastRotation = transform.eulerAngles.y;
        }

        /// <summary>
        /// Gears transmission
        /// </summary>
        private void Gearbox()
        {
            if (_transmissionType == VehicleTransmissionType.Automatic)
            {
                Vector3 localVelocity = transform.InverseTransformDirection(_rigidbody.velocity);
                //_movingBackwards = localVelocity.z < 0 && Mathf.RoundToInt(_currentSpeed) > 0;
                _movingBackwards = localVelocity.z < 0 && (_currentSpeed > 0.1f);    // Updated by Hank 2020-11-30

                if (_movingBackwards)
                    _currentGear = -1;
                else
                {
                    _currentGear = (_currentGear <= 0) ? 1 : _currentGear;

                    if (_currentGear < _numberOfGears && _currentSpeed >= _gearsSpeedLimits[_currentGear - 1])
                        _currentGear++;
                    else if (_currentGear > 1 && _currentSpeed < _gearsSpeedLimits[_currentGear - 2])
                        _currentGear--;
                }
            }
        }


        // 回傳移動狀態
        public VehicleTranslation TranslationStatus
        {
            get
            {
                Vector3 localVelocity = transform.InverseTransformDirection(_rigidbody.velocity);
                if (localVelocity.z < -0.05f)
                {
                    //if (_currentSpeed > 0.1f) // 倒車
                    //{
                    return VehicleTranslation.Reverse;
                    //}
                    //else // 慢慢倒車
                    //{
                    //    return VehicleTranslation.SlowReverse;
                    //}
                }
                else if (localVelocity.z > 0.05f)
                {
                    //if (_currentSpeed > 0.1f)   // 前進
                    //{
                    return VehicleTranslation.Forward;
                    //}
                    //else  // 慢慢前進
                    //{
                    //    return VehicleTranslation.SlowForward;
                    //}
                }

                return VehicleTranslation.Idle;
            }
        }

        /// <summary>
        /// Apply torque to all wheels connected to the drivetrain and controls max speed
        /// </summary>

        private void Engine()
        {
            float gearSpeedLimit = _movingBackwards ? _maxReverseSpeed : _gearsSpeedLimits[_currentGear - 1];
            SpeedControl(gearSpeedLimit);
            if (_torqueWheelsColliders != null)
            {
                /*
                _individualWheelTorque = _torqueWheelsCount > 0 ? (_currentTorque / _torqueWheelsCount) : _individualWheelTorque;

                _thrustTorque = _acceleration >= 0f ? (_acceleration * _individualWheelTorque) : (_acceleration * _individualWheelReverseTorque);
                _thrustTorque = _thrustTorque * (1f - _clutch);

                float computedTorque = _thrustTorque;
                float mult = 1f;
                if (Mathf.Abs(_acceleration) > 0.92f)
                {
                    prevHighAccelerationDuration += Time.fixedDeltaTime;
                }
                else
                {
                    prevHighAccelerationDuration = 0f;
                }

                if (prevHighAccelerationDuration > 1f)
                {
                    int p = Mathf.FloorToInt(prevHighAccelerationDuration);
                    mult = Mathf.Pow(_torqueMultiplication, p);

                    computedTorque = (computedTorque > _torqueLimitation) ? _torqueLimitation : _thrustTorque * mult;
                } 
                else
                {
                    computedTorque = _thrustTorque;
                }
                */

                float computedTorque = MapTorque();
                for (int i = 0; i < _torqueWheelsCount; i++)
                {
                    _torqueWheelsColliders[i].motorTorque = computedTorque;
                }
            }
        }

        private void SpeedControl(float speedLimit)
        {
            _currentSpeed = _rigidbody.velocity.magnitude * _speedUnitConversion;
            if (_gearPosition == VehicleGearPosition.Neutral)
            {
                noGasAccelerationSignedInt = 0;

                return;
            }

            if (Mathf.Abs(_currentSpeed) > speedLimit && _accelerationInt > noGasAccelerationDefaultInt)
            {
                noGasAccelerationSignedInt = -noGasAccelerationDefaultInt;
            }
            else if (Mathf.Abs(_currentSpeed) < _noGasMaxSpeed && _accelerationInt < noGasAccelerationDefaultInt)
            {
                noGasAccelerationSignedInt = noGasAccelerationDefaultInt;
            }
            else
            {
                noGasAccelerationSignedInt = 0;
            }
        }


        private float RoundToF1(float val)
        {
            int i = Mathf.RoundToInt(val * 10);
            if (i <= 1)
            {
                return 0f;
            }
            else
            {
                return i / 10f;
            }
        }
        /// <summary>
        /// Applies brake and hanbrake
        /// </summary>
        private void Brakes()
        {
            if (_handbrake > 0f) // Handbrake has priority
            {
                float handBrakeTorque = _handbrake * _handbrakeTorque;
                foreach (var wheel in _allWheelsColliders)
                    wheel.brakeTorque = handBrakeTorque;
            }
            else
            {
                // begin: Added by Hank 2020-11-30 當前進時，打到R檔；當後退時，打到D檔 ==> 緊急剎車
                if ((this.TranslationStatus == VehicleTranslation.Forward && this.GearPosition == VehicleGearPosition.Reverse) ||
                    (this.TranslationStatus == VehicleTranslation.Reverse && this.GearPosition == VehicleGearPosition.Drive))
                {
                    //_brakeOffset = 1f; // 緊急剎車
                    _brakeInt = 9;
                }

                //float brakeTorque = _brakeOffset * _brakesTorque;
                float brakeTorque = MapBrake();
                foreach (var wheel in _allWheelsColliders)
                {
                    wheel.brakeTorque = brakeTorque;
                }
            }
        }

        /// <summary>
        /// 根據速度增加抓地力(DownForce)
        /// </summary>
        private void ApplyDownForce()
        {
            _rigidbody.AddForce(-transform.up * _downforce * _rigidbody.velocity.magnitude);
        }

        /// <summary>
        /// Traction control to avoid wheels spinning too much
        /// </summary>
        private void ApplyTractionControl()
        {
            WheelHit wheelHit;

            foreach (WheelCollider wheelCollider in _torqueWheelsColliders)
            {
                wheelCollider.GetGroundHit(out wheelHit);

                if (wheelHit.forwardSlip >= _tractionSlipLimit && _currentTorque >= 0)
                {
                    _currentTorque -= 10 * _tractionControl;
                }
                else
                {

                    _currentTorque += 10 * _tractionControl;
                    if (_currentTorque > _maxTorque)
                        _currentTorque = _maxTorque;
                }
            }
        }

        private void UpdateWheelMeshesRotation(WheelCollider[] wheelColliders, GameObject[] wheelMeshes)
        {
            for (int i = 0; i < wheelMeshes.Length; i++)
            {
                if (i < wheelColliders.Length)
                {
                    wheelColliders[i].GetWorldPose(out _wheelPosition, out _wheelRotation);
                    wheelMeshes[i].transform.position = _wheelPosition;
                    wheelMeshes[i].transform.rotation = _wheelRotation;
                }
                else break;
            }
        }

        private void UpdatePedals()
        {
            _currentGasPedalAngle = Mathf.MoveTowards(_currentGasPedalAngle, Mathf.Abs(_accelerationGradient) * 25f, 70f * Time.deltaTime);
            _currentBrakesPedalAngle = Mathf.MoveTowards(_currentBrakesPedalAngle, _brakeForPedal * 15f, 200f * Time.deltaTime);
            //_currentClutchPedalAngle = Mathf.MoveTowards(_currentClutchPedalAngle, _clutch * 15f, 200f * Time.deltaTime);

            if (gasPedal != null) gasPedal.localEulerAngles = new Vector3(_currentGasPedalAngle, 0f, 0f);
            if (brakesPedal != null) brakesPedal.localEulerAngles = new Vector3(_currentBrakesPedalAngle * -1, 0f, 0f);
            //if (clutchPedal != null) clutchPedal.localEulerAngles = new Vector3(_currentClutchPedalAngle, 0f, 0f);
        }

        private void UpdateGearStick()
        {
            float angle = 0f;
            if (_gearPosition == VehicleGearPosition.Drive)
            {
                angle = 15f;
            }
            else if (_gearPosition == VehicleGearPosition.Reverse)
            {
                angle = -15f;
            }
            else if (_gearPosition == VehicleGearPosition.Neutral)
            {
                angle = 0f;
            }

            _gearStickForwardAngle = Mathf.MoveTowards(_gearStickForwardAngle, angle, 100f * Time.deltaTime);

            if (gearStick != null) gearStick.localEulerAngles = new Vector3(0f, _gearStickForwardAngle, 0f);
        }


        private void UpdateHandbrakeStick()
        {
            const float full = 60f;
            // _handbrake = 0 ====> Angle = 60
            // _handbrake = 1 ====> Angle = 0
            float angle = full - _handbrakeGradient * full;
            _handbrakeStickAngle = Mathf.MoveTowards(_handbrakeStickAngle, angle, 100f * Time.deltaTime);

            if (handbrakeStick != null) handbrakeStick.localEulerAngles = new Vector3(_handbrakeStickAngle, 0f, 0f);
        }

        private void UpdateUI()
        {
            speedText.text = this.SpeedString;
            gearText.text = this.GearString;

            if (wheelAngleText != null && this.forShowRotationAngleWheelsMeshes != null)
            {
                // 輪胎旋轉角度的計算: 觀察發現將y值減去z值得到的數值，若為正值或小於100則為左轉，若數值大於280則為右轉。
                // 
                float y = this.forShowRotationAngleWheelsMeshes.transform.localEulerAngles.y - this.forShowRotationAngleWheelsMeshes.transform.localEulerAngles.z;

                if (y >= 280f)
                {
                    y = -(360 - y);
                }
                _steeringWheelAngle = y;
                wheelAngleText.text = this.SteeringWheelAngleString;
            }
        }

        private float EulerToRotation(float value)
        {
            if (value > 180)
            {
                return value - 360f;
            }
            else if (value < -180)
            {
                return value + 360f;
            }
            else
            {
                return value;
            }
        }


        public static float Clamp0360(float eulerAngles)
        {
            float result = eulerAngles - Mathf.CeilToInt(eulerAngles / 360f) * 360f;
            if (result < 0)
            {
                result += 360f;
            }
            return result;
        }

        public string SteeringWheelAngleString
        {
            get
            {
                return _steeringWheelAngle.ToString("F0");
            }

        }

        public string SpeedString
        {
            get
            {
                //return string.Format("{0} {1}", _currentSpeed.ToString("F0"), speedUnit.ToString());
                //return string.Format("{0}", _currentSpeed.ToString("F0"));
                return string.Format("{0}", Mathf.RoundToInt(_currentSpeed));
            }
        }

        public string GearString
        {
            get
            {
                string val = "";
                switch (_gearPosition)
                {
                    case VehicleGearPosition.Park:
                        val = "P";
                        break;
                    case VehicleGearPosition.Neutral:
                        val = "N";
                        break;
                    case VehicleGearPosition.Drive:
                        val = "D";
                        break;
                    case VehicleGearPosition.Reverse:
                        val = "R";
                        break;
                }

                return val;
            }
        }

        private void VehicleSFX()
        {
            if (_isEngineOn && playSFX)
            {
                // Engine
                if (engineSFX != null && engineSFX.isPlaying)
                {
                    _currentGear = (_currentGear <= 0) ? 1 : _currentGear;

                    float speedGearRatio = _currentSpeed / _gearsSpeedLimits[_currentGear - 1];
                    float newEnginePitch = Mathf.Lerp(_minEnginePitch, _maxEnginePitch, speedGearRatio);
                    engineSFX.pitch = newEnginePitch;
                }

                // Truck and machinery reverse warning
                if (backUpBeeperSFX != null)
                {
                    if (_movingBackwards && !backUpBeeperSFX.isPlaying)
                        backUpBeeperSFX.Play();
                    else if (!_movingBackwards && backUpBeeperSFX.isPlaying)
                        backUpBeeperSFX.Stop();
                }

                if (signalLightsSFX != null)
                {
                    if ((_leftSinalLightsOn || _rightSinalLightsOn) && !signalLightsSFX.isPlaying)
                        signalLightsSFX.Play();
                    else if (!_leftSinalLightsOn && !_rightSinalLightsOn && signalLightsSFX.isPlaying)
                        signalLightsSFX.Stop();
                }
            }

            // Wheels skidding
            if (wheelsSkiddingSFX != null)
            {
                bool isAnyWheelSlipping = false;
                WheelHit wheelHit;
                foreach (WheelCollider wheelCollider in _allWheelsColliders)
                {
                    if (wheelCollider.GetGroundHit(out wheelHit))
                    {
                        if (Mathf.Abs(wheelHit.forwardSlip) >= _sfxForwardSlipLimit || Mathf.Abs(wheelHit.sidewaysSlip) >= _sfxSidewaysSlipLimit)
                        {
                            isAnyWheelSlipping = true;
                            break;
                        }
                    }
                }

                if (isAnyWheelSlipping)
                {
                    if (!wheelsSkiddingSFX.isPlaying)
                        wheelsSkiddingSFX.Play();
                }
                else
                {
                    if (wheelsSkiddingSFX.isPlaying)
                        wheelsSkiddingSFX.Stop();
                }
            }
        }

        /// <summary>
        /// Handles vehicle lights
        /// </summary>
        private void VehicleLights()
        {
            if (_isEngineOn)
            {
                if (reverseAlarmLights != null)
                {
                    foreach (var alarmLights in reverseAlarmLights)
                        alarmLights.enabled = _movingBackwards;

                    if (_movingBackwards && _rotateAlarmLights && alarmLightsPivot != null)
                        alarmLightsPivot.Rotate(alarmLightsPivot.up, _alarmLightsRotationSpeed, Space.Self);
                }

                // Brake lights
                if (brakeLights != null)
                {
                    foreach (var brakeLight in brakeLights)
                    {
                        //brakeLight.enabled = (_brakeOffset > 0f);
                        brakeLight.enabled = (_brakeInt > 0);
                    }
                }
            }
        }

        private void LeftSignalLights()
        {
            ToggleLights(signalLightsLeft);
        }

        private void RightSignalLights()
        {
            ToggleLights(signalLightsRight);
        }

        /// <summary>
        /// Toggle lights on/off depending on their current status
        /// </summary>
        /// <param name="lights"></param>
        private void ToggleLights(Light[] lights)
        {
            if (lights != null)
            {
                foreach (var light in lights)
                    light.enabled = !light.enabled;
            }
        }

        /// <summary>
        /// Toggle lights to the desired state
        /// </summary>
        /// <param name="lights"></param>
        /// <param name="onOff"></param>
        private void ToggleLights(Light[] lights, bool onOff)
        {
            if (lights != null)
            {
                foreach (var light in lights)
                    light.enabled = onOff;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="door"></param>
        private void ToogleDoor(VehicleDoor door)
        {
            if (door != null)
                door.IsOpen = !door.IsOpen;
        }

        #endregion

        #region PUBLIC VEHICLE METHODS

        /// <summary>
        /// Starts vehicle engine
        /// </summary>
        public void StartEngine()
        {
            _isEngineOn = true;

            if (Application.isPlaying)
            {
                if (engineStartSFX != null && !engineStartSFX.isPlaying)
                    engineStartSFX.Play();
                if (engineSFX != null && !engineSFX.isPlaying)
                    engineSFX.Play();
            }
        }

        /// <summary>
        /// Stop vehicle engine
        /// </summary>
        public void StopEngine()
        {
            _isEngineOn = false;

            if (Application.isPlaying)
            {
                if (engineSFX != null && engineSFX.isPlaying)
                    engineSFX.Stop();

                if (backUpBeeperSFX != null && backUpBeeperSFX.isPlaying)
                    backUpBeeperSFX.Stop();

                if (signalLightsSFX != null && signalLightsSFX.isPlaying)
                    signalLightsSFX.Stop();
            }
        }

        /// <summary>
        /// Vehicle horns
        /// </summary>
        public void Horn()
        {
            if (hornSFX != null)
                hornSFX.Play();
        }

        public void ToogleDriverDoor()
        {
            ToogleDoor(driverDoor);
        }

        public void TooglePassengerDoor(int index)
        {
            if (passengersDoors != null && passengersDoors.Length > index)
                ToogleDoor(passengersDoors[index]);
        }

        #endregion
    }
}
