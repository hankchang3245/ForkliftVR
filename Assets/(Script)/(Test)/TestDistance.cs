using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDistance : MonoBehaviour
{
    public GameObject obj1;

    public GameObject obj2;


    private void Start()
    {

        InvokeRepeating("TestDist", 0, 0.1f);
    }

    public void TestDist()
    {
        float angle = Quaternion.Angle(obj1.transform.rotation, obj2.transform.rotation);
        float dist = Vector3.Distance(obj1.transform.position, obj2.transform.position) * 100;

        Debug.Log("" + Time.time + ">>>>>>>>>>>>>>>>>>>>>> Angle:" + angle + "           Dist:" + dist);

    }

}
