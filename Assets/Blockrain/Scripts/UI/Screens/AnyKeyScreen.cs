using UnityEngine;
using UnityEngine.UI;

namespace Blockrain.UI.Screens
{
    public class AnyKeyScreen : MenuScreen
    {
        [SerializeField] private float requiredTime = 1f;
        [SerializeField] private Image progressBar;

        private float currentTime = 0f;

        protected override void Run()
        {
            if(currentTime >= requiredTime)
                ScreenManager.ShowScreen("Main");
            else if(Input.anyKey)
                currentTime += Time.deltaTime;
            else if(!Input.anyKey)
                currentTime -= Time.deltaTime;

            currentTime = Mathf.Clamp(currentTime, 0, requiredTime);
            progressBar.fillAmount = Mathf.Clamp01(currentTime / requiredTime);
        }
    }
}