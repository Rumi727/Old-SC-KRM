using SCKRM.InspectorEditor;
using SCKRM.Resources;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SCKRM.Renderer
{
    [RequireComponent(typeof(Image))]
    [AddComponentMenu("커널/Renderer/UI/Image", 1)]
    public class CustomImageRenderer : CustomRenderer
    {
        Image image;

        [SerializeField] string _path = "";
        public string path { get => _path; set => _path = value; }

        [SerializeField, SetName("스프라이트 설정")] CustomSpriteSetting customSprite = new CustomSpriteSetting();

        public override void Rerender()
        {
            if (image == null)
                image = GetComponent<Image>();

            if (image == null)
            {
                Object = null;
                return;
            }

            NameSpaceAndPath nameSpaceAndPath = ResourcesManager.GetNameSpaceAndPath(path);
            Texture2D texture = ResourcesManager.Search<Texture2D>(ResourcePack.GuiPath + nameSpaceAndPath.Path, nameSpaceAndPath.NameSpace);

            if (texture == null)
            {
                Object = null;
                image.sprite = null;
                return;
            }

            customSprite.RectMaxSet(texture);
            customSprite.PixelsPreUnitMaxSet();
            customSprite.sprite = Sprite.Create(texture, customSprite.rect, customSprite.pivot, customSprite.pixelsPerUnit, 1, SpriteMeshType.Tight, customSprite.border);

            image.sprite = customSprite.sprite;
            Object = customSprite.sprite;
        }
    }
}