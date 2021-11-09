using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace edu.tnu.dgd.setting
{
    public class ResetHeight : MonoBehaviour
    {
        private bool hasPressed;

        public Transform playerStartPosition;

        private void Start()
        {
            hasPressed = false;
        }

    }
}
