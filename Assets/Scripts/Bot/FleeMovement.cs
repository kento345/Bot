using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public class FleeMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
   /* [SerializeField] */private GameObject[] points;
    private GameObject target;
    private int index = 0;

    private float rotSpeed = 10.0f;

    Rigidbody rb;

    void Start()
    {
        points = GameObject.FindGameObjectsWithTag("point");
        rb = GetComponent<Rigidbody>();
        index = Random.Range(0,points.Length);
        target = points[index];
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(target.transform.position, transform.position) < 1.0f)
        {
            index = Random.Range(0, points.Length);
            //index++;

            if (index >= points.Length)
            {
                index = Random.Range(0, points.Length);
            }
            target = points[index];
        }
    }


    public void Move()
    {
        Vector3 dir = target.transform.position - transform.position;
        //移動用に正規化
        var dire = dir.normalized;

        //回転用にy軸無効
        dir.y = 0;

        //ターゲット方向への回転
        Quaternion rot = Quaternion.LookRotation(dir);
        rb.MoveRotation(Quaternion.Slerp(transform.rotation, rot, rotSpeed * Time.deltaTime));
        //ターゲットへの移動
        rb.MovePosition(rb.position + dire * speed * Time.deltaTime);
    }
}
