using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using AYellowpaper.SerializedCollections;

public class CardManager : MonoBehaviourSingleton<CardManager>
{
    [SerializedDictionary("Card Type", "List Card")]
    [SerializeField] private SerializedDictionary<CardType, List<Card>> _cards = new SerializedDictionary<CardType, List<Card>>();
    [SerializedDictionary("Card Type", "Price")]
    [SerializeField] private SerializedDictionary<CardType, int> _prices = new SerializedDictionary<CardType, int>()
    {
        { CardType.White, 0 },
        { CardType.Blue, 100 },
        { CardType.Purple, 200 },
        { CardType.Pink, 300 },
        { CardType.Red, 500 },
        { CardType.Yellow, 1000 }
    };

    private void Awake()
    {
        Refresh();
    }

    public void Refresh()
    {
        _cards = new SerializedDictionary<CardType, List<Card>>();
        Resources.LoadAll<Card>("SO/Cards").ToList().ForEach(i =>
        {
            if (!_cards.ContainsKey(i.CardType))
            {
                _cards.Add(i.CardType, new List<Card>());
            }
            _cards[i.CardType].Add(i);
        });
    }

    public int GetPrice(CardType type)
    {
        return _prices[type];
    }

    public SerializedDictionary<CardType, List<Card>> GetAllCards() => _cards;

    public CardType GetCardTypeByPrice(int price)
    {
        switch (price)
        {
            case < 100:
                return CardType.White;
            case < 200:
                return CardType.Blue;
            case < 300:
                return CardType.Purple;
            case < 500:
                return CardType.Pink;
            case < 1000:
                return CardType.Red;
        }
        return CardType.Yellow;
    }
}
