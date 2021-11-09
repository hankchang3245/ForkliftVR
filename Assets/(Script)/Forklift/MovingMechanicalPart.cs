using System;
using UnityEngine;
using UnityEngine.Events;

namespace edu.tnu.dgd.forklift
{
    [Serializable]
    public class MovingMechanicalPart : MechanicalPart, IMechanicalPart
    {
        #region Overriding Base Class Methods

        private float groundPositionY = -1.35f; // 地板的世界座標
        /// <summary>
        /// Linear interpolation between Min and Max values
        /// </summary>
        protected override void LinearMovement()
        {
            _transform.localPosition = Vector3.Lerp(_min, _max, _movementInput);
            //Debug.Log("LinearMovement=" + _transform.localPosition);
        }

        /// <summary>
        /// Non-Linear interpolation between Min and Max values
        /// </summary>
        protected override void NonLinearMovement()
        {
            _transform.localPosition = Vector3.Lerp(_min, _max, _movementFunction.Evaluate(_movementInput));
        }

        /// <summary>
        /// Set current to Min
        /// </summary>
        protected override void CurrentToMin()
        {
            _min = _transform.localPosition;
        }

        /// <summary>
        /// Set current to Max
        /// </summary>
        protected override void CurrentToMax()
        {
            _max = _transform.localPosition;
        }

        /// <summary>
        /// Set current to default
        /// </summary>
        protected override void CurrentToDefault()
        {
            _default = _transform.localPosition;
        }

        /// <summary>
        /// Set min to current
        /// </summary>
        protected override void MinToCurrent()
        {
            _transform.localPosition = _min;
        }

        /// <summary>
        /// Set max to current
        /// </summary>
        protected override void MaxToCurrent()
        {
            _transform.localPosition = _max;
        }

        /// <summary>
        /// Set default to current
        /// </summary>
        protected override void DefaultToCurrent()
        {
            _transform.localPosition = _default;
        }

        /// <summary>
        /// Return current value
        /// </summary>
        /// <returns></returns>
        protected override Vector3 GetCurrentValue()
        {
            return _transform.localPosition;
        }

        public float GetCurrentForkHeight()
        {
            return (_transform.position.y - groundPositionY) * 100f; ;
        }

        #endregion
    }
}
