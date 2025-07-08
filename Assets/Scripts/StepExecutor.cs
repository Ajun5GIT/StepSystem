using System.Collections;
using UnityEngine;

namespace StepSystem
{
    /// <summary>
    /// 移動するオブジェクトにアタッチして使用する。移動に関するデータをオブジェクトに反映するクラス
    /// </summary>
    public sealed class StepExecutor : MonoBehaviour
    {
        [SerializeField] private StepEditor m_stepEditor;

        private Steps m_steps = null;

        private IEnumerator Start()
        {
            if (m_stepEditor == null)
            {
                Debug.LogError($"{gameObject.name} オブジェクトのステップデータをアタッチしてください。");
                yield break;
            }

            m_steps = m_stepEditor.GetSteps();

            if (m_steps.GetStepCount() == 0)
            {
                yield break;
            }

            if (m_steps.InfiniteLoop)
            {
                while (true)
                {
                    yield return ExecuteSteps();
                }
            }
            else
            {
                for (int i = 0; i < m_steps.LoopCount; i++)
                {
                    yield return ExecuteSteps();
                }
            }
        }

        private IEnumerator ExecuteSteps()
        {
            foreach (Step step in m_steps)
                yield return step.Execute(transform);
        }
    }
}