using JetBrains.Annotations;

using System.Collections.Generic;
using System.Threading.Tasks;

using TunaTK;

using UnityAsync;

using UnityEngine;

namespace Blockrain.Entity
{
	public class EntityManager : InitialiseBehaviourSingleton<EntityManager>
	{
		[SerializeField] private Dictionary<string, Entity> entities = new Dictionary<string, Entity>();

		[CanBeNull]
		public Entity Find(string _guid)
		{
			entities.TryGetValue(_guid, out Entity entity);
			return entity;
		}

		protected override async Task OnInitialisation(params object[] _params)
		{
			Entity[] found = FindObjectsOfType<Entity>();
			entities.Clear();

			foreach(Entity entity in found)
			{
				entities.Add(entity.Guid, entity);
				await new WaitForEndOfFrame();
			}
		}
	}
}