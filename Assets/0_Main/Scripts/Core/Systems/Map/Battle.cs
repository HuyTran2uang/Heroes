using AYellowpaper.SerializedCollections;
using Minigame.Ball;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public class Battle
{
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    private Node[,] _nodes;
    private PlayerController _player;
    [SerializeField] private List<MonsterController> _monsters;
    private MapController _map;
    [SerializedDictionary("Card Type", "List Card")]
    [SerializeField] private SerializedDictionary<CardType, List<Card>> _cards = new SerializedDictionary<CardType, List<Card>>();
    private List<CardItemUI> _cardItemUIs = new List<CardItemUI>();

    public void CreateGrid(int width, int height)
    {
        _cards = CardManager.Instance.GetAllCards();
        _monsters = new List<MonsterController>();
        _width = width;
        _height = height;
        _nodes = new Node[width, height];
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Vector3 position = new Vector3(x - _width / 2f, 0, -(y - _height / 2f)) * 2.5f;
                _nodes[x, y] = new Node(x, y, position);
                //test
                //GameObject obj = new GameObject($"[{x},{y}]");
                //obj.transform.position = position;
            }
        }
    }

    public Node GetNode(int x, int y)
    {
        return _nodes[x, y];
    }

    #region Battle
    public void SetPlayer(PlayerController player)
    {
        _player = player;
        _player.transform.localEulerAngles = new Vector3(0, 90, 0);
        _player.View.CanvasView.transform.localEulerAngles = new Vector3(0, -90, 0);
        _player.transform.position = new Vector3(_map.Gate1.position.x, 0, _map.Gate1.position.z);
        _player.gameObject.SetActive(true);
    }

    public void SetMap(MapController map)
    {
        _map = map;
        _map.Init();
    }

    public void PlayerMoveToDefendPoint(Action onCompleted)
    {
        _player.MoveToNode(_nodes[0, _height / 2], () =>
        {
            _nodes[0, _height / 2].Lock(_player);
            onCompleted?.Invoke();
        });
    }
    
    public void SetMonsters(Unit[] units)
    {
        _monsters = SetTeamUnits(units);
        _monsters.ForEach(i =>
        {
            i.transform.localEulerAngles = new Vector3(0, -90, 0);
            i.View.CanvasView.transform.localEulerAngles = new Vector3(0, 90, 0);
        });
    }

    public IEnumerator CompletedInitSync()
    {
        yield return new WaitForSeconds(.5f);
        PlayerTurn();
    }

    public void PlayerTurn()
    {
        PlayerTurn(_player.HitEachTurn);
    }

    private void PlayerTurn(int hit)
    {
        hit--;
        List<MonsterController> monstersLife = _monsters.Where(i => i.IsDead == false).ToList();
        if (monstersLife.Count == 0)
        {
            MoveToGate();
            return;
        }
        MonsterController nearestMonster = monstersLife.OrderBy(i => Distance(_nodes[i.X, i.Y], _nodes[_player.X, _player.Y])).First();
        _player.OnAttack(nearestMonster, () =>
        {
            if (hit == 0)
            {
                PlayerEndTurn();
            }
            else
            {
                PlayerTurn(hit);
            }
        });
    }

    public void MonsterTurn()
    {
        List<MonsterController> monstersLife = _monsters.Where(i => i.IsDead == false).ToList();
        EachMonsterTurn(monstersLife);
    }

    private void EachMonsterTurn(List<MonsterController> monstersLife)
    {
        if (monstersLife.Count == 0)
        {
            MonsterEndTurn();
            return;
        }
        MonsterController monster = monstersLife.First();
        monstersLife.RemoveAt(0);
        if (monster.IsEnemyInScopeAttack(_player))
        {
            monster.OnAttack(_player, () =>
            {
                EachMonsterTurn(monstersLife);
            });
        }
        else
        {
            int x = monster.X;
            int y = monster.Y;
            if (monster.X == _player.X)
            {
                if (monster.Y > _player.Y)
                    y--;
                if (monster.Y < _player.Y)
                    y++;
            }
            else
            {
                x--;
            }
            Node targetNode = GetNode(x, y);
            if (targetNode.IsLocked)
            {
                MonsterEndTurn();
                return;
            }
            monster.MoveToNode(targetNode, () =>
            {
                EachMonsterTurn(monstersLife);
            });
        }
    }

    public void MonsterEndTurn()
    {
        if (_player.IsDead)
        {
            EndGame();
            return;
        }
        PlayerTurn();
    }

    public void PlayerEndTurn()
    {
        if(_monsters.All(i => i.IsDead))
        {
            MoveToGate();
            return;
        }
        MonsterTurn();
    }

    private void MoveToGate()
    {
        _player.Move(new Vector3(_map.Gate2.position.x, 0, _map.Gate2.position.z), () =>
        {
            _player.View.Model.SetActive(false);
            NextStage();
        }, 3);
    }

    private void NextStage()
    {
        Clear();
        _player.transform.position = new Vector3(_map.Gate1.position.x, 0, _map.Gate1.position.z);
        _player.View.Model.SetActive(true);
        if (GameController.Instance.NextStage())
        {

        }
    }

    public void StartStage()
    {
        PlayerTurn();
    }

    private void EndGame()
    {
        GameController.Instance.EndGame();
    }

    public int Distance(Node nodeA, Node nodeB)
    {
        int x = Mathf.Abs(nodeA.X - nodeB.X);
        int y = Mathf.Abs(nodeA.Y- nodeB.Y);
        return x + y;
    }
    #endregion

    public List<MonsterController> SetTeamUnits(Unit[] units)
    {
        switch (units.Length)
        {
            case 1:
                return SetTeam1Person(units).ToList();
            case 2:
                return SetTeam2Persons(units).ToList();
            case 3:
                return SetTeam3Persons(units).ToList();
        }
        return new List<MonsterController>();
    }

    #region Sort Team Monster -> can refactor to strategy dp
    private IEnumerable<MonsterController> SetTeam1Person(Unit[] units)
    {
        var unit = MonsterSpawner.Instance.Spawn(units[0]);
        unit.Init(units[0].Level);
        yield return unit;
        unit.transform.position = new Vector3(_map.Gate2.position.x, 0, _map.Gate2.position.z);
        unit.MoveToNode(_nodes[_width - 1, _height / 2], () =>
        {
            _nodes[_width - 1, _height / 2].Lock(unit);
        });
    }

    private IEnumerable<MonsterController> SetTeam2Persons(Unit[] units)
    {
        var unit1 = MonsterSpawner.Instance.Spawn(units[0]);
        unit1.Init(units[0].Level);
        yield return unit1;
        unit1.transform.position = new Vector3(_map.Gate2.position.x, 0, _map.Gate2.position.z);
        unit1.MoveToNode(_nodes[_width - 1, 0], () =>
        {
            _nodes[_width - 1, 0].Lock(unit1);
        });
        var unit2 = MonsterSpawner.Instance.Spawn(units[1]);
        unit2.Init(units[1].Level);
        yield return unit2;
        unit2.transform.position = new Vector3(_map.Gate2.position.x, 0, _map.Gate2.position.z);
        unit2.MoveToNode(_nodes[_width - 1, _height - 1], () =>
        {
            _nodes[_width - 1, _height - 1].Lock(unit2);
        });
    }

    private IEnumerable<MonsterController> SetTeam3Persons(Unit[] units)
    {
        var unit1 = MonsterSpawner.Instance.Spawn(units[0]);
        unit1.Init(units[0].Level);
        yield return unit1;
        unit1.transform.position = new Vector3(_map.Gate2.position.x, 0, _map.Gate2.position.z);
        unit1.MoveToNode(_nodes[_width - 1, 0], () =>
        {
            _nodes[_width - 1, 0].Lock(unit1);
        });
        var unit2 = MonsterSpawner.Instance.Spawn(units[1]);
        unit2.Init(units[1].Level);
        yield return unit2;
        unit2.transform.position = new Vector3(_map.Gate2.position.x, 0, _map.Gate2.position.z);
        unit2.MoveToNode(_nodes[_width - 1, _height / 2], () =>
        {
            _nodes[_width - 1, _height / 2].Lock(unit2);
        });
        var unit3 = MonsterSpawner.Instance.Spawn(units[2]);
        unit3.Init(units[2].Level);
        yield return unit3;
        unit3.transform.position = new Vector3(_map.Gate2.position.x, 0, _map.Gate2.position.z);
        unit3.MoveToNode(_nodes[_width - 1, _height - 1], () =>
        {
            _nodes[_width - 1, _height - 1].Lock(unit3);
        });
    }
    #endregion

    #region Card
    public Card RandomCardByLessPrice()
    {
        CardType type = CardManager.Instance.GetCardTypeByPrice(BallMiniGame.Instance.BallGot) - 1 < 0 ? 0 : CardManager.Instance.GetCardTypeByPrice(BallMiniGame.Instance.BallGot) - 1;
        var cards = _cards[type];
        return cards[UnityEngine.Random.Range(0, cards.Count)];
    }

    public Card RandomByPrice()
    {
        var cards = _cards[CardManager.Instance.GetCardTypeByPrice(BallMiniGame.Instance.BallGot)];
        return cards[UnityEngine.Random.Range(0, cards.Count)];
    }

    public Card RandomByGreatPrice()
    {
        CardType type = (int)(CardManager.Instance.GetCardTypeByPrice(BallMiniGame.Instance.BallGot) + 1) > Enum.GetValues(typeof(CardType)).Length ? (CardType)(Enum.GetValues(typeof(CardType)).Length - 1) : CardManager.Instance.GetCardTypeByPrice(BallMiniGame.Instance.BallGot) + 1;
        var cards = _cards[type];
        return cards[UnityEngine.Random.Range(0, cards.Count)];
    }

    public void Spawn3Cards()
    {
        Card card1 = RandomCardByLessPrice();
        _cards[card1.CardType].Remove(card1);
        Card card2 = RandomByPrice();
        _cards[card2.CardType].Remove(card2);
        Card card3 = RandomByGreatPrice();
        _cards[card3.CardType].Remove(card3);

        var ui1 = CardSpawner.Instance.Spawn();
        GameController.Instance.View.GameplayPage.CardPanel.AddCard(ui1);
        ui1.Init(card1);
        var ui2 = CardSpawner.Instance.Spawn();
        GameController.Instance.View.GameplayPage.CardPanel.AddCard(ui2);
        ui2.Init(card2);
        var ui3 = CardSpawner.Instance.Spawn();
        GameController.Instance.View.GameplayPage.CardPanel.AddCard(ui3);
        ui3.Init(card3);

        GameController.Instance.View.GameplayPage.CardPanel.gameObject.SetActive(true);
    }

    public void SelectCardCompleted()
    {
        GameController.Instance.View.GameplayPage.CardPanel.Clear();
    }
    #endregion

    public void Clear()
    {
        MonsterSpawner.Instance.Clear();
    }
}
