using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using edu.tnu.dgd.debug;


public class ControllerActiveChecker : MonoBehaviour
{
	[SerializeField]
	private GameObject _notificationPrefab = null;

	private GameObject _notification = null;
	private OVRCameraRig _cameraRig = null;
	private Transform _centerEye = null;

	[SerializeField]
	private OVRRaycaster ovrRaycaster;

	[SerializeField]
	private GameObject uiHelpersToInstantiate = null;

	public LaserPointer.LaserBeamBehavior laserBeamBehavior = LaserPointer.LaserBeamBehavior.On;

	public void Awake()
	{
		Assert.IsNotNull(ovrRaycaster);
		Assert.IsNotNull(_notificationPrefab);

		_notification = Instantiate(_notificationPrefab);
		StartCoroutine(GetCenterEye());
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

	private void CreateLaserPointer()
    {
		Instantiate(uiHelpersToInstantiate);

		LaserPointer lp = FindObjectOfType<LaserPointer>();
		if (!lp)
		{
			Debug.LogError("Debug UI requires use of a LaserPointer and will not function without it. Add one to your scene, or assign the UIHelpers prefab to the DebugUIBuilder in the inspector.");
			return;
		}
		lp.laserBeamBehavior = laserBeamBehavior;
		ovrRaycaster.pointer = lp.gameObject;
	}

	private void Update()
	{
		OVRPlugin.Controller cc = OVRPlugin.GetActiveController();

		//ShowDebugLog.instance.Log("Controller：" + cc.ToString());

		
		if (TouchScreenKeyboard.visible || cc == OVRPlugin.Controller.LTouch || cc == OVRPlugin.Controller.RTouch || cc == OVRPlugin.Controller.Touch)
		{
			_notification.SetActive(false);
			CreateLaserPointer();
		}
		else
		{
			_notification.SetActive(true);
			if (_centerEye) {
				_notification.transform.position = _centerEye.position + _centerEye.forward * 0.5f;
				_notification.transform.rotation = _centerEye.rotation;
			}
			
		}

	}

	private IEnumerator GetCenterEye()
	{
		if ((_cameraRig = FindObjectOfType<OVRCameraRig>()) != null)
		{
			while (!_centerEye)
			{
				_centerEye = _cameraRig.centerEyeAnchor;
				yield return null;
			}
		}
	}
}
