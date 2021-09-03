using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UITools
{


    [RequireComponent(typeof(TextMeshProUGUI))]
    public class FlexibleUITextMesh : FlexibleUI
    {
        public FlexibleUIData.TEXT_TYPES textTypes = FlexibleUIData.TEXT_TYPES.Body;
        public bool changeColor = true;
        public bool changeMaxSize = false;

        private TextMeshProUGUI textLabel;
    
        
        protected override void Awake()
        {
#if UNITY_EDITOR
            textLabel = GetComponent<TextMeshProUGUI>();
            base.Awake();
#endif
        }
        
        /// <summary>
        /// Sets the skin configuration of the text object
        /// </summary>
        protected override void OnSkinUI()
        {
            if (textTypes != FlexibleUIData.TEXT_TYPES.None)
            {
                if(textLabel == null)
                    textLabel = GetComponent<TextMeshProUGUI>();
                
                for (int i = 0; i < skinData.flexibleUIText.Count; i++)
                {
                    if (skinData.flexibleUIText[i].type.Equals(textTypes))
                    {
                    
                        textLabel.font = skinData.flexibleUITextMesh[i].fontAsset;
                        textLabel.fontSize = skinData.flexibleUITextMesh[i].fontSize;
                        textLabel.fontStyle = skinData.flexibleUITextMesh[i].fontStyle;
                    
                        //textLabel.lineSpacing = skinData.flexibleUIText[i].lineSpacing;
                        textLabel.text = ModifyText(textLabel.text, skinData.flexibleUIText[i].modifier);
                        
                        textLabel.autoSizeTextContainer = skinData.flexibleUIText[i].hasBestFit;
                        
                        if(changeMaxSize)
                            textLabel.fontSizeMax = skinData.flexibleUIText[i].fontSize;
                    
                        if(changeColor)
                            textLabel.color = skinData.flexibleUIText[i].color;

                        textLabel.material = skinData.flexibleUIText[i].hasOutline ? skinData.flexibleUIText[i].outlineMaterial : new Material(Shader.Find("UI/Default"));
                        //textOutline.enabled = skinData.flexibleUIText[i].hasOutline;
                        //textOutline.effectColor = skinData.flexibleUIText[i].outlineColor;
                        //textOutline.effectDistance = new Vector2( skinData.flexibleUIText[i].outlineX,  skinData.flexibleUIText[i].outlineY);
                    
                    }
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
