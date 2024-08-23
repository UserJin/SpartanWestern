using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCtrl : MonoBehaviour
{
    PlayerCtrl ps;
    private bool isReload; // 재장전 상태 여부
    public AudioClip audioFire; // 발사 효과음
    public AudioSource audioSource;
    private GameObject shotSound;
    [SerializeField]
    float reloadCoolTime; // 사격 쿨타임
    [SerializeField]
    private float laserShowTime; // 총 발사 후 궤적 표시되는 시간

    [SerializeField] bool isVrHand = false; // vr에서 핸드트래킹을 사용한다면 true

    [SerializeField]
    private Transform firePoint; // 총 발사 후 궤적 시작 위치
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
        if (ps.state != PlayerCtrl.State.DIE)
        {
            if ((Input.GetMouseButtonDown(0) || Input.GetButtonDown("Attack")) && !isReload)
            {
                Shoot();
            }
        }
    }
    private void OnEnable()
    {
        reloadCoolTime = 1.7f;
    }
    // 공격 함수
    // vr 핸드트래킹 상태와 아닌 상태를 구분하여 공격 방식이 달라짐
    void Shoot()
    {
        RaycastHit _hit;  
        if(isVrHand)
        {
            if (Physics.Raycast(firePoint.position, firePoint.forward * 100.0f, out _hit))
            {
                if (_hit.transform.gameObject.CompareTag("_Enemy"))
                {
                    _hit.transform.GetComponent<EnemyCtrl>().EnemyHit();
                }
                StartCoroutine(LaserRender(_hit.point)); // 히트한 위치가 존재하면 해당 위치로 레이저 끝지점 설정
            }
            else
            {
                StartCoroutine(LaserRender(firePoint.position + firePoint.forward * 10.0f)); // 히트한 위치가 없으면 총기 전방 지점을 레이저 끝지점으로 설정
            }
        }
        else
        {
            if (Physics.Raycast(ps.cam.transform.position, ps.cam.transform.forward * 100.0f, out _hit))
            {
                if (_hit.transform.gameObject.CompareTag("_Enemy"))
                {
                    _hit.transform.GetComponent<EnemyCtrl>().EnemyHit();
                }
                StartCoroutine(LaserRender(_hit.point)); // 히트한 위치가 존재하면 해당 위치로 레이저 끝지점 설정
            }
            else
            {
                StartCoroutine(LaserRender(firePoint.position + firePoint.forward * 10.0f)); // 히트한 위치가 없으면 총기 전방 지점을 레이저 끝지점으로 설정
            }
        }

        if (isVrHand)
            gameObject.GetComponent<RifleCtrlVR>().Shoot();
        else
            gameObject.GetComponent<RifleCtrl>().Shoot();

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
