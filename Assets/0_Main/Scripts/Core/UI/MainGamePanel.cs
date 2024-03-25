using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainGamePanel : MonoBehaviour
{
    [SerializeField] private Button _homeBtn;
    [SerializeField] private Slider _mapSlider;
    [SerializeField] private TMP_Text _lastMapText, _stageLabel;

    private void Awake()
    {
        _homeBtn.onClick.AddListener(() => { GoToHome(); });
    }

    public void GoToHome()
    {
        GameController.Instance.View.NoticePopup.Show(
            label: "Go to main menu", 
            content: "Are you sure?",
            confirm: () => {
                GameController.Instance.BackFromGameplayToHome();
                gameObject.SetActive(false);
            },
            cancel: () => {
                gameObject.SetActive(false);
            }
        );
    }

    public void SetMap(int countStage)
    {
        _lastMapText.text = countStage.ToString();
        _mapSlider.maxValue = countStage;
        _mapSlider.value = 0;
        _stageLabel.text = $"WAVE {1}/{_mapSlider.maxValue}";
    }

    public void SetStage(int currentStage)
    {
        _mapSlider.DOValue(currentStage, .5f);
        if (currentStage > _mapSlider.maxValue) return;
        _stageLabel.text = $"WAVE {currentStage + 1}/{_mapSlider.maxValue}";
    }
}
