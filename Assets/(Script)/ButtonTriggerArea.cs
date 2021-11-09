/************************************************************************************

Copyright (c) Facebook Technologies, LLC and its affiliates. All rights reserved.  

See SampleFramework license.txt for license terms.  Unless required by applicable law 
or agreed to in writing, the sample code is provided “AS IS” WITHOUT WARRANTIES OR 
CONDITIONS OF ANY KIND, either express or implied.  See the license for specific 
language governing permissions and limitations under the license.

************************************************************************************/

using UnityEngine;
using UnityEngine.Assertions;
using edu.tnu.dgd.debug;
using OculusSampleFramework;

namespace edu.tnu.dgd.vr
{
    /// <summary>
    /// Trigger zone of button, can be proximity, contact or action.
    /// </summary>
    public class ButtonTriggerArea : MonoBehaviour, ColliderZone
    {
        public enum ButtonType
        {
            MoveRight = 0,
            MoveLeft,
            MoveUp,
            MoveDown,
            MoveForward,
            MoveBackward,
            Action,
            CloseCalibration
        }
        public enum ActionType
        {
            None = 0,
            Calibration,
            LeftPosition,
            RightPosition,
            DoCalibrate
        }

        public static ActionType currentAction = ActionType.None;

        public ButtonType buttonType;

        public Collider Collider { get; private set; }
        public Interactable ParentInteractable { get; private set; }

        public InteractableCollisionDepth CollisionDepth => throw new System.NotImplementedException();

        private void Awake()
        {

        }

        private void OnTriggerEnter(Collider other)
        {

            if (buttonType == ButtonType.Action)
            {
                if (currentAction == ActionType.None)
                {
                    currentAction = ActionType.Calibration;
                }
                else if (currentAction == ActionType.Calibration)
                {
                    currentAction = ActionType.LeftPosition;
                }
                else if (currentAction == ActionType.LeftPosition)
                {
                    currentAction = ActionType.RightPosition;
                }
                else if (currentAction == ActionType.RightPosition)
                {
                    currentAction = ActionType.DoCalibrate;
                }
                else if (currentAction == ActionType.DoCalibrate)
                {
                    currentAction = ActionType.None;
                }

                switch (currentAction)
                {
                    case ActionType.None:
                        TestCalibration.instance.actionText.text = "無";
                        PartExplainingController.instance.StartExplaining();
                        break;

                    case ActionType.Calibration:
                        TestCalibration.instance.actionText.text = "調整機台";
                        break;

                    case ActionType.LeftPosition:
                        TestCalibration.instance.actionText.text = "左定位點";
                        break;

                    case ActionType.RightPosition:
                        TestCalibration.instance.actionText.text = "右定位點";
                        break;
                    case ActionType.DoCalibrate:
                        TestCalibration.instance.actionText.text = "執行定位";
                        break;
                }
            }

            if (buttonType == ButtonType.CloseCalibration && currentAction == ActionType.None)
            {
                TestCalibration.instance.CloseCalibration();
                return;
            }

            if (buttonType == ButtonType.MoveLeft && currentAction == ActionType.LeftPosition)
            {
                TestCalibration.instance.SetLeftPosition();
            }

            if (buttonType == ButtonType.MoveRight && currentAction == ActionType.RightPosition)
            {
                TestCalibration.instance.SetRightPosition();
            }

            if (buttonType == ButtonType.MoveLeft && currentAction == ActionType.DoCalibrate)
            {
                TestCalibration.instance.DoCalibrateLeft();
            }

            if (buttonType == ButtonType.MoveRight && currentAction == ActionType.DoCalibrate)
            {
                TestCalibration.instance.DoCalibrateRight();
            }
        }

        private void OnTriggerExit(Collider other)
        {

        }

        private void OnTriggerStay(Collider other)
        {
            if (currentAction == ActionType.Calibration)
            {
                switch (buttonType)
                {
                    case ButtonType.MoveForward:
                        TestCalibration.instance.MoveForward();
                        break;

                    case ButtonType.MoveBackward:
                        TestCalibration.instance.MoveBackward();
                        break;

                    case ButtonType.MoveLeft:
                        TestCalibration.instance.MoveLeft();
                        break;

                    case ButtonType.MoveRight:
                        TestCalibration.instance.MoveRight();
                        break;

                    case ButtonType.MoveUp:
                        TestCalibration.instance.MoveUp();
                        break;

                    case ButtonType.MoveDown:
                        TestCalibration.instance.MoveDown();
                        break;

                    case ButtonType.Action:
                        break;
                }

            }

        }
    }
}
