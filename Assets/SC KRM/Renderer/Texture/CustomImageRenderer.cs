using SCKRM.InspectorEditor;
using SCKRM.Json;
using SCKRM.Resources;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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

            NameSpaceAndPath nameSpaceAndPath = ResourcesManager.GetNameSpaceAndPath(path);
            Texture2D texture;
            if (customPath)
                texture = ResourcesManager.Search<Texture2D>(ResourcePack.AssetsPath + nameSpaceAndPath.Path, nameSpaceAndPath.NameSpace);
            else
                texture = ResourcesManager.Search<Texture2D>(ResourcePack.GuiPath + nameSpaceAndPath.Path, nameSpaceAndPath.NameSpace);

            if (texture == null)
            {
                Object = null;
                image.sprite = null;
                return;
            }

            SpriteJsonSetting spriteJsonSetting = new SpriteJsonSetting();
            spriteJsonSetting.border = new JVector4(customSprite.border);
            spriteJsonSetting.pivot = new JVector2(customSprite.pivot);
            spriteJsonSetting.pixelsPerUnit = customSprite.pixelsPerUnit;
            spriteJsonSetting.rect = new JRect(customSprite.rect);

            if (File.Exists(path + ".json"))
                JsonManager.JsonRead(path + ".json", out spriteJsonSetting, false);

            Rect rect = JRect.JRectToRect(spriteJsonSetting.rect);
            Vector2 pivot = JVector2.JVector3ToVector3(spriteJsonSetting.pivot);
            Vector4 border = JVector4.JVector4ToVector4(spriteJsonSetting.border);

            customSprite.RectMaxSet(texture);
            customSprite.PixelsPreUnitMaxSet();
            customSprite.sprite = Sprite.Create(texture, rect, pivot, spriteJsonSetting.pixelsPerUnit, 1, SpriteMeshType.FullRect, border);

            image.sprite = customSprite.sprite;
            Object = customSprite.sprite;
        }
    }
}