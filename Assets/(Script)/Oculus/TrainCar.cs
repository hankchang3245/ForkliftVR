using UnityEngine;
using UnityEngine.Assertions;

namespace Oculus
{
	public class TrainCar : TrainCarBase
	{
		[SerializeField] private TrainCarBase _parentLocomotive = null;
		[SerializeField] protected float _distanceBehindParent = 0.1f;

		public float DistanceBehindParentScaled
		{
			get { return scale * _distanceBehindParent; }
		}

		protected override void Awake()
		{
			base.Awake();
			Assert.IsNotNull(_parentLocomotive);
		}

		public override void UpdatePosition()
		{
			Distance = _parentLocomotive.Distance - DistanceBehindParentScaled;
			UpdateCarPosition();
			RotateCarWheels();
		}
	}
}
