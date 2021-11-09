using UnityEngine;

namespace edu.tnu.dgd.vehicle
{
    [CreateAssetMenu(fileName = "VehicleInputSettingsPC", menuName = "東南科大數位遊戲設計系/車輛輸入參數設定", order = 1)]
    public class VehicleInputSettingsPC : ScriptableObject
    {
        public KeyCode toggleEngine = KeyCode.T;
        public KeyCode acceleration = KeyCode.UpArrow;
        public KeyCode reverse = KeyCode.DownArrow;
        public KeyCode turnRight = KeyCode.RightArrow;
        public KeyCode turnLeft = KeyCode.LeftArrow;
        public KeyCode brakes = KeyCode.Space;
        public KeyCode handbrakeEnable = KeyCode.Insert;
        public KeyCode handbrakeDisable = KeyCode.Delete;
        public KeyCode clutch = KeyCode.LeftShift;
        public KeyCode horn = KeyCode.H;
        public KeyCode headlights = KeyCode.L;
        public KeyCode interiorLights = KeyCode.I;
        public KeyCode leftSignalLights = KeyCode.N;
        public KeyCode rightSignalLights = KeyCode.M;

        public KeyCode cameraLookLeftTop = KeyCode.Q;
        public KeyCode cameraLookLeftBottom = KeyCode.A;
        public KeyCode cameraLookLeftBackward = KeyCode.Z;

        public KeyCode cameraLookRightTop = KeyCode.E;
        public KeyCode cameraLookRightBottom = KeyCode.D;
        public KeyCode cameraLookRightBackward = KeyCode.C;

        public KeyCode cameraLookForward = KeyCode.W;
        public KeyCode cameraLookCenter = KeyCode.S;
        public KeyCode cameraLookBackward = KeyCode.X;

        public KeyCode cameraLookUp = KeyCode.U;
        public KeyCode cameraLookDown = KeyCode.J;

        public KeyCode toggleCamera = KeyCode.C;
        public KeyCode resetVehiclePosition = KeyCode.O;
        public KeyCode goToLastPosition = KeyCode.Alpha0;
        public KeyCode goTo2ndLastPosition = KeyCode.Alpha1;
        public KeyCode goTo3rdLastPosition = KeyCode.Alpha2;
        public KeyCode cleanObstacle = KeyCode.Y;

        public KeyCode gearPositionPark = KeyCode.End;
        public KeyCode gearPositionNeutral = KeyCode.Home;
        public KeyCode gearPositionDrive = KeyCode.PageUp;
        public KeyCode gearPositionReverse = KeyCode.PageDown;

        public KeyCode[] customEventTriggers;
    } 
}
