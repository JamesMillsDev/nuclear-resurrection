// Property of TUNACORN STUDIOS PTY LTD 2018
// 
// Creator: James Mills
// Creation Time: 08/03/2022 12:30 PM

using UnityEditor;

namespace TunaTK
{
	public class StandardShaderEditor : ShaderGUI
	{
		public override void OnGUI(MaterialEditor _materialEditor, MaterialProperty[] _properties)
		{
			MaterialProperty color = _properties[0];
			MaterialProperty mainTexture = _properties[1];
			MaterialProperty normalMap = _properties[2];
			MaterialProperty metallicMap = _properties[3];
			MaterialProperty smoothness = _properties[4];
			MaterialProperty metallic = _properties[5];
			
		}
	}
}