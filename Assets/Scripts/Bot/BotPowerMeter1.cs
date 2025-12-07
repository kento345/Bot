using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BotPowerMeter1 : MonoBehaviour
{
    [SerializeField] private Image MeterImage;

    private float meterSpeed = 1.0f;
    private Coroutine meter;

    [SerializeField] private float fillSpeed = 1.0f;

    private AtackMovement1 am;


    private void Start()
    {
        am = GetComponent<AtackMovement1>();
        MeterImage.fillAmount = 0;
    }

    private void Update()
    {
        if(am.isStrt)
        {
            MeterImage.fillAmount += fillSpeed * Time.deltaTime;
        }
        else if(!am.isStrt) 
        {
            MeterImage.fillAmount -= fillSpeed * Time.deltaTime;
        }

        // 0〜1 の範囲に制限
        MeterImage.fillAmount = Mathf.Clamp01(MeterImage.fillAmount);

        // Player のタックル力 (t) に反映
        am.SetChage(MeterImage.fillAmount * am.chargeMax);
    }
}
