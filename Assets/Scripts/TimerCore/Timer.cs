using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Timer
{
    /// <summary>
    /// һ����ʱ��
    /// </summary>
    public class Timer
    {
        #region ˽���ֶ�
        /// <summary>
        /// һ�ּ�ʱ��ʼʱ����һ��
        /// </summary>
        private Action _onTimingStart;
        /// <summary>
        /// �ڼ�ʱ�в��ϴ���
        /// </summary>
        private Action _onTiming;
        /// <summary>
        /// һ�ּ�ʱ����ʱ����һ��
        /// </summary>
        private Action _onTimingEnd;
        /// <summary>
        /// �Ƿ�ѭ����ʱ
        /// </summary>
        private bool _isLoop;
        /// <summary>
        /// һ�ּ�ʱʱ��
        /// </summary>
        private int _time;
        /// <summary>
        /// ��ǰ��ʱʱ��
        /// </summary>
        private int _currentTime;
        /// <summary>
        /// ��ǰ��ʱ���Ƿ���ͣ
        /// </summary>
        private bool _isPause;
        /// <summary>
        /// ��ǰ��ʱ���Ƿ���Ҫ����ı�ʶ
        /// </summary>
        private bool _isRemove;
        #endregion
        #region Constructor
        internal Timer(int time,bool isLoop,Action onTimingStart = null,Action onTiming = null,Action onTimingEnd = null)
        {
            _time = time * 10000;
            _currentTime = 0;
            _isLoop = isLoop;
            _isPause = true;//�����ʱ��ʱ��û�п�ʼ��ʱ
            _onTimingStart = onTimingStart;
            _onTiming = onTiming;
            _onTimingEnd = onTimingEnd;
            _isRemove = false;
        }
        #endregion
        /// <summary>
        /// ������ǰ��ʱ��
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
        /// ��ͣ��ʱ���߼�����ʱ
        /// </summary>
        internal void PauseOrContinueTiming()
        {
            _isPause = !_isPause;
        }
        /// <summary>
        /// ��ͣ��ʱ
        /// </summary>
        internal void PauseTiming()
        {
            _isPause = true;
        }
        /// <summary>
        /// ������ʱ
        /// </summary>
        internal void ContinueTiming()
        {
            _isPause = false;
        }
        /// <summary>
        /// ��ʱ������
        /// </summary>
        internal void ResetTimer()
        {
            _isPause = true;
            _currentTime = 0;
        }
        /// <summary>
        /// ��ȡ��ʱ����ǰ�ĺ���ֵ����������������ʱ��
        /// </summary>
        /// <returns></returns>
        internal int GetCurrentTime()
        {
            return _currentTime / 10000;
        }
        /// <summary>
        /// ���¼�ʱ
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
                    //�������ѭ����ʱ��һ�ּ�ʱʱ�䵽��֮���������ٵ�ǰ��ʱ��
                    //��ʱ������ֻ��Ҫ��ǵ�ǰ��ʱ�����ɹ������������
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
        /// ��ȡ��ǰ��������ٱ�ʶ
        /// </summary>
        /// <returns></returns>
        internal bool IsDestroy()
        {
            return _isRemove;
        }
        /// <summary>
        /// �ͷŵ�ǰ��ʱ��
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

