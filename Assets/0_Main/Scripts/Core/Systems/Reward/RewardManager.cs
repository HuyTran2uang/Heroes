using UnityEngine;

public class RewardManager : MonoBehaviourSingleton<RewardManager>
{
    [SerializeField] private int _rewardGold;

    public int RewardGold => _rewardGold;

    public void SetReward(int amountGold)
    {
        _rewardGold = amountGold;
    }

    public void ReceivedReward(bool isX2 = false)
    {
        if(isX2)
        {
            CurrencyManager.Instance.IncreaseGold(_rewardGold * 2);
            return;
        }
        CurrencyManager.Instance.IncreaseGold(_rewardGold);
    }
}
