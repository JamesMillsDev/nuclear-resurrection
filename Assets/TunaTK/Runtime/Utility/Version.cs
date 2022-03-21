// Property of TUNACORN STUDIOS PTY LTD 2018
//
// Creator: James Mills
// Creation Time: 28/10/2019 06:32 PM
// Created On: CHRONOS

using Debug = UnityEngine.Debug;

using Enum = System.Enum;
using Exception = System.Exception;
using Serializable = System.SerializableAttribute;

namespace TunaTK.Utility
{
    /// <summary>A type that can be used to store a version of a product, be it game or Asset.</summary>
	[Serializable]
	public class Version
    {
        /// <summary>The major version of the VersionID. This is only ever updated when the game is either a) re-written or b) moved from a non-full release version to a release version</summary>
        public int Major => major;
        /// <summary>The minor version of the VersionID. This is updated everytime a major feature is added to the game.</summary>
        public int Minor => minor;
        /// <summary>The patch version of the VersionID. This is updated everytime a set of bug fixes is added to the game.</summary>
        public int Patch => patch;
        /// <summary>This is the type of version this VersionID is. This is updated whenever the game goes through a development phase.</summary>
        public ReleaseType Release => release;

        //@cond
        private int major;
        private int minor;
        private int patch;
        private ReleaseType release;
        //@endcond

        //@cond
        //@endcond

        /// <summary>Generate a VersionID of 1.0.0 with the ReleaseType of <see cref="ReleaseType.Release"/>.</summary>
        public Version() : this(1, 0, 0, ReleaseType.Release) { }

        /// <summary>Generates a VersionID with the passed values.</summary>
        /// <param name="_major">The major version of the VersionID.</param>
        /// <param name="_minor">The minor version of the VersionID.</param>
        /// <param name="_patch">The patch version of the VersionID.</param>
        /// <param name="_release">The ReleaseType of the VersionID.</param>
        public Version(int _major, int _minor, int _patch, ReleaseType _release)
        {
            major = _major;
            minor = _minor;
            patch = _patch;
            release = _release;
        }

        /// <summary>
        /// Generates a version using the passed string using the <see cref="Parse"/> function.
        /// </summary>
        /// <param name="_version">The version string to be parsed.</param>
        public Version(string _version)
        {
            Version version = Parse(_version);
            major = version.Major;
            minor = version.Minor;
            patch = version.Patch;
            release = version.Release;
        }

        /// <summary>A default 1.0.0 version with the ReleaseType of <see cref="ReleaseType.Release"/>.</summary>
        public static Version One => new Version();

        /// <summary>Takes the passed string and converts it to a VersionID.</summary>
        /// <param name="_string">The string to parse to a VersionID.</param>
        public static Version Parse(string _string)
        {
            // Generate a new VersionID instance and split the parsed string into it's parts
            Version newVersion = new Version();
            string[] parts = _string.Split('.');

            try
            {
                // Try to parse the string parts to 
                newVersion.major = int.Parse(parts[0]);
                newVersion.minor = int.Parse(parts[1]);
                newVersion.patch = int.Parse(parts[2]);
                newVersion.release = ReleaseType.Release;
            }
            catch(Exception _e)
            {
                // Use thew version 1.0.0 as the version id if it fails and log the exception
                newVersion = Version.One;
                Debug.LogException(_e);
            }

            return newVersion;
        }

        /// <summary>Compares the passed object to this version id.</summary>
        /// <param name="_obj">The object we are trying to compare to this instance.</param>
        public override bool Equals(object _obj)
        {
            // Check if the passed object is a versionid, if not, return false
            if(!(_obj is Version))
                return false;

            // Compare the values of the passed version id to ours
            Version id = _obj as Version;
            return id.major == major && id.minor == minor && id.patch == patch && id.release == release;
        }

        /// <summary>Converts the VersionID to a string in the format '{Major}.{Minor}.{Patch}-{Release}'</summary>
        public override string ToString() => $"{major}.{minor}.{patch}-{release.ToString().ToUpper()}";
        /// <summary>Calculates the hashcode of the VersionID by using the internal values</summary>
        public override int GetHashCode() => major.GetHashCode() + minor.GetHashCode() + patch.GetHashCode() + release.GetHashCode();
        /// <summary>Uses the <see cref="Equals(object)"/> function to check if the two versions are the same.</summary>
        /// <param name="_left">The VersionID that is being compared to the right-hand-side of the operator.</param>
        /// <param name="_right">The VersionID that is being compared to the left-hand-side of the operator.</param>
        public static bool operator ==(Version _left, Version _right) => _left.Equals(_right);
        /// <summary>Uses the <see cref="Equals(object)"/> function to check if the two versions are different.</summary>
        /// <param name="_left">The VersionID that is being compared to the right-hand-side of the operator.</param>
        /// <param name="_right">The VersionID that is being compared to the left-hand-side of the operator.</param>
        public static bool operator !=(Version _left, Version _right) => !_left.Equals(_right);
        /// <summary>Checks if the <paramref name="_left"/> is a lower version than <paramref name="_right"/>.</summary>
        /// <param name="_left">The VersionID that is being compared to the right-hand-side of the operator.</param>
        /// <param name="_right">The VersionID that is being compared to the left-hand-side of the operator.</param>
        public static bool operator <(Version _left, Version _right) => _left.major < _right.major || _left.minor < _right.minor || _left.patch < _right.patch;
        /// <summary>Checks if the <paramref name="_left"/> is a higher version than <paramref name="_right"/>.</summary>
        /// <param name="_left">The VersionID that is being compared to the right-hand-side of the operator.</param>
        /// <param name="_right">The VersionID that is being compared to the left-hand-side of the operator.</param>
        public static bool operator >(Version _left, Version _right) => _left.major > _right.major || _left.minor > _right.minor || _left.patch > _right.patch;
    }
}