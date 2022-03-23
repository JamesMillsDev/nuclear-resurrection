using TunaTK.Events;

namespace NuclearResurrection.Entity.Player
{
	public class CameraStateChangeEvent : BaseEvent
	{
		public readonly bool state;
		public CameraStateChangeEvent(bool _newState) => state = _newState;
	}
}