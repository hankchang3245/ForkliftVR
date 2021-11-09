using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingAnimation : MonoBehaviour
{

    public Text dotsText;
    void OnEnable()
    {

        dotsText.text = "";

        StartCoroutine(StartAnimation());
    }


    IEnumerator StartAnimation()
    {
        InvokeRepeating("DotAnimation", 0, 0.2f);

        yield return null;
    }

    private void DotAnimation()
    {
        string val = dotsText.text;
        if (val.Length < 3)
        {
            val = val + ".";
            dotsText.text = val;
        }
        else
        {
            dotsText.text = "";
        }
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
}
