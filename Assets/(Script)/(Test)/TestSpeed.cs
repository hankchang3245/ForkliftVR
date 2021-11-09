using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using edu.tnu.dgd.debug;

public class TestSpeed : MonoBehaviour
{
    private float startTime;
    private float endTime;
    private float startZ;
    private float endZ;

    void Start()
    {
        
    }

   
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Start"))
        {
            startTime = Time.time;
            startZ = other.gameObject.transform.localPosition.z;
        } 
        else if (other.CompareTag("End"))
        {
            endTime = Time.time;
            endZ = other.gameObject.transform.localPosition.z;

            float speed = (endZ- startZ)*3.6f / (endTime - startTime);
            ShowDebugLog.instance.Log("Speed:" + speed.ToString("F2"), true);
        }
    }


}
