using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Assertions.Must;
using static UnityEngine.GraphicsBuffer;

public class SensorController : MonoBehaviour
{
    [SerializeField] private SphereCollider searchArea = default;
    [SerializeField] private float serchAngle = 45f;
    [SerializeField] private LayerMask objLayer = default;
    private Transform player = default;
    private BotController bc = default;
    private AtackMovement am = default;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bc = transform.parent.GetComponent<BotController>();
        am = transform.parent.GetComponent<AtackMovement>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = other.transform;
            var pos = transform.position; //自身の位置
            var playerDir = other.transform.position - pos;　//自身とPlayerの間の距離

            var angle = Vector3.Angle(transform.forward, playerDir); //自身の正面とPlayerの位置の間の角度

            var dist = Vector3.Distance(other.transform.position, pos);

            if (angle <= serchAngle)
            {
                if (!Physics.Linecast(pos + Vector3.up, other.transform.position + Vector3.up, objLayer))
                {
                    //視野角内にいるかどうか(いた場合の条件近距離)
                    if (Vector3.Distance(other.transform.position, pos) <= searchArea.radius * 0.5f
                         && Vector3.Distance(other.transform.position, pos) >= searchArea.radius * 0.05f)
                    {
                        bc.SetState(true);
                        am.SetAtack(true, player);
                    }


                    //自由行動中に視野角内と半径内の距離の位置にPlayerがいたら攻撃ステータスへ移行
                   /* if (dist <= searchArea.radius && dist >= searchArea.radius * 0.05f
                        && bc.state == BotController.BotState.Fleemove)
                    {
                        bc.SetState(true);
                        am.SetAtack(true,player);
                    }*/
                }
            }
            else if (angle > serchAngle)
            {
                bc.SetState(false);
                am.SetAtack(false,player);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            bc.SetState(false);
            am.SetAtack(false, player);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Handles.DrawSolidArc(transform.position, Vector3.up, Quaternion.Euler(0.0f, -serchAngle, 0f) * transform.forward, serchAngle * 2f, searchArea.radius);
    }
#endif
}
