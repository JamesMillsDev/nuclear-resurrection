// Property of TUNACORN STUDIOS PTY LTD 2018
//
// Creator: James Mills
// Creation Time: 08/01/2019 02:28 PM
// Created On: CHRONOS

using System;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TunaTK.Utility
{
    /// <summary>Contains a bunch of useful commonly created functions that can be used anywhere and everywhere.</summary>
    public static class SharedMethods
    {
#if UNITY_EDITOR
        /// <summary>Draws a wireframe capsule at the given location and rotation. This will look the same as the CapsuleCollider handle in the scene view.</summary>
        /// <param name="_position">The position to draw the capsule at.</param>
        /// <param name="_rotation">The rotation to draw the capsule.</param>
        /// <param name="_radius">The radius of the capsule.</param>
        /// <param name="_height">The height of the capsule.</param>
        /// <param name="_color">The color of the capsule.</param>
        public static void DrawWireCapsule(Vector3 _position, Quaternion _rotation, float _radius, float _height, Color _color = default(Color))
        {
            if(_color != default(Color))
                Handles.color = _color;

            Matrix4x4 angleMatrix = Matrix4x4.TRS(_position, _rotation, Handles.matrix.lossyScale);
            using(new Handles.DrawingScope(angleMatrix))
            {
                float pointOffset = (_height - (_radius * 2)) / 2;

                //draw sideways
                Handles.DrawWireArc(Vector3.up * pointOffset, Vector3.left, Vector3.back, -180, _radius);
                Handles.DrawLine(new Vector3(0, pointOffset, -_radius), new Vector3(0, -pointOffset, -_radius));
                Handles.DrawLine(new Vector3(0, pointOffset, _radius), new Vector3(0, -pointOffset, _radius));
                Handles.DrawWireArc(Vector3.down * pointOffset, Vector3.left, Vector3.back, 180, _radius);
                //draw frontways
                Handles.DrawWireArc(Vector3.up * pointOffset, Vector3.back, Vector3.left, 180, _radius);
                Handles.DrawLine(new Vector3(-_radius, pointOffset, 0), new Vector3(-_radius, -pointOffset, 0));
                Handles.DrawLine(new Vector3(_radius, pointOffset, 0), new Vector3(_radius, -pointOffset, 0));
                Handles.DrawWireArc(Vector3.down * pointOffset, Vector3.back, Vector3.left, -180, _radius);
                //draw center
                Handles.DrawWireDisc(Vector3.up * pointOffset, Vector3.up, _radius);
                Handles.DrawWireDisc(Vector3.down * pointOffset, Vector3.up, _radius);

            }
        }
#endif

        /// <summary>Compares two Vector2 variables two a certain decimal place count, the decimal places, the higher the acuracy.</summary>
        /// <param name="_first">The left-hand-side of the comparison operation.</param>
        /// <param name="_second">The right-hand-side of the comparison operation.</param>
        /// <param name="_fidelity">The amount of decimal places the comparison will be to.</param>
        public static bool Vector2Compare(Vector2 _first, Vector2 _second, int _fidelity)
        {
            Vector2 distance = _first - _second;
            return (Math.Round(Mathf.Abs(distance.x), _fidelity, MidpointRounding.AwayFromZero) < float.Epsilon && Math.Round(Mathf.Abs(distance.y), _fidelity, MidpointRounding.AwayFromZero) < float.Epsilon);
        }

        /// <summary>Validates the component is attached to the object by attempting to Get it, if failed, Add it.</summary>
        /// <typeparam name="T">The type of component to be validated.</typeparam>
        /// <param name="_gameObject">The object that the component is being validated on.</param>
        public static T ValidateComponent<T>(this GameObject _gameObject, bool _checkChildren = false) where T : Component
        {
            T component = _gameObject.GetComponent<T>();
            if(component != null)
                return component;

            if(_checkChildren)
                component = _gameObject.GetComponentInChildren<T>();

            if(component == null)
                component = _gameObject.AddComponent<T>();

            return component;
        }

        /// <summary></summary>
        /// <param name="_first"></param>
        /// <param name="_second"></param>
        public static float Mod(float _first, float _second) => _first - _second * Mathf.Floor(_first / _second);

        /// <summary>This will round a value to an int. This will round away from zero, as rounding normally works</summary>
        /// <param name="_toRound">The number to round.</param>
        public static int RoundToInt(float _toRound) => (int)Math.Round(_toRound, MidpointRounding.AwayFromZero);

        /// <summary>Reamaps the first value from aFromMin-aFromMax to aToMin-aToMax.\nie:Remap 2 from 1-3 to 0-10 gives 5</summary>
        /// <param name="_value">The value to be remapped to the new range</param>
        /// <param name="_fromMin">The minimum value of the from range.</param>
        /// <param name="_fromMax">The maximum value of the from range.</param>
        /// <param name="_toMin">The minimum value of the to range.</param>
        /// <param name="_toMax">The maximum value of the to range.</param>
        public static float Remap(float _value, float _fromMin, float _fromMax, float _toMin, float _toMax)
        {
            float fromAbs = _value - _fromMin;
            float fromMaxAbs = _fromMax - _fromMin;

            float normal = fromAbs / fromMaxAbs;

            float toMaxAbs = _toMax - _toMin;
            float toAbs = toMaxAbs * normal;

            return toAbs + _toMin;
        }

        /// <summary>The NumberPercent method is used to determine the percentage of a given value.</summary>
        /// <param name="_value">The value to determine the percentage from</param>
        /// <param name="_percent">The percentage to find within the given value.</param>
        /// <returns>A float containing the percentage value based on the given input.</returns>
        public static float NumberPercent(float _value, float _percent)
        {
            _percent = Mathf.Clamp(_percent, 0f, 100f);
            return (_percent == 0f ? _value : (_value - (_percent / 100f)));
        }

        /// <summary>The ColorDarken method takes a given colour and darkens it by the given percentage.</summary>
        /// <param name="_color">The source colour to apply the darken to.</param>
        /// <param name="_percent">The percent to darken the colour by.</param>
        /// <returns>The new colour with the darken applied.</returns>
        public static Color ColorDarken(Color _color, float _percent) => new Color(NumberPercent(_color.r, _percent), NumberPercent(_color.g, _percent), NumberPercent(_color.b, _percent), _color.a);

        /// <summary>Rounds the passed first parameter to the nearest multiple of the second parameter.</summary>
        /// <param name="_toRound">The value to be rounded to the nearest multiple</param>
        /// <param name="_multiple">The value to be rounded to the nearest multiple of.</param>
        public static float RoundToNearest(float _toRound, float _multiple) => Mathf.Round(_toRound / _multiple) * _multiple;

        /// <summary>Increments the passed <paramref name="_value"/> by the passed <paramref name="_increment"/>. If the value after incremention would fall outside the bounds 0-<paramref name="_max"/>, it will wrap it to the other side.</summary>
        /// <param name="_value">The value being decremented.</param>
        /// <param name="_max">The maximum value of the wrapping.</param>
        /// <param name="_increment">The amount to increment the value by. Defaulted to 1.</param>
        public static void IncrementAndWrap(ref int _value, int _max, int _increment = 1) => IncrementAndWrap(ref _value, 0, _max, _increment);

        /// <summary>Increments the passed <paramref name="_value"/> by the passed <paramref name="_decrement"/>. If the value after incrementation would fall outside the bounds <paramref name="_min"/>-<paramref name="_max"/>, it will wrap it to the other side.</summary>
        /// <param name="_value">The value being incremented.</param>
        /// <param name="_min">The minimum value of the wrapping.</param>
        /// <param name="_max">The maximum value of the wrapping.</param>
        /// <param name="_increment">The amount to increment the value by. Defaulted to 1.</param>
        public static void IncrementAndWrap(ref int _value, int _min, int _max, int _increment = 1)
        {
            _increment = Mathf.RoundToInt(Mathf.Clamp(_increment, 1, int.MaxValue));

            int val = _value + _increment;
            if(val >= _max)
                val = _min;

            _value = val;
        }

        /// <summary>Decrements the passed <paramref name="_value"/> by the passed <paramref name="_decrement"/>. If the value after decremention would fall outside the bounds 0-<paramref name="_max"/>, it will wrap it to the other side.</summary>
        /// <param name="_value">The value being decremented.</param>
        /// <param name="_max">The maximum value of the wrapping.</param>
        /// <param name="_decrement">The amount to decrement the value by. Defaulted to 1.</param>
        public static void DecrementAndWrap(ref int _value, int _max, int _decrement = 1) => DecrementAndWrap(ref _value, 0, _max, _decrement);

        /// <summary>Decrements the passed <paramref name="_value"/> by the passed <paramref name="_decrement"/>. If the value after decremention would fall outside the bounds <paramref name="_min"/>-<paramref name="_max"/>, it will wrap it to the other side.</summary>
        /// <param name="_value">The value being decremented.</param>
        /// <param name="_min">The minimum value of the wrapping.</param>
        /// <param name="_max">The maximum value of the wrapping.</param>
        /// <param name="_decrement">The amount to decrement the value by. Defaulted to 1.</param>
        public static void DecrementAndWrap(ref int _value, int _min, int _max, int _decrement = 1)
        {
            _decrement = Mathf.RoundToInt(Mathf.Clamp(_decrement, 1, int.MaxValue));

            int val = _value - _decrement;
            if(val < _min)
                val = _max;

            _value = val;
        }

        /// <summary>Calculates the distance and direction between the two passed variables.</summary>
        /// <param name="_from">The start point of the calculation.</param>
        /// <param name="_to">The end point of the calculation</param>
        /// <param name="_distance">The calculated distance between the <paramref name="_from"/> and <paramref name="_to"/>.</param>
        /// <param name="_direction">The calculated direction between the <paramref name="_from"/> and <paramref name="_to"/>.</param>
        public static void DirectionAndDistance(Vector3 _from, Vector3 _to, out float _distance, out Vector3 _direction)
        {
            var heading = _from - _to;
            _distance = heading.magnitude;
            _direction = heading / _distance;
        }
    }
}