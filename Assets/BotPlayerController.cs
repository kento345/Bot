using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BotPlayerController : MonoBehaviour
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

    [Header("攻撃設定")]

    [SerializeField] private float tackleForce;    //ブリンク力
    [SerializeField] private float tackleDuration = 0.5f;//持続時間
    [SerializeField] private float tackleCooldown = 1.0f;//クールダウン時間

    //-----硬直-----
    [SerializeField] private float StrongRecoveryTime = 1.0f; //硬直時間
    private float curentRecoveryTime;
    private bool isfinish = false;

    private bool isPrese = false;                 //攻撃キー入力フラグ
    [HideInInspector] public bool isStrt = false;//チャージ開始フラグ
    private float t = 0f;                        //チャージ量
    public float chargeMax = 5.0f;               //チャージ上限
    private bool isMax = false;                  //チャージがMaxかのフラグ

    [Header("ノックバック,無敵設定")]
    [SerializeField] private float WeakKnockbackForce = 2.5f; //弱ブリンクノックバック
    [SerializeField] private float StrongKnockbackForce = 5.0f;//強ブリンクノックバック
    private float curentknockbackForce = 0f;//現在のノックバック力


    private Rigidbody rb;
    private bool isTackling = false;
    private float lastTackleTime = 0f; // 最後のタックル時間


  

    [Header("当たり判定設定")]
    [SerializeField] private SphereCollider searchArea;
    [SerializeField] private float angle = 45f;

    //-----その他-----
    public List<GameObject> players = new List<GameObject>();  //Player達
    private float minDistance = Mathf.Infinity; //最短距離をだすための目安値

    public GameObject target;       //攻撃対象
    private float distance;          //攻撃対象との距離

    private void Awake()
    {
        speed2 = speed * ChargeMoveSpeedRate;
        rotSpeed2 = rotSpeed * ChargeRotateSpeedRate;
        //curentRecoveryTime = StrongRecoveryTime;
    }
    void Start()
    {
      rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //Listに自身以外のPlayerを追加
        players.Clear();
        minDistance = Mathf.Infinity;
        target = null;


        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (obj != this.gameObject)
            {
                players.Add(obj);

                float dist = (transform.position - obj.transform.position).sqrMagnitude;
                if (dist < minDistance)
                {
                    minDistance = dist;
                    target = obj;
                }
            }

           
        }

        if (target != null)
        {
            Move();
        }

        distance = Vector3.Distance(this.transform.position, target.transform.position);
        // 一定の範囲内に入ったら実行
        if (distance < 15f)
        {
            Atack();
        }

    }

    void Move()
    {
        if (isPrese)
        {
            curentSpeed = speed2;
            curentRotSpeed = rotSpeed2;
        }
        else
        {
            curentSpeed = speed;
            curentRotSpeed = rotSpeed;
        }

        if (!isTackling)
        {
            Vector3 dir = (target.transform.position - transform.position).normalized;

            Vector3 move = dir * curentSpeed * Time.deltaTime;
            rb.MovePosition(rb.position + move);

            if (move != Vector3.zero)
            {
                Quaternion Rot = Quaternion.LookRotation(dir, Vector3.up);
                rb.MoveRotation(Quaternion.Slerp(rb.rotation, Rot, curentRotSpeed * Time.deltaTime));
            }
        }
    }

    void Atack(bool a)
    {
        if (a)
        {
            isfinish = false;

            if (!isTackling && Time.time > lastTackleTime + tackleCooldown)
            {
                isStrt = true;
                isPrese = true;
            }
        }
        if (!a)
        {
            isPrese = false;
            if (isStrt && !isTackling && Time.time > lastTackleTime + tackleCooldown)
            {
                Tackle();
            }
            isStrt = false;
        }
    }
    void Tackle()
    {
        if (isfinish) { return; }
        isTackling = true;
        lastTackleTime = Time.time;

        rb.AddForce(transform.forward * tackleForce, ForceMode.Impulse);

        Invoke("EndTackle", tackleDuration);

    }
    void EndTackle()
    {
        rb.linearVelocity = Vector3.zero;
        isTackling = false;

        //ここで硬直処理
        if (isMax)
        {
            isfinish = true;
        }

        isMax = false;
    }
}
