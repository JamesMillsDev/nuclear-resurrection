#if UNITY_EDITOR
// Property of TUNACORN STUDIOS PTY LTD 2018
//
// Creator: James Mills
// Creation Time: 24/02/2020 05:07 PM
// Created On: CHRONOS

using System.Collections;
using System.Linq;

using TunaTK.Utility;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace TunaTK
{
	public static class GUIStyles
	{
	#region Private Members

		/// <summary>
		/// Name prefix
		/// </summary>
		private const string NAME_BASE = "TunaTK.Styles.";

		/// <summary>
		/// The font color
		/// </summary>
		private static readonly Color fontColor = Color.white * 0.85f;

		/// <summary>
		/// The font size of big texts
		/// </summary>
		private const int FONT_BIG_SIZE = 13;

		/// <summary>
		/// The font size of small texts
		/// </summary>
		private const int FONT_SMALL_SIZE = 9;

		/// <summary>
		/// The default margins size
		/// </summary>
		private const int DEFAULT_PADDING = 8;

		/// <summary>
		/// The default margin rectoffset
		/// </summary>
		private static readonly RectOffset defaultPaddingRectOffset = new RectOffset(DEFAULT_PADDING, DEFAULT_PADDING, DEFAULT_PADDING, DEFAULT_PADDING);

	#endregion

	#region Properties

		public static GUIStyle Background { get; private set; }

		public static GUIStyle BackgroundNoBorder { get; private set; }

		public static GUIStyle Label { get; private set; }
		public static GUIStyle LabelBold { get; private set; }
		public static GUIStyle LabelCentered { get; private set; }
		public static GUIStyle LabelBoldCentered { get; private set; }
		public static GUIStyle LabelBig { get; private set; }
		public static GUIStyle LabelBoldBig { get; private set; }
		public static GUIStyle LabelCenteredBig { get; private set; }
		public static GUIStyle LabelBoldCenteredBig { get; private set; }
		public static GUIStyle LabelSmall { get; private set; }
		public static GUIStyle LabelBoldSmall { get; private set; }
		public static GUIStyle LabelCenteredSmall { get; private set; }
		public static GUIStyle LabelBoldCenteredSmall { get; private set; }
		public static GUIStyle LabelBackground { get; private set; }
		public static GUIStyle LabelBoldBackground { get; private set; }
		public static GUIStyle LabelCenteredBackground { get; private set; }
		public static GUIStyle LabelBoldCenteredBackground { get; private set; }
		public static GUIStyle LabelBigBackground { get; private set; }
		public static GUIStyle LabelBoldBigBackground { get; private set; }
		public static GUIStyle LabelCenteredBigBackground { get; private set; }
		public static GUIStyle LabelBoldCenteredBigBackground { get; private set; }
		public static GUIStyle LabelSmallBackground { get; private set; }
		public static GUIStyle LabelBoldSmallBackground { get; private set; }
		public static GUIStyle LabelCenteredSmallBackground { get; private set; }
		public static GUIStyle LabelBoldCenteredSmallBackground { get; private set; }

		public static GUIStyle Checker => null;

		public static GUIStyle Button { get; private set; }
		public static GUIStyle ButtonBig { get; private set; }
		public static GUIStyle ButtonBold { get; private set; }
		public static GUIStyle ButtonBigBold { get; private set; }
		public static GUIStyle ButtonImageOnly { get; private set; }
		public static GUIStyle ButtonPressed { get; private set; }
		public static GUIStyle ButtonPressedBig { get; private set; }
		public static GUIStyle ButtonPressedBold { get; private set; }
		public static GUIStyle ButtonPressedBigBold { get; private set; }
		public static GUIStyle ButtonPressedImageOnly { get; private set; }
		public static GUIStyle ButtonNoHover { get; private set; }
		public static GUIStyle ButtonNoBorder { get; private set; }
		public static GUIStyle ButtonBigNoBorder { get; private set; }
		public static GUIStyle ButtonBoldNoBorder { get; private set; }
		public static GUIStyle ButtonBigBoldNoBorder { get; private set; }
		public static GUIStyle ButtonImageOnlyNoBorder { get; private set; }
		public static GUIStyle ButtonPressedNoBorder { get; private set; }
		public static GUIStyle ButtonPressedBigNoBorder { get; private set; }
		public static GUIStyle ButtonPressedBoldNoBorder { get; private set; }
		public static GUIStyle ButtonPressedBigBoldNoBorder { get; private set; }
		public static GUIStyle ButtonPressedImageOnlyNoBorder { get; private set; }
		public static GUIStyle ButtonNoHoverNoBorder { get; private set; }

		public static GUIStyle EmptyMiddleAligned { get; private set; }
		public static GUIStyle EmptyMiddleAlignedTop { get; private set; }

		public static GUIStyle TitleStyle { get; private set; }
		public static GUIStyle HeaderStyle { get; private set; }
		public static GUIStyle CenteredTextStyle { get; private set; }
		public static GUIStyle IconStyle { get; private set; }

		public static GUIStyle ButtonLeftActive { get; private set; }
		public static GUIStyle ButtonMidActive { get; private set; }
		public static GUIStyle ButtonRightActive { get; private set; }

		public static GUIStyle ButtonLeftInactive { get; private set; }
		public static GUIStyle ButtonMidInactive { get; private set; }
		public static GUIStyle ButtonRightInactive { get; private set; }

		public static GUIStyle ToggleButtonInactive { get; private set; }
		public static GUIStyle ToggleButtonActive { get; private set; }

		public static GUIStyle Spacer { get; private set; }

		public static GUIResources Resources { get; private set; }

	#endregion

	#region Constructor

		/// <summary>
		/// Constructor
		/// </summary>
		public static IEnumerator Rebuild()
		{
			AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>("TunaTK/Core/GUIResources");
			
			yield return handle;

			Resources = handle.Result.GetComponent<GUIResources>();

			TitleStyle = new GUIStyle(Resources.Skin.label)
			{
				fontStyle = FontStyle.Bold,
				fontSize = 25,
				alignment = TextAnchor.MiddleCenter
			};

			HeaderStyle = new GUIStyle(TitleStyle)
			{
				fontSize = 20
			};

			CenteredTextStyle = new GUIStyle(Resources.Skin.label)
			{
				alignment = TextAnchor.MiddleCenter,
				fontSize = 10,
				fontStyle = FontStyle.Normal,
				stretchWidth = true
			};

			IconStyle = new GUIStyle(Resources.Skin.label)
			{
				alignment = TextAnchor.LowerCenter
			};

			Background = new GUIStyle
			{
				alignment = TextAnchor.MiddleCenter,
				border = new RectOffset(2, 2, 2, 2),
				imagePosition = ImagePosition.ImageOnly,
				name = NAME_BASE + "Background",
				normal =
				{
					background = Resources.Skin.box.normal.background,
					textColor = fontColor
				},
				padding = defaultPaddingRectOffset
			};

			BackgroundNoBorder = new GUIStyle(Background)
			{
				padding = new RectOffset(0, 0, 0, 0),
				name = Background.name + "NoBorder"
			};

			Label = new GUIStyle
			{
				alignment = TextAnchor.MiddleLeft,
				name = NAME_BASE + "Label",
				normal =
				{
					textColor = fontColor
				},
				padding = new RectOffset(3, 3, 3, 3),
				richText = true,
				wordWrap = true
			};

			LabelBold = new GUIStyle(Label)
			{
				fontStyle = FontStyle.Bold,
				name = Label.name + "Bold"
			};

			LabelCentered = new GUIStyle(Label)
			{
				alignment = TextAnchor.UpperCenter,
				name = Label.name + "Centered"
			};

			LabelBoldCentered = new GUIStyle(LabelBold)
			{
				alignment = LabelCentered.alignment,
				name = Label.name + "BoldCentered"
			};

			LabelBig = new GUIStyle(Label)
			{
				fontSize = FONT_BIG_SIZE,
				name = Label.name + "Big"
			};

			LabelBoldBig = new GUIStyle(LabelBold)
			{
				fontSize = LabelBig.fontSize,
				name = LabelBold.name + "Big"
			};

			LabelCenteredBig = new GUIStyle(LabelCentered)
			{
				fontSize = LabelBig.fontSize,
				name = LabelCentered.name + "Big"
			};

			LabelBoldCenteredBig = new GUIStyle(LabelBoldCentered)
			{
				fontSize = LabelBig.fontSize,
				name = LabelBoldCentered.name + "Big"
			};

			LabelSmall = new GUIStyle(Label)
			{
				fontSize = FONT_SMALL_SIZE,
				name = Label.name + "Small"
			};

			LabelBoldSmall = new GUIStyle(LabelBold)
			{
				fontSize = LabelSmall.fontSize,
				name = LabelBold.name + "Small"
			};

			LabelCenteredSmall = new GUIStyle(LabelCentered)
			{
				fontSize = LabelSmall.fontSize,
				name = LabelCentered.name + "Small"
			};

			LabelBoldCenteredSmall = new GUIStyle(LabelBoldCentered)
			{
				fontSize = LabelSmall.fontSize,
				name = LabelBoldCentered.name + "Small"
			};

			LabelBackground = new GUIStyle(Label)
			{
				border = new RectOffset(1, 1, 1, 1),
				normal =
				{
					background = Resources.Skin.label.normal.background
				},
				padding = defaultPaddingRectOffset,
				name = Label.name + "Background"
			};

			LabelBoldBackground = new GUIStyle(LabelBold)
			{
				border = LabelBackground.border,
				normal =
				{
					background = LabelBackground.normal.background
				},
				padding = defaultPaddingRectOffset,
				name = LabelBold.name + "Background"
			};

			LabelCenteredBackground = new GUIStyle(LabelCentered)
			{
				border = LabelBackground.border,
				normal =
				{
					background = LabelBackground.normal.background
				},
				padding = defaultPaddingRectOffset,
				name = LabelCentered.name + "Background"
			};

			LabelBoldCenteredBackground = new GUIStyle(LabelBoldCentered)
			{
				border = LabelBackground.border,
				normal =
				{
					background = LabelBackground.normal.background
				},
				padding = defaultPaddingRectOffset,
				name = LabelBoldCentered.name + "Background"
			};

			LabelBigBackground = new GUIStyle(LabelBig)
			{
				border = LabelBackground.border,
				normal =
				{
					background = LabelBackground.normal.background
				},
				padding = defaultPaddingRectOffset,
				name = LabelBig.name + "Background"
			};

			LabelBoldBigBackground = new GUIStyle(LabelBoldBig)
			{
				border = LabelBackground.border,
				normal =
				{
					background = LabelBackground.normal.background
				},
				padding = defaultPaddingRectOffset,
				name = LabelBoldBig.name + "Background"
			};

			LabelCenteredBigBackground = new GUIStyle(LabelCenteredBig)
			{
				border = LabelBackground.border,
				normal =
				{
					background = LabelBackground.normal.background
				},
				padding = defaultPaddingRectOffset,
				name = LabelCenteredBig.name + "Background"
			};

			LabelBoldCenteredBigBackground = new GUIStyle(LabelBoldCenteredBig)
			{
				border = LabelBackground.border,
				normal =
				{
					background = LabelBackground.normal.background
				},
				padding = defaultPaddingRectOffset,
				name = LabelBoldCenteredBig.name + "Background"
			};

			LabelSmallBackground = new GUIStyle(LabelSmall)
			{
				border = LabelBackground.border,
				normal =
				{
					background = LabelBackground.normal.background
				},
				padding = defaultPaddingRectOffset,
				name = LabelSmall.name + "Background"
			};

			LabelBoldSmallBackground = new GUIStyle(LabelBoldSmall)
			{
				border = LabelBackground.border,
				normal =
				{
					background = LabelBackground.normal.background
				},
				padding = defaultPaddingRectOffset,
				name = LabelBoldSmall.name + "Background"
			};

			LabelCenteredSmallBackground = new GUIStyle(LabelCenteredSmall)
			{
				border = LabelBackground.border,
				normal =
				{
					background = LabelBackground.normal.background
				},
				padding = defaultPaddingRectOffset,
				name = LabelCenteredSmall.name + "Background"
			};

			LabelBoldCenteredSmallBackground = new GUIStyle(LabelBoldCenteredSmall)
			{
				border = LabelBackground.border,
				normal =
				{
					background = LabelBackground.normal.background
				},
				padding = defaultPaddingRectOffset,
				name = LabelBoldCenteredSmall.name + "Background"
			};

			Button = new GUIStyle
			{
				alignment = TextAnchor.MiddleCenter,
				border = new RectOffset(1, 1, 1, 1),
				name = NAME_BASE + "Button",
				padding = defaultPaddingRectOffset,
				normal =
				{
					background = Resources.Skin.button.normal.background,
					textColor = fontColor
				}
			};
			Button.onNormal.background = Button.onNormal.background;
			Button.onNormal.textColor = Button.normal.textColor;
			Button.active.background = Resources.Skin.button.active.background;
			Button.active.textColor = Button.normal.textColor;
			Button.onActive.background = Button.active.background;
			Button.onActive.textColor = Button.normal.textColor;
			Button.focused.background = Resources.Skin.button.focused.background;
			Button.focused.textColor = Button.normal.textColor;
			Button.onFocused.background = Button.focused.background;
			Button.onFocused.textColor = Button.normal.textColor;
			Button.hover.background = Resources.Skin.button.hover.background;
			Button.hover.textColor = Button.normal.textColor;
			Button.onHover.background = Button.hover.background;
			Button.onHover.textColor = Button.normal.textColor;

			ButtonBig = new GUIStyle(Button)
			{
				fontSize = FONT_BIG_SIZE,
				name = Button.name + "Big"
			};

			ButtonBold = new GUIStyle(Button)
			{
				fontStyle = FontStyle.Bold,
				name = Button.name + "Bold"
			};

			ButtonBigBold = new GUIStyle(Button)
			{
				fontSize = ButtonBig.fontSize,
				fontStyle = ButtonBold.fontStyle,
				name = Button.name + "BigBold"
			};

			ButtonImageOnly = new GUIStyle(Button)
			{
				imagePosition = ImagePosition.ImageOnly,
				name = Button.name + "ImageOnly"
			};

			ButtonPressed = new GUIStyle(Button)
			{
				name = $"{Button.name} + Pressed",
				normal =
				{
					background = Resources.Skin.button.active.background
				}
			};
			ButtonPressed.onNormal.background = ButtonPressed.normal.background;

			ButtonPressedBig = new GUIStyle(ButtonPressed)
			{
				fontSize = FONT_BIG_SIZE,
				name = ButtonPressed.name + "Big"
			};

			ButtonPressedBold = new GUIStyle(ButtonPressed)
			{
				fontStyle = FontStyle.Bold,
				name = ButtonPressed.name + "Bold"
			};

			ButtonPressedBigBold = new GUIStyle(ButtonPressed)
			{
				fontSize = ButtonBig.fontSize,
				fontStyle = ButtonBold.fontStyle,
				name = ButtonPressed.name + "BigBold"
			};

			ButtonPressedImageOnly = new GUIStyle(ButtonPressed)
			{
				imagePosition = ImagePosition.ImageOnly,
				name = Button.name + "PressedImageOnly"
			};

			ButtonNoHover = new GUIStyle
			{
				alignment = Button.alignment,
				border = Button.border,
				imagePosition = Button.imagePosition,
				name = Button.name + "NoHover",
				active =
				{
					background = Button.active.background,
					textColor = Button.active.textColor
				},
				onActive =
				{
					background = Button.active.background,
					textColor = Button.active.textColor
				},
				normal =
				{
					background = Button.normal.background,
					textColor = Button.normal.textColor
				},
				onNormal =
				{
					background = Button.normal.background,
					textColor = Button.normal.textColor
				},
				padding = Button.padding
			};

			ButtonNoBorder = new GUIStyle(Button)
			{
				padding = new RectOffset(0, 0, 0, 0),
				name = Button.name + "NoBorder"
			};

			ButtonBigNoBorder = new GUIStyle(ButtonBig)
			{
				padding = new RectOffset(0, 0, 0, 0),
				name = ButtonBig.name + "NoBorder"
			};

			ButtonBoldNoBorder = new GUIStyle(ButtonBold)
			{
				padding = new RectOffset(0, 0, 0, 0),
				name = ButtonBold.name + "NoBorder"
			};

			ButtonBigBoldNoBorder = new GUIStyle(ButtonBigBold)
			{
				padding = new RectOffset(0, 0, 0, 0),
				name = ButtonBigBold.name + "NoBorder"
			};

			ButtonImageOnlyNoBorder = new GUIStyle(ButtonImageOnly)
			{
				padding = new RectOffset(0, 0, 0, 0),
				name = ButtonImageOnly.name + "NoBorder"
			};

			ButtonPressedNoBorder = new GUIStyle(ButtonPressed)
			{
				padding = new RectOffset(0, 0, 0, 0),
				name = ButtonPressed.name + "NoBorder"
			};

			ButtonPressedBigNoBorder = new GUIStyle(ButtonPressedBig)
			{
				padding = new RectOffset(0, 0, 0, 0),
				name = ButtonPressedBig.name + "NoBorder"
			};

			ButtonPressedBoldNoBorder = new GUIStyle(ButtonPressedBold)
			{
				padding = new RectOffset(0, 0, 0, 0),
				name = ButtonPressedBold.name + "NoBorder"
			};

			ButtonPressedBigBoldNoBorder = new GUIStyle(ButtonPressedBigBold)
			{
				padding = new RectOffset(0, 0, 0, 0),
				name = ButtonPressedBigBold.name + "NoBorder"
			};

			ButtonPressedImageOnlyNoBorder = new GUIStyle(ButtonPressedImageOnly)
			{
				padding = new RectOffset(0, 0, 0, 0),
				name = ButtonPressedImageOnly.name + "NoBorder"
			};

			ButtonNoHoverNoBorder = new GUIStyle(ButtonNoHover)
			{
				padding = new RectOffset(0, 0, 0, 0),
				name = ButtonNoHover.name + "NoBorder"
			};

			EmptyMiddleAligned = new GUIStyle
			{
				alignment = TextAnchor.MiddleCenter,
				imagePosition = ImagePosition.ImageOnly,
				name = NAME_BASE + "EmptyMiddleAligned",
				padding = new RectOffset(0, 0, 0, 0)
			};

			EmptyMiddleAlignedTop = new GUIStyle(EmptyMiddleAligned)
			{
				alignment = TextAnchor.UpperCenter,
				name = EmptyMiddleAligned.name + "Top"
			};

			ButtonLeftInactive = new GUIStyle("ButtonLeft");
			ButtonLeftActive = new GUIStyle(ButtonLeftInactive);
			ButtonLeftActive.normal.background = ButtonLeftActive.active.background;

			ButtonLeftInactive = new GUIStyle("ButtonLeft");
			ButtonLeftActive = new GUIStyle(ButtonLeftInactive);
			ButtonLeftActive.normal.background = ButtonLeftActive.active.background;

			ButtonMidInactive = new GUIStyle("ButtonMid");
			ButtonMidActive = new GUIStyle(ButtonMidInactive);
			ButtonMidActive.normal.background = ButtonMidActive.active.background;

			ButtonRightInactive = new GUIStyle("ButtonRight");
			ButtonRightActive = new GUIStyle(ButtonRightInactive);
			ButtonRightActive.normal.background = ButtonRightActive.active.background;

			ToggleButtonInactive = new GUIStyle("Button");
			ToggleButtonActive = new GUIStyle(ToggleButtonInactive)
			{
				normal =
				{
					background = ToggleButtonInactive.active.background
				}
			};

			Spacer = new GUIStyle(FindCustomStyle("Spacer"));
		}

		private static GUIStyle FindCustomStyle(string _styleName) => Resources.Skin.customStyles.FirstOrDefault(_style => _style.name == _styleName);

	#endregion
	}
}
#endif