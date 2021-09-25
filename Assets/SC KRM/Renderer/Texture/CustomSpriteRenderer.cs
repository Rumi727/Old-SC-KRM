using SCKRM.InspectorEditor;
using SCKRM.Json;
using SCKRM.Resources;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SCKRM.Renderer
{
    [RequireComponent(typeof(SpriteRenderer))]
    [AddComponentMenu("커널/Renderer/Sprite Renderer", 1)]
    public class CustomSpriteRenderer : CustomRenderer
    {
        SpriteRenderer spriteRenderer;

        [SerializeField] string _path = "";
        [SerializeField] Vector2 _size;
        public string path { get => _path; set => _path = value; }
        public Vector2 size { get => _size; set => _size = value; }

        [SerializeField, SetName("스프라이트 설정")] CustomSpriteSetting customSprite = new CustomSpriteSetting();

        public override void Rerender()
        {
            if (spriteRenderer == null)
                spriteRenderer = GetComponent<SpriteRenderer>();

            NameSpaceAndPath nameSpaceAndPath = ResourcesManager.GetNameSpaceAndPath(path);
            Texture2D texture;
            if (customPath)
                texture = ResourcesManager.Search<Texture2D>(ResourcePack.AssetsPath + nameSpaceAndPath.Path, nameSpaceAndPath.NameSpace);
            else
                texture = ResourcesManager.Search<Texture2D>(ResourcePack.TexturePath + nameSpaceAndPath.Path, nameSpaceAndPath.NameSpace);

            if (texture == null)
            {
                Object = null;
                spriteRenderer.sprite = null;
                return;
            }

            customSprite.RectMaxSet(texture);
            customSprite.PixelsPreUnitMaxSet();

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

            customSprite.sprite = Sprite.Create(texture, rect, pivot, spriteJsonSetting.pixelsPerUnit, 1, SpriteMeshType.FullRect, border);

            spriteRenderer.sprite = customSprite.sprite;
            spriteRenderer.size = size;
            Object = customSprite.sprite;
        }
    }
}