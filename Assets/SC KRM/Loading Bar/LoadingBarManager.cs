using SCKRM.InspectorEditor;
using SCKRM.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCKRM.Loading
{
    [AddComponentMenu("커널/로딩/로딩 설정", 0)]
    public class LoadingBarManager : MonoBehaviour
    {
        public static LoadingBarManager instance { get; private set; }

        
        [SerializeField, SetName("로딩 바가 생성될 트랜스폼")] Transform parentTransform;

        void Awake()
        {
            if (instance != null)
                Destroy(gameObject);
            
            instance = this;
        }

        //로딩 바를 생성합니다
        public static LoadingBar Create() => ObjectPoolingSystem.ObjectCreate("Loading Bar", instance.parentTransform).GetComponent<LoadingBar>();
    }
}