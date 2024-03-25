using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitController : MonoBehaviour
{
    [SerializeField] protected int _x;
    [SerializeField] protected int _y;
    [SerializeField] protected bool _isDead;
    [SerializeField] protected Transform _shootPoint;

    //
    public abstract int CurrentHealth { get; protected set; }
    public abstract int Shield { get; set; }
    public abstract int MaxHealth { get; }
    public abstract int MeleeDam { get; }
    public abstract int RangeDam { get; }
    public abstract int CritRate { get; }
    public abstract int CritDamage { get; }
    public abstract int HitEachTurn { get; }
    public abstract int Lifesteal { get; }

    public int X => _x;
    public int Y => _y;
    public bool IsDead => _isDead;

    public virtual void MoveToNode(Node targetNode, Action onCompleted)
    {
        transform.DOMove(targetNode.Position, 1f).OnComplete(() =>
        {
            _x = targetNode.X;
            _y = targetNode.Y;
            onCompleted?.Invoke();
        });
    }

    public virtual void Move(Vector3 position, Action onCompleted)
    {
        transform.DOMove(position, 1f).OnComplete(() =>
        {
            onCompleted?.Invoke();
        });
    }

    public abstract bool IsEnemyInScopeAttack(UnitController target);

    public abstract void OnAttack(UnitController unit, Action onCompleted);

    protected virtual int DamPassedShieldBlocked(int amount)
    {
        int realDamage = amount - Shield < 0 ? 0 : amount - Shield;
        Shield = Shield - amount < 0 ? 0 : Shield - amount;
        return realDamage;
    }

    public virtual void TakeDamage(int amount, bool isCrit, UnitController attacker, Action onCompleted)
    {
        //amount = DamPassedShieldBlocked(amount);
        //CurrentHealth -= amount;
        //attacker.HealthRegen((int)(amount * Lifesteal / 100f));
        //if (CurrentHealth <= 0)
        //{
        //    CurrentHealth = 0;
        //    attacker.OnKilled();
        //    Die(onCompleted);
        //    return;
        //}
        //onCompleted?.Invoke();
    }

    protected virtual void Die(Action onCompleted)
    {
        _isDead = true;
        gameObject.SetActive(false);
        onCompleted?.Invoke();
    }

    public abstract void OnKilled();

    public virtual void HealthRegen(int amount)
    {
        if (amount == 0 || CurrentHealth == MaxHealth)
        {
            return;
        }
        CurrentHealth = CurrentHealth + amount < MaxHealth ? CurrentHealth + amount : MaxHealth;
    }
}

public enum AttackType
{
    Melee = 0,
    Range = 1
}