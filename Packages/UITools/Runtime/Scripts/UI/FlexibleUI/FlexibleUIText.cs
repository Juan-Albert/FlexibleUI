﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UITools
{
    public enum TextModifier
    {
        None,
        ToUppercase,
        ToLowercase
    }

    [RequireComponent(typeof(Text))]
    public class FlexibleUIText : FlexibleUI
    {
        public FlexibleEnum textType = new FlexibleEnum(FlexibleEnum.FlexibleEnumTypes.Text);
        public bool changeColor = true;
        public bool changeMaxSize = false;

        
        private Text textLabel;
    
        
        protected override void Awake()
        {
#if UNITY_EDITOR
            textLabel = GetComponent<Text>();
            base.Awake();
#endif
        }
        
        /// <summary>
        /// Sets the skin configuration of the text object
        /// </summary>
        protected override void OnSkinUI()
        {
            if (textType.enumSelected != "None")
            {
                if(textLabel == null)
                    textLabel = GetComponent<Text>();
                
                for (int i = 0; i < skinData.flexibleUIText.Count; i++)
                {
                    if (!skinData.flexibleUIText[i].name.Equals(textType.enumSelected)) continue;
                    
                    textLabel.font = skinData.flexibleUIText[i].fontAsset;
                    textLabel.fontSize = skinData.flexibleUIText[i].fontSize;
                    textLabel.fontStyle = skinData.flexibleUIText[i].fontStyle;
                    
                    //textLabel.lineSpacing = skinData.flexibleUIText[i].lineSpacing;
                    textLabel.text = ModifyText(textLabel.text, skinData.flexibleUIText[i].modifier);
                        
                    textLabel.resizeTextForBestFit = skinData.flexibleUIText[i].hasBestFit;
                        
                    if(changeMaxSize)
                        textLabel.resizeTextMaxSize = skinData.flexibleUIText[i].fontSize;
                    
                    if(changeColor)
                        textLabel.color = skinData.flexibleUIText[i].color;

                    textLabel.material = skinData.flexibleUIText[i].hasOutline ? skinData.flexibleUIText[i].outlineMaterial : new Material(Shader.Find("UI/Default"));
                    //textOutline.enabled = skinData.flexibleUIText[i].hasOutline;
                    //textOutline.effectColor = skinData.flexibleUIText[i].outlineColor;
                    //textOutline.effectDistance = new Vector2( skinData.flexibleUIText[i].outlineX,  skinData.flexibleUIText[i].outlineY);
                }
            }
            
            base.OnSkinUI();
        }
        
        /// <summary>
        /// Apply a text modifier to the text parameter
        /// </summary>
        protected string ModifyText(string text, TextModifier modifier)
        {
            if (!string.IsNullOrEmpty(text))
            {
                if (modifier == TextModifier.None) return text;
                if (modifier == TextModifier.ToLowercase) return text.ToLower();
                if (modifier == TextModifier.ToUppercase) return text.ToUpper();
            }
            return text;
            
        }
    }

}

