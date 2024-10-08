using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCtrl : MonoBehaviour
{
    [SerializeField] private float detectionRange = 30.0f;
    private float dist = 0.0f;

    private GameObject player;
    private GameObject hookPoint;
    public GameObject expEffect;
    public Animator anim;

    private Transform tr;

    private GameObject bulletPrefab;
    private GameObject firePoint;
    private float fireStartRate = 1.0f;
    private float fireRate = 1.0f;
    //private float bulletSpeed = 20.0f;

    GameObject hat;

    private AudioSource aS;
    // Enemy의 상태
    public enum State
    {
        IDLE,
        TRACE,
        HIT,
        DIE
    }

    public State state = State.IDLE;

    // Start is called before the first frame update
    void Start()
    {
        aS = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("_Player");
        hookPoint = gameObject.transform.GetChild(0).Find("EnemyHookPoint").gameObject;
        tr = gameObject.transform;
        hookPoint.SetActive(false);
        //bulletPrefab = Resources.Load<GameObject>("Bullet/EnemyBullet");
        bulletPrefab = Resources.Load<GameObject>("Bullet/Bullet");
        firePoint = tr.Find("FirePoint").gameObject;
        anim = GetComponent<Animator>();
        hat = tr.GetChild(0).Find("Hat_1").gameObject;
    }

    // Update is called once per frame
    void Update()
    {

        // 적이 사망하거나 피격당하지 않았을 때만 실행
        if (state != State.DIE && state != State.HIT)
        {
            dist = Vector3.Distance(player.transform.position, tr.position);
            //플레이어 감지
            if (dist <= detectionRange && state != State.TRACE)
            {
                state = State.TRACE;
                //사격 시작
                InvokeRepeating("Fire", fireStartRate, fireRate);
            }
            else if(dist > detectionRange && state != State.IDLE)
            {
                state = State.IDLE;
                //사격 중지
                CancelInvoke("Fire");
            }
            //추적모드일때
            if(state == State.TRACE)
            {
                tr.LookAt(new Vector3(player.transform.position.x, tr.position.y, player.transform.position.z));
            }
        }   
    }

    public void Init()
    {
        gameObject.SetActive(true);
        state = State.IDLE;
        hookPoint.SetActive(false);
        hookPoint.GetComponent<HookPoint>().state = HookPoint.State.ONABLE;
        hat.transform.SetParent(tr.GetChild(0).transform);
        hat.transform.localPosition = new Vector3(0.544f, -0.093f, -0.044f);
        hat.transform.localRotation = Quaternion.Euler(-87.827f, 89.998f, 0.002f);
        hat.GetComponent<Rigidbody>().useGravity = false;
        hat.GetComponent<Rigidbody>().isKinematic = true;
        hat.GetComponent<BoxCollider>().enabled = false;
    }

    //사격
    public void Fire()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.transform);
        bullet.transform.SetParent(null);
        aS.Play();

        //bullet.transform.LookAt(new Vector3 (player.transform.position.x, player.transform.position.y, player.transform.position.z));
        //bullet.GetComponent<Rigidbody>().velocity = (player.transform.position - tr.position).normalized * bulletSpeed;
    }

    // 적이 플레이어의 총에 피격 시 실행
    public void EnemyHit()
    {
        if(state != State.DIE && state != State.HIT)
        {
            state = State.HIT;
            //anim.SetBool("Dead", true);
            Force2Hat();
            hookPoint.SetActive(true);
            //CancelInvoke("Fire");
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    //모자 날려버리기
    public void Force2Hat()
    {
        if(hat != null)
        {
            hat.transform.SetParent(null);
            hat.GetComponent<Rigidbody>().useGravity = true;
            hat.GetComponent<Rigidbody>().isKinematic = false;
            hat.GetComponent<Rigidbody>().AddForce((tr.position - player.transform.position).normalized * 10.0f + Vector3.up, ForceMode.Impulse);
            hat.GetComponent<BoxCollider>().enabled = true;
        }
    }

    // 적이 hit상태일때 플레이어가 rush하면 실행
    public void EnemyDie()
    {
        state = State.DIE;
        ComboManager.instance.AddCombo();
        anim.SetBool("Dead", true);
        CancelInvoke("Fire");
        Invoke("ExplosionDie", 0.3f);
        
    }

    void ExplosionDie()
    {
        GameObject exp = Instantiate(expEffect, transform.position, Quaternion.identity);
        Destroy(exp, 1f);
        gameObject.SetActive(false);
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        player.GetComponent<PlayerRewind>().savedata.SaveEnemy(gameObject);
        //Destroy(gameObject);
    }
    
}
