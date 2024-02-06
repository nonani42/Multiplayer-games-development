using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    public void Start()
    {
        gameObject.SetActive(false);
        _slider.value = 0;
    }

    public void StartLoading()
    {
        _slider.value = 0;
        gameObject.SetActive(true);
        StartCoroutine(FillSlider());
    }

    private IEnumerator FillSlider()
    {
        float diff = 0.3f;
        while (_slider.value < _slider.maxValue)
        {
            _slider.value += diff;
            yield return new WaitForSeconds(0.3f);
        }
    }

    public void FinishLoading()
    {
        StopAllCoroutines();
        _slider.value = _slider.maxValue;
        gameObject.SetActive(false);
    }
}
