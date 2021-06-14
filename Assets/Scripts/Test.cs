using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Timer;//ʹ��ʱ��Ҫ���������ռ�

/// <summary>
/// ��ʱ��������
/// ��ʱ���ĺ���ֵ����̫�̣������䲻׼ȷ
/// ��ʱ������ʹ��Time.deltaTime����ʱ����㣬ʵ�ʼ�ʱ��������������һ�����
/// </summary>
public class Test : MonoBehaviour
{
    //������ʱ�����Ƶ��ַ���
    private string _500Milli;
    private string _800Milli;
    private string _1200Milli;
    private string _120Milli;
    private void Awake()
    {
        _500Milli = "500Milliseconds";
        _800Milli = "800Milliseconds";
        _1200Milli = "1200Milliseconds";
        _120Milli = "120Milliseconds";
    }
    void Start()
    {
        //��Ӽ�ʱ��
        TimerManager.Instance.AddTimer(_500Milli, 500, true, () =>
           {
               Debug.Log("500 milliseconds start");
           }, null, () =>
          {
               Debug.Log("500 milliseconds end");
           });
        TimerManager.Instance.AddTimer(_800Milli, 800, true, () =>
        {
            Debug.Log("800 milliseconds start");
            //��ͣ�����_20Milli�����ʱ������
            TimerManager.Instance.PauseOrContinueTimer(_120Milli);
        }, null, () =>
        {
            Debug.Log("800 milliseconds end");
        });
        TimerManager.Instance.AddTimer(_1200Milli, 1200, false, () =>
        {
            Debug.Log("1200 milliseconds start");
        }, null, () =>
        {
            Debug.Log("1200 milliseconds end");
            //����_20Milli�����ʱ��������
            TimerManager.Instance.DestroyTimer(_120Milli);
        });
        //�������м�ʱ��
        TimerManager.Instance.StartAllTimer();

        TimerManager.Instance.AddTimer(_120Milli, 120, true, null, () =>
        {
            //��ǰ��ʱ���ڼ�ʱ�д�ӡ����ֵ
            Debug.Log("current time of _120Milli is " + TimerManager.Instance.GetCurrentTime(_120Milli));
        }, null);
        //���м�ʱ����Ҫ�����������У���ǰ��ʱ����������
        TimerManager.Instance.StartTimer(_120Milli);
    }
    private void Update()
    {
        //Debug.Log(Time.deltaTime);
    }
}
