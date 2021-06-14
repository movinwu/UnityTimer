using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Timer
{
    /// <summary>
    /// 一个计时器
    /// </summary>
    public class Timer
    {
        #region 私有字段
        /// <summary>
        /// 一轮计时开始时触发一次
        /// </summary>
        private Action _onTimingStart;
        /// <summary>
        /// 在计时中不断触发
        /// </summary>
        private Action _onTiming;
        /// <summary>
        /// 一轮计时结束时触发一次
        /// </summary>
        private Action _onTimingEnd;
        /// <summary>
        /// 是否循环计时
        /// </summary>
        private bool _isLoop;
        /// <summary>
        /// 一轮计时时长
        /// </summary>
        private int _time;
        /// <summary>
        /// 当前计时时间
        /// </summary>
        private int _currentTime;
        /// <summary>
        /// 当前计时器是否暂停
        /// </summary>
        private bool _isPause;
        /// <summary>
        /// 当前计时器是否需要清除的标识
        /// </summary>
        private bool _isRemove;
        #endregion
        #region Constructor
        internal Timer(int time,bool isLoop,Action onTimingStart = null,Action onTiming = null,Action onTimingEnd = null)
        {
            _time = time * 10000;
            _currentTime = 0;
            _isLoop = isLoop;
            _isPause = true;//构造计时器时还没有开始计时
            _onTimingStart = onTimingStart;
            _onTiming = onTiming;
            _onTimingEnd = onTimingEnd;
            _isRemove = false;
        }
        #endregion
        /// <summary>
        /// 开启当前计时器
        /// </summary>
        internal void StartTiming()
        {
            _isPause = false;
            _currentTime = 0;
            if(_onTimingStart != null)
            {
                _onTimingStart();
            }
        }
        /// <summary>
        /// 暂停计时或者继续计时
        /// </summary>
        internal void PauseOrContinueTiming()
        {
            _isPause = !_isPause;
        }
        /// <summary>
        /// 暂停计时
        /// </summary>
        internal void PauseTiming()
        {
            _isPause = true;
        }
        /// <summary>
        /// 继续计时
        /// </summary>
        internal void ContinueTiming()
        {
            _isPause = false;
        }
        /// <summary>
        /// 计时器归零
        /// </summary>
        internal void ResetTimer()
        {
            _isPause = true;
            _currentTime = 0;
        }
        /// <summary>
        /// 获取计时器当前的毫秒值，可用于制作倒计时等
        /// </summary>
        /// <returns></returns>
        internal int GetCurrentTime()
        {
            return _currentTime / 10000;
        }
        /// <summary>
        /// 更新计时
        /// </summary>
        internal void UpdateTiming()
        {
            if (!_isPause)
            {
                if(_currentTime < _time)
                {
                    _currentTime += (int)(Time.deltaTime * 10000000);
                    if(_onTiming != null)
                    {
                        _onTiming();
                    }
                }
                else
                {
                    if(_onTimingEnd != null)
                    {
                        _onTimingEnd();
                    }
                    //如果不是循环计时，一轮计时时间到了之后马上销毁当前计时器
                    //计时器销毁只需要标记当前计时器，由管理类进行销毁
                    if (!_isLoop)
                    {
                        Destroy();
                    }
                    else
                    {
                        StartTiming();
                    }
                }
            }
        }
        /// <summary>
        /// 获取当前对象的销毁标识
        /// </summary>
        /// <returns></returns>
        internal bool IsDestroy()
        {
            return _isRemove;
        }
        /// <summary>
        /// 释放当前计时器
        /// </summary>
        internal void Dispose()
        {
            _onTiming = null;
            _onTimingEnd = null;
            _onTimingStart = null;
        }
        internal void Destroy()
        {
            Dispose();
            _isRemove = true;
        }
    }
}

