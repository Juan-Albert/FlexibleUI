using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UITools
{
    
    public class ScrollSnapBase : MonoBehaviour, IBeginDragHandler, IDragHandler, IScrollSnap
    {
        internal RectTransform panelDimensions;
        internal RectTransform _screensContainer;
        internal bool _isVertical;

        internal int _screens = 1;
        internal float _scrollStartPosition;
        internal float _childSize;
        private float _childPos, _maskSize;
        internal Vector2 _childAnchorPoint;
        internal ScrollRect _scroll_rect;
        internal Vector2 _lerp_target;
        internal bool _lerp;
        internal bool _pointerDown = false;
        internal bool _settled = true;
        internal Vector2 _startPosition = new Vector3();
        [Tooltip("The currently active page")]
        internal int _currentScreen;
        internal int _previousScreen;
        internal bool _moveStarted;
        private int _bottomItem, _topItem;
        
        [Serializable]
        public class SelectionChangeStartEvent : UnityEvent { }
        [Serializable]
        public class SelectionPageChangedEvent : UnityEvent<int> { }
        [Serializable]
        public class SelectionChangeEndEvent : UnityEvent<int> { }
        
        [Tooltip("The screen / page to start the control on\n*Note, this is a 0 indexed array")]
        [SerializeField]
        public int startingScreen = 0;

        [Tooltip("Button to go to the previous page. (optional)")]
        public GameObject prevButton;

        [Tooltip("Button to go to the next page. (optional)")]
        public GameObject nextButton;

        [Tooltip("Transition speed between pages. (optional)")]
        public float transitionSpeed = 7.5f;
        
        [Tooltip("Fast Swipe makes swiping page next / previous (optional)")]
        public Boolean useFastSwipe = false;

        [Tooltip("Offset for how far a swipe has to travel to initiate a page change (optional)")]
        public int fastSwipeThreshold = 100;

        [Tooltip("Speed at which the ScrollRect will keep scrolling before slowing down and stopping (optional)")]
        public int swipeVelocityThreshold = 200;
        
        public int CurrentPage
        {
            get => _currentScreen;

            internal set
            {
                if (_screensContainer == null)
                    Setup();
                if ((value == _currentScreen || value < 0 || value >= _screensContainer.childCount) &&
                    (value != 0 || _screensContainer.childCount != 0)) return;
                
                _previousScreen = _currentScreen;
                _currentScreen = value;
                //if (MaskArea) UpdateVisible();
                if (!_lerp) ScreenChange();
                OnCurrentScreenChange(_currentScreen);
            }
        }
        
        [Tooltip("By default the container will lerp to the start when enabled in the scene, this option overrides this and forces it to simply jump without lerping")]
        public bool jumpOnEnable = false;

        [Tooltip("By default the container will return to the original starting page when enabled, this option overrides this behaviour and stays on the current selection")]
        public bool restartOnEnable = false;

        [Tooltip("(Experimental)\nBy default, child array objects will use the parent transform\nHowever you can disable this for some interesting effects")]
        public bool useParentTransform = true;

        [Space]
        [SerializeField]
        [Tooltip("Event fires when a user starts to change the selection")]
        private SelectionChangeStartEvent m_OnSelectionChangeStartEvent = new SelectionChangeStartEvent();
        public SelectionChangeStartEvent OnSelectionChangeStartEvent { get { return m_OnSelectionChangeStartEvent; } set { m_OnSelectionChangeStartEvent = value; } }

        [SerializeField]
        [Tooltip("Event fires as the page changes, while dragging or jumping")]
        private SelectionPageChangedEvent m_OnSelectionPageChangedEvent = new SelectionPageChangedEvent();
        public SelectionPageChangedEvent OnSelectionPageChangedEvent { get { return m_OnSelectionPageChangedEvent; } set { m_OnSelectionPageChangedEvent = value; } }

        [SerializeField]
        [Tooltip("Event fires when the page settles after a user has dragged")]
        private SelectionChangeEndEvent m_OnSelectionChangeEndEvent = new SelectionChangeEndEvent();
        public SelectionChangeEndEvent OnSelectionChangeEndEvent { get { return m_OnSelectionChangeEndEvent; } set { m_OnSelectionChangeEndEvent = value; } }
        
        protected RectTransform[] childObjects;
        void Awake()
        {
            if (_screensContainer == null)
                Setup();
        }

        void Setup()
        {
            if (_scroll_rect == null)
            {
                _scroll_rect = gameObject.GetComponent<ScrollRect>();
            }
            if (_scroll_rect.horizontalScrollbar && _scroll_rect.horizontal)
            {
                var hscroll = _scroll_rect.horizontalScrollbar.gameObject.AddComponent<ScrollSnapScrollbarHelper>();
                hscroll.ss = this;
            }
            if (_scroll_rect.verticalScrollbar && _scroll_rect.vertical)
            {
                var vscroll = _scroll_rect.verticalScrollbar.gameObject.AddComponent<ScrollSnapScrollbarHelper>();
                vscroll.ss = this;
            }
            panelDimensions = gameObject.GetComponent<RectTransform>();
            
            if (startingScreen < 0)
            {
                startingScreen = 0;
            }

            _screensContainer = _scroll_rect.content;

            InitialiseChildObjects();

            if (nextButton)
                nextButton.GetComponent<Button>().onClick.AddListener(() => { NextScreen(); });

            if (prevButton)
                prevButton.GetComponent<Button>().onClick.AddListener(() => { PreviousScreen(); });
        }
        
        internal void InitialiseChildObjects()
        {
            int childCount = _screensContainer.childCount;
            childObjects = new RectTransform[childCount];
            for (int i = 0; i < childCount; i++)
            {
                childObjects[i] = _screensContainer.GetChild(i).GetComponent<RectTransform>();
            }
        }

        //Function for switching screens with buttons
        public void NextScreen()
        {
            if (_currentScreen < _screens - 1)
            {
                if (!_lerp) StartScreenChange();

                _lerp = true;
                CurrentPage = _currentScreen + 1;
                GetPositionForPage(_currentScreen, ref _lerp_target);
                ScreenChange();
            }
        }

        //Function for switching screens with buttons
        public void PreviousScreen()
        {
            if (_currentScreen > 0)
            {
                if (!_lerp) StartScreenChange();

                _lerp = true;
                CurrentPage = _currentScreen - 1;
                GetPositionForPage(_currentScreen, ref _lerp_target);
                ScreenChange();
            }
        }
        
        /// <summary>
        /// Function for switching to a specific screen
        /// *Note, this is based on a 0 starting index - 0 to x
        /// </summary>
        /// <param name="screenIndex">0 starting index of page to jump to</param>
        public void GoToScreen(int screenIndex)
        {
            if (screenIndex <= _screens - 1 && screenIndex >= 0)
            {
                if (!_lerp) StartScreenChange();

                _lerp = true;
                CurrentPage = screenIndex;
                GetPositionForPage(_currentScreen, ref _lerp_target);
                ScreenChange();
            }
        }
        
        /// <summary>
        /// Event fires when the user starts to change the page, either via swipe or button.
        /// </summary>
        public void StartScreenChange()
        {
            if (_moveStarted) return;
            
            _moveStarted = true;
            OnSelectionChangeStartEvent.Invoke();
        }
        
        /// <summary>
        /// Returns the Transform of the Currentpage
        /// </summary>
        /// <returns>Currently selected Page Transform</returns>
        public Transform CurrentPageObject()
        {
            return _screensContainer.GetChild(CurrentPage);
        }
        
        /// <summary>
        /// Returns the Transform of the Currentpage in an out param for performance
        /// </summary>
        /// <param name="returnObject">Currently selected Page Transform</param>
        public void CurrentPageObject(out Transform returnObject)
        {
            returnObject = _screensContainer.GetChild(CurrentPage);
        }
        
        internal int GetPageForPosition(Vector3 pos)
        {
            /*
            return _isVertical ?
                (int)Math.Round((_scrollStartPosition - pos.y) / _childSize) :
                (int)Math.Round((_scrollStartPosition - pos.x) / _childSize);
                */

            int nearestChild = 0;
            float minDistance = float.MaxValue;
            for (int i = 0; i < childObjects.Length; ++i)
            {
                float newDistance = _isVertical
                    ? Mathf.Abs(panelDimensions.localPosition.y - panelDimensions.transform.InverseTransformPoint(childObjects[i].position).y)
                    : Mathf.Abs(panelDimensions.localPosition.x - panelDimensions.transform.InverseTransformPoint(childObjects[i].position).x);
                
                //CamperoUtils.Log("Cointainer pos: " + panelDimensions.localPosition.x + " Item pos: " + panelDimensions.transform.InverseTransformPoint(childObjects[i].position).x  + " Abs pos: " + Mathf.Abs(panelDimensions.localPosition.x - panelDimensions.transform.InverseTransformPoint(childObjects[i].position).x), Color.green);

                if (newDistance < minDistance)
                {
                    minDistance = newDistance;
                    nearestChild = i;
                    //CamperoUtils.Log("newDistance: " + newDistance + " nearestChild: " + nearestChild  + " minDistance: " + minDistance, Color.green);

                }
            }

            return nearestChild;
        }

        //todo crear un intervalo para que sea mas dinamico
        internal bool IsRectSettledOnaPage(Vector3 pos)
        {
            return (_isVertical ? 
                       Mathf.Abs(pos.y - _lerp_target.y) : 
                       Mathf.Abs(pos.x - _lerp_target.x)) < 5f;

        }

        
        internal void GetPositionForPage(int page, ref Vector2 target)
        {

            RectTransform itemTransform = (RectTransform) childObjects[page].transform;
            target = (Vector2)_scroll_rect.transform.InverseTransformPoint(_screensContainer.position)
                                            - (Vector2)_scroll_rect.transform.InverseTransformPoint(itemTransform.position);
        }
        
        internal void ScreenChange()
        {
            OnSelectionPageChangedEvent.Invoke(_currentScreen);
        }

        internal void EndScreenChange()
        {
            OnSelectionChangeEndEvent.Invoke(_currentScreen);
            _settled = true;
            _moveStarted = false;
        }
        
        internal void ScrollToClosestElement()
        {
            _lerp = true;
            CurrentPage = GetPageForPosition(_screensContainer.localPosition);
            //Debug.Log("Child Size: " + _childSize + " Scroll Normalized Value: " + _scroll_rect.horizontalNormalizedPosition  + " Container Pos: " + _screensContainer.localPosition + " Current Page: " + CurrentPage);
            GetPositionForPage(_currentScreen, ref _lerp_target);
            OnCurrentScreenChange(_currentScreen);
        }


        internal void OnCurrentScreenChange(int currentScreen)
        {
            ToggleNavigationButtons(currentScreen);
            
        }
        
        private void ToggleNavigationButtons(int targetScreen)
        {
            if (prevButton)
            {
                prevButton.GetComponent<Button>().interactable = targetScreen > 0;
            }

            if (nextButton)
            {
                nextButton.GetComponent<Button>().interactable = targetScreen < _screensContainer.transform.childCount - 1;
            }
        }
        
        private void OnValidate()
        {
            if (_scroll_rect == null)
            {
                _scroll_rect = GetComponent<ScrollRect>();
            }
            if (!_scroll_rect.horizontal && !_scroll_rect.vertical)
            {
                Debug.LogError("ScrollRect has to have a direction, please select either Horizontal OR Vertical with the appropriate control.");
            }
            if (_scroll_rect.horizontal && _scroll_rect.vertical)
            {
                Debug.LogError("ScrollRect has to be unidirectional, only use either Horizontal or Vertical on the ScrollRect, NOT both.");
            }

            var children = gameObject.GetComponent<ScrollRect>().content.childCount;
            if (children != 0 || childObjects != null)
            {
                var childCount = childObjects == null || childObjects.Length == 0 ? children : childObjects.Length;
                if (startingScreen > childCount - 1)
                {
                    startingScreen = childCount - 1;
                }

                if (startingScreen < 0)
                {
                    startingScreen = 0;
                }
            }
        }
        
        #region Drag Interfaces

        public void OnBeginDrag(PointerEventData eventData)
        {
            _pointerDown = true;
            _settled = false;
            StartScreenChange();
            _startPosition = _screensContainer.localPosition;
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            _lerp = false;
        }

        #endregion
        
        
        #region IScrollSnap Interface
        
        int IScrollSnap.CurrentPage()
        {
            return CurrentPage = GetPageForPosition(_screensContainer.localPosition);
        }
        
        public void SetLerp(bool value)
        {
            _lerp = value;
        }
        
        public void ChangePage(int page)
        {
            GoToScreen(page);
        }
        
        #endregion
        
        
        
        
        ////////////////////////////////////////////////////////////////////////////////////////////////
/*
        float[] points;
        [Tooltip("how many screens or pages are there within the content (steps)")]
        public int screens = 1;
        float stepSize;

        public float initPos = 0.5f;

        ScrollRect scroll;
        bool LerpH;
        float targetH;
        [Tooltip("Snap horizontally")]
        public bool snapInH = true;

        bool LerpV;
        float targetV;
        [Tooltip("Snap vertically")]
        public bool snapInV = true;

        // Use this for initialization
        void Start()
        {
            scroll = gameObject.GetComponent<ScrollRect>();
            scroll.inertia = false;

            if (screens > 0)
            {
                points = new float[screens];
                stepSize = 1 / (float)(screens - 1);

                for (int i = 0; i < screens; i++)
                {
                    points[i] = i * stepSize;
                }
            }
            else
            {
                points[0] = 0;
            }

            StartCoroutine(WaitToInit());
        }

        IEnumerator WaitToInit()
        {
            yield return new WaitForSeconds(0.1f);
            InitScrollBar();
        }

        void InitScrollBar()
        {
            if (snapInH)
            {
                scroll.horizontalNormalizedPosition = initPos;
                scroll.horizontalScrollbar.value = initPos;
            }

            if (snapInV)
            {
                scroll.verticalNormalizedPosition = 1 - initPos;
                scroll.verticalScrollbar.value = 1 - initPos;
            }
        }

        void Update()
        {
            if (LerpH)
            {
                scroll.horizontalNormalizedPosition = Mathf.Lerp(scroll.horizontalNormalizedPosition, targetH, 10 * scroll.elasticity * Time.deltaTime);
                if (Mathf.Approximately(scroll.horizontalNormalizedPosition, targetH)) LerpH = false;
            }
            if (LerpV)
            {
                scroll.verticalNormalizedPosition = Mathf.Lerp(scroll.verticalNormalizedPosition, targetV, 10 * scroll.elasticity * Time.deltaTime);
                if (Mathf.Approximately(scroll.verticalNormalizedPosition, targetV)) LerpV = false;
            }
        }

        public void DragEnd()
        {
            if (scroll.horizontal && snapInH)
            {
                targetH = points[FindNearest(scroll.horizontalNormalizedPosition, points)];
                LerpH = true;
            }
            if (scroll.vertical && snapInV)
            {
                targetV = points[FindNearest(scroll.verticalNormalizedPosition, points)];
                LerpV = true;
            }
        }

        public void OnDrag()
        {
            LerpH = false;
            LerpV = false;
        }

        int FindNearest(float f, float[] array)
        {
            float distance = Mathf.Infinity;
            int output = 0;
            for (int index = 0; index < array.Length; index++)
            {
                if (Mathf.Abs(array[index] - f) < distance)
                {
                    distance = Mathf.Abs(array[index] - f);
                    output = index;
                }
            }
            return output;
        }*/

    }
}


