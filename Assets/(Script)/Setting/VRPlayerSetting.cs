using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace edu.tnu.dgd.setting
{
    public class VRPlayerSetting : MonoBehaviour
    {
        public static int countInMainScene = 0;
        void Awake()
        {
            /*
            // when back to the MainMenu scene there two player will be appearing, should inactive one
            Player[] players = FindObjectsOfType<Player>();
            if (players != null && players.Length > 1)
            {
                Player anotherPlayer = this.gameObject.GetComponent<Player>();
                this.gameObject.SetActive(false);
            }
            */
            
        }
    }
}