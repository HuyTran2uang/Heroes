using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : MonoBehaviour
{
    [SerializeField] private Button _backBtn;

    private void Awake()
    {
        _backBtn.onClick.AddListener(() => { Hide(); });
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
