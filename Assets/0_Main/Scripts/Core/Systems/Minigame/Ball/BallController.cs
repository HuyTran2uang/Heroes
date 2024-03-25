using System.Collections;
using UnityEngine;

namespace Minigame.Ball
{
    public class BallController : MonoBehaviour
    {
        private int _id;
        [SerializeField] private Rigidbody _rb;
        private bool _isMoving;

        public int Id { get { return _id; } set { _id = value; } }
        public bool IsMoving => _isMoving;
        private bool _isTiming;

        private void OnEnable()
        {
            _isMoving = false;
            _isTiming = false;
            _rb.velocity = Vector3.zero;
            BallMiniGame.Instance.AddBall(this);
            StopAllCoroutines();
        }

        public void Init()
        {
            _rb.useGravity = false;
            gameObject.SetActive(true);
        }

        private void Update()
        {
            if(_rb.velocity == Vector3.zero && _rb.useGravity)
            {
                _isMoving = false;
                if (_isTiming == false)
                {
                    _isTiming = true;
                    StartCoroutine(IsIdle(3));
                }
            }

            if(_rb.velocity != Vector3.zero)
            {
                _isMoving = true;
                if( _isTiming == true)
                {
                    _isTiming = false;
                    StopAllCoroutines();
                }
            }

            if(_rb.velocity.y < 0)
            {
                _rb.velocity += Physics.gravity * Time.deltaTime;
            }

            if(_rb.velocity.y > 0)
            {
                _rb.velocity = new Vector3(_rb.velocity.x, 0);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Ball Bonus")
            {
                collision.gameObject.GetComponent<Bonus>().Receive(this);
            }
            if (collision.gameObject.tag == "Ball Receiver")
            {
                BallMiniGame.Instance.IncreaseBallToUse(1);
                gameObject.SetActive(false);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Ball Bonus")
            {
                other.gameObject.GetComponent<Bonus>().Receive(this);
            }
            if (other.gameObject.tag == "Ball Receiver")
            {
                BallMiniGame.Instance.IncreaseBallToUse(1);
                gameObject.SetActive(false);
            }
        }

        private void OnDisable()
        {
            _isMoving = false;
            StopAllCoroutines();
            if (BallMiniGame.Instance.IsCompletedMoving() && GameController.Instance.IsPlayingMiniGame)
            {
                BallMiniGame.Instance.CompletedMinigame();
            }
            BallSpawner.Instance.AddToPool(this);
        }

        private IEnumerator IsIdle(float duration)
        {
            yield return new  WaitForSeconds(duration);
            gameObject.SetActive(false);
        }
    }
}
