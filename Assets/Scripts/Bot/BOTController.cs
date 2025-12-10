using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BOTController : MonoBehaviour
{
    [Header("移動設定")]
    [SerializeField] private float speed = 5.0f; //移動スピード
    [SerializeField] private float ChargeMoveSpeedRate = 0.3f; //チャージ・硬直中の速度倍率
    private float speed2 = 0; //チャージ中のスピード
    private float curentSpeed = 0;  //現在のスピード

    [SerializeField] private float rotSpeed = 10.0f; //旋回スピード
    [SerializeField] private float ChargeRotateSpeedRate = 0.7f; //チャージ・硬直中の旋回倍率
    private float rotSpeed2 = 0;　//チャージ中旋回スピード
    private float curentRotSpeed = 0;//現在の旋回スピード

    //-------------------------------------------------------
    //自由行動
    private GameObject[] points = default; //移動先のポイント
    private GameObject point;              //次移動するポイント
    private int index = 0;                 //どの移動先にするかのインデックス

    //攻撃
    public List<GameObject> targetList = new List<GameObject>();  　//敵リスト
    private GameObject Target;

    private void Awake()
    {
        speed2 = speed * ChargeMoveSpeedRate;
        rotSpeed2 = rotSpeed * ChargeRotateSpeedRate;
    }

    void Start()
    {
        //自由行動(ランダム)
        points = GameObject.FindGameObjectsWithTag("point");
        index = Random.Range(0, points.Length);
        point = points[index];
    }

    // Update is called once per frame
    void Update()
    {
        //移動
        if (Vector3.Distance(point.transform.position, transform.position) < 1.0f)
        {
            index = Random.Range(0, points.Length);

            if (index >= points.Length)
            {
                index = Random.Range(0, points.Length);
            }
            point = points[index];
        }

        FleeMove();

        //
        if(targetList.Count == 0) { return; }
        if(!Target)
        {
            Target = targetList[Random.Range(0, targetList.Count)];
        }
        var dist = Vector3.Distance(transform.position, Target.transform.position);

        if (dist >= 1.0f || dist <= 10.0f)
        {
            AtackMove();    
        }
        else
        {
            FleeMove();
        }
    }

    void FleeMove()
    {
        curentSpeed = speed;
        curentRotSpeed = rotSpeed;

        Vector3 dir = point.transform.position - transform.position;
        //var diire = dir.normalized;

        dir.y = 0;

        Vector3 move = dir.normalized * curentSpeed * Time.deltaTime;
        transform.position += move;

        if (dir != Vector3.zero)
        {
            Quaternion rot = Quaternion.LookRotation(dir,Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation,rot,curentRotSpeed * Time.deltaTime);
        }
    }

    void AtackMove()
    {
        curentSpeed = speed2;
        curentRotSpeed = rotSpeed2;

        var dir = Target.transform.position - transform.position;
        dir.y = 0;

        Vector3 move = dir.normalized * curentSpeed * Time.deltaTime;
        transform.position += move;

        if (dir != Vector3.zero)
        {
            Quaternion rot = Quaternion.LookRotation(dir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, curentRotSpeed * Time.deltaTime);
        }
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            foreach (var target in targetList)
            {
                //重複防止
                if (target == other.gameObject) { return; }
            }
            //リストに追加
            targetList.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            /*foreach (var target in targetList)
            {
                if (target != other.gameObject) { return; }
            }*/
            targetList.Remove(other.gameObject);
        }
    }
}
