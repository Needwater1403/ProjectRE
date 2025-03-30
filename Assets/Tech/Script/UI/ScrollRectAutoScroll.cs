using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class ScrollRectAutoScroll : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float scrollSpeed = 10f;
    private bool mouseOver;

    public List<Transform> m_Selectables = new List<Transform>();
    private ScrollRect m_ScrollRect;

    private Vector2 m_NextScrollPosition = Vector2.up;
    public int index = 0;
    void OnEnable()
    {
        if (m_ScrollRect)
        {
            
            //m_ScrollRect.content.GetComponentsInChildren(m_Selectables);
            var a = m_ScrollRect.content.GetComponentsInChildren<Button>();

            for (var i = 0; i < a.Length; i++)
            {
                if (i == index)
                {
                    index += 5;
                    m_Selectables.Add(a[i].transform);
                }
            }
        }
    }

    private void Awake()
    {
        m_ScrollRect = GetComponent<ScrollRect>();
    }

    private void Start()
    {
        var a = m_ScrollRect.content.GetComponentsInChildren<Button>();

        for (var i = 0; i < a.Length; i++)
        {
            if (i == index)
            {
                index += 5;
                m_Selectables.Add(a[i].transform);
            }
        }
        ScrollToSelected(true);
    }

    private void Update()
    {
        // Scroll via input.
        InputScroll();
        if (!mouseOver)
        {
            // Lerp scrolling
            m_ScrollRect.normalizedPosition = Vector2.Lerp(m_ScrollRect.normalizedPosition, m_NextScrollPosition, scrollSpeed * Time.unscaledDeltaTime);
        }
        else
        {
            m_NextScrollPosition = m_ScrollRect.normalizedPosition;
        }
    }

    private void InputScroll()
    {
        if (m_Selectables.Count > 0)
        {
            ScrollToSelected(false);
        }
    }

    private void ScrollToSelected(bool quickScroll)
    {
        var selectedIndex = -1;
        var selectedElement = EventSystem.current.currentSelectedGameObject ? EventSystem.current.currentSelectedGameObject.transform : null;

        if (selectedElement)
        {
            selectedIndex = m_Selectables.IndexOf(selectedElement);
        }
        if (selectedIndex > -1)
        {
            if (quickScroll)
            {
                m_ScrollRect.normalizedPosition = new Vector2(0, 1 - (selectedIndex / ((float)m_Selectables.Count - 1)));
                m_NextScrollPosition = m_ScrollRect.normalizedPosition;
            }
            else
            {
                m_NextScrollPosition = new Vector2(0, 1 - (selectedIndex / ((float)m_Selectables.Count - 1)));
            }
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
        ScrollToSelected(false);
    }
}

