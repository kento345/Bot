using UnityEngine;
using UnityEngine.InputSystem.Users;
using UnityEngine.Rendering;

public class AtackMovement1 : MonoBehaviour
{
    [Header("移動設定")]
    //----------------------------------------------
    [SerializeField] private float speed = 5.0f; //移動スピード
    [SerializeField] private float ChargeMoveSpeedRate = 0.3f; //チャージ・硬直中の速度倍率
    private float speed2 = 0; //チャージ中のスピード
    private float curentSpeed = 0;  //現在のスピード
    [SerializeField] private float rotSpeed = 10.0f; //旋回スピード
    [SerializeField] private float ChargeRotateSpeedRate = 0.7f; //チャージ・硬直中の旋回倍率
    private float rotSpeed2 = 0;　//チャージ中旋回スピード
    private float curentRotSpeed = 0;//現在の旋回スピード

    [Header("タックル設定")]

    [SerializeField] private float tackleForce = 15.0f;    //タックルパワー
    [SerializeField] private float tackleDuration = 0.5f;//タックル状態の持続時間
    [SerializeField] private float tackleCooldown = 1.0f;//タックルのクールダウン時間
    [SerializeField] private float WeakKnockbackForce = 2.5f; //弱パンチノックバック
    [SerializeField] private float StrongKnockbackForce = 5.0f;//強パンチノックバック
    private float curentknockbackForce = 0f;//現在のノックバック力

    private float rigidity = 0.5f; //硬直時間
    private bool isfinish = false;

    private Rigidbody rb;
    private bool isTackling = false;
    private float lastTackleTime = 0f; // 最後のタックル時間
    private bool isAtacked = false;
    private Transform target = default;


    [HideInInspector] public bool isStrt = false;
    private float t = 0f;
    public float chargeMax = 5.0f;
    private bool isMax = false;

    private void Awake()
    {
        speed2 = speed * ChargeMoveSpeedRate;
        rotSpeed2 = rotSpeed * ChargeRotateSpeedRate;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void Atack()
    { 
        if (isAtacked)
        {
            if (!isTackling && Time.time > lastTackleTime + tackleCooldown)
            {
                isStrt = true;
            }
        }
        if (!isAtacked || isMax)
        {
            if (isStrt && !isTackling && Time.time > lastTackleTime + tackleCooldown)
            {
                Tackle();
            }
            isStrt = false;
        }
    }
    public void SetChage(float value)
    {
        t = value;
    }
    public void SetAtack(bool isx,Transform player)
    {
        isAtacked = isx;
        target = player;
    }

    private void FixedUpdate()
    {
        Atack();
        Move();

        if (isStrt)
        {
            if (t < chargeMax)
            {
                t += Time.deltaTime;
                //Debug.Log(t);
            }
            if(t >= chargeMax)
            {
                isMax = true;
            }
        }
        else if (!isStrt)
        {
            t = 0f;
        }
        if (isfinish)
        {
            if (rigidity > 0)
            {
                rigidity -= Time.deltaTime;
            }
            if (rigidity <= 0)
            {
                isfinish = false;
                rigidity = 0.5f;
            }
        }
    }
    void Move()
    {
        if (isfinish) { return; }
        if (isTackling) { return; }
        if (target == null) { return; }
        if (isAtacked)
        {
            curentSpeed = speed2;
            curentRotSpeed = rotSpeed2;
        }
        else if(!isAtacked)
        {
            curentSpeed = speed;
            curentRotSpeed = rotSpeed;

           /* Vector3 dir = (target.position - transform.position);
            var dire = dir.normalized;
            dir.y = 0f; // 水平にしたいなら残す

            rb.MovePosition(rb.position + dire * curentSpeed * Time.deltaTime);

            Quaternion rot = Quaternion.LookRotation(dir);
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, rot, curentRotSpeed * Time.deltaTime));*/
        }

        Vector3 dir = (target.position - transform.position);
        var dire = dir.normalized;
        dir.y = 0f; // 水平にしたいなら残す

        rb.MovePosition(rb.position + dire * curentSpeed * Time.deltaTime);

        Quaternion rot = Quaternion.LookRotation(dir);
        rb.MoveRotation(Quaternion.Slerp(transform.rotation, rot, curentRotSpeed * Time.deltaTime));



    }//---------------------------------------------
    void Tackle()
    {
        if (isfinish) { return; }
        isTackling = true;
        lastTackleTime = Time.time;

        Vector3 dir = (target.position - transform.position);
        dir.y = 0f; // 水平にしたいなら残す
        dir = dir.normalized;

        rb.AddForce(dir * tackleForce * t, ForceMode.Impulse);

        Invoke("EndTackle", tackleDuration);
    }

    void EndTackle()
    {
        isTackling = false;
        // 勢いを止める（急ブレーキ）
        rb.linearVelocity = Vector3.zero;

        //ここで硬直処理
        isfinish = true;

        isMax = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody enemyrb = collision.gameObject.GetComponent<Rigidbody>();
        if (enemyrb != null)
        {
            if (isMax)
            {
                curentknockbackForce = StrongKnockbackForce;
            }
            else
            {
                curentknockbackForce = WeakKnockbackForce;
            }
            //Vector3 knockBackDir = collision.transform.position - transform.position;
            Vector3 knockBackDir = transform.forward;

            knockBackDir.y = 0f;
            Debug.Log(curentknockbackForce);
            enemyrb.AddForce(knockBackDir.normalized * curentknockbackForce, ForceMode.Impulse);
        }
    }
}
