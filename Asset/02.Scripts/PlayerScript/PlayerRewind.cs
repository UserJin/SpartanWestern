using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerRewind : MonoBehaviour
{
    Camera cam;
    GameObject player;
    Transform p_tr;

    public SaveData savedata;

    
    public float rewindInterval = 0.0001f; //사용하지 않음
    public int rewindTerm = 3;
    public int interval = 5; // 1/interval 초 마다 저장
    public int segmentCount = 10;
    public (Vector3, Quaternion) savepoint;

    public int labTime = 0; // labTime때문에 버그생기는 거 같아서 사용안하게 바꿉니다.

    Coroutine SaveCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        p_tr = gameObject.transform;
        RenewSavePoint((p_tr.position, Quaternion.Euler(0, -110, 0)));
        SaveCoroutine = StartCoroutine(Save());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Save()
    {
        while (true)
        {
            //labTime = 0;
            //for (int i = 0; i < segmentCount; i++)
            //{
            //    yield return new WaitForSeconds(interval / segmentCount);
            //    labTime++;
            //    //print(savedata.Count + ":" + labTime);
            //}
            yield return new WaitForSecondsRealtime(1.0f/interval);
            savedata.SavePlayerData(p_tr);
            //print("save: " + savedata.Count);
        }
    }

    IEnumerator Rewind((Vector3, Quaternion)[] data)
    {
        gameObject.GetComponent<RewindFilter>().StartRewindEffect();
        for (int i = 0; i < data.Length; i++) {
            yield return null;
            //(p_tr.position, p_tr.rotation) = data[i];
            p_tr.position = data[i].Item1;
           
        }

        gameObject.GetComponent<PlayerCtrl>().Rewind();
        gameObject.GetComponent<FocusCtrl>().Rewind();
        gameObject.GetComponent<RewindFilter>().StopRewindEffect();
        SaveCoroutine = StartCoroutine(Save());
    }

    (Vector3, Quaternion)[] Interpolation(List<(Vector3, Quaternion)> data)
    {
        (Vector3, Quaternion)[] results = new (Vector3, Quaternion)[(data.Count-1) * segmentCount + 1];
        for (int i= 0; i < data.Count-1; i++)
        {
            for (int j = 0; j < segmentCount; j++)
            {
                Vector3 pos = Vector3.Lerp(data[i].Item1, data[i + 1].Item1, (float)j/segmentCount);
                Quaternion rot = Quaternion.Lerp(data[i].Item2, data[i + 1].Item2, (float)j / segmentCount);
                results[j+i*segmentCount] = (pos, rot);
            }
        }
        results[(data.Count - 1) * segmentCount] = data[data.Count - 1];
        return results;
    }

    public void Load()
    {
        StopCoroutine(SaveCoroutine);
        gameObject.GetComponent<PlayerCtrl>().ChangeState(PlayerCtrl.State.DIE);
        gameObject.GetComponent<FocusCtrl>().state = FocusCtrl.State.DEAD;
        //player.GetComponent<Rigidbody>().isKinematic = true;
        //player.GetComponent<Rigidbody>().useGravity = false;
        List <(Vector3, Quaternion)> playerData = new List<(Vector3, Quaternion)>();
        List<GameObject> diedEnemies;

        playerData.Add((p_tr.position, p_tr.rotation));

        for (int i = 0; i < rewindTerm * interval; i++)
        {
            playerData.Add(savedata.LoadPlayerData());
        }
        StartCoroutine(Rewind(Interpolation(playerData)));
        //역행한 시간에 처치한 Enemy 리스폰
        diedEnemies = savedata.LoadEnemies();
        for (int i = 0; i < diedEnemies.Count; i++)
        {
            diedEnemies[i].gameObject.GetComponent<EnemyRewind>().Respawn();
        }
    }

    void RenewSavePoint((Vector3, Quaternion) savepoint)
    {
        this.savepoint = savepoint;
        savedata = new SaveData(savepoint);
    }

    public class SaveData
    {
        public int Count => playerData.Count;

        public (Vector3, Quaternion) startPoint;

        Stack<(Vector3, Quaternion)> playerData = new Stack<(Vector3, Quaternion)>();
        Stack<(int, GameObject)> enemies = new Stack<(int, GameObject)>();

        public SaveData((Vector3, Quaternion) startPoint)
        {
            this.startPoint = startPoint;
        }

        internal void SavePlayerData(Transform p_tr)
        {
            playerData.Push((p_tr.position, p_tr.rotation));
        }

        internal void SaveEnemy(GameObject enemy)
        {
            enemies.Push((Count, enemy));
        }

        internal (Vector3, Quaternion) LoadPlayerData()
        {
            if (playerData.Count == 0) return startPoint;
            return playerData.Pop();
        }

        internal List<GameObject> LoadEnemies()
        {
            List<GameObject> enemyList = new List<GameObject>();
            (int, GameObject) data;
            while (enemies.TryPeek(out data) && data.Item1 >= Count)
            {
                enemyList.Add(data.Item2);
                enemies.Pop();
            }
            return enemyList;
        }
    }
}
