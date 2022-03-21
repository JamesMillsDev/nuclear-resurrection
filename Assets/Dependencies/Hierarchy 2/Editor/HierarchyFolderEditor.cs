using UnityEditor;

using UnityEngine;
using UnityEngine.UIElements;

namespace Hierarchy2
{
	[CustomEditor(typeof(HierarchyFolder))]
	internal class HierarchyFolderEditor : Editor
	{
		public override VisualElement CreateInspectorGUI()
		{
			HierarchyFolder script = target as HierarchyFolder;

			VisualElement root = new VisualElement();

			if(script != null)
			{
				IMGUIContainer imguiContainer = new IMGUIContainer(() =>
				{
					script.flattenMode = (HierarchyFolder.FlattenMode) EditorGUILayout.EnumPopup("Flatten Mode", script.flattenMode);
					if(script.flattenMode != HierarchyFolder.FlattenMode.None)
					{
						script.flattenSpace = (HierarchyFolder.FlattenSpace) EditorGUILayout.EnumPopup("Flatten Space", script.flattenSpace);
						script.destroyAfterFlatten = EditorGUILayout.Toggle("Destroy After Flatten", script.destroyAfterFlatten);
					}
				});
				root.Add(imguiContainer);
			}

			return root;
		}

		[MenuItem("GameObject/Hierarchy 2/Hierarchy Folder", priority = 100)]
		private static void CreateInstance(MenuCommand _command)
		{
			GameObject gameObject = new GameObject("Folder", typeof(HierarchyFolder))
			{
				transform =
				{
					parent = Selection.activeTransform
				}
			};

			Undo.RegisterCreatedObjectUndo(gameObject, "Create Hierarchy Folder");
			if(_command.context)
				Undo.SetTransformParent(gameObject.transform, ((GameObject) _command.context).transform, "Create Hierarchy Folder");

			Selection.activeTransform = gameObject.transform;
		}
	}
}