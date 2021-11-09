using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputeAngleBetweenTwoVector : MonoBehaviour
{
    public Transform redLeftPosition;
    public Transform redRightPosition;

    public Transform blueLeftPosition;
    public Transform blueRightPosition;

    public Transform root;
    private float angle;

    void Start()
    {
        angle = ComputerAngle();
        Debug.Log("Angle:" + angle);
    }

    public float ComputerAngle()
    {
        Vector3 one = redRightPosition.position - redLeftPosition.position;
        one = new Vector3(one.x, 0, one.z);

        Vector3 two = blueRightPosition.position - blueLeftPosition.position;
        two = new Vector3(two.x, 0, two.z);


        return Vector3.SignedAngle(one, two, Vector3.up);
    }

    public void DoRotate()
    {
        root.Rotate(0, angle*-1, 0, Space.World);
    }

}
