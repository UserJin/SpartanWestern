using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRFireCtrl : MonoBehaviour
{
    PlayerCtrl ps;
    private bool isReload; // 재장전 상태 여부
    public AudioClip audioFire;
    public AudioSource audioSource;
    private GameObject shotSound;
    [SerializeField]
    float reloadCoolTime; // 사격 쿨타임
    [SerializeField]
    private float laserShowTime; // 총 발사 후 궤적 표시되는 시간

    [SerializeField]
    private Transform firePoint;
    [SerializeField]
    private LineRenderer lineRenderer; // 총 발사 후 궤적 표시용 컴포넌트

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
    // 공격 함수
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
            StartCoroutine(LaserRender(_hit.point)); // 히트한 위치가 존재하면 해당 위치로 레이저 끝지점 설정
        }
        else
        {
            StartCoroutine(LaserRender(firePoint.position + firePoint.right * 10.0f)); // 히트한 위치가 없으면 총기 전방 지점을 레이저 끝지점으로 설정
        }
        GetComponent<RifleCtrl>().Shoot();

        PlaySound("FIRE");
        isReload = true;
        StartCoroutine(Reload());

    }
    // 사격 쿨타임 코루틴
    IEnumerator Reload()
    {
        yield return new WaitForSeconds(reloadCoolTime);
        isReload = false;
    }
    //리로드 시간 다르게
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
