using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using edu.tnu.dgd.game;

public class MyToggleButton : MonoBehaviour
{
    public Sprite backgroundSprite;
    public Sprite checkmarkSprite;
    public Image checkmark;
    private bool check = true;

    private void Start()
    {
        check = PlayerPrefs.GetInt(StringConstants.Setting_GuideDisplay) > 0;
        SetToggle(check);
    }
    public void Toggle()
    {
        check = !check;
        PlayerPrefs.SetInt(StringConstants.Setting_GuideDisplay, (check ? 1:0));
        SetToggle(check);
    }

    private void SetToggle(bool val)
    {
        GuideController.instance.ShowHideGuidePanel(val);
        if (val)
        {
            checkmark.sprite = checkmarkSprite;
        }
        else
        {
            checkmark.sprite = backgroundSprite;
        }
    }
}
