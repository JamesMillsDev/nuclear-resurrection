// Property of TUNACORN STUDIOS PTY LTD 2018
// 
// Creator: James Mills
// Creation Time: 28/02/2022 9:39 AM

using TunaTK.Events;

namespace TunaTK.Augments
{
	public abstract class EventAugment<USER> : Augment<USER>, IEventHandler where USER : User<USER>
	{
		protected override void OnEnable()
		{
			EventBus.RegisterObject(this);
			base.OnEnable();
		}

		protected virtual void OnDisable() => EventBus.RemoveObject(this);
	}
}