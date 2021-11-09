using UnityEngine;
using UnityEngine.Events;
using edu.tnu.dgd.game;

namespace edu.tnu.dgd.forklift
{
    [System.Serializable]
    public class ForkliftController : MonoBehaviour
    {
        private static ForkliftController _instance;
        public static ForkliftController instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<ForkliftController>();
                }

                return _instance;
            }
        }

        public float forksVerticalSpeed = 0.3f;
        public float forksHorizontalSpeed = 0.5f;
        public float mastTiltSpeed = 0.3f;

        [SerializeField] private bool _isEngineOn = true;

        [SerializeField] public RotatingMechanicalPart mainMast;
        [SerializeField] public MovingMechanicalPart secondaryMast;
        [SerializeField] public MovingMechanicalPart forksCylinders;
        [SerializeField] public MovingMechanicalPart forks;
        [SerializeField] public Transform forksVerticalLever;
        [SerializeField] public Transform forksHorizontalLever;
        [SerializeField] public Transform mastTiltLever;
        [SerializeField] public AudioSource forkMovingSFX;
        [SerializeField] public AudioSource forkStartMovingSFX;
        [SerializeField] public AudioSource forkStopMovingSFX;
        [SerializeField] public bool playSFX = true;
        [SerializeField] public float sfxVolume = 0.5f;

        private float _verticalLeverAngle = 0;
        private float _horizontalLeverAngle = 0;
        private float _tiltLeverAngle = 0;

        [Range(0f, 1f)] private float _forksVertical;
        [Range(0f, 1f)] private float _secondaryMastVertical;
        [Range(0f, 1f)] private float _forksHorizontal;
        [Range(0f, 1f)] private float _mastTilt;

        public float ForksVertical { get { return _forksVertical; } set { _forksVertical = value; } }
        public float ForksHorizontal { get { return _forksHorizontal; } set { _forksHorizontal = value; } }
        public float MastTilt { get { return _mastTilt; } set { _mastTilt = value; } }


        private ForkHeight _forkHeight = ForkHeight.Low;

        public UnityEvent OnForkHeightChanged;


        /*
         * public YAxisStopValue[] autoForkVerticalStopList;
         */

        public bool IsEngineOn
        {
            get { return _isEngineOn; }
            set
            {
                if (!_isEngineOn && value)
                    StartEngine();
                else if (_isEngineOn && !value)
                    StopEngine();
            }
        }

        private void Awake()
        {

            this.playSFX = PlayerPrefs.GetInt(StringConstants.Setting_ForkliftSFX, 0) > 0;
            this.sfxVolume = PlayerPrefs.GetInt(StringConstants.Setting_ForkliftSFXVolume, 3) / 10f;

        }
        /// <summary>
        /// Initialize forklift
        /// </summary>
        private void Start()
        {
            _mastTilt = mainMast.MovementInput;
            _forksHorizontal = forksCylinders.MovementInput;
            _forksVertical = forks.MovementInput;
            _secondaryMastVertical = secondaryMast.MovementInput;

            forksVerticalSpeed = Mathf.Abs(forksVerticalSpeed);
            forksHorizontalSpeed = Mathf.Abs(forksHorizontalSpeed);
            mastTiltSpeed = Mathf.Abs(mastTiltSpeed);

            if (forkMovingSFX != null)
            {
                forkMovingSFX.volume = sfxVolume;
            }

            if (forkStartMovingSFX != null)
            {
                forkStartMovingSFX.volume = sfxVolume;
            }

            if (forkStopMovingSFX != null)
            {
                forkStopMovingSFX.volume = sfxVolume;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void LateUpdate()
        {
            if (_isEngineOn)
            {
                bool forksMoving = forks.IsMoving || secondaryMast.IsMoving || mainMast.IsMoving || forksCylinders.IsMoving;
                ForksMovementSFX(forksMoving); //Should be called on late update to track SFX correctly

                float height = forks.GetCurrentForkHeight();
                if (height < 15f )
                {
                    if (_forkHeight != ForkHeight.Low)
                    {
                        _forkHeight = ForkHeight.Low;
                        if (OnForkHeightChanged != null)
                        {
                            OnForkHeightChanged.Invoke();
                        }
                    }
                } 
                else if (height >= 15f && height < 40f )
                {
                    if (_forkHeight != ForkHeight.Movemovingable)
                    {
                        _forkHeight = ForkHeight.Movemovingable;
                        if (OnForkHeightChanged != null)
                        {
                            OnForkHeightChanged.Invoke();
                        }
                    }
                }
                else if (height >= 40f)
                {
                    if (_forkHeight != ForkHeight.High)
                    {
                        _forkHeight = ForkHeight.High;
                        if (OnForkHeightChanged != null)
                        {
                            OnForkHeightChanged.Invoke();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Starts vehicle engine
        /// </summary>
        public void StartEngine()
        {
            _isEngineOn = true;
        }

        /// <summary>
        /// Stop vehicle engine
        /// </summary>
        public void StopEngine()
        {
            _isEngineOn = false;
        }

        /// <summary>
        /// Handles forks vertical movement
        /// </summary>
        /// <param name="verticalInput">-1 = down | 0 = none | 1 = up</param>
        public void MoveForksVertically(float verticalInput)
        {
            if (_isEngineOn)
            {
                _forksVertical += _secondaryMastVertical <= 0 ? (verticalInput * Time.deltaTime * forksVerticalSpeed) : 1f;
                _forksVertical = Mathf.Clamp01(_forksVertical);

                _secondaryMastVertical += _forksVertical >= 1 ? (verticalInput * Time.deltaTime * forksVerticalSpeed) : 0f;
                _secondaryMastVertical = Mathf.Clamp01(_secondaryMastVertical);

                forks.MovementInput = _forksVertical;
                secondaryMast.MovementInput = _secondaryMastVertical;

                //Debug.LogFormat("secondaryMast.y={0} forks.y={1}", secondaryMast.transform.localPosition.y, forks.transform.localPosition.y);
            }
        }

        /// <summary>
        /// Handles forks horizontal movement
        /// </summary>
        /// <param name="horizontalInput">-1 = left | 0 = none | 1 = right</param>
        public void MoveForksHorizontally(int horizontalInput)
        {
            if (_isEngineOn)
            {
                _forksHorizontal += (horizontalInput * Time.deltaTime * forksHorizontalSpeed);
                _forksHorizontal = Mathf.Clamp01(_forksHorizontal);
                forksCylinders.MovementInput = _forksHorizontal; 
            }
        }

        /// <summary>
        /// Handles mast rotation
        /// </summary>
        /// <param name="direction">1 = backwards | 0 = none | -1 = forward</param>
        public void RotateMast(float direction)
        {
            if (_isEngineOn)
            {
                _mastTilt += (-direction * Time.deltaTime * mastTiltSpeed);
                _mastTilt = Mathf.Clamp01(_mastTilt);
                mainMast.MovementInput = _mastTilt;
            }
        }

        /*
        /// <summary>
        /// Animate levers accordingly to player's input
        /// </summary>
        /// <param name="forkVerticalInput"></param>
        /// <param name="mastTiltInput"></param>
        */
        public void UpdateLevers(float forkVerticalInput, float mastTiltInput)
        {
            if (_isEngineOn)
            {
                _verticalLeverAngle = Mathf.MoveTowards(_verticalLeverAngle, forkVerticalInput * -15f, 70f * Time.deltaTime);
                //_horizontalLeverAngle = Mathf.MoveTowards(_horizontalLeverAngle, forkHorizontalInput * 10f, 70f * Time.deltaTime);
                _tiltLeverAngle = Mathf.MoveTowards(_tiltLeverAngle, mastTiltInput * -15f, 70f * Time.deltaTime);

                if (forksVerticalLever != null) forksVerticalLever.localEulerAngles = new Vector3(_verticalLeverAngle, 0f, 0f);
                //if (forksHorizontalLever != null) forksHorizontalLever.localEulerAngles = new Vector3(_horizontalLeverAngle, 0f, 0f);
                if (mastTiltLever != null) mastTiltLever.localEulerAngles = new Vector3(_tiltLeverAngle, 0f, 0f);
            }
        }

        private void ForksMovementSFX(bool forksMoving)
        {
            if (playSFX == false)
            {
                return;
            }

            if (_isEngineOn && forkMovingSFX != null)
            {
                if (!forkMovingSFX.isPlaying && forksMoving)
                {
                    forkMovingSFX.Play();

                    if (forkStartMovingSFX != null && !forkStartMovingSFX.isPlaying)
                        forkStartMovingSFX.Play();
                }
                else if (forkMovingSFX.isPlaying && !forksMoving)
                {
                    forkMovingSFX.Stop();

                    if (forkStopMovingSFX != null && !forkStopMovingSFX.isPlaying)
                        forkStopMovingSFX.Play();
                }
            }
        }


        public void ForkHeightChange()
        {
            Debug.Log(">>>>>>>>>>>>>>>>>>>>>>>" + _forkHeight);
        }
    }

    [System.Serializable]
    public struct YAxisStopValue
    {
        public float secondaryMastStop;
        public float forkStop;
    }

    public enum ForkHeight
    {
        Low = 0,
        Movemovingable = 1,
        High = 2
    }
}
