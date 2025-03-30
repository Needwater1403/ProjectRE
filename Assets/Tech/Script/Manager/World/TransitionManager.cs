using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager  Instance;

    [Title("Loading Screen")]
    [SerializeField] private CanvasGroup _canvasGroup;

    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI progressText;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void SetProgressBar(float value)
    {
        if(value!=0) _slider.value = 1 / value;
        progressText.SetText($"{Mathf.RoundToInt(value*100/0.9f)}");
    }
    public void SetLoadingScreen(bool isEnable, float delay = .5f, bool canShowTravelDestinationPopUp = false)
    {
        if (isEnable)
        {
            _slider.value = 0;
            progressText.SetText("0");
            _canvasGroup.gameObject.SetActive(true);
            _canvasGroup.DOFade(1, 1);
        }
        else StartCoroutine(HideLoadingScreen(delay));
    }
    
    private IEnumerator HideLoadingScreen(float delayPopUp)
    {
        yield return new WaitForSeconds(1);
        //PlayerManager.Instance.SetFreezeStatus(false);
        _canvasGroup.DOFade(0, 1.5f);
        yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(delayPopUp);
        _canvasGroup.gameObject.SetActive(false);
        PlayerManager.Instance.SetFreezeStatus(false);
    }
}
