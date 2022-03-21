// Property of TUNACORN STUDIOS PTY LTD 2018
// 
// Creator: James Mills
// Creation Time: 28/02/2022 11:26 AM

using UnityEngine;

namespace TunaTK.Cursors
{
	/// <summary>
	/// Small helper class to convert viewport, screen or world positions to canvas space.
	/// Only works with screen space canvases.
	/// </summary>
	/// <example>
	/// <code>
	/// objectOnCanvasRectTransform.anchoredPosition = specificCanvas.WorldToCanvasPoint(worldspaceTransform.position);
	/// </code>
	/// </example>
	public static class CanvasPositioningExtensions
	{
		public static Vector3 WorldToCanvasPosition(this Canvas _canvas, Vector3 _worldPosition, Camera _camera = null)
		{
			if(_camera == null)
			{
				_camera = Camera.main;
			}

			if(_camera != null)
			{
				Vector3 viewportPosition = _camera.WorldToViewportPoint(_worldPosition);
				return _canvas.ViewportToCanvasPosition(viewportPosition);
			}
		
			return Vector3.zero;
		}

		public static Vector3 ScreenToCanvasPosition(this Canvas _canvas, Vector3 _screenPosition)
		{
			Vector3 viewportPosition = new Vector3(_screenPosition.x / Screen.width,
				_screenPosition.y / Screen.height,
				0);
			return _canvas.ViewportToCanvasPosition(viewportPosition);
		}

		public static Vector3 ViewportToCanvasPosition(this Canvas _canvas, Vector3 _viewportPosition)
		{
			Vector3 centerBasedViewPortPosition = _viewportPosition - new Vector3(0.5f, 0.5f, 0);
			RectTransform canvasRect = _canvas.GetComponent<RectTransform>();
			Vector2 scale = canvasRect.sizeDelta;
			return Vector3.Scale(centerBasedViewPortPosition, scale);
		}
	}
}