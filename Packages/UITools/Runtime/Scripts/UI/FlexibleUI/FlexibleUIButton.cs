using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UITools
{
    public enum ButtonMode
    {
        Default,
        Free,
        Cancel,
        Special
    }

    public enum SoundTapType
    {
        UIForward,
        UIBack
    }
    
    [RequireComponent(typeof(CanvasGroup))]
    //[RequireComponent(typeof(Button))]
    [RequireComponent(typeof(Image))]
    public class FlexibleUIButton : FlexibleUI, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {
        
        [Serializable]
        public class ClickEvent : UnityEvent { }
        
        [SerializeField]
        public FlexibleEnum buttonType = new FlexibleEnum(FlexibleEnum.FlexibleEnumTypes.Button);

        [Header("Button Config")] 
        public bool interactable = true;
        public ButtonMode buttonMode = ButtonMode.Default;
        
        [Header("Button Tween")] 
        public bool hasPressTween = true;
        public Vector3 pressed = new Vector3(0.9f, 0.9f, 1f);
        public float duration = 0.1f;
        
        [Header("Button Sound")]
        public bool hasSound = true;
        public SoundTapType soundTapType = SoundTapType.UIForward;
        
        [HideInInspector]
        public CanvasGroup canvasGroup;
        //[HideInInspector]
        //public Button button;
        [HideInInspector]
        public Image image;

        [HideInInspector]
        public RectTransform rectTransform;
        
        [HideInInspector] 
        public FlexibleUIText buttonText;

        private Tweener twButton;
        
        [SerializeField]
        [Tooltip("Event fires when a user starts to change the selection")]
        private ClickEvent m_OnClickEvent = new ClickEvent();
        public ClickEvent OnClickEvent { get { return m_OnClickEvent; } set { m_OnClickEvent = value; } }
        

        
        protected override void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            image = GetComponent<Image>();
            
#if UNITY_EDITOR

            if (buttonText == null && buttonType.enumSelected != "None")
            {
                buttonText = Instantiate(Resources.Load<GameObject>("UITools/FlexibleUI/Text")).GetComponent<FlexibleUIText>();
                buttonText.transform.SetParent(this.transform, false);
                buttonText.transform.localPosition = Vector3.zero;
                buttonText.textType.enumSelected = "None";
            }

            base.Awake();
#endif
        }
        
        /// <summary>
        /// Sets the skin configuration of the button object
        /// </summary>
        protected override void OnSkinUI()
        {
            if (buttonType.enumSelected != "None")
            {
                image.enabled = true;
                for (int i = 0; i < skinData.flexibleUIButtons.Count; i++)
                {
                
                    if (skinData.flexibleUIButtons[i].name.Equals(buttonType.enumSelected))
                    {

                        if(buttonText != null)
                            buttonText.gameObject.SetActive(skinData.flexibleUIButtons[i].hasText);


                        for (int j = 0; j < skinData.flexibleUIButtons[i].buttonProperties.Count; j++)
                        {
                            if (skinData.flexibleUIButtons[i].buttonProperties[j].buttonMode.Equals(buttonMode))
                            {
                                image.sprite = skinData.flexibleUIButtons[i].buttonProperties[j].buttonSprite;
                                image.type = skinData.flexibleUIButtons[i].buttonProperties[j].imageType;
                                break;
                            }
                        }
                        
                        break;
                    }
                }
            }
            base.OnSkinUI();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!interactable) return;
            
            if (hasPressTween)
            {
                if(twButton != null && twButton.IsActive())
                    twButton.Kill();
                
                transform.localScale = Vector3.one;
                twButton = this.transform.DOScale(pressed, duration).SetEase(Ease.OutSine);
            }

        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!interactable) return;
            
            if (hasPressTween)
            {
                if(twButton != null && twButton.IsActive())
                    twButton.Kill();
                
                transform.localScale = pressed;
                twButton = this.transform.DOScale(Vector3.one, duration).SetEase(Ease.InSine);
            }
            
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!interactable) return;
            
            if (hasSound)
            {
                //AkSoundEngine.PostEvent("Tap_Generic", this.gameObject);
                //if (soundTapType == SoundTapType.UIForward)
                //{
                //    SoundManager.instance.PlayClip(AudioBank.instance.tapButton, false);
                //}
                //else
                //{
                //    SoundManager.instance.PlayClip(AudioBank.instance.tapClose, false);
                //}
            }
            
            OnClickEvent?.Invoke();
        }
    }

}

