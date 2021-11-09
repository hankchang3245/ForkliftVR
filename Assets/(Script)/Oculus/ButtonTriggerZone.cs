using UnityEngine;
using UnityEngine.Assertions;

namespace Oculus
{
	public class ButtonTriggerZone : MonoBehaviour, ColliderZone
	{
		[SerializeField] private GameObject _parentInteractableObj = null;

		public Collider Collider { get; private set; }
		public Interactable ParentInteractable { get; private set; }

		public InteractableCollisionDepth CollisionDepth
		{
			get
			{
				var myColliderZone = (ColliderZone)this;
				var depth = ParentInteractable.ProximityCollider == myColliderZone ? InteractableCollisionDepth.Proximity :
				  ParentInteractable.ContactCollider == myColliderZone ? InteractableCollisionDepth.Contact :
				  ParentInteractable.ActionCollider == myColliderZone ? InteractableCollisionDepth.Action :
				  InteractableCollisionDepth.None;
				return depth;
			}
		}

		private void Awake()
		{
			Assert.IsNotNull(_parentInteractableObj);

			Collider = GetComponent<Collider>();
			ParentInteractable = _parentInteractableObj.GetComponent<Interactable>();
		}
	}
}
