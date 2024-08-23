using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public void TargetHit()
    {
        GetComponent<Renderer>().material.color = Color.red;
    }
}
