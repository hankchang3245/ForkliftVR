using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForkAlignmentIndicator : MonoBehaviour
{
    public Collider collider1;

    public Collider collider2;

    public Collider thisCollider;

    public GameObject highlight;

    private void FixedUpdate()
    {
        if (thisCollider.bounds.Intersects(collider1.bounds) || thisCollider.bounds.Intersects(collider2.bounds))
        {
            highlight.SetActive(true);
        }
        else
        {
            highlight.SetActive(false);
        }
    }


}
