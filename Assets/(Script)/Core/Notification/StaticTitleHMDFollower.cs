using UnityEngine;
using edu.tnu.dgd.vr;

namespace edu.tnu.dgd.notification
{
	public class StaticTitleHMDFollower : MonoBehaviour
	{
		private Transform lookAtJointTransform;
		private Vector3 targetPosition;
		private Vector3 lookAtPosition = Vector3.zero;
		private CameraObjectFacade vrObjectFacade;

		void Awake()
		{
			lookAtJointTransform = transform.Find("teleport_marker_lookat_joint");
		}

		private void Start()
        {
			vrObjectFacade = FindObjectOfType<CameraObjectFacade>();
		}

		void Update()
		{
			targetPosition = vrObjectFacade.cameraEyeCenterPosition;
			lookAtPosition.x = targetPosition.x;
			lookAtPosition.y = lookAtJointTransform.position.y;
			lookAtPosition.z = targetPosition.z;

			lookAtJointTransform.LookAt( lookAtPosition );
		}
	}
}
