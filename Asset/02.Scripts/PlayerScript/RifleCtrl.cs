using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleCtrl : MonoBehaviour
{

    public Animator anim;
    public float delay2reload = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot()
    {
        anim.CrossFadeInFixedTime("Shoot_1Hand", delay2reload);
    }
}
