using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using edu.tnu.dgd.effect;

public class PartExplainingController : MonoBehaviour
{
    private GameObject highlightObject;

    private float blinkStartTime;
    private float blinkDuration;

    private static PartExplainingController _instance;
    public float hightlightTime = 5f;

    private int prevIndex = -1;

    [HideInInspector]
    public bool isBlinking = false;

    public List<PartStruct> forkliftParts;


    public static PartExplainingController instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PartExplainingController>();
            }

            return _instance;
        }
    }




    private void Start()
    {
        foreach(PartStruct part in forkliftParts)
        {
            if (part.label != null && part.highlightObject != null)
            {
                part.label.gameObject.SetActive(false);
            }
        }
    }

    public void StartExplaining()
    {
        /*
        foreach (PartStruct part in forkliftParts)
        {
            if (part.label != null && part.meshRenderer != null)
            {
                StartCoroutine(HightlightPart(part));
            }
        }
        */

        InvokeRepeating("Hightlight", 5f, 5f);


        //this.gameObject.transform.parent.parent.gameObject.SetActive(false);
    }

    /*
    private void Hightlight()
    {
        StartCoroutine(HightlightPart());
    }
    */
    public void Hightlight(int idx)
    {
        StartCoroutine(HightlightPart(idx));
    }

    IEnumerator HightlightPart(int index)
    {

        // 停止前一個閃爍的部件
        if (prevIndex >= 0 && prevIndex < forkliftParts.Count)
        {
            CancelInvoke("Hightlight");

            PartStruct part = forkliftParts[prevIndex];
            BlinkingEffector effector = part.highlightObject.GetComponent<BlinkingEffector>();
            if (effector != null)
            {
                effector.StopBlinking();
            }
        }


        // 開始閃爍目前的部件
        if (index < forkliftParts.Count)
        {
            PartStruct part = forkliftParts[index];
            TogglePartHighlight(part, true);

            BlinkingEffector effector = null;
            if (part.highlightObject != null)
            {
                effector = part.highlightObject.GetComponent<BlinkingEffector>();
                if (effector != null)
                {
                    effector.StartBlinking(hightlightTime);
                }                
            }

            prevIndex = index;

            yield return new WaitForSeconds(hightlightTime);
            
            TogglePartHighlight(part, false);
            /*
            if (highLight != null)
            {
                highLight.gameObject.SetActive(false);
                if (part.originalMeshObject != null)
                {
                    part.originalMeshObject.SetActive(true);
                }
            }
            */

            if (effector != null)
            {
                effector.StopBlinking();
            }

            
        }
        else
        {
            CancelInvoke("Hightlight");
        }
    }
    
    private void TogglePartHighlight(PartStruct part, bool showHighlight)
    {
        GameObject highlightObject = part.highlightObject;
        GameObject originalMeshObject = part.originalMeshObject;
        GameObject label = part.label;
        if (label != null)
        {
            label.SetActive(showHighlight);
        }

        Transform highLight = highlightObject.transform.Find("HighlightGlow");
        if (highLight != null)
        {
            highLight.gameObject.SetActive(showHighlight);
        }

        if (originalMeshObject != null)
        {
            originalMeshObject.SetActive(!showHighlight);
        }
    }

    public void HideOutline()
    {
        if (highlightObject != null)
        {
            highlightObject.SetActive(false);
        }
    }

    public void ShowOutline()
    {
        if (highlightObject != null)
        {
            highlightObject.SetActive(true);
        }
    }

    private void ShowOutlineBlink(float duration)
    {
        stopPrevOutline();

        blinkDuration = duration;
        blinkStartTime = Time.time;
        isBlinking = true;
    }

    private void stopPrevOutline()
    {
        if (!isBlinking)
        {
            return;
        }

        blinkDuration = 0f;
        isBlinking = false;
        highlightObject.SetActive(false);
        this.CancelInvoke();
    }

    private void ToggleOutline()
    {
        if ((Time.time - blinkStartTime) >= blinkDuration)
        {
            isBlinking = false;
            highlightObject.SetActive(false);
            this.CancelInvoke();
            return;
        }
        if (highlightObject == null)
        {
            return;
        }

        if (highlightObject.activeSelf)
        {
            highlightObject.SetActive(false);
        }
        else
        {
            highlightObject.SetActive(true);
        }
    }

}

[System.Serializable]
public struct PartStruct
{
    public GameObject label;
    public GameObject highlightObject;
    public GameObject originalMeshObject;
}