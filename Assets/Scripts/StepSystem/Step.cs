using System;
using System.Collections;
using UnityEngine;

namespace StepSystem
{
    /// <summary>
    /// 状態変化に関するデータをまとめたクラス
    /// </summary>
    [Serializable]
    public sealed class Step
    {
        [SerializeField] private TimeSecondsData m_timeData = new TimeSecondsData();

        [SerializeField] private Vector3
            m_transformAmount,
            m_rotationAmount,
            m_scaleAmount;

        public TimeSecondsData TimeData => m_timeData;
        
        public Vector3 TransformAmount {
            get { return m_transformAmount; }
            set { m_transformAmount = value; }
        }

        public Vector3 RotationAmount {
            get { return m_rotationAmount; }
            set { m_rotationAmount = value; }
        }

        public Vector3 ScaleAmount {
            get { return m_scaleAmount; }
            set { m_scaleAmount = value; }
        }

        public IEnumerator Execute(Transform transform)
        {
            //実行前の待機
            if (m_timeData.Before > Time.deltaTime) yield return new WaitForSeconds(m_timeData.Before);

            //移動　回転　スケーリングの実行
            if (m_timeData.Execution > Time.deltaTime)
            {
                float elapsedTime = .0f;
                Vector3 initialPos = transform.position, initialRot = transform.eulerAngles, initialScale = transform.localScale;
                Vector3 goalPos = initialPos + m_transformAmount, goalRot = initialRot + m_rotationAmount, goalScale = initialScale + m_scaleAmount;

                while (elapsedTime <= m_timeData.Execution)
                {
                    float progressRatio = elapsedTime / m_timeData.Execution;
                    transform.position = Vector3.Lerp(initialPos, goalPos, progressRatio);
                    transform.rotation = Quaternion.Euler(Vector3.Lerp(initialRot, goalRot, progressRatio));
                    transform.localScale = Vector3.Lerp(initialScale, goalScale, progressRatio);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                transform.position = goalPos;
                transform.rotation = Quaternion.Euler(goalRot);
                transform.localScale = goalScale;
            }

            //実行後の待機
            if (m_timeData.After > Time.deltaTime) yield return new WaitForSeconds(m_timeData.After);
        }

        /// <summary>
        /// 実行に伴う時間に関してまとめたクラス
        /// </summary>
        [Serializable]
        public sealed class TimeSecondsData
        {
            [SerializeField] private float
                m_before = .0f,//実行前　待機時間
                m_execution = 1.0f,//実行時間
                m_after = .0f;//実行後　待機時間

            public float Before { 
                get { return m_before; }
                set { m_before = Math.Abs(value); }//時間のデータは必ず正にする
            }

            public float Execution { 
                get { return m_execution; }
                set { m_execution = Math.Abs(value); }
            }
            public float After { 
                get { return m_after; }
                set { m_after = Math.Abs(value); }
            }
        } 
    }
}