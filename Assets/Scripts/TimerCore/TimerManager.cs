using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Timer
{
    /// <summary>
    /// ��ʱ��������
    /// </summary>
    public class TimerManager : MonoBehaviour
    {
        #region �̰߳�ȫ�ĵ���ģ�飬�̳�Mono
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
                        DontDestroyOnLoad(go);//���泡����������
                        _instance = go.AddComponent<TimerManager>();
                    }
                }
                return _instance;
            }
        }
        #endregion
        #region �ֶκ�����
        /// <summary>
        /// �洢���м�ʱ�����ֵ�
        /// </summary>
        private Dictionary<string, Timer> _timerDic = new Dictionary<string, Timer>();
        /// <summary>
        /// �洢���д����ٵļ�ʱ�����Ƶ��ֵ�
        /// </summary>
        private List<string> _destroyTimerNameList = new List<string>();
        #endregion
        /// <summary>
        /// ���ָ�����Ƶļ�ʱ�������Ὺʼ��ʱ
        /// </summary>
        /// <param name="timerName">��ʱ��������</param>
        /// <param name="milliseconds">��ʱ��һ�ּ�ʱ�ĺ���ֵ</param>
        /// <param name="isLoop">��ʱ���Ƿ�ѭ����ʱ</param>
        /// <param name="onTimingStart">ÿ�ּ�ʱ��ʼʱִ��һ��</param>
        /// <param name="onTiming">��ʱ�в���ִ��</param>
        /// <param name="onTimingEnd">ÿ�ּ�ʱ����ʱִ��һ��</param>
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
        /// ָ����ʱ����ʼ��ʱ
        /// </summary>
        /// <param name="timerName">��ʱ������</param>
        /// <returns>�Ƿ�ɹ���ʼ��ʱ</returns>
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
        /// �������м�ʱ��
        /// </summary>
        public void StartAllTimer()
        {
            foreach (var pairs in _timerDic)
            {
                pairs.Value.StartTiming();
            }
        }
        /// <summary>
        /// ��ͣ���߼���ָ����ʱ��
        /// </summary>
        /// <param name="timerName">��ʱ������</param>
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
        /// ��ͣ���м�ʱ��
        /// </summary>
        public void PauseAllTimer()
        {
            foreach(var pairs in _timerDic)
            {
                pairs.Value.PauseTiming();
            }
        }
        /// <summary>
        /// �������м�ʱ��
        /// </summary>
        public void ContinueAllTimer()
        {
            foreach(var pairs in _timerDic)
            {
                pairs.Value.ContinueTiming();
            }
        }
        /// <summary>
        /// ��ȡָ����ʱ���ĵ�ǰ��ʱ����ֵ
        /// ����ֵ-1�����ʱ��������
        /// </summary>
        /// <param name="timerName">��ʱ������</param>
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
        /// ����ָ����timer
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
        /// ����ָ��timer��˽�з���
        /// </summary>
        /// <param name="timerName"></param>
        /// <param name="timer"></param>
        private void RealDestroyTimer(string timerName,Timer timer)
        {
            _timerDic.Remove(timerName);
            timer = null;
        }
        /// <summary>
        /// update������ÿ֡�������м�ʱ�������ٱ�ǵļ�ʱ��
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

