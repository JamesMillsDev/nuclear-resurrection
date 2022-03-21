// Property of TUNACORN STUDIOS PTY LTD 2018
// 
// Creator: James Mills
// Creation Time: 13/03/2022 12:21 PM

using Sirenix.OdinInspector.Editor;

using System.Collections.Generic;
using System.Threading;

using UnityEditor;

using UnityEngine;

namespace TunaTK.Threading
{
	[CustomEditor(typeof(ThreadDispatcher))]
	public class ThreadDispatcherEditor : OdinEditor
	{
		private InspectorProperty pRunners;
		private InspectorProperty pThreads;
		
		protected override void OnEnable()
		{
			base.OnEnable();
			
			pRunners = Tree.GetPropertyAtPath("runners");
			pThreads = Tree.GetPropertyAtPath("threads");
		}

		protected override void DrawTree()
		{
			Tree.DrawMonoScriptObjectField = false;
			
			serializedObject.Update();
			
			Tree.BeginDraw(true);

			EditorGUILayout.BeginVertical(GUI.skin.box);
			{
				// ReSharper disable once SuspiciousTypeConversion.Global
				// ReSharper disable once PossibleNullReferenceException
				EditorGUILayout.LabelField($"Alive Thread Count: {((pThreads.ValueEntry.WeakSmartValue) as List<Thread>).Count}");
				pRunners.Draw();
			}
			EditorGUILayout.EndVertical();

			Tree.EndDraw();

			serializedObject.ApplyModifiedProperties();
		}
	}
}