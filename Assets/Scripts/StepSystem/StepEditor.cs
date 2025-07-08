using UnityEngine;

namespace StepSystem
{
    /// <summary>
    /// 状態変化に関するデータを保存するScriptableObject
    /// </summary>
    [CreateAssetMenu(fileName = "StepEditor", menuName = "ScriptableObject/StepEditor")]
    public sealed class StepEditor : ScriptableObject
    {
        [SerializeField] private Steps m_steps = new Steps();

        public Steps GetSteps() => m_steps;
    }
}