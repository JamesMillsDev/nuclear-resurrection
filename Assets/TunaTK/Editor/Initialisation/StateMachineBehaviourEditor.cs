#if UNITY_EDITOR
// Property of TUNACORN STUDIOS PTY LTD 2018
//
// Creator: James Mills
// Creation Time: 17/04/2019 06:57 PM
// Created On: CHRONOS

using Sirenix.OdinInspector.Editor;

using System.Linq;

using UnityEditor;

using UnityEngine;

namespace TunaTK
{
	[CustomEditor(typeof(InitialisableStateMachineBehaviour), true)]
	public class InitialisableStateMachineBehaviourEditor : OdinEditor
	{
		private SerializedProperty pScript;
		private SerializedProperty pAutoInitialise;

		private bool drawExtraProperties = false;

		private string[] propertyPathToExcludeForChildClasses;

		// Use this for initialization
		protected override void OnEnable()
		{
			pScript = serializedObject.FindProperty("m_Script");
			pAutoInitialise = serializedObject.FindProperty("autoInitialise");

			propertyPathToExcludeForChildClasses = new[]
			{
				pScript.propertyPath,
				pAutoInitialise.propertyPath
			};

			drawExtraProperties = EditorHelper.GetPropertyCountExcluding(serializedObject, propertyPathToExcludeForChildClasses) > 0;

			OnInit();
		}

		/// <summary>
		/// Forces the drawing of extra properties for child editors... MUST be used in OnInit
		/// </summary>
		protected void EnableExtraPropsDrawing()
		{
			drawExtraProperties = true;
		}

		protected virtual void OnInit() { }

		// Draw the custom inspector
		protected override void DrawTree()
		{
			Tree.DrawMonoScriptObjectField = false;

			Tree.BeginDraw(true);

			serializedObject.Update();

			EditorGUILayout.BeginVertical(GUI.skin.box);
			{
				EditorGUILayout.PropertyField(pAutoInitialise);
			}
			EditorGUILayout.EndVertical();

			if(drawExtraProperties)
			{
				EditorGUILayout.BeginVertical(GUI.skin.box);
				{
					ChildClassPropertiesGUI();
				}
				EditorGUILayout.EndVertical();
			}

			serializedObject.ApplyModifiedProperties();

			Tree.EndDraw();
		}

		protected virtual void ChildClassPropertiesGUI() => RenderPropertiesExcluding(this.Tree, propertyPathToExcludeForChildClasses);

		private void RenderPropertiesExcluding(PropertyTree _tree, params string[] _propertyToExclude)
		{
			InspectorProperty property = _tree.GetRootProperty(0);

			while(property != null)
			{
				if(!_propertyToExclude.Contains(property.Name))
					property.Draw();

				property = property.NextProperty(false);
			}
		}
	}
}
#endif