using DG.Tweening;
using Inventory;
using Minigame.Ball;
using System;
using System.Collections;
using UnityEngine;

public class PlayerController : UnitController
{
    private PlayerView _view;
    private CardStats _cardStats;
    private BaseStats _baseStats;
    private bool _isMeleeAttacking;
    private int _shield;

    public CardStats CardStats { get { return _cardStats; } protected set { _cardStats = value; } }
    public BaseStats BaseStats { get { return _baseStats; } protected set { _baseStats = value; } }
    public PlayerView View
    {
        get
        {
            if(_view is null)
            {
                _view = GetComponent<PlayerView>();
            }
            return _view;
        }
    }
    #region Stats
    public override int MaxHealth
    {
        get
        {
            return BaseStats.HealthPoint + CardStats.HealthPoint + EquipmentManager.Instance.Hp;
        }
    }
    public override int MeleeDam
    {
        get
        {
            return BaseStats.MeleeDam + CardStats.MeleeDam + EquipmentManager.Instance.MeleeDam;
        }
    }
    public override int RangeDam
    {
        get
        {
            return BaseStats.RangeDam + CardStats.RangeDam + EquipmentManager.Instance.RangeDam;
        }
    }
    public override int CritRate
    {
        get
        {
            return BaseStats.CritRate + CardStats.CritRate;
        }
    }
    public override int CritDamage
    {
        get
        {
            return BaseStats.CritDamage + CardStats.CritDamage;
        }
    }
    public override int HitEachTurn
    {
        get
        {
            return BaseStats.HitEachTurn + CardStats.HitEachTurn;
        }
    }
    public override int CurrentHealth { get ; protected set; }
    public override int Shield
    {
        get { return _shield; }
        set
        {
            _shield = value;
            View.HealthBar.Set(MaxHealth, CurrentHealth, Shield);
        }
    }
    public int Ball
    {
        get
        {
            return CardStats.Ball;
        }
    }
    public override int Lifesteal
    {
        get
        {
            return CardStats.Lifesteal;
        }
    }
    public virtual int Block
    {
        get
        {
            return CardStats.Block;
        }
    }
    #endregion


    public void Initialize()
    {
        View.Initialize();
        _isMeleeAttacking = true;
        //controllers
        BaseStats = new BaseStats(100, 10, 100, 1, 5, 5);
        CardStats = new CardStats(this);
        //
        CurrentHealth = MaxHealth;
        View.HealthBar.Set(MaxHealth, CurrentHealth, Shield);
    }

    public void HealthFull()
    {
        CurrentHealth = MaxHealth;
        View.HealthBar.Set(MaxHealth, CurrentHealth, Shield);
    }

    public void SetHealth(int currentHp, int shield)
    {
        CurrentHealth = currentHp;
        Shield = shield;
        View.HealthBar.Set(MaxHealth, CurrentHealth, Shield);
    }

    protected virtual int DamPassedBlock(int amount)
    {
        return amount - Block < 0 ? 0 : amount - Block;
    }

    public override void TakeDamage(int amount, bool isCrit, UnitController attacker, Action onCompleted)
    {
        amount = DamPassedBlock(amount);
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
        View.HealthBar.Set(MaxHealth, CurrentHealth, Shield);
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
        _isDead = true;
        View.Animator.SetTrigger("Die");
        StartCoroutine(Utilities.DelayCallback(1 / View.Animator.speed, () => { onCompleted?.Invoke(); }));
    }

    private void MeleeAttack(UnitController target, Action onCompleted)
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
    }

    public override void OnAttack(UnitController target, Action onCompleted)
    {
        float speed;
        if (IsMeleeAttack(target))
        {
            if (!_isMeleeAttacking)
            {
                View.Animator.speed = 4;
                View.Animator.SetTrigger("Change");
                speed = 1 / View.Animator.speed;
            }
            else
            {
                speed = 0;
            }
            StartCoroutine(Utilities.DelayCallback(speed, () => { View.Animator.speed = 1; View.ChangeWeapon(true); MeleeAttack(target, onCompleted); }));
            _isMeleeAttacking = true;
        }
        else
        {
            if (_isMeleeAttacking)
            {
                View.Animator.speed = 4;
                View.Animator.SetTrigger("Change");
                speed = 1 / View.Animator.speed;
            }
            else
            {
                speed = 0;
            }
            StartCoroutine(Utilities.DelayCallback(speed, () => { View.Animator.speed = 1; View.ChangeWeapon(false); RangeAttack(target, onCompleted); }));
            _isMeleeAttacking = false;
        }
    }

    private bool IsMeleeAttack(UnitController target)
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
        return false;
    }

    public override bool IsEnemyInScopeAttack(UnitController target)
    {
        return true;
    }

    public override void OnKilled()
    {
        BallMiniGame.Instance.AddBall(Ball);
    }

    public override void HealthRegen(int amount)
    {
        if (amount == 0 || CurrentHealth == MaxHealth)
        {
            return;
        }
        CurrentHealth = CurrentHealth + amount < MaxHealth ? CurrentHealth + amount : MaxHealth;
        var text = View.DamageTextSpawner.Spawn();
        StatusText statusText = StatusText.Health;
        text.Show(amount, statusText);
        View.HealthBar.Set(MaxHealth, CurrentHealth, Shield);
    }

    public override void MoveToNode(Node targetNode, Action onCompleted)
    {
        View.Animator.speed = 1;
        View.Animator.SetBool("IsRunning", true);
        transform.DOMove(targetNode.Position, 1f).OnComplete(() =>
        {
            _x = targetNode.X;
            _y = targetNode.Y;
            View.Animator.speed = 1;
            View.Animator.SetBool("IsRunning", false);
            onCompleted?.Invoke();
        });
    }

    public override void Move(Vector3 position, Action onCompleted)
    {
        View.Animator.speed = 1;
        View.Animator.SetBool("IsRunning", true);
        transform.DOMove(position, 1f).OnComplete(() =>
        {
            View.Animator.speed = 1;
            View.Animator.SetBool("IsRunning", false);
            onCompleted?.Invoke();
        });
    }

    public void Move(Vector3 position, Action onCompleted, float duration = 1)
    {
        View.Animator.speed = 1;
        View.Animator.SetBool("IsRunning", true);
        transform.DOMove(position, duration).OnComplete(() =>
        {
            View.Animator.speed = 1;
            View.Animator.SetBool("IsRunning", false);
            onCompleted?.Invoke();
        });
    }
}
