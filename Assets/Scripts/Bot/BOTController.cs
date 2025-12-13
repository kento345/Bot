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
    //攻撃
    [Header("パンチ設定")]

    [SerializeField] private BoxCollider box;
    [SerializeField] private float Power = 10.0f;
    [SerializeField] private float WeakKnockbackForce = 0.5f; //弱パンチノックバック
    [SerializeField] private float StrongKnockbackForce = 5.0f;//強パンチノックバック
    private float curentknockbackForce = 0f;//現在のノックバック力
    //private float tackleCooldown = 1.0f;//攻撃クールダウン時間
    [SerializeField] private float HitDuration = 0.2f; //攻撃判定の持続時間
    [SerializeField] private float wait = 0.25f;

    [SerializeField] private float invincibleTime = 1.0f; //無敵
    private bool isInvincible = false;

    [SerializeField] private float StrongRecoveryTime = 1.0f; //硬直時間
    private float curentRecoveryTime;
    private bool isfinish = false;


    private Rigidbody rb;
    private bool isTackling = false;
    private float lastTackleTime = 0f; // 最後のタックル時間

    private bool isPrese = false; //押されているかフラグ
    [HideInInspector] public bool isStrt = false;//タイマスタートフラグ
 /*   private float t = 0f; //タイマー
    public float chargeMax = 5.0f; //タイマー上限
    private bool isMax = false;//チャージがMaxかのフラグ*/



    public List<GameObject> targetList = new List<GameObject>();  　//敵リスト
    private GameObject Target;                                      //敵ターゲット

    private void Awake()
    {
        speed2 = speed * ChargeMoveSpeedRate;
        rotSpeed2 = rotSpeed * ChargeRotateSpeedRate;
    }

    void Start()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Player");
        targetList.AddRange(targets);
    }

    // Update is called once per frame
    void Update()
    {
        //移動

        //
        if(targetList.Count == 0) { return; }
        if(!Target)
        {
            Target = targetList[Random.Range(0, targetList.Count)];
        }
        var dist = Vector3.Distance(transform.position, Target.transform.position);

        if (dist >= 1.0f || dist <= 10.0f)
        {
            //AtackMove();    
        }
        else
        {
          /*  FleeMove();*/
        }
    }

   /* void FleeMove()
    {
        curentSpeed = speed;
        curentRotSpeed = rotSpeed;

        Vector3 dir = point.transform.position - transform.position;

        dir.y = 0;

        Vector3 move = dir.normalized * curentSpeed * Time.deltaTime;
        transform.position += move;

        if (dir != Vector3.zero)
        {
            Quaternion rot = Quaternion.LookRotation(dir,Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation,rot,curentRotSpeed * Time.deltaTime);
        }
    }*/

  /*  void AtackMove()
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
*/
    

   /* private void OnTriggerEnter(Collider other)
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
            *//*foreach (var target in targetList)
            {
                if (target != other.gameObject) { return; }
            }*//*
            targetList.Remove(other.gameObject);
        }
    }*/
}
