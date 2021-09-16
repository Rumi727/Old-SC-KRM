using SCKRM.InspectorEditor;
using SCKRM.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCKRM.Loading
{
    [AddComponentMenu("Ŀ��/�ε�/�ε� ����", 0)]
    public class LoadingBarManager : MonoBehaviour
    {
        public static LoadingBarManager instance { get; private set; }

        
        [SerializeField, SetName("�ε� �ٰ� ������ Ʈ������")] Transform parentTransform;

        void Awake()
        {
            if (instance != null)
                Destroy(gameObject);
            
            instance = this;
        }

        //�ε� �ٸ� �����մϴ�
        public static LoadingBar Create() => ObjectPoolingSystem.ObjectCreate("Loading Bar", instance.parentTransform).GetComponent<LoadingBar>();
    }
}