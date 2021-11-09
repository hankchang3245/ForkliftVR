using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartKey : MonoBehaviour
{


    public PartExplainingController partExplainingController;

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.StartsWith("b_r_index"))
        {
            partExplainingController.StartExplaining();
        }
    }
}
