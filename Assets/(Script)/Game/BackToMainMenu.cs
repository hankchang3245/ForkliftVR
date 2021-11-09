using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace edu.tnu.dgd.game
{
    public class BackToMainMenu : MonoBehaviour
    {

        public void GoToMainMenu()
        {
            PlayerPrefs.SetString("PrevActivity", PlayerPrefs.GetString("CurrActivity", StringConstants.Scene_Default));
            PlayerPrefs.SetString("CurrActivity", StringConstants.Activity_MainMenu);

            SceneManager.LoadSceneAsync(StringConstants.Scene_MainMenu);
        }

        public void OpenLoginPanel()
        {

        }
    }

}
