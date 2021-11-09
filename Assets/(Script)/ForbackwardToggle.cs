using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace edu.tnu.dgd.vr.forklift
{
    public class ForbackwardToggle : MonoBehaviour
    {
        private Transform forwardPosition;
        private Transform nonePosition;
        private Transform backwardPosition;
        private ForbackwardState currentState = ForbackwardState.None;

        void Start()
        {
            nonePosition = gameObject.transform;
            forwardPosition = transform.Find("ForwardPosition");
            backwardPosition = transform.Find("BackwardPosition");
        }

        public void ToForward()
        {
            Vector3 endPosition = forwardPosition.localRotation.eulerAngles;
            transform.DORotate(endPosition, 1f, RotateMode.LocalAxisAdd);
            currentState = ForbackwardState.Forward;
        }

        public void ToBackward()
        {
            Vector3 endPosition = backwardPosition.localRotation.eulerAngles;
            transform.DORotate(endPosition, 1f, RotateMode.LocalAxisAdd);
            currentState = ForbackwardState.Backward;
        }
        public void ToNone()
        {
            Vector3 endPosition = nonePosition.localRotation.eulerAngles;
            transform.DORotate(endPosition, 1f, RotateMode.LocalAxisAdd);
            currentState = ForbackwardState.None;
        }

    }
}