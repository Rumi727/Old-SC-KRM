using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Threading.Tasks;

namespace SCKRM.InspectorEditor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Kernel), true)]
    public class KernelEditor : Editor
    {
        bool repaint = false;

        void OnEnable()
        {
            if (Application.isPlaying)
            {
                repaint = true;
                Repainter();
            }
        }

        void OnDisable() => repaint = false;

        async void Repainter()
        {
            while (repaint)
            {
                Repaint();
                await Task.Delay(100);
            }
        }

        public override void OnInspectorGUI()
        {
            if (Application.isPlaying)
            {
                EditorGUILayout.LabelField("델타 타임 - " + Kernel.deltaTime);
                EditorGUILayout.LabelField("FPS 델타 타임 - " + Kernel.fpsDeltaTime);
                EditorGUILayout.LabelField("스케일 되지 않은 델타 타임 - " + Kernel.unscaledDeltaTime);
                EditorGUILayout.LabelField("스케일 되지 않은 FPS 델타 타임 - " + Kernel.fpsUnscaledDeltaTime);

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("FPS - " + Kernel.fps);

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("데이터 경로 - " + Kernel.dataPath);
                EditorGUILayout.LabelField("스트리밍 에셋 경로 - " + Kernel.streamingAssetsPath);
                EditorGUILayout.LabelField("영구 데이터 경로 - " + Kernel.persistentDataPath);

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("회사 이름 - " + Kernel.companyName);
                EditorGUILayout.LabelField("제품 이름 - " + Kernel.productName);
                EditorGUILayout.LabelField("버전 - " + Kernel.version);

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("실행 중인 플랫폼 - " + Kernel.platform);

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("FPS 제한 - " + Kernel.fpsLimit);
                EditorGUILayout.LabelField("포커스가 아닐 때 FPS 제한 - " + Kernel.notFocusFpsLimit);
                EditorGUILayout.LabelField("AFK 상태일 때 FPS 제한 - " + Kernel.afkFpsLimit);

                if (Kernel.isAFK)
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("AFK 상태");
                }

                EditorGUILayout.Space();
                Kernel.gameSpeed = EditorGUILayout.FloatField("게임 속도", Kernel.gameSpeed);
            }
            else
            {
                EditorGUILayout.LabelField("스트리밍 에셋 경로 - " + Kernel.streamingAssetsPath);
                EditorGUILayout.LabelField("실행 중인 플랫폼 - " + Kernel.platform);

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("FPS 제한 - " + Kernel.fpsLimit);
                EditorGUILayout.LabelField("포커스가 아닐 때 FPS 제한 - " + Kernel.notFocusFpsLimit);
                EditorGUILayout.LabelField("AFK 상태일 때 FPS 제한 - " + Kernel.afkFpsLimit);
            }
        }
    }
}