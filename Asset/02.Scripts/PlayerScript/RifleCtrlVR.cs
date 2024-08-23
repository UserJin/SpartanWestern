using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleCtrlVR : MonoBehaviour
{

    public Animator anim;
    public float delay2reload = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.transform.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot()
    {
        anim.CrossFadeInFixedTime("ReboundVR", delay2reload);
    }
}
