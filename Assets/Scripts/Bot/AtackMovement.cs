using UnityEngine;
using UnityEngine.Rendering;

public class AtackMovement : MonoBehaviour
{

    [SerializeField] private float tackleForce = 15.0f;    //タックルパワー
    [SerializeField] private float tackleDuration = 0.5f;//タックル状態の持続時間
    [SerializeField] private float tackleCooldown = 1.0f;//タックルのクールダウン時間
    [SerializeField] private float knockbackForce = 5.0f;//ノックバック力

    private Rigidbody rb;
    private bool isTackling = false;
    private float lastTackleTime = 0f; // 最後のタックル時間
    private bool isAtack = false;   


    [HideInInspector] public bool isStrt = false;
    private float t = 0f;
    public float chargeMax = 5.0f;

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
        if (isAtack)
        {
            if (!isTackling && Time.deltaTime > lastTackleTime + tackleCooldown)
            {
                Debug.Log("100");
                isStrt = true;
            }
        }
/*        if (!isAtack)
        {
            if (isStrt && !isTackling && Time.time > lastTackleTime + tackleCooldown)
            {
                Tackle();
            }
            isStrt = false;
        }*/
    }
    public void SetChage(float value)
    {
        t = value;
    }
    public void SetAtack(bool isx)
    {
        isAtack = isx;
    }

    private void FixedUpdate()
    {
        if (isStrt)
        {
            if (t < chargeMax)
            {
                t += Time.deltaTime;
                //Debug.Log(t);
            }
        }
        else if (!isStrt)
        {
            t = 0f;
        }
    }
    void Tackle()
    {
        isTackling = true;
        lastTackleTime = Time.time;

        rb.AddForce(transform.forward * tackleForce * t, ForceMode.Impulse);

        Invoke("EndTackle", tackleDuration);
    }

    void EndTackle()
    {
        isTackling = false;
        // 勢いを止める（急ブレーキ）
        rb.linearVelocity = Vector3.zero;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody enemyrb = collision.gameObject.GetComponent<Rigidbody>();
        if (enemyrb != null)
        {
            Vector3 knockBackDir = collision.transform.position - transform.position;
            knockBackDir.y = 0f;
            enemyrb.AddForce(knockBackDir.normalized * knockbackForce, ForceMode.Impulse);
        }
    }

}
