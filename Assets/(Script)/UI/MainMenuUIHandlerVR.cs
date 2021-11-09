using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

namespace edu.tnu.dgd.ui
{
    public class MainMenuUIHandlerVR : MonoBehaviour
    {
        [SerializeField]
        private OVRRaycaster ovrRaycaster;

        [SerializeField]
        private GameObject uiHelpersToInstantiate = null;

        public LaserPointer.LaserBeamBehavior laserBeamBehavior = LaserPointer.LaserBeamBehavior.On;

        public void Awake()
        {
            Assert.IsNotNull(ovrRaycaster);

            Instantiate(uiHelpersToInstantiate);

            LaserPointer lp = FindObjectOfType<LaserPointer>();
            if (!lp)
            {
                Debug.LogError("Debug UI requires use of a LaserPointer and will not function without it. Add one to your scene, or assign the UIHelpers prefab to the DebugUIBuilder in the inspector.");
                return;
            }
            lp.laserBeamBehavior = laserBeamBehavior;
            ovrRaycaster.pointer = lp.gameObject;

            StartCoroutine(DisableAllCollider());
        }

        IEnumerator DisableAllCollider()
        {
            Collider[] allcol = ovrRaycaster.gameObject.GetComponentsInChildren<Collider>();
            foreach (Collider col in allcol)
            {
                col.enabled = false;
            }

            yield return null; ;
        }


    }

}