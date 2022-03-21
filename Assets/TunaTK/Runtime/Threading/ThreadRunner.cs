// Property of TUNACORN STUDIOS PTY LTD 2018
// 
// Creator: James Mills
// Creation Time: 13/03/2022 12:19 PM

using System.Threading.Tasks;

namespace TunaTK.Threading
{
	public abstract class ThreadRunner : InitialiseBehaviour
	{
		protected ThreadDispatcher dispatcher;

		public void Prepare(ThreadDispatcher _dispatcher) => dispatcher = _dispatcher;
		public void ResetThread() => dispatcher.StartThread(this);
		
		protected override Task OnInitialisation(params object[] _params) => Task.CompletedTask;

		public abstract void WorkerThread();
	}
}