using SCKRM.InspectorEditor;
using SCKRM.Resources;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SCKRM.Renderer
{
    [RequireComponent(typeof(Slider))]
    [AddComponentMenu("커널/Renderer/UI/Slider", 2)]
    public class CustomSliderRenderer : CustomRenderer
    {
        Slider slider;

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
            if (slider == null)
                slider = GetComponent<Slider>();

            if (slider.image == null)
            {
                Object = null;
                return;
            }

            slider.transition = Selectable.Transition.SpriteSwap;

            SpriteSetting(normalPath, buttonSprite.normalSprite);
            SpriteSetting(highlightedPath, buttonSprite.highlightedSprite);
            SpriteSetting(pressedPath, buttonSprite.pressedSprite);
            SpriteSetting(selectedPath, buttonSprite.selectedSprite);
            SpriteSetting(disabledPath, buttonSprite.disabledSprite);

            SpriteState spriteState = new SpriteState();

            slider.image.sprite = buttonSprite.normalSprite.sprite;
            spriteState.highlightedSprite = buttonSprite.highlightedSprite.sprite;
            spriteState.pressedSprite = buttonSprite.pressedSprite.sprite;
            spriteState.selectedSprite = buttonSprite.selectedSprite.sprite;
            spriteState.disabledSprite = buttonSprite.disabledSprite.sprite;

            Object = buttonSprite.normalSprite.sprite;
            slider.spriteState = spriteState;
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

            spriteSetting.RectMaxSet(texture);
            spriteSetting.PixelsPreUnitMaxSet();
            spriteSetting.sprite = Sprite.Create(texture, spriteSetting.rect, spriteSetting.pivot, spriteSetting.pixelsPerUnit, 1, SpriteMeshType.FullRect, spriteSetting.border);
        }
    }
}