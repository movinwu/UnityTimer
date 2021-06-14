using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Timer
{
    /// <summary>
    /// 计时器管理器
    /// </summary>
    public class TimerManager : MonoBehaviour
    {
        #region 线程安全的单例模块，继承Mono
        private static GameObject _lock = new GameObject("Lock");
        private static TimerManager _instance;
        public static TimerManager Instance
        {
            get
            {
                lock (_lock)
                {
                    if(_instance == null)
                    {
                        GameObject go = new GameObject("TimerManager");
                        DontDestroyOnLoad(go);//不随场景加载销毁
                        _instance = go.AddComponent<TimerManager>();
                    }
                }
                return _instance;
            }
        }
        #endregion
        #region 字段和属性
        /// <summary>
        /// 存储所有计时器的字典
        /// </summary>
        private Dictionary<string, Timer> _timerDic = new Dictionary<string, Timer>();
        /// <summary>
        /// 存储所有待销毁的计时器名称的字典
        /// </summary>
        private List<string> _destroyTimerNameList = new List<string>();
        #endregion
        /// <summary>
        /// 添加指定名称的计时器，不会开始计时
        /// </summary>
        /// <param name="timerName">计时器的名称</param>
        /// <param name="milliseconds">计时器一轮计时的毫秒值</param>
        /// <param name="isLoop">计时器是否循环计时</param>
        /// <param name="onTimingStart">每轮计时开始时执行一次</param>
        /// <param name="onTiming">计时中不断执行</param>
        /// <param name="onTimingEnd">每轮计时结束时执行一次</param>
        /// <returns></returns>
        public bool AddTimer(string timerName,int milliseconds,bool isLoop,Action onTimingStart = null,Action onTiming = null,Action onTimingEnd = null)
        {
            if (_timerDic.ContainsKey(timerName))
            {
                return false;
            }

            Timer timer = new Timer(milliseconds, isLoop, onTimingStart, onTiming, onTimingEnd);
            _timerDic.Add(timerName, timer);
            return true;
        }
        /// <summary>
        /// 指定计时器开始计时
        /// </summary>
        /// <param name="timerName">计时器名称</param>
        /// <returns>是否成功开始计时</returns>
        public bool StartTimer(string timerName)
        {
            if (_timerDic.ContainsKey(timerName))
            {
                _timerDic[timerName].StartTiming();
                return true;
            }
            return false;
        }
        /// <summary>
        /// 开启所有计时器
        /// </summary>
        public void StartAllTimer()
        {
            foreach (var pairs in _timerDic)
            {
                pairs.Value.StartTiming();
            }
        }
        /// <summary>
        /// 暂停或者继续指定计时器
        /// </summary>
        /// <param name="timerName">计时器名称</param>
        /// <returns></returns>
        public bool PauseOrContinueTimer(string timerName)
        {
            if (_timerDic.ContainsKey(timerName))
            {
                _timerDic[timerName].PauseOrContinueTiming();
                return true;
            }
            return false;
        }
        /// <summary>
        /// 暂停所有计时器
        /// </summary>
        public void PauseAllTimer()
        {
            foreach(var pairs in _timerDic)
            {
                pairs.Value.PauseTiming();
            }
        }
        /// <summary>
        /// 继续所有计时器
        /// </summary>
        public void ContinueAllTimer()
        {
            foreach(var pairs in _timerDic)
            {
                pairs.Value.ContinueTiming();
            }
        }
        /// <summary>
        /// 获取指定计时器的当前计时毫秒值
        /// 返回值-1代表计时器不存在
        /// </summary>
        /// <param name="timerName">计时器名称</param>
        /// <returns></returns>
        public int GetCurrentTime(string timerName)
        {
            if (_timerDic.ContainsKey(timerName))
            {
                return _timerDic[timerName].GetCurrentTime();
            }
            return -1;
        }
        /// <summary>
        /// 销毁指定的timer
        /// </summary>
        /// <param name="timerName"></param>
        /// <returns></returns>
        public bool DestroyTimer(string timerName)
        {
            if (_timerDic.ContainsKey(timerName))
            {
                _timerDic[timerName].Destroy();
                return true;
            }
            return false;
        }
        /// <summary>
        /// 销毁指定timer，私有方法
        /// </summary>
        /// <param name="timerName"></param>
        /// <param name="timer"></param>
        private void RealDestroyTimer(string timerName,Timer timer)
        {
            _timerDic.Remove(timerName);
            timer = null;
        }
        /// <summary>
        /// update函数，每帧调用所有计时器和销毁标记的计时器
        /// </summary>
        void Update()
        {
            if(_timerDic.Count > 0)
            {
                foreach (var pairs in _timerDic)
                {
                    if (pairs.Value.IsDestroy())
                    {
                        _destroyTimerNameList.Add(pairs.Key);
                    }
                    else
                    {
                        pairs.Value.UpdateTiming();
                    }
                }
            }
            if(_destroyTimerNameList.Count > 0)
            {
                string name = "";
                Timer timer = null;
                for(int i = 0;i < _destroyTimerNameList.Count; i++)
                {
                    name = _destroyTimerNameList[i];
                    timer = _timerDic[name];
                    RealDestroyTimer(name, timer);
                }
                _destroyTimerNameList.Clear();
            }
            
        }
    }
}

