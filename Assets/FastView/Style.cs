using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.TextCore.Text;
using FontStyles = UnityEngine.TextCore.Text.FontStyles;

namespace FastView
{
    [CreateAssetMenu(menuName = "FastView/Style", fileName = "New Style")]
    public class Style : ScriptableObject
    {
        public FontAsset fontAsset;
        public FontStyles fontStyles;
        public TextAlignmentOptions textAlignment;
        public float fontSize;
        public float width;
        public float height;
        public Color contentColor;
        public Color backgroundColor;
        public Texture2D icon;
        public Texture2D backgroundTexture;
        public Style onHover;
        public Style onClicked;
    }
}