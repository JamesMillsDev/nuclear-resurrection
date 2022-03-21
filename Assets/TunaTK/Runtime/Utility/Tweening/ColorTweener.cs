// Property of TUNACORN STUDIOS PTY LTD 2018
//
// Creator: James Mills
// Creation Time: 30/10/2019 10:03 AM
// Created On: CHRONOS

using UnityEngine;

namespace TunaTK.Utilities
{
	public class ColorTweener : BaseTweener 
	{
        private MeshRenderer renderer;
        private Color startColor;
        private Color endColor;

		public ColorTweener(MeshRenderer _renderer, Color _start, Color _end)
		{
            renderer = _renderer;
            startColor = _start;
            endColor = _end;
		}

        public override void Tween(float _factor)
        {
            renderer.material.color = Color.Lerp(startColor, endColor, _factor);
        }
    }
}