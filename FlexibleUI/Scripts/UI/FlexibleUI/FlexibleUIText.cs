using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CamperoEditorTool
{
    public enum TextModifier
    {
        None,
        ToUppercase,
        ToLowercase
    }

    [RequireComponent(typeof(Text))]
    [RequireComponent(typeof(Outline))]
    public class FlexibleUIText : FlexibleUI
    {
        public FlexibleUIData.TEXT_TYPES textTypes = FlexibleUIData.TEXT_TYPES.Body;
        public bool changeColor = true;


        private Text textLabel;
        private Outline textOutline;
    
        [ContextMenu("Reset")]
        protected override void Awake()
        {
#if UNITY_EDITOR
            textLabel = GetComponent<Text>();
            textOutline = GetComponent<Outline>();
            base.Awake();
#endif
        }
        [ContextMenu("Apply values")]
        protected override void OnSkinUI()
        {
            if (textTypes != FlexibleUIData.TEXT_TYPES.None)
            {
                for (int i = 0; i < skinData.flexibleUIText.Count; i++)
                {
                    if (skinData.flexibleUIText[i].type.Equals(textTypes))
                    {
                    
                        textLabel.font = skinData.flexibleUIText[i].fontAsset;
                        textLabel.fontSize = skinData.flexibleUIText[i].fontSize;
                        textLabel.fontStyle = skinData.flexibleUIText[i].fontStyle;
                    
                        //textLabel.lineSpacing = skinData.flexibleUIText[i].lineSpacing;
                        textLabel.text = ModifyText(textLabel.text, skinData.flexibleUIText[i].modifier);
                        
                        textLabel.resizeTextForBestFit = skinData.flexibleUIText[i].hasBestFit;
                        textLabel.resizeTextMaxSize = skinData.flexibleUIText[i].fontSize;
                    
                        if(changeColor)
                            textLabel.color = skinData.flexibleUIText[i].color;

                        textOutline.enabled = skinData.flexibleUIText[i].hasOutline;
                        textOutline.effectColor = skinData.flexibleUIText[i].outlineColor;
                        textOutline.effectDistance = new Vector2( skinData.flexibleUIText[i].outlineX,  skinData.flexibleUIText[i].outlineY);
                    
                    }
                }
            }
            
            base.OnSkinUI();
        }
        
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

