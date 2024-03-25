using Minigame.Ball;
using UnityEngine;

public abstract class Card : ScriptableObject
{
    [SerializeField] private int _id;
    [SerializeField] private Sprite _image;
    [SerializeField] private CardType _type;

    public int Id { get { return _id; } protected set { _id = value; } }
    public Sprite Image { get => _image; protected set { _image = value; } }
    public CardType CardType { get => _type; protected set { _type = value; } }

    public int GetPrice()
    {
        return CardManager.Instance.GetPrice(_type);
    }
    public bool IsEnoughPrice()
    {
        return BallMiniGame.Instance.BallGot >= GetPrice();
    }
    public abstract string GetDescription();
    public abstract void Use(PlayerController user);
}

public enum CardType
{
    White = 0,
    Blue = 1,
    Purple = 2,
    Pink = 3,
    Red = 4,
    Yellow = 5
}