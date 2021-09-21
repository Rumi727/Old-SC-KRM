using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCKRM.Resources.UI
{
    [AddComponentMenu("커널/Resources/리소스팩 리스트/팩 폴더 열기 함수", 3)]
    public class OpenPackFolder : MonoBehaviour
    {
        public void Open() => ResourcesManager.OpenResourcePackFolder();
    }
}