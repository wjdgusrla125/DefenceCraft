using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingProgress : MonoBehaviour
{
    public Slider progressSlider;
    public TextMeshProUGUI progressText;

    private void Start()
    {
        // 초기값 설정
        progressSlider.value = 0f;
        progressText.text = "0%";
    }

    public void UpdateProgress(float progress)
    {
        progressSlider.value = progress;
        progressText.text = $"{Mathf.Round(progress * 100)}%";
    }
}