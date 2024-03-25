using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Minigame.Ball
{
    public class Bonus : MonoBehaviour
    {
        [SerializeField] private int _id;
        [SerializeField] private List<int> _cacheBall = new List<int>();
        [SerializeField] protected TMP_Text _txt;
        private int _mul;

        public int Id => _id;

        public void Init(int mul)
        {
            if (mul == 0)
                return;
            _mul = mul;
            _txt.text = $"x{_mul}";
            gameObject.SetActive(true);
        }

        public virtual void Receive(BallController ball)
        {
            if (_cacheBall.Contains(ball.Id)) return;
            _cacheBall.Add(ball.Id);
            for (int i = 0; i < _mul; i++)
            {
                var item = BallSpawner.Instance.Spawn();
                _cacheBall.Add(item.Id);
                item.transform.position = ball.transform.position;
                item.transform.position += new Vector3(0, -1, 0);
                item.gameObject.SetActive(true);
            }
        }

        private void OnDisable()
        {
            _cacheBall.Clear();
        }
    }
}
