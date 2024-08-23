using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRBash : MonoBehaviour
{
    public bool isBashDown; // �转 �غ� ���¸� �ǹ�
    public float recogYDown = 0.5f; // �转 �غ� ���¿��� �󸶳� ������ �转�� ������� �����ϴ� ��
    public Camera cam;

    [SerializeField] private float startTrY; // �转 �غ���¿� �����Ҷ�, ��ġ y���� ���
    public AudioSource audioSource; // �转�� ���� ����
    public Rigidbody p_rb; // �÷��̾�� �ִ� ��
    
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
                        _hit.transform.gameObject.GetComponent<EnemyCtrl>().SendMessage("EnemyDie"); // �� óġ �޽��� ������
                        StompEnemy(); // �÷��̾ �������� ���
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
