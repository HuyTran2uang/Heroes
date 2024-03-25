using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Minigame.Ball
{
    public class BallMiniGame : MonoBehaviourSingleton<BallMiniGame>
    {
        private BallController _ball;
        [SerializeField] private GameObject _matrixObj;
        [SerializeField] private int _ballCountForMinigame;
        public Vector3 _position, _cachePos;
        private Vector3 OriginalPos = new Vector3(.5f, -96f, -.5f);
        [SerializeField] private List<Bonus> _bonuses = new List<Bonus>();
        private List<BallController> _balls = new List<BallController>();
        [SerializeField] private int _ballCountGot;
        private bool _isStarting = false;

        public int BallGot { get { return _ballCountGot; } private set { _ballCountGot = value; } }

        public void StartMap()
        {
            _ballCountGot = 0;
            GameController.Instance.View.GameplayPage.SetBallText(_ballCountGot);
        }

        public void IncreaseBallToUse(int amount)
        {
            _ballCountGot += amount;
            GameController.Instance.View.GameplayPage.SetBallText(_ballCountGot);
        }

        public bool UseBallGot(int amout)
        {
            if (amout > _ballCountGot)
            {
                return false;
            }
            _ballCountGot -= amout;
            GameController.Instance.View.GameplayPage.SetBallText(_ballCountGot);
            return true;
        }

        private void Reset()
        {
            _bonuses = GetComponentsInChildren<Bonus>(true).ToList();
        }

        public void AddBall(int count)
        {
            _ballCountForMinigame += count;
        }

        public void AddBall(BallController ball)
        {
            _balls.Add(ball);
        }

        public bool IsCompletedMoving()
        {
            return _balls.All(i => i.IsMoving == false);
        }

        public void Clear()
        {
            if (_isStarting)
            {
                _isStarting = false;
                _ballCountForMinigame = 0;
                _bonuses.ForEach(i => i.gameObject.SetActive(false));
                _matrixObj.SetActive(false);
                _balls.ForEach(i => i.gameObject.SetActive(false));
                _balls.Clear();
                BallSpawner.Instance.Clear();
            }
        }

        public void CompletedMinigame()
        {
            if (_isStarting)
            {
                _isStarting = false;
                _ballCountForMinigame = 0;
                _balls.ForEach(i => i.gameObject.SetActive(false));
                _balls.Clear();
                _bonuses.ForEach(i => i.gameObject.SetActive(false));
                _matrixObj.SetActive(false);
                GameController.Instance.CompletedMiniGame();
            }
        }

        public void StartGameMode()
        {
            GameController.Instance.PlayMinigame();
            _isStarting = true;
            BallSpawner.Instance.ResetSetId();
            _ball = BallSpawner.Instance.Spawn();
            _ball.transform.localPosition = OriginalPos;
            _ball.Init();
            _ballCountForMinigame--;
            SetBonus();
            _matrixObj.gameObject.SetActive(true);
        }

        private void SetBonus()
        {
            //all
            List<Bonus> bonuses = _bonuses;
            //last id
            int lastID = bonuses.OrderByDescending(i => i.Id).First().Id;
            //
            List<Bonus> lastBonuses = bonuses.Where(i => i.Id == lastID).ToList();
            List<Bonus> difLast = bonuses.Where(i => i.Id != lastID).ToList();
            List<Bonus> randomBonuses = new List<Bonus>();
            for (int i = 0; i < 5; i++)
            {
                var randomBonus = difLast[Random.Range(0, difLast.Count)];
                randomBonuses.Add(randomBonus);
                difLast.Remove(randomBonus);
            }
            //-------- set random list --------------
            //x5
            int val5 = Random.Range(0, 2) > 0 ? 5 : 0;
            if (val5 != 0 && randomBonuses.Count > 0)
            {
                var random_x5 = randomBonuses[Random.Range(0, randomBonuses.Count)];
                random_x5.Init(val5);
                randomBonuses.Remove(random_x5);
            }
            //x4
            int val4 = Random.Range(0, 2) > 0 ? 4 : 0;
            if (val4 != 0 && randomBonuses.Count > 0)
            {
                var random_x4 = randomBonuses[Random.Range(0, randomBonuses.Count)];
                random_x4.Init(val4);
                randomBonuses.Remove(random_x4);
            }
            //x3
            for (int i = 0; i < 2; i++)
            {
                int val3 = Random.Range(0, 2) > 0 ? 3 : 0;
                if (val3 != 0 && randomBonuses.Count > 0)
                {
                    var random_x3 = randomBonuses[Random.Range(0, randomBonuses.Count)];
                    random_x3.Init(val3);
                    randomBonuses.Remove(random_x3);
                }
            }
            //x2,x1
            SetRandom12(randomBonuses);
            //-------- set last id list --------------
            SetLastIdBonus(lastBonuses, 2);
        }

        private void SetRandom12(List<Bonus> bonuses)
        {
            if (bonuses.Count == 0) return;
            int randomVal = Random.Range(1, 3);
            var item = bonuses[Random.Range(0, bonuses.Count)];
            item.Init(randomVal);
            bonuses.Remove(item);
            SetRandom12(bonuses);
        }

        private void SetLastIdBonus(List<Bonus> bonuses, int val)
        {
            if (bonuses.Count == 0) return;
            var item = bonuses[Random.Range(0, bonuses.Count)];
            item.Init(val);
            bonuses.Remove(item);
            SetLastIdBonus(bonuses, val + 1);
        }

        private void Update()
        {
            if (_isStarting == false) return;
            if (_ball == null) return;
            Touch();
            Move();
            FigerOutTouch();
        }

        private void Touch()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    Vector3 posCasted = hit.point;
                    posCasted.y = 0;
                    posCasted.z = 0;
                    _position = posCasted;
                }
            }
        }

        private void Move()
        {
            if (Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    Vector3 posCasted = hit.point;
                    posCasted.y = 0;
                    posCasted.z = 0;
                    float distance = Vector3.Distance(_position, posCasted);
                    Vector3 direction = Vector3.Normalize(posCasted - _position);
                    _position = posCasted;
                    if (Mathf.Abs((_ball.transform.position + direction * distance).x) < 3.5)
                    {
                        _ball.transform.position += direction * distance;
                    }
                }
            }
        }

        private void FigerOutTouch()
        {
            if (Input.GetMouseButtonUp(0))
            {
                _cachePos = _ball.transform.position;
                _ball.GetComponent<Rigidbody>().useGravity = true;
                _ball = null;
                StartCoroutine(SpawnBall());
            }
        }

        private IEnumerator SpawnBall()
        {
            while (_ballCountForMinigame > 0)
            {
                yield return new WaitForSeconds(.3f);
                var item = BallSpawner.Instance.Spawn();
                item.transform.position = _cachePos;
                item.gameObject.SetActive(true);
                _ballCountForMinigame--;
            }
        }
    }
}
