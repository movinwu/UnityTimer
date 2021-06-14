using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Timer;//使用时需要引用命名空间

/// <summary>
/// 计时器测试类
/// 计时器的毫秒值不能太短，否则及其不准确
/// 计时器核心使用Time.deltaTime进行时间计算，实际计时过程中往往会有一点误差
/// </summary>
public class Test : MonoBehaviour
{
    //用作计时器名称的字符串
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
        //添加计时器
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
            //暂停或继续_20Milli这个计时器运行
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
            //结束_20Milli这个计时器的运行
            TimerManager.Instance.DestroyTimer(_120Milli);
        });
        //启动所有计时器
        TimerManager.Instance.StartAllTimer();

        TimerManager.Instance.AddTimer(_120Milli, 120, true, null, () =>
        {
            //当前计时器在计时中打印毫秒值
            Debug.Log("current time of _120Milli is " + TimerManager.Instance.GetCurrentTime(_120Milli));
        }, null);
        //所有计时器都要启动才能运行，当前计时器单独启动
        TimerManager.Instance.StartTimer(_120Milli);
    }
    private void Update()
    {
        //Debug.Log(Time.deltaTime);
    }
}
