using UnityEngine;
using Minigame.Ball;

public class GameController : MonoBehaviourSingleton<GameController>
{
    private GameView _view;
    private PlayerController _player;
    private MapController _map;
    private Battle _battle;
    private bool _isPlayingMiniGame;
    private PreviewController _preview;

    public GameView View
    {
        get
        {
            if(_view == null)
            {
                _view = FindObjectOfType<GameView>();
            }
            return _view;
        }
    }
    public PlayerController Player => _player;
    public PreviewController Preview
    {
        get
        {
            if (_preview == null)
            {
                _preview = FindObjectOfType<PreviewController>();
            }
            return _preview;
        }
    }
    public Battle Battle => _battle;
    public bool IsPlayingMiniGame => _isPlayingMiniGame;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        MapManager.Instance.Initialize();
        CurrencyManager.Instance.Initialize();

        View.LoadingPage.gameObject.SetActive(false);
        View.MainPage.gameObject.SetActive(true);
    }

    public void BackFromGameplayToHome()
    {
        CardManager.Instance.Refresh();
        CameraController.Instance.FightMode(0, null);
        _isPlayingMiniGame = false;
        MapManager.Instance.CurrentStage = 0;
        ClearMap();
        BulletSpawner.Instance.Clear();
        BallMiniGame.Instance.Clear();

        View.MainPage.gameObject.SetActive(true);
        View.GameplayPage.gameObject.SetActive(false);
    }

    public void PlayMinigame()
    {
        _isPlayingMiniGame = true;
    }

    public bool Play()
    {
        if (MapManager.Instance.CurrentMap >= MapManager.Instance.Count())
        {
            Debug.Log("This is last map");
            return false;
        }
        //spawn player
        MapManager.Instance.CurrentStage = 0;
        var playerPrefab = Resources.Load<PlayerController>("Prefabs/Player/Default Player");
        var player = Instantiate(playerPrefab);
        _player = player;
        _player.Initialize();
        //map
        _map = MapSpawner.Instance.Spawn(MapManager.Instance.CurrentMap);
        var stage = _map.Map.Stages[MapManager.Instance.CurrentStage];
        var units = stage.Units;

        View.GameplayPage.MainGamePanel.SetMap(_map.Map.Stages.Length);

        //battle
        _battle = new Battle();
        _battle.CreateGrid(4, 3);
        _battle.SetMap(_map);
        _battle.SetPlayer(_player);
        _battle.PlayerMoveToDefendPoint(() => { StartCoroutine(_battle.CompletedInitSync()); } );
        _battle.SetMonsters(units);
        BallMiniGame.Instance.StartMap();
        return true;
    }

    public bool NextStage()
    {
        MapManager.Instance.CurrentStage++;
        View.GameplayPage.MainGamePanel.SetStage(MapManager.Instance.CurrentStage);
        if (MapManager.Instance.CurrentStage == _map.Map.Stages.Length)
        {
            CompletedMap();
            return false;
        }
        var stage = _map.Map.Stages[MapManager.Instance.CurrentStage];
        var units = stage.Units;
        _battle.CreateGrid(4, 3);
        _battle.PlayerMoveToDefendPoint(() =>
        {
            CameraController.Instance.MinigameMode(1, () =>
            {
                //start mini game
                BallMiniGame.Instance.StartGameMode();
            });
        });
        _battle.SetMonsters(units);
        return true;
    }

    public void EndGame()
    {
        Debug.Log("end game");
        View.GameplayPage.CompletedPanel.ShowLosePanel();
    }

    public void CompletedMap()
    {
        CardManager.Instance.Refresh();
        MapManager.Instance.CompletedCurrentMap();
        RewardManager.Instance.SetReward(1000 + MapManager.Instance.CurrentMap * 1000);
        View.GameplayPage.CompletedPanel.ShowWinPanel(RewardManager.Instance.RewardGold);
    }

    public void BackToMainPage()
    {
        ClearMap();
        View.MainPage.gameObject.SetActive(true);
        View.GameplayPage.CompletedPanel.gameObject.SetActive(false);
        View.GameplayPage.gameObject.SetActive(false);
    }

    public void Continue()
    {
        Player.HealthFull();
        Battle.PlayerEndTurn();
        View.GameplayPage.CompletedPanel.gameObject.SetActive(false);
    }

    private void ClearMap()
    {
        Destroy(_player.gameObject);
        _player = null;
        _map.gameObject.SetActive(false);
        _map = null;
        _battle.Clear();
        _battle = null;
    }

    public void CompletedMiniGame()
    {
        _isPlayingMiniGame = false;
        CameraController.Instance.FightMode(1, () =>
        {
            _battle.Spawn3Cards();
        });
    }
}
