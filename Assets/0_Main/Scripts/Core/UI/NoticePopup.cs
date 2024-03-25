using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NoticePopup : MonoBehaviour
{
    [SerializeField] private Button _confirmBtn, _cancelBtn;
    [SerializeField] private TMP_Text _label, _content;
    private Action _onConfirm, _onCancel;

    private void Awake()
    {
        _confirmBtn.onClick.AddListener(() => { Confirm(); });
        _cancelBtn.onClick.AddListener(() => { Cancel(); });
    }

    public void Show(string label, string content, Action confirm, Action cancel)
    {
        _label.text = label;
        _content.text = content;
        _onConfirm = confirm;
        _onCancel = cancel;
        gameObject.SetActive(true);
    }

    private void Confirm()
    {
        _onConfirm?.Invoke();
        gameObject.SetActive(false);
    }

    private void Cancel()
    {
        _onCancel?.Invoke();
        gameObject.SetActive(false);
    }
}
