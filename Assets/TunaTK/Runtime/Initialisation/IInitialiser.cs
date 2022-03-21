// Property of TUNACORN STUDIOS PTY LTD 2018
// 
// Creator: James Mills
// Creation Time: 14/11/2018 09:47 PM
// Created On: CHRONOS

using System.Threading.Tasks;

namespace TunaTK
{
    /// <summary>The base interface that any class type can implement in order to have the same functionality as an Initialisable.</summary>
    public interface IInitialiser
    {
        /// <summary>The boolean that determines whether or not this Initialisable is ready to be used.</summary>
        bool Initialised { get; }
        /// <summary>Calling this will force the Initialisable to become active and is used as a setup function.</summary>
        /// <param name="_params">The parameters that get passed into any Initialisable.</param>
        Task Initialise_Async(params object[] _params);
    }
}
