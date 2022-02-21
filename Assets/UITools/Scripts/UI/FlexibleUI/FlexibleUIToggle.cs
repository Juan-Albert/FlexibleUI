using System;
using System.Collections;
using System.Collections.Generic;
using UITools;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FlexibleUIToggle : FlexibleUI
{
    [Serializable]
    public class ClickEvent : UnityEvent<int> { }

    private const int MINNumberOfToggles = 2;
    private const int MAXNumberOfToggles = 5;
    
    [SerializeField]
    public FlexibleEnum toggleType = new FlexibleEnum(FlexibleEnum.FlexibleEnumTypes.Toggle);
    
    [Range(MINNumberOfToggles,MAXNumberOfToggles)]
    public int toggleQuantity = MINNumberOfToggles;

    public int toggleSpace = 0;
    public Vector2 toggleSize = new Vector2(10,10);

    
    
    [HideInInspector]
    public RectTransform rectTransform;
    
    private int _currentToggle = 0;

    public int CurrentToggle
    {
        get => _currentToggle;

        set
        {
            _currentToggle = value;
            
            ChangeToggleSkin();
        }
    }
    private RectTransform _toggleContainer;
    
    [HideInInspector]
    [SerializeField]
    private List<FlexibleUIButton> _currentToggleList;
    [HideInInspector]
    [SerializeField]
    private List<FlexibleUIButton> _totalToggleList;
    
    [Space]
    [SerializeField]
    [Tooltip("Event fires when a user click on a toggle")]
    private ClickEvent m_OnToggleChangeEvent = new ClickEvent();
    public ClickEvent OnToggleChangeEvent { get => m_OnToggleChangeEvent; set => m_OnToggleChangeEvent = value; }

    protected override void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        _currentToggleList ??= new List<FlexibleUIButton>();
        _totalToggleList ??= new List<FlexibleUIButton>();
        
        Setup();
        
        base.Awake();
    }
    

    protected override void OnSkinUI()
    {
        base.OnSkinUI();

        if (_toggleContainer == null)
        {
            var togglesContainer = transform.Find("TogglesContainer");

            _toggleContainer = (RectTransform)togglesContainer;
        }

        toggleType.flexibleEnumType = FlexibleEnum.FlexibleEnumTypes.Toggle;
        //_toggleOptions ??= new List<FlexibleUIButton>();

        CheckValidToggles();
        CheckExistingToggles();
        CheckActiveToggles();

        AdjustToggleSizeAndPosition();
        
        ChangeToggleSkin();
    }

    private void Setup()
    {
        CreateToggleContainer();

        int numberOfToggles = _totalToggleList.Count;
        foreach (Transform child in _toggleContainer)
        {
            numberOfToggles++;
            if (child.gameObject.TryGetComponent(out FlexibleUIButton flexibleUIButton)
                && numberOfToggles <= toggleQuantity 
                && _totalToggleList.Exists(x => x == flexibleUIButton))
            {
                _totalToggleList.Add(flexibleUIButton);
            }
            else
            {
                numberOfToggles--;
            }
        }

        for (int i = numberOfToggles; i < MAXNumberOfToggles; i++)
        {
            FlexibleUIButton toggle = Instantiate(Resources.Load<GameObject>("UITools/FlexibleUI/Button"), _toggleContainer).GetComponent<FlexibleUIButton>();

            if (toggle != null)
            {
                _totalToggleList.Add(toggle);
            }
        }

        for (int i = 0; i < _totalToggleList.Count; i++)
        {
            var index = i;
            _totalToggleList[i].OnClickEvent.AddListener(delegate { OnToggleChanged(index); });
        }
        
    }

    private void CreateToggleContainer()
    {
        if (_toggleContainer == null)
        {
            var togglesContainer = transform.Find("TogglesContainer");

            _toggleContainer = (RectTransform)togglesContainer;
        }
        
        if (_toggleContainer != null) return;
        
        _toggleContainer = new GameObject().AddComponent<RectTransform>();
        _toggleContainer.gameObject.name = "TogglesContainer";
        _toggleContainer.SetParent(transform, false);
        _toggleContainer.anchorMin = new Vector2(0, 0);
        _toggleContainer.anchorMax = new Vector2(1, 1);
        _toggleContainer.offsetMin = new Vector2(0, 0);
        _toggleContainer.offsetMax = new Vector2(0, 0);
    }
    
    private void CheckValidToggles()
    {
        if (_totalToggleList.Exists(x => x == null))
        {
            for (int i = 0; i < _totalToggleList.Count; i++)
            {
                if (_totalToggleList[i] != null) continue;
            
                FlexibleUIButton toggle = Instantiate(Resources.Load<GameObject>("UITools/FlexibleUI/Button"), _toggleContainer).GetComponent<FlexibleUIButton>();

                if (toggle != null)
                {
                    var index = i;
                    toggle.OnClickEvent.AddListener(delegate { OnToggleChanged(index); });
                    _totalToggleList[i] = toggle;
                    toggle.transform.SetSiblingIndex(i);
                }

            }
        }

        if (_currentToggleList.Exists(x => x == null))
        {
            for (int i = 0; i < _currentToggleList.Count; i++)
            {
                if(_currentToggleList[i] != null) continue;

                _currentToggleList[i] = _totalToggleList[i];
            }
        }
    }

    private void CheckExistingToggles()
    {
        if (_totalToggleList.Count < MAXNumberOfToggles)
        {
            for (int i = _totalToggleList.Count; i < MAXNumberOfToggles; i++)
            {
                FlexibleUIButton toggle = Instantiate(Resources.Load<GameObject>("UITools/FlexibleUI/Button"), _toggleContainer).GetComponent<FlexibleUIButton>();

                if (toggle != null)
                {
                    var index = i;
                    toggle.OnClickEvent.AddListener(delegate { OnToggleChanged(index); });
                    _totalToggleList.Add(toggle);
                    toggle.gameObject.SetActive(false);
                }

            }
        }
        else if(_totalToggleList.Count > MAXNumberOfToggles)
        {
            for (int i = _totalToggleList.Count - 1; i >= toggleQuantity; i--)
            {
                _totalToggleList[i].gameObject.SetActive(false);
                _totalToggleList.RemoveAt(i);
            }
        }
    }

    private void CheckActiveToggles()
    {

        if (_currentToggleList.Count < toggleQuantity)
        {
            for (int i = _currentToggleList.Count; i < toggleQuantity; i++)
            {
                _currentToggleList.Add(_totalToggleList[i]);
                _currentToggleList[i].gameObject.SetActive(true);

            }
        }
        else
        {
            for (int i = _currentToggleList.Count - 1; i >= toggleQuantity; i--)
            {
                _currentToggleList[i].gameObject.SetActive(false);
                _currentToggleList.RemoveAt(i);
            }
        }

    }

    private void AdjustToggleSizeAndPosition()
    {
        var totalSize = toggleSize.x * toggleQuantity + toggleSpace * (toggleQuantity - 1);
        var toggleHalfRange = (totalSize - toggleSize.x) * .5f;
        for (int i = 0; i < _currentToggleList.Count; i++)
        {
            _currentToggleList[i].rectTransform.anchorMin = new Vector2(.5f, .5f);
            _currentToggleList[i].rectTransform.anchorMax = new Vector2(.5f, .5f);
            _currentToggleList[i].rectTransform.sizeDelta = toggleSize;
            _currentToggleList[i].rectTransform.anchoredPosition = new Vector2( -(toggleHalfRange - (toggleSize.x + toggleSpace) * i), 0f) ;
        }
    }

    private void ChangeToggleSkin()
    {
        if (toggleType.enumSelected != "None")
        {
            for (int i = 0; i < skinData.flexibleUIToggles.Count; i++)
            {
                if (skinData.flexibleUIToggles[i].name.Equals(toggleType.enumSelected))
                {
                    for (var j = 0; j < _currentToggleList.Count; j++)
                    {
                        if (skinData.flexibleUIToggles[i].haveOwnColor)
                        {
                            _currentToggleList[j].image.color = skinData.flexibleUIToggles[i].toggleColors[j];
                        }
                        else
                        {
                            _currentToggleList[j].image.color = _currentToggle == j
                                ? skinData.flexibleUIToggles[i].toggleSelectedColor
                                : skinData.flexibleUIToggles[i].toggleUnSelectedColor;
                        }
                        
                        
                        _currentToggleList[j].image.sprite = skinData.flexibleUIToggles[i].toggleSprite;

                        _currentToggleList[j].image.color = UIUtils.GetColorWithAlpha(
                            _currentToggleList[j].image.color,
                            skinData.flexibleUIToggles[i].alphaAlwaysActive || _currentToggle == j ? 1f : 0f);
                    }

                    break;
                }
            }
        }
    }

    private void OnToggleChanged(int toggle)
    {
        if (_currentToggle == toggle) return;
        
        _currentToggle = toggle;
        ChangeToggleSkin();
        OnToggleChangeEvent.Invoke(toggle);
    }

}
