// Property of TUNACORN STUDIOS PTY LTD 2018
// 
// Creator: James Mills
// Creation Time: 28/02/2022 8:53 AM

using System;
using System.Threading.Tasks;

using TunaTK.AssetManagement;
using TunaTK.Utility;

using UnityEngine;

namespace TunaTK.Augments
{
	public abstract class Augment<USER> : AssetBehaviour, IInitialiser where USER : User<USER>
	{
		protected USER user;
		
		//@cond
		public bool Initialised { get; protected set; }
		//@endcond

		/// <summary>This determines if the Initialisable will automatically setup in the specified <see cref="initialisationMethod"/>.</summary>
		[SerializeField] protected bool autoInitialise;

		/// <summary>The stage that the object will be initialised in if <see cref="autoInitialise"/> is true.</summary>
		[SerializeField] protected InitialisationMethod initialisationMethod = InitialisationMethod.Awake;

		/// <summary>Calls <see cref="OnInitialisation(USER,object[])"/> passing through <paramref name="_params"/> to it.</summary>
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

			user = (USER) _params[0];

			// Copy the data from the initial parameters ignoring the user.
			object[] parameters = new object[_params.Length - 1];
			if(parameters.Length > 0)
				Array.Copy(_params, 1, parameters, 0, parameters.Length);
			
			// Run the OnInitialise function and make Initialised true
			await OnInitialisation(user, parameters);
			Initialised = true;
		}

		/// <summary>This is the function that will actually run when the object is Initialised. This is the setup function where capabilities can do anything they need to setup.</summary>
		/// <param name="_user">The user controlling this Capability</param>
		/// <param name="_params">All the data necessary for the Initialisable to setup.</param>
		protected virtual Task OnInitialisation(USER _user, object[] _params) => Task.CompletedTask;

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