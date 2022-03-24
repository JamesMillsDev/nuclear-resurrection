// Property of TUNACORN STUDIOS PTY LTD 2018
// 
// Creator: James Mills
// Creation Time: 16/03/2022 10:02 PM

using CMF;

using NuclearResurrection.Entity.Player;

using System.Threading.Tasks;

using TunaTK.Augments;

using UnityEngine;

namespace NuclearResurrection.Compatibility.CMF
{
	public class CmfAugment : Augment<PlayerEntity>
	{
		[SerializeField] private new Camera camera;
		[SerializeField] private CharacterInput characterInput;
		[SerializeField] private CeilingDetector ceilingDetector;
		[SerializeField] private CameraController cameraController;
		[SerializeField] private CameraDistanceRaycaster distanceRaycaster;
		
		private PlayerController playerController;

		public void Enable()
		{
			playerController.enabled = true;
			cameraController.enabled = true;
		}

		public void Disable()
		{
			playerController.enabled = false;
			cameraController.enabled = false;
		}

		protected override Task OnInitialisation(PlayerEntity _user, object[] _params)
		{
			playerController = _user.PlayerController;
			playerController.Setup(characterInput, ceilingDetector, camera.transform);

			distanceRaycaster.ignoreList[0] = _user.Collider;
			
			_user.Input.camera = camera;
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;

			if(_user.TryGetAugment(out EntityInteractionAugment augment))
			{
				
			}
			
			return base.OnInitialisation(_user, _params);
		}
	}
}