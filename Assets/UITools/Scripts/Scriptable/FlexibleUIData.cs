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

    [Serializable]
    public class FlexibleTextData 
    {
        public string name;

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
        public bool hasText;

        [Header("Flexible Button Properties")]
        public List<FlexibleButtonProperties> buttonProperties;

    }
    
    [Serializable]
    public class FlexibleToggleData
    {
        public string name;

        public bool haveOwnColor;
        public bool alphaAlwaysActive;
        
        public Color toggleSelectedColor;
        public Color toggleUnSelectedColor;
        public List<Color> toggleColors;
        public Sprite toggleSprite;

    }

    public List<FlexibleTextData> flexibleUIText;
    public List<FlexibleTextMeshData> flexibleUITextMesh;
    public List<FlexibleButtonData> flexibleUIButtons;
    public List<FlexibleToggleData> flexibleUIToggles;


}
