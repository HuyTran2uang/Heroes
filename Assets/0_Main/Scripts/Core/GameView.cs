using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameView : MonoBehaviour
{
    private LoadingPage _loadingPage;
    private MainPage _mainPage;
    private GameplayPage _gameplayPage;
    private NoticePopup _noticePopup;


    public LoadingPage LoadingPage
    {
        get
        {
            if (_loadingPage == null)
            {
                _loadingPage = FindObjectOfType<LoadingPage>(true);
            }
            return _loadingPage;
        }
    }
    public MainPage MainPage
    {
        get
        {
            if (_mainPage == null)
            {
                _mainPage = FindObjectOfType<MainPage>(true);
            }
            return _mainPage;
        }
    }
    public GameplayPage GameplayPage
    {
        get
        {
            if (_gameplayPage == null)
            {
                _gameplayPage = FindObjectOfType<GameplayPage>(true);
            }
            return _gameplayPage;
        }
    }
    public NoticePopup NoticePopup
    {
        get
        {
            if (_noticePopup == null)
            {
                _noticePopup = FindObjectOfType<NoticePopup>(true);
            }
            return _noticePopup;
        }
    }
}
