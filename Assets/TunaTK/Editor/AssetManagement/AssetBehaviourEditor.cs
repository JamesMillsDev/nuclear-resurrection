// Property of TUNACORN STUDIOS PTY LTD 2018
// 
// Creator: James Mills
// Creation Time: 28/02/2022 11:36 AM

using System;
using System.Linq;
using System.Collections;

using TunaTK.AssetManagement;

using Unity.EditorCoroutines.Editor;

using UnityEditor;

using UnityEngine;

using InspectorProperty = Sirenix.OdinInspector.Editor.InspectorProperty;
using OdinEditor = Sirenix.OdinInspector.Editor.OdinEditor;
using PropertyTree = Sirenix.OdinInspector.Editor.PropertyTree;

namespace TunaTK.AssetManagement
{
	[CustomEditor(typeof(AssetBehaviour), true), CanEditMultipleObjects]
	public class AssetBehaviourEditor : OdinEditor
	{
		private SerializedProperty pScript;
		private InspectorProperty pGuid;

		private bool drawExtraProperties;

		private string[] propertyPathToExcludeForChildClasses;

		// Use this for initialization
		protected override void OnEnable()
		{
			EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindowItemOnGui;
			
			pScript = serializedObject.FindProperty("m_Script");
			pGuid = Tree.GetPropertyAtPath("guid");

			propertyPathToExcludeForChildClasses = new[]
			{
				pScript.propertyPath,
				pGuid.UnityPropertyPath
			};

			drawExtraProperties = EditorHelper.GetPropertyCountExcluding(Tree, propertyPathToExcludeForChildClasses) > 0;

			OnInit();
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			
			EditorApplication.hierarchyWindowItemOnGUI -= OnHierarchyWindowItemOnGui;
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
				if(string.IsNullOrEmpty((string) pGuid.ValueEntry.WeakSmartValue))
					pGuid.ValueEntry.WeakSmartValue = Guid.NewGuid().ToString();

				pGuid.Draw();
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

		[MenuItem("CONTEXT/AssetBehaviour/TunaTK/Regenerate GUID")]
		private static void RegenerateGuid(MenuCommand _command)
		{
			IAssetBehaviour behaviour = (IAssetBehaviour) _command.context;
			behaviour.Guid = behaviour.RegenerateGuid();
		}
		
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

		private void OnHierarchyWindowItemOnGui(int _instanceId, Rect _rect)
		{
			if(Event.current.commandName == "Duplicate")
				EditorCoroutineUtility.StartCoroutine(GuidRegenerate_Async(), this);
		}

		private static IEnumerator GuidRegenerate_Async()
		{
			yield return new WaitForSeconds(.1f);
			
			IAssetBehaviour[] behaviours = Selection.activeGameObject.GetComponents<IAssetBehaviour>();
			foreach(IAssetBehaviour behaviour in behaviours)
				behaviour.RegenerateGuid();
		}
	}
}