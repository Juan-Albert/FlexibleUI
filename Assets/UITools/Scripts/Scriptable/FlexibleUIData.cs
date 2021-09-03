using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UITools;

[CreateAssetMenu(fileName = "FlexibleUIData", menuName = "FlexibleUIData", order=3)]
public class FlexibleUIData : ScriptableObject
{
    public enum TEXT_TYPES 
    {
        None,
        Head,
        Body,
        Title,
        Button
    }
    
    public enum BUTTON_TYPES 
    {
        None,
        Rectangle,
        Round
        
    }
    
    [Serializable]
    public class FlexibleTextData 
    {
        public string name;
        public TEXT_TYPES type;

        [Header("Flexible Text Parameters")]
        public Font fontAsset;
        public FontStyle fontStyle;
        public int fontSize;
        public int lineSpacing;
        public TextModifier modifier;
        public Color color;
        public bool hasBestFit;
        
        [Header("Flexible Text Outline")]
        public bool hasOutline;
        public Material outlineMaterial;
        //public Color outlineColor;
        //public float outlineX;
        //public float outlineY;
    }
    
    [Serializable]
    public class FlexibleTextMeshData 
    {
        public string name;
        public TEXT_TYPES type;

        [Header("Flexible Text Parameters")]
        public TMP_FontAsset fontAsset;
        public FontStyles fontStyle;
        public int fontSize;
        public int lineSpacing;
        public TextModifier modifier;
        public Color color;
        public bool autoSize;
        
        //[Header("Flexible Text Outline")]
        //public bool hasOutline;
        //public Material outlineMaterial;
        //public Color outlineColor;
        //public float outlineX;
        //public float outlineY;
    }

    [Serializable]
    public class FlexibleButtonProperties
    {
        public ButtonMode buttonMode;
        public Sprite buttonSprite;
        public Image.Type imageType;
    }

    [Serializable]
    public class FlexibleButtonData
    {
        public string name;
        public BUTTON_TYPES type;
        public bool hasText;

        [Header("Flexible Button Properties")]
        public List<FlexibleButtonProperties> buttonProperties;

    }

    public List<FlexibleTextData> flexibleUIText;
    public List<FlexibleTextMeshData> flexibleUITextMesh;
    public List<FlexibleButtonData> flexibleUIButtons;

}
