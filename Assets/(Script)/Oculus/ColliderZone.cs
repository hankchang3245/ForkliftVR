using System;
using UnityEngine;

namespace Oculus
{
	public interface ColliderZone
	{
		Collider Collider { get; }
		// Which interactable do we belong to?
		Interactable ParentInteractable { get; }
		InteractableCollisionDepth CollisionDepth { get; }
	}

	public class ColliderZoneArgs : EventArgs
	{
		public readonly ColliderZone Collider;
		public readonly float FrameTime;
		public readonly InteractableTool CollidingTool;
		public readonly InteractionType InteractionT;

		public ColliderZoneArgs(ColliderZone collider, float frameTime,
		  InteractableTool collidingTool, InteractionType interactionType)
		{
			Collider = collider;
			FrameTime = frameTime;
			CollidingTool = collidingTool;
			InteractionT = interactionType;
		}
	}

	public enum InteractionType
	{
		Enter = 0,
		Stay,
		Exit
	}
}
