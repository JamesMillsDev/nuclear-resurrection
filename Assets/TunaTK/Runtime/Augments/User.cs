// Property of TUNACORN STUDIOS PTY LTD 2018
// 
// Creator: James Mills
// Creation Time: 28/02/2022 8:54 AM

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

using TunaTK.Utility;

using UnityEngine;

namespace TunaTK.Augments
{
	public abstract class User<USER> : InitialiseBehaviour where USER : User<USER>
	{
		/// <summary> An accessible, but non-editable list of all augments in the system. </summary>
		public ReadOnlyCollection<Augment<USER>> Augments => runtimeAugments.AsReadOnly();

		/// <summary> The list of augments that will be added to the user at runtime. </summary>
		[SerializeField] protected List<Augment<USER>> augments = new List<Augment<USER>>();

		/// <summary> The list of augments that were spawned at runtime. </summary>
		protected readonly List<Augment<USER>> runtimeAugments = new List<Augment<USER>>();

		//@cond
		protected override Task OnInitialisation(params object[] _params)
		{
			SpawnAugments();

			// Initialise all augments in the runtime list
			runtimeAugments.ForEach(_augment => _augment.Initialise_Async(this).Process());

			return Task.CompletedTask;
		}
		//@endcond

		/// <summary> The function that is used to spawn the augments onto the user. This is virtual to allow custom behaviour for spawning </summary>
		protected virtual void SpawnAugments()
		{
			// Spawn a GameObject as a child of this object that will hold all augment objects
			GameObject augmentHolder = new GameObject("Augments");
			augmentHolder.transform.SetParent(transform, false);

			// Loop through all augments and instantiate them to populate the runtime list
			augments.ForEach(_augment =>
			{
				Augment<USER> augment = Instantiate(_augment, augmentHolder.transform);
				runtimeAugments.Add(augment);
				
				// Modify the spawned object's name to fit our naming convention
				augment.name = $"[AUGMENT] {augment.name.Replace("Augment", "").Replace("(Clone)", "")}";
			});
		}

		#region HELPER_FUNCTIONS

		/// <summary> Checks if the augment of the passed type is on the user. </summary>
		/// <typeparam name="AUGMENT"> The type of augment you are checking is on the user. </typeparam>
		public bool HasAugment<AUGMENT>() where AUGMENT : Augment<USER> => runtimeAugments.Any(_c => _c.GetType() == typeof(AUGMENT));
		
		/// <summary> Attempts to find the augment in the system, return if it is found and populates the value with the found augment. </summary>
		/// <param name="_augment"> The augment that will be populated if any with the type passed is found. </param>
		/// <typeparam name="AUGMENT"> The type of augment you are trying to get from the user. </typeparam>
		public bool TryGetAugment<AUGMENT>(out AUGMENT _augment) where AUGMENT : Augment<USER>
		{
			// Attempt to get the first found augment in the runtime list and return whether or not it found one
			_augment = (AUGMENT) runtimeAugments.FirstOrDefault(_c => _c.GetType() == typeof(AUGMENT));
			return _augment != null;
		}
		
		/// <summary> Attempts to find the augment in the system, return if it is found and populates the value with the found augment. </summary>
		/// <param name="_augment"> The augment that will be populated if any with the type passed is found. </param>
		/// <typeparam name="AUGMENT"> The type of augment you are trying to get from the user. </typeparam>
		public bool TryGetAllAugmentsOfType<AUGMENT>(out IEnumerable<AUGMENT> _augment) where AUGMENT : Augment<USER>
		{
			// Attempt to get the first found augment in the runtime list and return whether or not it found one
			_augment = (IEnumerable<AUGMENT>) runtimeAugments.Where(_c => _c.GetType() == typeof(AUGMENT));
			return _augment.Any();
		}
		
		/// <summary> Attempts to remove the augment from the system provided it actually is present. </summary>
		/// <param name="_augment"> The augment you are trying to remove fro the user. </param>
		/// <typeparam name="USER"> The type of user associated with the augment you are trying to remove. </typeparam>
		[SuppressMessage("ReSharper", "UnusedMethodReturnValue.Global")]
		public bool RemoveAugment(Augment<USER> _augment)
		{
			if(runtimeAugments.Contains(_augment))
			{
				Destroy(_augment);
				runtimeAugments.Remove(_augment);
				
				return true;
			}

			return false;
		}
		
		/// <summary> Attempts to add the augment from the system. </summary>
		/// <param name="_augment"> The augment you are trying to add to the user. </param>
		/// <typeparam name="USER"> The type of user associated with the augment you are trying to add. </typeparam>
		[SuppressMessage("ReSharper", "UnusedMethodReturnValue.Global")]
		public virtual void AddAugment(Augment<USER> _augment)
		{
			GameObject augmentHolder = transform.Find("Augments").gameObject;
			Augment<USER> augment = Instantiate(_augment, augmentHolder.transform);
			runtimeAugments.Add(augment);
				
			// Modify the spawned object's name to fit our naming convention
			augment.name = $"[AUGMENT] {augment.name.Replace("Augment", "").Replace("(Clone)", "")}";
		}

		#endregion
	}
}