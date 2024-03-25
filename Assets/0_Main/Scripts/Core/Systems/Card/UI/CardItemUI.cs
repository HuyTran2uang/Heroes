using Minigame.Ball;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardItemUI : MonoBehaviour
{
    [SerializeField] private Image _icon, _color;
    [SerializeField] private TMP_Text _description, _priceTxt;
    [SerializeField] private Button _selectBtn, _watchAdsBtn;
    private Action _onSelected;

    private void Awake()
    {
        _selectBtn.onClick.AddListener(() => { _onSelected?.Invoke(); });
        _watchAdsBtn.onClick.AddListener(() => { _onSelected?.Invoke(); });
    }

    private Color GetColor(CardType cardType)
    {
        switch (cardType)
        {
            case CardType.White: return new Color(65 / 255f, 65 / 255f, 65 / 255f, 1);
            case CardType.Blue: return new Color(5 / 255f, 122 / 255f, 203 / 255f, 1);
            case CardType.Purple: return new Color(86 / 255f, 10 / 255f, 163 / 255f, 1);
            case CardType.Pink: return new Color(160 / 255f, 10 / 255f, 163 / 255f, 1);
            case CardType.Red: return new Color(163 / 255f, 10 / 255f, 38 / 255f, 1);
            case CardType.Yellow: return new Color(236 / 255f, 240 / 255f, 27 / 255f, 1);
        }
        return new Color(65 / 255f, 65 / 255f, 65 / 255f, 1);
    }

    public void Init(Card card)
    {
        _color.color = GetColor(card.CardType);
        _icon.sprite = card.Image;
        _icon.SetNativeSize();
        _description.text = card.GetDescription();
        _priceTxt.text = card.GetPrice() == 0 ? "Free" : card.GetPrice().ToString();
        _onSelected = () =>
        {
            if (BallMiniGame.Instance.UseBallGot(card.GetPrice()))
            {
                card.Use(GameController.Instance.Player);
                GameController.Instance.Battle.SelectCardCompleted();
                GameController.Instance.Battle.StartStage();
            }
            else
            {
                Debug.Log("Not enough money");
            }
        };
        _watchAdsBtn.gameObject.SetActive(!card.IsEnoughPrice());
        gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        CardSpawner.Instance.AddToPool(this);
    }
}
