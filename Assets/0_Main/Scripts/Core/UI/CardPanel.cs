using System.Collections.Generic;
using UnityEngine;

public class CardPanel : MonoBehaviour
{
    [SerializeField] private List<CardItemUI> _cards = new List<CardItemUI>();

    private void OnEnable()
    {
        GameController.Instance.View.GameplayPage.MainGamePanel.gameObject.SetActive(false);
    }

    public void AddCard(CardItemUI card)
    {
        _cards.Add(card);
    }

    public void Clear()
    {
        _cards.ForEach(i => i.gameObject.SetActive(false));
        _cards.Clear();
        GameController.Instance.View.GameplayPage.MainGamePanel.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
