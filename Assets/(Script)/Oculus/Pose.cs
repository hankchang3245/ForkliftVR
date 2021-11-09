using UnityEngine;

namespace Oculus
{
	public class Pose
	{
		public Vector3 Position;
		public Quaternion Rotation;

		public Pose()
		{
			Position = Vector3.zero;
			Rotation = Quaternion.identity;
		}

		public Pose(Vector3 position, Quaternion rotation)
		{
			Position = position;
			Rotation = rotation;
		}
	}
}
