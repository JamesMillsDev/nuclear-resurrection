// Property of TUNACORN STUDIOS PTY LTD 2018
//
// Creator: James Mills
// Creation Time: 24/05/2021 12:16 PM
// Created On: NS00J002003DE01

using UnityEngine;

namespace TunaTK.Utility
{
	public static class GizmoUtils
	{
		public static void DrawCylinder(float _height, Transform _transform, Color _color, float _radius, bool _fromBase = false)
		{
			DrawCircle(!_fromBase ? _height * .5f : _height, _transform, _color, _radius);
			DrawCircle(!_fromBase ? -(_height * .5f) : 0, _transform, _color, _radius);
			Matrix4x4 original = Gizmos.matrix;
			Gizmos.matrix = _transform.localToWorldMatrix;
			Gizmos.color = _color;

			for(float theta = 0; theta < Mathf.PI * 2; theta += Mathf.PI * .5f)
			{
				Vector3 basePos = new Vector3(Mathf.Cos(theta) * _radius, 0, Mathf.Sin(theta) * _radius);

				Gizmos.DrawLine(basePos + (Vector3.up * (_fromBase ? 0 : -(_height * .5f))), basePos + (Vector3.up * (_fromBase ? _height : _height * .5f)));
			}

			Gizmos.matrix = original;
		}

		public static void DrawCircle(float _height, Transform _transform, Color _color, float _radius)
		{
			Gizmos.color = _color;

			Matrix4x4 original = Gizmos.matrix;
			Gizmos.matrix = _transform.localToWorldMatrix;

			float theta = 0;
			float x = Mathf.Cos(theta) * _radius;
			float y = Mathf.Sin(theta) * _radius;
			Vector3 pos = new Vector3(x, _height, y);
			Vector3 lastPos = pos;

			for(theta = 0; theta < Mathf.PI * 2; theta += .1f)
			{
				x = Mathf.Cos(theta) * _radius;
				y = Mathf.Sin(theta) * _radius;
				Vector3 newPos = new Vector3(x, _height, y);

				Gizmos.DrawLine(pos, newPos);
				pos = newPos;
			}

			Gizmos.DrawLine(pos, lastPos);

			Gizmos.matrix = original;
		}
	}
}