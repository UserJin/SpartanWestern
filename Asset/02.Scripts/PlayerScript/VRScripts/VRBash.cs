using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRBash : MonoBehaviour
{
    public bool isBashDown; // 배쉬 준비 상태를 의미
    public float recogYDown = 0.5f; // 배쉬 준비 상태에서 얼마나 내리면 배쉬를 사용할지 결정하는 값
    public Camera cam;

    [SerializeField] private float startTrY; // 배쉬 준비상태에 돌입할때, 위치 y값을 기억
    public AudioSource audioSource; // 배쉬에 사용될 사운드
    public Rigidbody p_rb; // 플레이어에게 주는 힘
    
    void Start()
    {
        isBashDown = false;
        cam = Camera.main;
        audioSource = gameObject.transform.Find("bashSound").GetComponent<AudioSource>();
        p_rb = GameObject.FindWithTag("_Player").GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 2, Color.green);
        if ((transform.localPosition.y > cam.transform.localPosition.y) && !isBashDown)
        {
            startTrY = transform.localPosition.y;
            isBashDown = true;
        }
        if (isBashDown && ((startTrY - transform.localPosition.y) > recogYDown))
        {
            isBashDown = false;
            RaycastHit _hit;
            //audioSource.Play();
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 3, Color.green, 100.0f);
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out _hit, 3, 1 << LayerMask.NameToLayer("ENEMY")))
            {
                if (_hit.transform.gameObject.CompareTag("_Enemy"))
                {
                    if (_hit.transform.gameObject.GetComponent<EnemyCtrl>().state == EnemyCtrl.State.HIT)
                    {
                        _hit.transform.gameObject.GetComponent<EnemyCtrl>().SendMessage("EnemyDie"); // 적 처치 메시지 보내기
                        StompEnemy(); // 플레이어를 공중으로 띄움
                    }
                }
            }
        }
    }

    void StompEnemy()
    {
        p_rb.AddForce(Vector3.up * 20.0f, ForceMode.Impulse);
    }
}
