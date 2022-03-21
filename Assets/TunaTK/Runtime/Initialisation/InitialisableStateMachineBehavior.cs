// Property of TUNACORN STUDIOS PTY LTD 2018
// 
// Creator: James Mills
// Creation Time: 11/14/2018 10:00:00 PM
// Created On: CHRONOS

using Sirenix.OdinInspector;

using UnityEngine;

namespace TunaTK
{
	/// <summary>The Initialisable version of a StateMachineBehaviour within Unity. It allows initialising of StateMachine states making the setup phases of them super easy.</summary>
	public abstract class InitialisableStateMachineBehaviour : SerializedStateMachineBehaviour
	{
		//@cond
		public bool Initialised { get; protected set; } = false;
		//@endcond

		[SerializeField]
		// ReSharper disable once InvalidXmlDocComment
		/// <summary>This determines if the Initialisable will automatically setup.</summary>
		protected bool autoInitialise = false;

		/// <summary>Calls <see cref="OnInitialisation(Animator,object[])"/> passing through <paramref name="_params"/> to it.</summary>
		/// <param name="_animator">The animator associated with this behaviour.</param>
		/// <param name="_params">All the data necessary for the Initialisable to setup.</param>
		/// <example>Example call of Initialise.<code>
		/// foobar.Initialise(animator, 1, "test", true);
		/// </code></example>
		public void Initialise(Animator _animator, params object[] _params)
		{
			// If the object is already initialised, warn the developer and ignore the initialise call.
			if(Initialised)
			{
				Debug.LogWarning($"Initialisable state machine behaviour [{_animator.gameObject.name}] already initialized! Ignoring call....");
				return;
			}

			// Run the OnInitialise function and make Initialised true
			OnInitialisation(_animator, _params);
			Initialised = true;
		}

		/// <summary>This is the function that will actually run when the object is Initialised. This is the setup function where Initialisables can do anything they need to setup.</summary>
		/// <param name="_params">All the data necessary for the Initialisable to setup.</param>
		/// <param name="_animator">The animator associated with this behaviour.</param>
		protected abstract void OnInitialisation(Animator _animator, params object[] _params);

		//@cond
		// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
		public override void OnStateEnter(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
		{
			if(autoInitialise)
				Initialise(_animator);
		}
		//@endcond
	}
}