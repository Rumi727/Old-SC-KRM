using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCKRM.Font
{
    [ExecuteAlways]
    [AddComponentMenu("Ŀ��/Font/�۲��� �ؽ��� ���� ��带 ������ ����", 0)]
    public class FontPointSetting : MonoBehaviour
    {
        [SerializeField]
        UnityEngine.Font[] font;

        void Awake()
        {
            for (int i = 0; i < font.Length; i++)
                font[i].material.mainTexture.filterMode = FilterMode.Point;
        }
    }
}