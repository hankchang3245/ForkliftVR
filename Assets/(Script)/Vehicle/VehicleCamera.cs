using System;
using UnityEngine;

namespace edu.tnu.dgd.vehicle
{
    [RequireComponent(typeof(Camera))]
    public class VehicleCamera : MonoBehaviour
    {
        //Local only
        private Camera _camera;
        private Transform _transform;
        private VehicleController _targetVehicleController;
        private int _selectedCameraIndex;
        private int _cameraTypesLength;

        [SerializeField] private Transform _target;
        [SerializeField] private bool _cameraToggleEnabled = true;
        [SerializeField] private VehicleCameraType _cameraType;
        [SerializeField] private float _tpsHeight = 2f;
        [SerializeField] private float _tpsDistance = 5f;
        [SerializeField] private float _tpsRotationSpeed = 5f;
        [SerializeField] private float _fpsRotationSpeed = 5f;

        [SerializeField] private Transform _fpsCameraPositionForward;
        [SerializeField] private Transform _fpsCameraPositionCenter;
        [SerializeField] private Transform _fpsCameraPositionBackward;

        [SerializeField] private Transform _fpsCameraPositionLeftTop;
        [SerializeField] private Transform _fpsCameraPositionLeftBottom;
        [SerializeField] private Transform _fpsCameraPositionLeftBackward;

        [SerializeField] private Transform _fpsCameraPositionRightTop;
        [SerializeField] private Transform _fpsCameraPositionRightBottom;
        [SerializeField] private Transform _fpsCameraPositionRightBackward;

        [SerializeField] private Transform _fpsCameraPositionUp;
        [SerializeField] private Transform _fpsCameraPositionDown;

        [SerializeField] private float _fpsHorizontalAngleLimit = 90f;
        [SerializeField] private float _fpsVerticalAngleLimit = 30f;
        [SerializeField] private Vector3 _topPositionOffset = new Vector3(-10, 20, -10);
        [SerializeField] private bool _topDownOrthographicCam = true;

        public Transform Target { get { return _target; } set { _target = value; ValidateTarget(); } }
        public VehicleCameraType CameraType { get { return _cameraType; } set { _cameraType = value; } }
        public bool CameraToggleEnabled { get { return _cameraToggleEnabled; } set { _cameraToggleEnabled = value; } }
        public float TpsHeight { get { return _tpsHeight; } set { _tpsHeight = Mathf.Abs(value); } }
        public float TpsDistance { get { return _tpsDistance; } set { _tpsDistance = Mathf.Abs(value); } }
        public float TpsRotationSpeed { get { return _tpsRotationSpeed; } set { _tpsRotationSpeed = Mathf.Abs(value); } }
        public float FpsRotationSpeed { get { return _tpsRotationSpeed; } set { _tpsRotationSpeed = Mathf.Abs(value); } }
        public Transform FpsCameraPositionForward { get { return _fpsCameraPositionForward; } set { _fpsCameraPositionForward = value; } }
        public Transform FpsCameraPositionCenter { get { return _fpsCameraPositionCenter; } set { _fpsCameraPositionCenter = value; } }
        public Transform FpsCameraPositionBackward { get { return _fpsCameraPositionBackward; } set { _fpsCameraPositionBackward = value; } }
        public Transform FpsCameraPositionLeftTop { get { return _fpsCameraPositionLeftTop; } set { _fpsCameraPositionLeftTop = value; } }
        public Transform FpsCameraPositionLeftBottom { get { return _fpsCameraPositionLeftBottom; } set { _fpsCameraPositionLeftBottom = value; } }
        public Transform FpsCameraPositionLeftBackward { get { return _fpsCameraPositionLeftBackward; } set { _fpsCameraPositionLeftBackward = value; } }
        public Transform FpsCameraPositionRightTop { get { return _fpsCameraPositionRightTop; } set { _fpsCameraPositionRightTop = value; } }
        public Transform FpsCameraPositionRightBottom { get { return _fpsCameraPositionRightBottom; } set { _fpsCameraPositionRightBottom = value; } }
        public Transform FpsCameraPositionRightBackward { get { return _fpsCameraPositionRightBackward; } set { _fpsCameraPositionRightBackward = value; } }

        public Transform FpsCameraPositionUp { get { return _fpsCameraPositionUp; } set { _fpsCameraPositionUp = value; } }
        
        public Transform FpsCameraPositionDown { get { return _fpsCameraPositionDown; } set { _fpsCameraPositionDown = value; } }
        public float FpsHorizontalAngleLimit { get { return _fpsHorizontalAngleLimit; } set { _fpsHorizontalAngleLimit = Mathf.Abs(value); } }
        public float FpsVerticalAngleLimit { get { return _fpsVerticalAngleLimit; } set { _fpsVerticalAngleLimit = Mathf.Abs(value); } }
        public Vector3 TopPositionOffset { get { return _topPositionOffset; } set { _topPositionOffset = value; } }
        public bool TopDownOrthographicCam { get { return _topDownOrthographicCam; } set { _topDownOrthographicCam = value; } }

        // Use this for initialization
        void Start()
        {
            _camera = GetComponent<Camera>();
            _transform = GetComponent<Transform>();
            _selectedCameraIndex = (int)_cameraType;
            _cameraTypesLength = Enum.GetNames(typeof(VehicleCameraType)).Length;
            ValidateTarget();
            MoveToStartPosition();
        }

        /// <summary>
        /// FixedUpdate used instead of LateUpdate to avoid camera sttutering
        /// </summary>
        /// 
        /*
        void FixedUpdate()
        {
            if (_target != null)
            {
                if (!ValidateTarget())
                    return;

                switch (_cameraType)
                {
                    case VehicleCameraType.TPS:
                        TPSCameraUpdate();
                        break;
                    case VehicleCameraType.FPS:
                        FPSCameraUpdate();
                        break;
                    case VehicleCameraType.TopDown:
                        TopDownCameraUpdate();
                        break;
                }

                if (_targetVehicleController.CameraToggleRequested)
                    ToggleCameraType();
            }

        }
        */

        /// <summary>
        /// Validate if target is a vehicle
        /// </summary>
        /// <returns>True if vehicle</returns>
        private bool ValidateTarget()
        {
            if (_targetVehicleController == null)
            {
                _targetVehicleController = _target.gameObject.GetComponent<VehicleController>();

                if (_targetVehicleController == null)
                {
                    Debug.LogWarning("VehicleCamera - Target is not a vehicle");
                    _target = null;
                    return false;
                }
                else
                {
                    _targetVehicleController.cameraDirectionChanged.AddListener(MoveToSpecifiedPosition);
                }
            }

            return true;
        }

        /// <summary>
        /// Handles TPS Camera
        /// </summary>
        /// 
        /*
        private void TPSCameraUpdate()
        {
            float currentAngleY = _transform.eulerAngles.y;
            float currentHeight = _transform.position.y;

            float newAngleY = _target.eulerAngles.y;
            float newHeight = _target.position.y + _tpsHeight;

            switch (_targetVehicleController.CamLookDirection)
            {
                case VehicleCameraLookDirection.Backward: newAngleY += 180; break;
                case VehicleCameraLookDirection.Right: newAngleY += 90; break;
                case VehicleCameraLookDirection.Left: newAngleY -= 90; break;
            }

            currentAngleY = Mathf.LerpAngle(currentAngleY, newAngleY, _tpsRotationSpeed * Time.deltaTime);
            currentHeight = Mathf.Lerp(currentHeight, newHeight, _tpsRotationSpeed * Time.deltaTime);

            Quaternion newRotation = Quaternion.Euler(0, currentAngleY, 0);

            _transform.position = _target.position; // Reset position
            _transform.position -= newRotation * Vector3.forward * _tpsDistance; // Apply distance
            _transform.position = new Vector3(_transform.position.x, currentHeight, _transform.position.z); // Apply Rotation

            _transform.LookAt(new Vector3(_target.position.x, _target.position.y + _tpsHeight, _target.position.z)); // Look at vehicle
        }
        */

        /// <summary>
        /// Handles FPS Camera
        /// </summary>
        private void FPSCameraUpdate()
        {
            Transform newTr = MapToDirectionTransform(_targetVehicleController.CamLookDirection);
            _transform.localPosition = newTr.localPosition;
            _transform.localRotation = newTr.localRotation;
        }


        private Transform MapToDirectionTransform(VehicleCameraLookDirection dir)
        {
            switch (_targetVehicleController.CamLookDirection)
            {
                case VehicleCameraLookDirection.Forward:
                    return _fpsCameraPositionForward;
                case VehicleCameraLookDirection.Center:
                    return _fpsCameraPositionCenter;
                case VehicleCameraLookDirection.Backward:
                    return _fpsCameraPositionBackward;

                case VehicleCameraLookDirection.LeftTop:
                    return _fpsCameraPositionLeftTop;
                case VehicleCameraLookDirection.LeftBottom:
                    return _fpsCameraPositionLeftBottom;
                case VehicleCameraLookDirection.LeftBackward:
                    return _fpsCameraPositionLeftBackward;

                case VehicleCameraLookDirection.RightTop:
                    return _fpsCameraPositionRightTop;
                case VehicleCameraLookDirection.RightBottom:
                    return _fpsCameraPositionRightBottom;
                case VehicleCameraLookDirection.RightBackward:
                    return _fpsCameraPositionRightBackward;

                case VehicleCameraLookDirection.Up:
                    return _fpsCameraPositionUp;

                case VehicleCameraLookDirection.Down:
                    return _fpsCameraPositionDown;
                
                default:
                    return _fpsCameraPositionCenter;
            }
        }

        private void TopDownCameraUpdate()
        {
            _transform.position = _target.position + _topPositionOffset;
            _transform.LookAt(_target);
        }

        public void MoveToStartPosition()
        {
            MoveToSpecifiedPosition();
        }

        public void MoveToSpecifiedPosition()
        {
            if (_target == null)
            {
                Debug.LogWarning("VehicleCamera - Target cannot be null");
                return;
            }
            Transform newTransform = MapToDirectionTransform(_targetVehicleController.CamLookDirection);

            if (_camera == null)
                _camera = GetComponent<Camera>();

            if (_transform == null)
                _transform = GetComponent<Transform>();

            switch (_cameraType)
            {
                case VehicleCameraType.TPS:
                    ClearTargetParenting();
                    _transform.position = _target.position + (_target.forward * -_tpsDistance);
                    _transform.position = new Vector3(_transform.position.x, _tpsHeight, _transform.position.z);
                    _transform.rotation = _target.rotation;
                    _camera.orthographic = false;
                    break;
                case VehicleCameraType.FPS:
                    EnforceTargetParenting();
                    _transform.localPosition = newTransform.localPosition;
                    _transform.localRotation = newTransform.localRotation;
                    _camera.orthographic = false;
                    break;
                case VehicleCameraType.TopDown:
                    ClearTargetParenting();
                    _transform.position = _target.position + _topPositionOffset;
                    _transform.LookAt(_target);
                    _camera.orthographic = _topDownOrthographicCam;
                    break;
            }
        }

        /// <summary>
        /// Toggle camera
        /// </summary>
        public void ToggleCameraType()
        {
            if (_cameraToggleEnabled)
            {
                _selectedCameraIndex++;
                _selectedCameraIndex = _selectedCameraIndex >= _cameraTypesLength ? 0 : _selectedCameraIndex;
                _cameraType = (VehicleCameraType)_selectedCameraIndex;
                MoveToStartPosition();
            }
        }

        /// <summary>
        /// Make sure the camera is parented to the target
        /// </summary>
        private void EnforceTargetParenting()
        {
            if (_transform.parent != _target)
                _transform.SetParent(_target);
        }

        /// <summary>
        /// Make sure the camera is NOT parented to the target
        /// </summary>
        private void ClearTargetParenting()
        {
            if (_transform.parent == _target)
                _transform.SetParent(null);
        }
    }
}
