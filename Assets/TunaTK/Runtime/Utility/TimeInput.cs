// Property of TUNACORN STUDIOS PTY LTD 2018
//
// Creator: James Mills
// Creation Time: 08/04/2019 03:18 PM
// Created On: DESKTOP-JN21C3R

using UnityEngine;

using Serializable = System.SerializableAttribute;

namespace TunaTK.Utilities
{
    /// <summary>A type that can be used to store time in seconds and minutes and display them easily.</summary>
    [Serializable]
	public class TimeInput
    {
        /// <summary>Creates a TimeInput with no minutes or seconds.</summary>
        public static TimeInput Zero = new TimeInput(0);

        /// <summary>Converts the minutes and seconds into a total seconds variable.</summary>
        public float Value => (minutes * 60) + seconds;
        /// <summary>The amount of minutes held in this TimeInput.</summary>
        public float Minutes => minutes;
        /// <summary>The amount of seconds held in this TimeInput.</summary>
        public float Seconds => seconds;

        //@cond
        [SerializeField]private float minutes;
        [SerializeField]private float seconds;
        //@endcond

        /// <summary>Converts the passed <paramref name="_minutes"/> and <paramref name="_seconds"/> to the TimeInput type.</summary>
		public TimeInput(float _minutes, float _seconds)
		{
            minutes = _minutes;
            seconds = _seconds;
		}

        /// <summary>Converts the passed <paramref name="_seconds"/> to the TimeInput type calculating the amount of minutes and seconds from it.</summary>
        public TimeInput(float _totalSeconds)
        {
            // If the amount of seconds passed is greater than 59, convert it to a minute
            if(_totalSeconds > 59)
                minutes = Mathf.FloorToInt(_totalSeconds / 60f);

            seconds = Mathf.FloorToInt(_totalSeconds % 60);
        }

        /// <summary>Compares the passed object to this TimeInput.</summary>
        /// <param name="_obj">The object we are trying to compare to this instance.</param>
        public override bool Equals(object _object)
        {
            // Check if the passed object is a TimeInput, if not, return false
            if(!(_object is TimeInput))
                return false;

            // Compare the values of the passed TimeInput to ours.
            TimeInput other = _object as TimeInput;
            return (minutes == other.minutes) && (seconds == other.seconds);
        }

        /// <summary>Creates a new hash code based on the minutes and seconds hash code sums.</summary>
        public override int GetHashCode() => minutes.GetHashCode() + seconds.GetHashCode();
        /// <summary>Converts the Time to a string in the format 0:0.</summary>
        public override string ToString() => string.Format("{0:00}:{1:00}", minutes, seconds);
        /// <summary>Subtracts the <paramref name="_left"/> value from the <paramref name="_right"/> value.</summary>
        public static TimeInput operator -(TimeInput _left, TimeInput _right)
        {
            // Add the minutes and seconds of both halves together
            float seconds = _left.seconds - _right.seconds;
            float minutes = _left.minutes - _right.minutes;

            // If the seconds are below 0, add 60 seconds and subtract one minute
            if(seconds < 0)
            {
                seconds += 60;
                minutes--;
            }

            return new TimeInput(minutes, seconds);
        }
        /// <summary>Adds the <paramref name="_left"/> value from the <paramref name="_right"/> value.</summary>
        public static TimeInput operator +(TimeInput _left, TimeInput _right)
        {
            // Add the minutes and seconds of both halves together
            float seconds = _left.seconds + _right.seconds;
            float minutes = _left.minutes + _right.minutes;

            // If the seconds are above 60, subtract 60 seconds and add one minute
            if(seconds > 59)
            {
                seconds -= 60;
                minutes++;
            }

            return new TimeInput(minutes, seconds);
        }
        /// <summary>Checks if the <paramref name="_left"/> value matches the <paramref name="_right"/> value.</summary>
        public static bool operator ==(TimeInput _left, TimeInput _right) => _left.Equals(_right);
        /// <summary>Checks if the <paramref name="_left"/> value doesn't match the <paramref name="_right"/> value.</summary>
        public static bool operator != (TimeInput _left, TimeInput _right) => !_left.Equals(_right);
        /// <summary>Checks if the <paramref name="_left"/> value is less than the <paramref name="_right"/> value.</summary>
        public static bool operator <(TimeInput _left, TimeInput _right) => _left.minutes < _right.minutes && _left.seconds < _right.seconds;
        /// <summary>Checks if the <paramref name="_left"/> value is greater than the <paramref name="_right"/> value.</summary>
        public static bool operator >(TimeInput _left, TimeInput _right) => _left.minutes > _right.minutes && _left.seconds > _right.seconds;
    }
}