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
        Confirm,
        Decline,
        Warning,
        Standard,
        Reward
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
        
        public FlexibleUIData.BUTTON_TYPES buttonTypes;
        
        [Header("Button Config")] 

        public ButtonMode buttonMode= ButtonMode.Default;

        public Selectable.Transition buttonTransition = Selectable.Transition.None;
        public bool imageByColor;

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

        private Tweener twButton;
        
        [SerializeField]
        [Tooltip("Event fires when a user starts to change the selection")]
        private ClickEvent m_OnClickEvent = new ClickEvent();
        public ClickEvent OnClickEvent { get { return m_OnClickEvent; } set { m_OnClickEvent = value; } }
        

        [ContextMenu("Reset")]
        protected override void Awake()
        {
#if UNITY_EDITOR
            canvasGroup = GetComponent<CanvasGroup>();
            image = GetComponent<Image>();
            //button = GetComponent<Button>();

            base.Awake();
#endif
        }
        [ContextMenu("Apply values")]
        protected override void OnSkinUI()
        {
            if (buttonTypes != FlexibleUIData.BUTTON_TYPES.None)
            {
                image.enabled = true;
                for (int i = 0; i < skinData.flexibleUIButtons.Count; i++)
                {
                
                    if (skinData.flexibleUIButtons[i].type.Equals(buttonTypes))
                    {
                        //button.targetGraphic = image;

                        image.sprite = skinData.flexibleUIButtons[i].buttonSprite;
                        image.type = Image.Type.Sliced;
/*
                        switch (buttonTransition)
                        {
                            case Selectable.Transition.None:
                                button.transition = buttonTransition;
                                break;
                            
                            case Selectable.Transition.ColorTint:
                                button.transition = buttonTransition;
                                button.colors = skinData.flexibleUIButtons[i].buttonColorBlock;
                                break;
                            
                            case Selectable.Transition.SpriteSwap:
                                button.transition = buttonTransition;
                                button.spriteState = skinData.flexibleUIButtons[i].buttonSpriteState;
                                break;
                        }
*/

                        switch (buttonMode)
                        {
                            case ButtonMode.Default:
                                if (imageByColor)
                                {
                                    image.color = skinData.flexibleUIButtons[i].defaultColor;
                                    image.sprite = skinData.flexibleUIButtons[i].defaultIcon;
                                }
                                else
                                {
                                    image.color = Color.white;
                                    image.sprite = skinData.flexibleUIButtons[i].defaultIcon;
                                }

                                break;

                            case ButtonMode.Confirm:
                                if (imageByColor)
                                {
                                    image.color = skinData.flexibleUIButtons[i].confirmColor;
                                    image.sprite = skinData.flexibleUIButtons[i].defaultIcon;
                                }
                                else
                                {
                                    image.color = Color.white;
                                    image.sprite = skinData.flexibleUIButtons[i].confirmIcon;
                                }

                                break;

                            case ButtonMode.Decline:
                                if (imageByColor)
                                {
                                    image.color = skinData.flexibleUIButtons[i].declineColor;
                                    image.sprite = skinData.flexibleUIButtons[i].defaultIcon;
                                }
                                else
                                {
                                    image.color = Color.white;
                                    image.sprite = skinData.flexibleUIButtons[i].declineIcon;
                                }

                                break;

                            case ButtonMode.Warning:
                                if (imageByColor)
                                {
                                    image.color = skinData.flexibleUIButtons[i].warningColor;
                                    image.sprite = skinData.flexibleUIButtons[i].defaultIcon;
                                }
                                else
                                {
                                    image.color = Color.white;
                                    image.sprite = skinData.flexibleUIButtons[i].warningIcon;
                                }

                                break;
                            
                            case ButtonMode.Standard:
                                if (imageByColor)
                                {
                                    image.color = skinData.flexibleUIButtons[i].standardColor;
                                    image.sprite = skinData.flexibleUIButtons[i].defaultIcon;
                                }
                                else
                                {
                                    image.color = Color.white;
                                    image.sprite = skinData.flexibleUIButtons[i].standardIcon;
                                }

                                break;
                            
                            case ButtonMode.Reward:
                                if (imageByColor)
                                {
                                    image.color = skinData.flexibleUIButtons[i].rewardColor;
                                    image.sprite = skinData.flexibleUIButtons[i].defaultIcon;
                                }
                                else
                                {
                                    image.color = Color.white;
                                    image.sprite = skinData.flexibleUIButtons[i].rewardIcon;
                                }
                                break;
                            }
                        break;
                    }
                }
            }
            base.OnSkinUI();

        }

        public void OnPointerDown(PointerEventData eventData)
        {            

            if (hasPressTween)
            {
                twButton?.Kill();
                twButton = this.transform.DOScale(pressed, duration).SetEase(Ease.OutSine);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (hasPressTween)
            {
                twButton?.Kill();
                twButton = this.transform.DOScale(Vector3.one, duration).SetEase(Ease.InSine);
            }

            OnClickEvent?.Invoke();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
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
        }
    }

}

