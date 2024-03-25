using Inventory;
using UnityEngine;

public abstract class ItemSO : ScriptableObject
{
    [SerializeField] private Sprite _image;
    [SerializeField] private string _description;
    [SerializeField] private string _name;
    [SerializeField] private int _price;
    [SerializeField] private PriceType _priceType;

    public Sprite Image { get { return _image; } protected set { _image = value; } }
    public string Name { get { return _name; } protected set { _name = value; } }
    public int Price { get => _price; protected set { _price = value; } }
    public PriceType PriceType { get => _priceType; protected set { _priceType = value; } }

    public virtual string Description { get { return _description; } protected set { _description = value; } }
    public abstract void Use();
    public abstract void Show();
}

public enum PriceType
{
    Gold,
    Diamond,
    Ads
}
