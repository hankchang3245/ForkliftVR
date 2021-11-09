using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRotate : MonoBehaviour
{
    public Transform cylinderPivot;
    public Transform cylinderEnd;

    public Transform targetPivot;
    public Transform target;

    private Vector3 prevAngle;
    private Vector3 targetAngle;


    private float angleX;
    private float angleY;
    private float angleZ;
    public float speed = 10;

   
    void Start()
    {
        Vector3 vec1 = cylinderEnd.position - cylinderPivot.position;
        Vector3 vec2 = target.position - targetPivot.position;

        prevAngle = cylinderPivot.eulerAngles;

        angleX = Vector3.Angle(new Vector3(vec1.x, 0, 0), new Vector3(vec2.x, 0, 0));
        angleY = Vector3.Angle(new Vector3(0, vec1.y, 0), new Vector3(0, vec2.y, 0));
        angleZ = Vector3.Angle(new Vector3(0, 0, vec1.z), new Vector3(0, 0, vec2.z));

        targetAngle = new Vector3(prevAngle.x + angleX, prevAngle.y + angleY, prevAngle.z + angleZ);

        Debug.LogFormat("Angle: {0},{1},{2} ", prevAngle.x, prevAngle.y, prevAngle.z);

    }

    void Update()
    {
        float xx = Mathf.MoveTowards(prevAngle.x, targetAngle.x, Time.deltaTime * speed);
        float yy = Mathf.MoveTowards(prevAngle.y, targetAngle.y, Time.deltaTime * speed);
        float zz = Mathf.MoveTowards(prevAngle.z, targetAngle.z, Time.deltaTime * speed);

        prevAngle = new Vector3(xx, yy, zz);
        cylinderPivot.transform.localEulerAngles = prevAngle;
    }
}
