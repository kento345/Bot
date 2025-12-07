using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;




public class BotController : MonoBehaviour
{
    public enum BotState
    {
        Fleemove,
        Atack,
        Avoid
    };

    //行動ステート
    public BotState state = default;
    private Transform[] targetTrans = default;
    private Vector3[] desination = default;

    public bool isAtack = false;

    //----------
    private FleeMovement fm;
    private AtackMovement am;
    //----------

    private void Start()
    {
        fm = GetComponent<FleeMovement>();
        am = GetComponent<AtackMovement>();
    }

    private void Update()
    {
        switch(state)
        {
            case BotState.Fleemove:
                Flee();
                break;
            case BotState.Atack:
                Atack();
                break;
        }
    }


    void Flee()
    {
        fm.Move();

        if (isAtack)
        {
            state = BotState.Atack;
        }
    }
    void Atack()
    {
        am.Atack();

        if (!isAtack)
        {
            state = BotState.Fleemove;
        }
    }

    public void SetState(bool isx)
    {
       isAtack = isx;
    }
}
