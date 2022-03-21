#if UNITY_EDITOR
// Property of TUNACORN STUDIOS PTY LTD 2018
//
// Creator: James Mills
// Creation Time: 24/11/2019 09:20 PM
// Created On: CHRONOS

using JetBrains.Annotations;

using Sirenix.OdinInspector.Editor;

using System;

namespace TunaTK
{
	public class RenderableSubProperty
	{
		public InspectorProperty property;
		[CanBeNull] public Action<InspectorProperty> renderFunction;

		public RenderableSubProperty(InspectorProperty _property, Action<InspectorProperty> _renderFunction)
		{
			property = _property;
			renderFunction = _renderFunction;
		}
	}
}
#endif