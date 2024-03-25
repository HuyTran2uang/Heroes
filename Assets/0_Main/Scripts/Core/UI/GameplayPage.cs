using TMPro;
using UnityEngine;

public class GameplayPage : MonoBehaviour
{
    [SerializeField] private CardPanel _cardPanel;
    [SerializeField] private CompletedPanel _completedPanel;
    [SerializeField] private MainGamePanel _mainGamePanel;
    [SerializeField] private TMP_Text _ballText;

    public CardPanel CardPanel => _cardPanel;
    public CompletedPanel CompletedPanel => _completedPanel;
    public MainGamePanel MainGamePanel => _mainGamePanel;

    private void OnEnable()
    {
        MainGamePanel.gameObject.SetActive(true);
    }

    public void SetBallText(int amout)
    {
        _ballText.text = amout.ToString();
    }
}
