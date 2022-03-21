#if UNITY_EDITOR
// Property of TUNACORN STUDIOS PTY LTD 2018
//
// Creator: James Mills
// Creation Time: 17/04/2019 06:57 PM
// Created On: CHRONOS

using Sirenix.OdinInspector.Editor;

using System.Linq;

using UnityEditor;
using UnityEditor.AnimatedValues;

using UnityEngine;

namespace TunaTK
{
	[CustomEditor(typeof(InitialiseBehaviour), true)]
	public class InitialiseBehaviourEditor : OdinEditor
	{
		private SerializedProperty pScript;
		private SerializedProperty pAutoInitialise;
		private SerializedProperty pInitialisationStage;

		private readonly AnimBool showInitStage = new AnimBool();
		private bool drawExtraProperties = false;

		private string[] propertyPathToExcludeForChildClasses;

		// Use this for initialization
		protected override void OnEnable()
		{
			pScript = serializedObject.FindProperty("m_Script");
			pAutoInitialise = serializedObject.FindProperty("autoInitialise");
			pInitialisationStage = serializedObject.FindProperty("initialisationMethod");

			showInitStage.value = pAutoInitialise.boolValue;
			showInitStage.valueChanged.AddListener(Repaint);

			propertyPathToExcludeForChildClasses = new[]
			{
				pScript.propertyPath,
				pAutoInitialise.propertyPath,
				pInitialisationStage.propertyPath
			};

			drawExtraProperties = EditorHelper.GetPropertyCountExcluding(Tree, propertyPathToExcludeForChildClasses) > 0;

			OnInit();
		}

		/// <summary>
		/// Forces the drawing of extra properties for child editors... MUST be used in OnInit
		/// </summary>
		protected void EnableExtraPropsDrawing() => drawExtraProperties = true;

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
				showInitStage.target = pAutoInitialise.boolValue == true;

				if(EditorGUILayout.BeginFadeGroup(showInitStage.faded))
				{
					EditorGUILayout.PropertyField(pInitialisationStage);
				}

				EditorGUILayout.EndFadeGroup();
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

		protected virtual void ChildClassPropertiesGUI() => RenderPropertiesExcluding(Tree, propertyPathToExcludeForChildClasses);

		private void RenderPropertiesExcluding(PropertyTree _tree, params string[] _propertyToExclude)
		{
			InspectorProperty property = _tree.GetRootProperty(0);

			while(property != null)
			{
				if(!_propertyToExclude.Contains(property.Name))
				{
					property.Draw();
				}

				property = property.NextProperty(false);
			}
		}
	}
}
#endif