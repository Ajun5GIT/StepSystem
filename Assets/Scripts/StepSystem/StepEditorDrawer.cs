#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace StepSystem
{
    /// <summary>
    /// StepEditor.csに保存されたデータの描画を担当するクラス
    /// </summary>
    [CustomEditor(typeof(StepEditor)), CanEditMultipleObjects]
    public sealed class StepEditorDrawer : Editor
    {
        private Steps m_steps = null;

        //起動時にStepEditor.csに保存されたデータを取得
        public void OnEnable()
        {
            m_steps = (target as StepEditor).GetSteps();
        }

        public override void OnInspectorGUI()
        {
            if (m_steps == null) return;

            //削除用のデータを保存ための変数
            Step removeStep = null;

            //挿入用のデータを保存ための変数
            (int index, Step content) insertStep = (-1, null);

            serializedObject.Update();

            EditorGUILayout.LabelField("●　ステップエディタ");

            //ループ条件に関するUIの描画
            using (new GUILayout.HorizontalScope())
            {
                m_steps.InfiniteLoop = EditorGUILayout.Toggle("無限ループ", m_steps.InfiniteLoop);
                if (!m_steps.InfiniteLoop)
                {
                    m_steps.LoopCount = EditorGUILayout.IntField("ループ回数", m_steps.LoopCount);
                }
            }

           　//先頭の要素を作成ためのボタン 
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("先頭の要素を作成", GUILayout.Width(100f)))
                {
                    insertStep = (0, new Step());
                }
            }


            EditorGUILayout.Space(15);

            //各状態変化に関するデータを描画
            int i = 1;
            foreach (Step step in m_steps)
            {
                EditorGUILayout.LabelField($"ステップ : {i.ToString()} ", GUILayout.Width(100f));

                GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(5));

                //時間に関するUI
                using (new GUILayout.HorizontalScope())
                {
                    EditorGUILayout.PrefixLabel("時間 秒 [ 前 実行 後 ]");

                    step.TimeData.Before = LabeledFloatField("B", step.TimeData.Before);// 実行前の待機時間
                    step.TimeData.Execution = LabeledFloatField("N", step.TimeData.Execution);// 実行時間
                    step.TimeData.After = LabeledFloatField("A", step.TimeData.After);// 実行後の待機時間
                }

                //時間に関するUI描画用の関数 
                float LabeledFloatField(string label, float val)
                {
                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.Label(label, GUILayout.Width(13f));
                        val = EditorGUILayout.FloatField(val, GUILayout.Width(EditorGUIUtility.labelWidth / 2.42f ));
                    }

                    if (val < .0f) val = .0f;
                    return val;
                }

                step.TransformAmount = EditorGUILayout.Vector3Field("移動量", step.TransformAmount);
                step.RotationAmount = EditorGUILayout.Vector3Field("回転量", step.RotationAmount);
                step.ScaleAmount = EditorGUILayout.Vector3Field("拡縮量", step.ScaleAmount);

                EditorGUILayout.Space(7);

                // 削除・挿入　ボタン
                using (new GUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("削除 －", GUILayout.Width(60f)))
                    {
                        // foreach イテレーション中に削除しない
                        removeStep = step;
                    }

                    GUILayout.FlexibleSpace();

                    if (GUILayout.Button("追加 ＋", GUILayout.Width(60f)))
                    {
                        // foreach イテレーション中に挿入しない
                        insertStep  = (i, new Step());
                    }
                }

                EditorGUILayout.Space(15);
                i++;
            }
            
            if (removeStep != null) m_steps.Remove(removeStep);

            if (insertStep.index != -1) m_steps.Insert(insertStep.index, insertStep.content);

            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(target);
        }
    }
}
# endif