using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugInput : MonoBehaviour
{
    void Update()
    {
        if (Input.GetButton("Focus")) Debug.Log("Focus");
        if (Input.GetButton("Attack")) Debug.Log("Attack");
        if (Input.GetButton("Jump")) Debug.Log("Jump");

    }
}
