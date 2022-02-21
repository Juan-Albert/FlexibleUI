using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UITools
{


    [RequireComponent(typeof(TextMeshProUGUI))]
    public class FlexibleUITextMesh : FlexibleUI
    {
        [SerializeField]
        public FlexibleEnum textType = new FlexibleEnum(FlexibleEnum.FlexibleEnumTypes.TextMesh);
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
            if (textType.enumSelected != "None")
            {
                if(textLabel == null)
                    textLabel = GetComponent<TextMeshProUGUI>();
                
                for (int i = 0; i < skinData.flexibleUIText.Count; i++)
                {
                    if (skinData.flexibleUIText[i].name.Equals(textType.enumSelected))
                    {
                    
                        textLabel.font = skinData.flexibleUITextMesh[i].fontAsset;
                        textLabel.fontSize = skinData.flexibleUITextMesh[i].fontSize;
                        textLabel.fontStyle = skinData.flexibleUITextMesh[i].fontStyle;
                    
                        //textLabel.lineSpacing = skinData.flexibleUIText[i].lineSpacing;
                        textLabel.text = UIUtils.ModifyText(textLabel.text, skinData.flexibleUIText[i].modifier);
                        
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
        
    }

}
