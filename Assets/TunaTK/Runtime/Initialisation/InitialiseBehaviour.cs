// Property of TUNACORN STUDIOS PTY LTD 2018
// 
// Creator: James Mills
// Creation Time: 11/14/2018 10:00:00 PM
// Created On: CHRONOS

using System.Threading.Tasks;

using TunaTK.AssetManagement;
using TunaTK.Utility;

using UnityEngine;

namespace TunaTK
{
	/// <summary>
	/// Base class that allows setup chains to be created. This is using Odin's SerializedMonoBehaviour so dictionaries will be serialized.
	/// </summary>
	public abstract class InitialiseBehaviour : AssetBehaviour, IInitialiser
	{
		//@cond
		public bool Initialised { get; protected set; }
		//@endcond

		/// <summary>This determines if the Initialisable will automatically setup in the specified <see cref="initialisationMethod"/>.</summary>
		[SerializeField] protected bool autoInitialise;

		/// <summary>The stage that the object will be initialised in if <see cref="autoInitialise"/> is true.</summary>
		[SerializeField] protected InitialisationMethod initialisationMethod = InitialisationMethod.Awake;
		
		/// <summary>Calls <see cref="OnInitialisation(object[])"/> passing through <paramref name="_params"/> to it.</summary>
		/// <param name="_params">All the data necessary for the Initialisable to setup.</param>
		/// <example>Example call of Initialise.<code>
		/// foobar.Initialise(1, "test", true);
		/// </code></example>
		public async Task Initialise_Async(params object[] _params)
		{
			// If the object is already initialised, warn the developer and ignore the initialise call.
			if(Initialised)
			{
				Debug.LogWarning($"Initialisable object [{gameObject.name}] already initialized! Ignoring call....");
				return;
			}

			// Run the OnInitialise function and make Initialised true
			await OnInitialisation(_params);
			Initialised = true;
		}

		/// <summary>This is the function that will actually run when the object is Initialised. This is the setup function where Initialisables can do anything they need to setup.</summary>
		/// <param name="_params">All the data necessary for the Initialisable to setup.</param>
		protected abstract Task OnInitialisation(params object[] _params);

		/// <summary>Only called once every frame after the object has been initialized.</summary>
		protected virtual void InitialisedUpdate()
		{
		}

		/// <summary>Only called once every physics frame after the object has been initialized.</summary>
		protected virtual void InitialisedFixedUpdate()
		{
		}

		/// <summary>Only called once at the end of every frame after the object has been initialized.</summary>
		protected virtual void InitialisedLateUpdate()
		{
		}

		//@cond
		// Called before OnEnable and Start
		protected virtual void Awake()
		{
			if(autoInitialise && initialisationMethod == InitialisationMethod.Awake)
				Initialise_Async().Process();
		}

		// Called before Start and everytime the object becomes active
		protected virtual void OnEnable()
		{
			if(autoInitialise && initialisationMethod == InitialisationMethod.OnEnable)
				Initialise_Async().Process();
		}

		// Called last in the init chain
		protected virtual void Start()
		{
			if(autoInitialise && initialisationMethod == InitialisationMethod.Start)
				Initialise_Async().Process();
		}

		// Called once every frame.
		protected virtual void Update()
		{
			if(Initialised)
				InitialisedUpdate();
		}

		// Called once every frame.
		protected virtual void FixedUpdate()
		{
			if(Initialised)
				InitialisedFixedUpdate();
		}

		// Called once every frame.
		protected virtual void LateUpdate()
		{
			if(Initialised)
				InitialisedLateUpdate();
		}
		//@endcond
	}
}