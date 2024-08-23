using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRFireCtrl : MonoBehaviour
{
    PlayerCtrl ps;
    private bool isReload; // ������ ���� ����
    public AudioClip audioFire;
    public AudioSource audioSource;
    private GameObject shotSound;
    [SerializeField]
    float reloadCoolTime; // ��� ��Ÿ��
    [SerializeField]
    private float laserShowTime; // �� �߻� �� ���� ǥ�õǴ� �ð�

    [SerializeField]
    private Transform firePoint;
    [SerializeField]
    private LineRenderer lineRenderer; // �� �߻� �� ���� ǥ�ÿ� ������Ʈ

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<PlayerCtrl>();
        lineRenderer = GetComponent<LineRenderer>();
        shotSound = gameObject.transform.Find("shotSound").gameObject;
        reloadCoolTime = 1.7f;
        isReload = false;
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, lineRenderer.GetPosition(0));
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetMouseButtonDown(0) || Input.GetButtonDown("Attack")) && !isReload)
        {
            Shoot();
        }
        Debug.DrawRay(firePoint.position, firePoint.right * 100.0f, Color.blue);
    }
    private void OnEnable()
    {
        reloadCoolTime = 1.7f;
    }
    // ���� �Լ�
    void Shoot()
    {
        RaycastHit _hit;
        
        if (Physics.Raycast(firePoint.position, firePoint.right * 100.0f, out _hit))
        {
            Debug.Log(_hit.transform.name);
            if (_hit.transform.gameObject.CompareTag("_Enemy"))
            {
                _hit.transform.GetComponent<EnemyCtrl>().EnemyHit();
            }
            if (_hit.transform.gameObject.CompareTag("_Target"))
            {
                _hit.transform.GetComponent<Target>().TargetHit();
                Debug.Log("Hit");
            }
            StartCoroutine(LaserRender(_hit.point)); // ��Ʈ�� ��ġ�� �����ϸ� �ش� ��ġ�� ������ ������ ����
        }
        else
        {
            StartCoroutine(LaserRender(firePoint.position + firePoint.right * 10.0f)); // ��Ʈ�� ��ġ�� ������ �ѱ� ���� ������ ������ ���������� ����
        }
        GetComponent<RifleCtrl>().Shoot();

        PlaySound("FIRE");
        isReload = true;
        StartCoroutine(Reload());

    }
    // ��� ��Ÿ�� �ڷ�ƾ
    IEnumerator Reload()
    {
        yield return new WaitForSeconds(reloadCoolTime);
        isReload = false;
    }
    //���ε� �ð� �ٸ���
    public void SetReloadCoolTime(float f)
    {
        reloadCoolTime = f;
    }
    public void PlaySound(string action)
    {
        switch (action)
        {
            case "FIRE":
                shotSound.GetComponent<AudioPlay>().audioPlay();
                break;

        }
        //audioSource.clip = null;
    }

    IEnumerator LaserRender(Vector3 dest)
    {
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, dest);
        yield return new WaitForSeconds(laserShowTime);
        lineRenderer.SetPosition(1, lineRenderer.GetPosition(0));
    }
}
