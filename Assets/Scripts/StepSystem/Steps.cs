using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StepSystem
{
    /// <summary>
    /// 状態変化に関するデータのリストや、ループ回数などの情報をまとめたクラス
    /// </summary>
    [Serializable]
    public sealed class Steps
    {
        [SerializeField] private bool m_infiniteLoop = true;
        [SerializeField] private int m_loopCount = 1;
        [SerializeField] private List<Step> m_steps = new List<Step>();

        public bool InfiniteLoop
        {
            get { return m_infiniteLoop; }
            set { m_infiniteLoop = value; }
        }

        public int LoopCount
        {
            get { return m_loopCount; }
            set { m_loopCount = Math.Abs(value); }
        }

        public void Insert(int index, Step step)
        {
            m_steps.Insert(index, step);
        }

        public void Remove(Step step)
        {
            m_steps.Remove(step);
        }

        public int GetStepCount()
        {
            return m_steps.Count;
        }

        public IEnumerator GetEnumerator()
        {
            return m_steps.GetEnumerator();
        }
    }
}