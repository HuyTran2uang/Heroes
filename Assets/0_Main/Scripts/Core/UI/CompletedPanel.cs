using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CompletedPanel : MonoBehaviour
{
    [SerializeField] private GameObject _winPanel, _losePanel;
    [SerializeField] private TMP_Text _rewardTxt;
    [SerializeField] private Button _claimBtn, _x2ClaimBtn, _backToMainPageBtn, _continueBtn;

    private void Awake()
    {
        _claimBtn.onClick.AddListener(() => { ClaimReward(); });
        _x2ClaimBtn.onClick.AddListener(() => { ClaimX2Reward(); });
        _backToMainPageBtn.onClick.AddListener(() => { BackToMainPage(); });
        _continueBtn.onClick.AddListener(() => { Continue(); });
    }

    private void OnEnable()
    {
        GameController.Instance.View.GameplayPage.MainGamePanel.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        GameController.Instance.View.GameplayPage.MainGamePanel.gameObject.SetActive(true);
        _winPanel.SetActive(false);
        _losePanel.SetActive(false);
    }

    public void ShowWinPanel(int goldAmount)
    {
        _rewardTxt.text = goldAmount.ToString();
        _winPanel.SetActive(true);
        gameObject.SetActive(true);
    }

    public void ShowLosePanel()
    {
        _losePanel.SetActive(true);
        gameObject.SetActive(true);
    }

    public void ClaimReward()
    {
        RewardManager.Instance.ReceivedReward();
        GameController.Instance.BackToMainPage();
    }

    public void ClaimX2Reward()
    {
        RewardManager.Instance.ReceivedReward(true);
        GameController.Instance.BackToMainPage();
    }

    public void BackToMainPage()
    {
        GameController.Instance.BackToMainPage();
    }

    public void Continue()
    {
        GameController.Instance.Continue();
    }
}
