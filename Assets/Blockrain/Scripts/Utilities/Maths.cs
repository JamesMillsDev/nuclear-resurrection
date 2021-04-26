namespace Blockrain.Utilities
{
    public static class Maths
    {
        /// <summary>
        /// Converts a number (_value) from one range to another
        /// </summary>
        /// <param name="_value">The number being converted.</param>
        /// <param name="_oldMin">The current minimum range the number is in.</param>
        /// <param name="_oldMax">The current maximum range the number is in.</param>
        /// <param name="_newMin">The target minimum range the number is in.</param>
        /// <param name="_newMax">The target minimum range the number is in.</param>
        /// <returns>The number converted into the correct range.</returns>
        public static float Map(float _value, 
            float _oldMin, float _oldMax, 
            float _newMin, float _newMax)
            => _newMin + (_value - _oldMin) * (_newMax - _newMin) / (_oldMax - _oldMin);
    }
}