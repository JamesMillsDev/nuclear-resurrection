// Property of TUNACORN STUDIOS PTY LTD 2018
// 
// Creator: James Mills
// Creation Time: 28/02/2022 9:28 AM

using TunaTK.AssetManagement;

namespace TunaTK.Events
{
	/// <summary> A base MonoBehaviour that automatically registers the script into the Event system  </summary>
	public class EventBehaviour : AssetBehaviour, IEventHandler
	{
		protected virtual void OnEnable() => EventBus.RegisterObject(this);

		protected virtual void OnDisable() => EventBus.RemoveObject(this);
	}
}