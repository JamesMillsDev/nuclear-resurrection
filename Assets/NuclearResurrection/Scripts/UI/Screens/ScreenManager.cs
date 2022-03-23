using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NuclearResurrection.UI.Screens
{
    public class ScreenManager : MonoBehaviour
    {
        [System.Serializable]
        private struct MenuScreenReference
        {
            public MenuScreen screen;
            public string id;
            public bool isDefault;

            public void SetActive(bool _active)
            {
                if(_active)screen.Enable();
                else screen.Disable();
            }
        }

        public static ScreenManager instance = null;

        [SerializeField] private List<MenuScreenReference> screens = new List<MenuScreenReference>();

        private string activeScreen = "";
        private Dictionary<string, MenuScreenReference> screenDict = new Dictionary<string, MenuScreenReference>();

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else if(instance != this)
            {
                Destroy(gameObject);
                return;
            }
        }

        public static void ShowScreen(string _id)
        {
            instance.ShowScreenInternal(_id);
        }

        private void ShowScreenInternal(string _id)
        {
            if(screenDict.TryGetValue(activeScreen, out MenuScreenReference reference))
                reference.SetActive(false);

            if(screenDict.TryGetValue(_id, out reference))
                reference.SetActive(true);
        }

        // Start is called before the first frame update
        void Start()
        {
            screens.ForEach(screen =>
            {
                if(screen.isDefault)
                {
                    activeScreen = screen.id;
                    screen.screen.gameObject.SetActive(true);
                }
                else
                {
                    screen.screen.gameObject.SetActive(true);
                }

                screenDict.Add(screen.id, screen);
            });
        }
    }
}