using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace edu.tnu.dgd.vkey
{
    public class VRInputField : MonoBehaviour
    {
        public InputField inputField;
        public KeyboardHandler kbHandler;
        public Text[] clearTextWhenOpenKeyboard;

        void Start()
        {
            
        }

        public string GetText()
        {
            return inputField.text;
        }

        public void SetText(string tx)
        {
            this.inputField.text = tx;
        }


        public void ClearText()
        {
            foreach (Text txt in clearTextWhenOpenKeyboard)
            {
                txt.text = "";
            }
        }
    }

}
