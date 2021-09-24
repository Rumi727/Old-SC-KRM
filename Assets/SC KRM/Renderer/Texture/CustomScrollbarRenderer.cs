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
    [RequireComponent(typeof(Scrollbar))]
    [AddComponentMenu("커널/Renderer/UI/Scrollbar", 4)]
    public class CustomScrollbarRenderer : CustomRenderer
    {
        Scrollbar scrollbar;

        [SerializeField] string _normalPath = "";
        [SerializeField] string _highlightedPath = "";
        [SerializeField] string _pressedPath = "";
        [SerializeField] string _selectedPath = "";
        [SerializeField] string _disabledPath = "";
        public string normalPath { get => _normalPath; set => _normalPath = value; }
        public string highlightedPath { get => _highlightedPath; set => _highlightedPath = value; }
        public string pressedPath { get => _pressedPath; set => _pressedPath = value; }
        public string selectedPath { get => _selectedPath; set => _selectedPath = value; }
        public string disabledPath { get => _disabledPath; set => _disabledPath = value; }

        [SerializeField] CustomButtonSetting buttonSprite = new CustomButtonSetting();

        public override void Rerender()
        {
            if (scrollbar == null)
                scrollbar = GetComponent<Scrollbar>();

            if (scrollbar.image == null)
            {
                Object = null;
                return;
            }

            scrollbar.transition = Selectable.Transition.SpriteSwap;

            SpriteSetting(normalPath, buttonSprite.normalSprite);
            SpriteSetting(highlightedPath, buttonSprite.highlightedSprite);
            SpriteSetting(pressedPath, buttonSprite.pressedSprite);
            SpriteSetting(selectedPath, buttonSprite.selectedSprite);
            SpriteSetting(disabledPath, buttonSprite.disabledSprite);

            SpriteState spriteState = new SpriteState();

            scrollbar.image.sprite = buttonSprite.normalSprite.sprite;
            spriteState.highlightedSprite = buttonSprite.highlightedSprite.sprite;
            spriteState.pressedSprite = buttonSprite.pressedSprite.sprite;
            spriteState.selectedSprite = buttonSprite.selectedSprite.sprite;
            spriteState.disabledSprite = buttonSprite.disabledSprite.sprite;

            Object = buttonSprite.normalSprite.sprite;
            scrollbar.spriteState = spriteState;
        }

        public void SpriteSetting(string path, CustomSpriteSetting spriteSetting)
        {
            NameSpaceAndPath nameSpaceAndPath = ResourcesManager.GetNameSpaceAndPath(path);
            Texture2D texture;
            if (customPath)
                texture = ResourcesManager.Search<Texture2D>(ResourcePack.AssetsPath + nameSpaceAndPath.Path, nameSpaceAndPath.NameSpace);
            else
                texture = ResourcesManager.Search<Texture2D>(ResourcePack.GuiPath + nameSpaceAndPath.Path, nameSpaceAndPath.NameSpace);

            if (texture == null)
            {
                spriteSetting.sprite = null;
                return;
            }

            SpriteJsonSetting spriteJsonSetting = new SpriteJsonSetting();
            spriteJsonSetting.border = new JVector4(spriteSetting.border);
            spriteJsonSetting.pivot = new JVector2(spriteSetting.pivot);
            spriteJsonSetting.pixelsPerUnit = spriteSetting.pixelsPerUnit;
            spriteJsonSetting.rect = new JRect(spriteSetting.rect);

            if (File.Exists(path + ".json"))
                JsonManager.JsonRead(path + ".json", out spriteJsonSetting, false);

            Rect rect = JRect.JRectToRect(spriteJsonSetting.rect);
            Vector2 pivot = JVector2.JVector3ToVector3(spriteJsonSetting.pivot);
            Vector4 border = JVector4.JVector4ToVector4(spriteJsonSetting.border);

            spriteSetting.RectMaxSet(texture);
            spriteSetting.PixelsPreUnitMaxSet();
            spriteSetting.sprite = Sprite.Create(texture, rect, pivot, spriteJsonSetting.pixelsPerUnit, 1, SpriteMeshType.FullRect, border);
        }
    }
}