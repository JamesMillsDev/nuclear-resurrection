using TunaTK.Events;

namespace Blockrain.Entity.Player
{
	public class CameraStateChangeEvent : BaseEvent
	{
		public readonly bool state;
		public CameraStateChangeEvent(bool _newState) => state = _newState;
	}
}