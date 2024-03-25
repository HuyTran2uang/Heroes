using DG.Tweening;
using System;
using UnityEngine;

public class CameraController : MonoBehaviourSingleton<CameraController>
{
    private Vector3 _fightPos = new Vector3(0, 6.3f, -22.65f);
    private Vector3 _fightAngles = new Vector3(30, 0, 0);
    private Vector3 _minigamePos = new Vector3(0, -101f, -22.65f);
    private Vector3 _minigameAngles = new Vector3(0, 0, 0);

    public void FightMode(float duration, Action onCompleted)
    {
        transform.DOMove(_fightPos, duration);
        transform.DORotate(_fightAngles, duration).OnComplete(() => { onCompleted?.Invoke(); });
    }

    public void MinigameMode(float duration, Action onCompleted)
    {
        transform.DOMove(_minigamePos, duration);
        transform.DORotate(_minigameAngles, duration).OnComplete(() => { onCompleted?.Invoke(); });
    }
}
