using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NuclearResurrection.UI.Screens
{
    public abstract class MenuScreen : MonoBehaviour
    {
        public bool Active { get; private set; } = true;

        public void Enable()
        {
            Active = true;
            gameObject.SetActive(true);
        }
        
        public void Disable()
        {
            Active = false;
            gameObject.SetActive(false);
        }

        // Start is called before the first frame update
        protected virtual void Start() { }

        // Update is called once per frame
        void Update()
        {
            if(Active)
                Run();
        }

        protected abstract void Run();
    }
}