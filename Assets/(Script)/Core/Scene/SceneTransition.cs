using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace edu.tnu.dgd.scene
{
    public static class SceneTransition
    {
        static bool areWeFading = false;

        public static void Fade(string scene, Color col, float multiplier)
        {
            if (areWeFading)
            {
                Debug.Log("Already Fading");
                return;
            }

            GameObject init = new GameObject();
            init.name = "Fader";
            Canvas myCanvas = init.AddComponent<Canvas>();
            myCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            init.AddComponent<FadeEffect>();
            init.AddComponent<CanvasGroup>();
            init.AddComponent<Image>();

            FadeEffect scr = init.GetComponent<FadeEffect>();
            scr.fadeDamp = multiplier;
            scr.fadeScene = scene;
            scr.fadeColor = col;
            scr.start = true;
            areWeFading = true;
            scr.InitiateFader();

        }

        public static void DoneFading()
        {
            areWeFading = false;
        }
    }

}
