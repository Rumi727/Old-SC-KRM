using SCKRM.InspectorEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCKRM.Renderer
{
    [ExecuteAlways]
    [AddComponentMenu("Ŀ��/Renderer/Renderer", 0)]
    public class CustomRenderer : MonoBehaviour
    {
        public UnityEngine.Object Object;

        void Awake()
        {
            if (!Application.isPlaying || Object == null)
                Rerender();
        }

        public virtual void Rerender()
        {
            
        }
    }

    [System.Serializable]
    public class CustomSpriteSetting
    {
        Sprite _sprite;
        public Sprite sprite { get => _sprite; set => _sprite = value; }



        [SerializeField, SetName("��������Ʈ �ڸ���")] Rect _rect = Rect.zero;
        public Rect rect { get => _rect; set => _rect = value; }

        [SerializeField] Vector4 _border = Vector4.zero;
        public Vector4 border { get => _border; set => _border = value; }

        
        [Space(10)]


        [SerializeField, SetName("��������Ʈ �߽�")] Vector2 _pivot = new Vector2(0.5f, 0.5f);
        public Vector2 pivot { get => _pivot; set => _pivot = value; }

        [SerializeField] float _pixelsPerUnit = 100;
        public float pixelsPerUnit { get => _pixelsPerUnit; set => _pixelsPerUnit = value; }

        public void RectMaxSet(Texture2D texture)
        {
            if (_rect.width < 1)
                _rect.width = 1;
            if (_rect.width > texture.width)
                _rect.width = texture.width;
            if (_rect.height < 1)
                _rect.height = 1;
            if (_rect.height > texture.height)
                _rect.height = texture.height;

            if (_rect.x < 0)
                _rect.x = 0;
            if (_rect.x > texture.width - _rect.width)
                _rect.x = texture.width - _rect.width;
            if (_rect.y < 0)
                _rect.y = 0;
            if (_rect.y > texture.height - _rect.height)
                _rect.y = texture.height - _rect.height;
        }

        public void PixelsPreUnitMaxSet()
        {
            if (_pixelsPerUnit < 0.01f)
                _pixelsPerUnit = 0.01f;
        }
    }

    [System.Serializable]
    public class CustomButtonSetting
    {
        [SerializeField, SetName("�Ϲ� ��������Ʈ ����")] CustomSpriteSetting _normalSprite = new CustomSpriteSetting();
        public CustomSpriteSetting normalSprite { get => _normalSprite; set => _normalSprite = value; }
        
        [SerializeField, SetName("���̶���Ʈ�� ��������Ʈ ����")] CustomSpriteSetting _highlightedSprite = new CustomSpriteSetting();
        public CustomSpriteSetting highlightedSprite { get => _highlightedSprite; set => _highlightedSprite = value; }
        
        [SerializeField, SetName("���� ��������Ʈ ����")] CustomSpriteSetting _pressedSprite = new CustomSpriteSetting();
        public CustomSpriteSetting pressedSprite { get => _pressedSprite; set => _pressedSprite = value; }
        
        [SerializeField, SetName("���õ� ��������Ʈ ����")] CustomSpriteSetting _selectedSprite = new CustomSpriteSetting();
        public CustomSpriteSetting selectedSprite { get => _selectedSprite; set => _selectedSprite = value; }
        
        [SerializeField, SetName("��Ȱ��ȭ�� ��������Ʈ ����")] CustomSpriteSetting _disabledSprite = new CustomSpriteSetting();
        public CustomSpriteSetting disabledSprite { get => _disabledSprite; set => _disabledSprite = value; }
    }
}