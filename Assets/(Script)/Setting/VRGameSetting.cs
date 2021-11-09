using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace edu.tnu.dgd.setting
{
    public class VRGameSetting : MonoBehaviour
    {
        public float displayTeleportHintInterval = 20f; // second
        private float startTime = 0f;

        public bool hideController = true;
        public bool useHandControllerHoverComponent = false;

        public bool useHandHoverSphere = false;
        public float hoverSphereRadius = 0.05f;

        public bool useFingerJointHover = true;
        public float fingerJointHoverRadius = 0.03f;

        public Transform playerStartPosition;

        [Tooltip("當true時每次進入新場景，顯示提示；當false時只有第一次進入場景時提示。")]
        public bool alwaysShowTeleportHint = false;

        void Awake()
        {
            startTime = Time.time;
        }

        void Start()
        {
            MoveToPlayerStartPosition();
        }

        private void MoveToPlayerStartPosition()
        {
            float playerHeight = PlayerPrefs.GetFloat("PlayerHeightOffset");
            Vector3 newPos = new Vector3(playerStartPosition.localPosition.x,
                playerStartPosition.localPosition.y + playerHeight, 
                playerStartPosition.localPosition.z);
            //Player.instance.gameObject.transform.localPosition = newPos; // playerStartPosition.localPosition;
            //Player.instance.gameObject.transform.localRotation = playerStartPosition.localRotation;
        }
        /*
        private void OnLevelWasLoaded(int level)
        {
            GameObject playerCamera = GameObject.Find("VRCamera");  //get the VRcamera object
            Vector3 globalCameraPosition = playerCamera.transform.position;  //get the global position of VRcamera
            Vector3 globalPlayerPosition = Player.instance.transform.position;
            Vector3 globalOffsetCameraPlayer = 
                    new Vector3(globalCameraPosition.x - globalPlayerPosition.x, 0, globalCameraPosition.z - globalPlayerPosition.z);
            Vector3 newRigPosition = 
                    new Vector3(playerStartPosition.transform.position.x - globalOffsetCameraPlayer.x, this.transform.position.y, playerStartPosition.transform.position.z - globalOffsetCameraPlayer.z);
            Player.instance.transform.position = newRigPosition;

        }

        void Update()
        {
            if (Teleport.instance != null)
            {
                if ((Time.time - startTime) > displayTeleportHintInterval)
                {
                    Teleport.instance.CancelTeleportHint();
                }
            }
            HideController(hideController);
            PlayerDisableSomeHover();
        }

        public void PlayerDisableSomeHover()
        {
            if (Player.instance == null || Player.instance.hands == null)
            {
                return;
            }

            for (int i = 0; i < Player.instance.hands.Length; i++)
            {
                Hand hand = Player.instance.hands[i];
                hand.useControllerHoverComponent = useHandControllerHoverComponent;
                hand.useHoverSphere = useHandHoverSphere;
                if (useHandHoverSphere)
                {
                    hand.hoverSphereRadius = hoverSphereRadius;
                    if (hand.hoverSphereTransform != null)
                    {
                        hand.hoverSphereTransform.localPosition = new Vector3(-0.0145f, 0.0066f, -0.1139f);
                    }
                }

                if (useFingerJointHover)
                {
                    hand.fingerJointHoverRadius = fingerJointHoverRadius;
                }
            }
        }

        public void HideController(bool hide)
        {
            for (int handIndex = 0; Player.instance != null && handIndex < Player.instance.hands.Length; handIndex++)
            {
                Hand hand = Player.instance.hands[handIndex];
                if (hand != null)
                {
                    if (hide)
                    {
                        hand.HideController(true);
                    }
                    else
                    {
                        hand.ShowController(true);
                    }
                }
            }
        }
        */
    }
}