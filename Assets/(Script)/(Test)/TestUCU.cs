using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestUCU : MonoBehaviour
{
    GameObject go;
    // Start is called before the first frame update
    void Start()
    {

        bool bb = true;
        int a = 1234;
        float b = a;

        Vector2 c = new Vector2(1, 1);
        Vector3 d = c;


        Debug.Log(d);

        byte by = 0;

        int mm = by;
        by = (byte)mm;

        float nn = by;
        char ch = 'A';
        int ll = ch;

        double db = 0;

        long ln = 0;

        double dd = ln;

        float fff = ln;
        short sh = 0;

        mm = sh;

        sh = by;

        int ii = ch;

        sh = (short) ch;

        sh = by;

    }

}
