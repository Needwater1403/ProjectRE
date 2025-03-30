using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
[AddComponentMenu("UI/Extensions/UIHorizontalScrollToSelection")]
public class UIHorizontalScrollToSelection : MonoBehaviour
{
    public ScrollRect MyScrollRect;
    private Vector3 localPos;

    // private void Update()
    // {
    //     if (EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.transform.IsChildOf(MyScrollRect.content))
    //     {
    //         var a = EventSystem.current.currentSelectedGameObject.GetComponent<RectTransform>();
    //         localPos = MyScrollRect.GetSnapToPositionToBringChildIntoView(a);
    //         MyScrollRect.content.localPosition =
    //             new Vector3(
    //                 Mathf.SmoothStep(MyScrollRect.content.localPosition.x, localPos.x, Time.unscaledDeltaTime * 13),
    //                 MyScrollRect.content.localPosition.y, MyScrollRect.content.localPosition.z);
    //     }
    // }

    public void ScrollToSelectItem(RectTransform contentChild)
    {
        localPos = MyScrollRect.GetSnapToPositionToBringChildIntoView(contentChild);
        MyScrollRect.content.DOLocalMove(
            new Vector3(localPos.x, MyScrollRect.content.localPosition.y, MyScrollRect.content.localPosition.z), .5f);
    }
}