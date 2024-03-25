using DG.Tweening;
using Minigame.Ball;
using System;
using System.Collections;
using UnityEngine;

public class MonsterController : UnitController
{
    [SerializeField] private MonsterView _view;
    [SerializeField] private MonsterBaseStats _baseStats;

    [SerializeField] private int _id;
    [SerializeField] private int _level;
    [SerializeField] private AttackType _attackType;
    //stats
    [SerializeField] private int _currentHealth;
    //

    public MonsterView View
    {
        get
        {
            if(_view == null)
            {
                _view = GetComponentInChildren<MonsterView>(true);
            }
            return _view;
        }
    }
    public int Id => _id;
    public AttackType AttackType => _attackType;
    public MonsterBaseStats BaseStats { get { return _baseStats; } protected set { _baseStats = value; } }
    #region Stats
    public override int MaxHealth
    {
        get
        {
            return BaseStats.Hp + _level * 10;
        }
    }
    public override int MeleeDam
    {
        get
        {
            return BaseStats.Atk + _level * 1;
        }
    }
    public override int RangeDam
    {
        get
        {
            return BaseStats.Atk + _level * 1;
        }
    }
    public override int CritRate
    {
        get
        {
            return BaseStats.CritRate;
        }
    }
    public override int CritDamage
    {
        get
        {
            return BaseStats.CritDamage;
        }
    }
    public override int HitEachTurn
    {
        get
        {
            return 1;
        }
    }
    public override int CurrentHealth { get; protected set; }
    public override int Shield { get; set; }
    public override int Lifesteal
    {
        get
        {
            return 0;
        }
    }
    #endregion

    public void Init(int level)
    {
        _isDead = false;
        _level = level;
        CurrentHealth = MaxHealth;
        View.HealthBar.Set(MaxHealth, CurrentHealth);
        View.ChangeWeapon(_attackType == AttackType.Melee);
        gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        View.Clear();
    }

    private void MelleAttack(UnitController target, Action onCompleted)
    {
        View.Animator.speed = 4;
        View.Animator.SetTrigger("Slash");
        StartCoroutine(DuelDamage(1 / View.Animator.speed, target, onCompleted));
    }

    private IEnumerator DuelDamage(float duration, UnitController target, Action onCompleted)
    {
        yield return new WaitForSeconds(duration);
        int damage = MeleeDam;
        bool isCrited = UnityEngine.Random.Range(0f, 100f) < CritRate;
        damage = isCrited ? damage + (int)(damage * CritDamage / 100f) : damage;
        target.TakeDamage(damage, isCrited, this, () => { onCompleted?.Invoke(); });
        View.Animator.speed = 1;
    }

    private void RangeAttack(UnitController target, Action onCompleted)
    {
        View.Animator.speed = 4;
        View.Animator.SetTrigger("Drat");
        StartCoroutine(ThrowBullet(1 / View.Animator.speed, target, onCompleted));
    }

    private IEnumerator ThrowBullet(float duration, UnitController target, Action onCompleted)
    {
        yield return new WaitForSeconds(duration);
        var bullet = BulletSpawner.Instance.Spawn(BulletType.Normal);
        bullet.transform.position = _shootPoint.position;
        bullet.Init(this, target, onCompleted);
        View.Animator.speed = 1;
    }

    public override void OnAttack(UnitController target, Action onCompleted)
    {
        if (_attackType == AttackType.Melee)
        {
            MelleAttack(target, onCompleted);
        }
        else
        {
            RangeAttack(target, onCompleted);
        }
    }

    public override void TakeDamage(int amount, bool isCrit, UnitController attacker, Action onCompleted)
    {
        //base.TakeDamage(amount, attacker, onCompleted);
        //View.HealthBar.Set(MaxHealth, CurrentHealth);
        //var damage = View.DamageTextSpawner.Spawn();
        //damage.Show(amount);
        amount = DamPassedShieldBlocked(amount);
        CurrentHealth -= amount;
        attacker.HealthRegen((int)(amount * attacker.Lifesteal / 100f));
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            attacker.OnKilled();
            Die(onCompleted);
            return;
        }
        View.Animator.speed = 4;
        View.Animator.SetTrigger("Beaten");
        View.HealthBar.Set(MaxHealth, CurrentHealth);
        var damage = View.DamageTextSpawner.Spawn();
        StatusText statusText = StatusText.NormalDamage;
        statusText = isCrit ? StatusText.CritDamage : statusText;
        damage.Show(amount, statusText);
        StartCoroutine(Utilities.DelayCallback(1 / View.Animator.speed, () =>
        {
            View.Animator.speed = 1;
            onCompleted?.Invoke();
        }));
    }

    protected override void Die(Action onCompleted)
    {
        BallMiniGame.Instance.AddBall(BaseStats.Ball);
        base.Die(onCompleted);
    }

    public override bool IsEnemyInScopeAttack(UnitController target)
    {
        if (_attackType == AttackType.Melee)
        {
            Battle grid = GameController.Instance.Battle;
            Node nodeA = GameController.Instance.Battle.GetNode(X, Y);
            Node nodeB = GameController.Instance.Battle.GetNode(target.X, target.Y);
            int distance = grid.Distance(nodeA, nodeB);
            if (distance == 1)
            {
                return true;
            }

            if (distance == 2 && Mathf.Abs(nodeA.X - nodeB.X) == 1 && Mathf.Abs(nodeA.Y - nodeB.Y) == 1)
            {
                return true;
            }
        }
        else
        {
            return true;
        }
        return false;
    }

    public override void OnKilled()
    {
        
    }

    public override void HealthRegen(int amount)
    {
        base.HealthRegen(amount);
        View.HealthBar.Set(MaxHealth, CurrentHealth);
    }

    public override void MoveToNode(Node targetNode, Action onCompleted)
    {
        View.Animator.SetBool("IsRunning", true);
        transform.DOMove(targetNode.Position, 1f).OnComplete(() =>
        {
            _x = targetNode.X;
            _y = targetNode.Y;
            View.Animator.SetBool("IsRunning", false);
            onCompleted?.Invoke();
        });
    }

    public override void Move(Vector3 position, Action onCompleted)
    {
        View.Animator.SetBool("IsRunning", true);
        transform.DOMove(position, 1f).OnComplete(() =>
        {
            View.Animator.SetBool("IsRunning", false);
            onCompleted?.Invoke();
        });
    }
}
