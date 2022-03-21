// Property of TUNACORN STUDIOS PTY LTD 2018
//
// Creator: James Mills
// Creation Time: 24/07/2019 10:18 AM
// Created On: CHRONOS

using Sirenix.OdinInspector;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using TunaTK.Augments;
using TunaTK.Utility;

using UnityAsync;

using UnityEngine;
using UnityEngine.UI;

namespace TunaTK.Cursors
{
	public abstract class CursorAugment<USER> : Augment<USER> where USER : User<USER>
	{
		[Serializable]
		public struct CustomCursor
		{
			public Sprite sprite;
			public string id;
			public bool isDefault;
		}

		[SerializeField] private CustomCursor[] cursors;
		[SerializeField] private Image cursorImage;
		
		private string current;
		private bool hasSetCurrent;

		private Canvas canvas;
		private readonly Dictionary<string, Sprite> cursorDictionary = new Dictionary<string, Sprite>();

		protected override async Task OnInitialisation(USER _user, params object[] _params)
		{
			Cursor.visible = false;

			foreach(CustomCursor cursor in cursors)
			{
				if(cursor.isDefault && !hasSetCurrent)
				{
					hasSetCurrent = true;
					cursorImage.sprite = cursor.sprite;
					current = cursor.id;
				}
				
				cursorDictionary.Add(cursor.id, cursor.sprite);
			}

			await new WaitForSeconds(.5f);

			canvas = gameObject.GetComponent<Canvas>();
			CanvasScaler scaler = gameObject.GetComponent<CanvasScaler>();
			scaler.referenceResolution = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
			scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			scaler.matchWidthOrHeight = .5f;
		}

		public void SetCursor(string _id)
		{
			if(_id != current && cursorDictionary.TryGetValue(_id, out Sprite newCursor))
			{
				current = _id;
				cursorImage.sprite = newCursor;
			}
		}

		protected override void InitialisedUpdate() => cursorImage.rectTransform.anchoredPosition = canvas.ScreenToCanvasPosition(Input.mousePosition);
	}
}