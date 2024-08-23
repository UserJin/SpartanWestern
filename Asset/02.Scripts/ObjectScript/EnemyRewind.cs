using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRewind : MonoBehaviour
{
    public GameObject expEffect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Respawn()
    {
        
        gameObject.GetComponent<EnemyCtrl>().Init();
        GameObject exp = Instantiate(expEffect.transform.GetChild(0).gameObject, transform.position, Quaternion.identity);
        Destroy(exp, 2f);
    }
}
