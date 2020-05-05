using System;
using System.Collections;
using System.Collections.Generic;
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
        Main,
        Small,
        Medium
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
    public class FlexibleButtonData
    {
        public string name;
        public BUTTON_TYPES type;
        
        [Header("Flexible Button Sprite")]
        public Sprite buttonSprite;
        //public SpriteState buttonSpriteState;
    
        //[Header("Flexible Button Colors")]
        //public ColorBlock buttonColorBlock;
    
        [Header("Flexible Button Uses")]
        public Color defaultColor;
        public Sprite defaultIcon;

        public Color confirmColor;
        public Sprite confirmIcon;

        public Color declineColor;
        public Sprite declineIcon;

        public Color warningColor;
        public Sprite warningIcon;
        
        public Color standardColor;
        public Sprite standardIcon;
        
        public Color rewardColor;
        public Sprite rewardIcon;
    }

    public List<FlexibleTextData> flexibleUIText;
    public List<FlexibleButtonData> flexibleUIButtons;

}
