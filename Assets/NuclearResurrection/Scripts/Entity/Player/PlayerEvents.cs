using TunaTK.Events;

using UnityEngine;

namespace NuclearResurrection.Entity.Player
{
	public class CameraStateChangeEvent : BaseEvent
	{
		public readonly bool state;
		public readonly Camera camera;
		public CameraStateChangeEvent(bool _newState, Camera _camera)
		{
			camera = _camera;
			state = _newState;
		}
	}
}