using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCKRM.Resources.UI
{
    [AddComponentMenu("Ŀ��/Resources/���ҽ��� ����Ʈ/�� ���� ���� �Լ�", 3)]
    public class OpenPackFolder : MonoBehaviour
    {
        public void Open() => ResourcesManager.OpenResourcePackFolder();
    }
}