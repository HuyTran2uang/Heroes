using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    [SerializeField] private Button _musicBtn, _soundBtn, _closeBtn;
    [SerializeField] private Slider _musicSlider, _soundSlider;
    [SerializeField] private RectTransform _board;
    [SerializeField] private Image _soundIcon, _musicIcon;
    [SerializeField] private Sprite _iconSoundOn, _iconSoundOff;
    [SerializeField] private Sprite _iconMusicOn, _iconMusicOff;

    private void Awake()
    {
        _musicBtn.onClick.AddListener(() => { Music(); });
        _soundBtn.onClick.AddListener(() => { Sound(); });
        _closeBtn.onClick.AddListener(() => { Hide(); });

        _musicSlider.onValueChanged.AddListener((val) => { MusicSliderChangeValue(val); });
        _soundSlider.onValueChanged.AddListener((val) => { SoundSliderChangeValue(val); });
    }

    private void OnEnable()
    {
        Show();
    }

    private void Music()
    {
        bool isOpen;
        AudioManager.Instance.Music(out isOpen);
        _musicIcon.sprite = isOpen ? _iconMusicOn : _iconMusicOff;
    }

    private void Sound()
    {
        bool isOpen;
        AudioManager.Instance.Sound(out isOpen);
        _soundIcon.sprite = isOpen ? _iconSoundOn : _iconSoundOff;
    }

    private void MusicSliderChangeValue(float value)
    {
        AudioManager.Instance.ChangeVolume(AudioType.Music, value);
    }

    private void SoundSliderChangeValue(float value)
    {
        AudioManager.Instance.ChangeVolume(AudioType.Sound, value);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        _board.DOAnchorPosY(-50, .5f).OnComplete(() =>
        {
            _board.DOAnchorPosY(0, .5f);
        });
    }

    private void Hide()
    {
        _board.DOAnchorPosY(-50, .5f).OnComplete(() =>
        {
            _board.DOAnchorPosY(2000, .5f).OnComplete(() =>
            {
                gameObject.SetActive(false);
                GameController.Instance.View.GameplayPage.gameObject.SetActive(false);
                GameController.Instance.View.MainPage.gameObject.SetActive(true);
            });
        });
    }
}
