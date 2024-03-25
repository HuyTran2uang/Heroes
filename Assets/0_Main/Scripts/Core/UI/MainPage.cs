using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Inventory;

public class MainPage : MonoBehaviour
{
    [SerializeField] private Button _fightBtn, _settingBtn, _nextMapBtn, _prevMapBtn, _shopBtn, _inventoryBtn;
    [SerializeField] private Image _mapImage;
    [SerializeField] private TMP_Text _goldText, _mapText;
    [SerializeField] private SettingPanel _settingPanel;
    [SerializeField] private InventoryPanel _inventoryPanel;
    [SerializeField] private ShopPanel _shopPanel;

    public InventoryPanel InventoryPanel => _inventoryPanel;

    private void Awake()
    {
        _fightBtn.onClick.AddListener(() => { Fight(); });
        _settingBtn.onClick.AddListener(() => { OpenSetting(); });
        _nextMapBtn.onClick.AddListener(() => { NextMap(); });
        _prevMapBtn.onClick.AddListener(() => { PrevMap(); });
        _shopBtn.onClick.AddListener(() => { OpenShop(); });
        _inventoryBtn.onClick.AddListener(() => { OpenInventory(); });
    }

    private void Fight()
    {
        if (GameController.Instance.Play() == false) return;
        gameObject.SetActive(false);
        GameController.Instance.View.GameplayPage.gameObject.SetActive(true);
    }

    private void OpenShop()
    {
        _shopPanel.gameObject.SetActive(true);
    }

    private void OpenInventory()
    {
        _inventoryPanel.gameObject.SetActive(true);
    }

    private void OpenSetting()
    {
        _settingPanel.gameObject.SetActive(true);
    }

    public void SetMap(Sprite mapSprite)
    {
        _mapImage.sprite = mapSprite;
        _mapImage.SetNativeSize();
    }

    public void SetGoldText(int amount)
    {
        _goldText.text = $"{amount}";
    }

    public void SetMapText(int index)
    {
        _mapText.text = $"Map {index}";
    }

    private void NextMap()
    {
        MapManager.Instance.SelectNextMap();
    }

    private void PrevMap()
    {
        MapManager.Instance.SelectPrevMap();
    }

    public void DisplaySelectMapBtns(int currentMap, int mapPassed, int countMap)
    {
        _nextMapBtn.gameObject.SetActive(false);
        _prevMapBtn.gameObject.SetActive(false);
        _fightBtn.gameObject.SetActive(currentMap < countMap);

        if (mapPassed > 0)
        {
            if (currentMap == countMap)
            {
                _prevMapBtn.gameObject.SetActive(true);
            }
            else
            {
                if (currentMap == mapPassed)
                {
                    _prevMapBtn.gameObject.SetActive(true);
                }
                else
                {
                    _nextMapBtn.gameObject.SetActive(true);
                }
            }
        }
    }
}
