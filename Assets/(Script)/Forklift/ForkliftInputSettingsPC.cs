using UnityEngine;

namespace edu.tnu.dgd.forklift
{
    [CreateAssetMenu(fileName = "ForkliftInputSettingsPC", menuName = "東南科大數位遊戲設計系/堆高機輸入參數設定", order = 1)]
    public class ForkliftInputSettingsPC : ScriptableObject
    {
        public KeyCode toggleEngine = KeyCode.T;
        public KeyCode forksUp = KeyCode.Keypad6;
        public KeyCode forksDown = KeyCode.Keypad3;
        public KeyCode mastTiltBackwards = KeyCode.Keypad1;
        public KeyCode mastTiltForwards = KeyCode.Keypad4;
        public KeyCode forksRight = KeyCode.Keypad9;
        public KeyCode forksLeft = KeyCode.Keypad7;

        public KeyCode[] customEventTriggers;
    }
}
