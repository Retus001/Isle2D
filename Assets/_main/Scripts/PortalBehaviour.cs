using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalBehaviour : MonoBehaviour
{
    public Animator tpAnim;
    public GameObject linkedSheep;

    public void TriggerTP()
    {
        tpAnim.SetTrigger("Teleport");
        Invoke("DestroySheep", 0.2f);
    }

    private void DestroySheep()
    {
        if(linkedSheep != null)
            Destroy(linkedSheep);
    }
}
