using Unity.VisualScripting;
using UnityEngine;

public class CurrencyManager : MonoBehaviourSingleton<CurrencyManager>, IInitializable
{
    [SerializeField] private int _gold;
    [SerializeField] private int _diamond;

    public int Gold
    {
        get { return _gold; }
        private set
        {
            _gold = value;
            GameController.Instance.View.MainPage.SetGoldText(Gold);
        }
    }
    public int Diamond { get { return _diamond; } private set { _diamond = value; } }

    public bool UseGold(int amount)
    {
        if(amount > Gold)
        {
            Debug.Log("No enough gold");
            return false;
        }
        Gold -= amount;
        return true;
    }

    public bool IncreaseGold(int amount)
    {
        Gold += amount;
        return true;
    }

    public bool UseDiamond(int amount)
    {
        if (amount > Diamond)
        {
            Debug.Log("No enough Diamond");
            return false;
        }
        Diamond -= amount;
        return true;
    }

    public bool IncreaseDiamond(int amount)
    {
        Diamond += amount;
        return true;
    }

    public void Initialize()
    {
        GameController.Instance.View.MainPage.SetGoldText(Gold);
    }
}
