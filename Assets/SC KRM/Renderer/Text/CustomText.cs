using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCKRM.Renderer
{
    [AddComponentMenu("커널/Renderer/Text/Text", 0)]
    public class CustomText : MonoBehaviour
    {
        [SerializeField] string _jsonKey = "";
        public string jsonKey { get => _jsonKey; set => _jsonKey = value; }

        public virtual void Rerender()
        {

        }
    }
}